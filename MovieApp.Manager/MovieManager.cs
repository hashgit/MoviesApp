using System;
using System.Collections.Generic;
using System.Linq;
using MovieApp.Models;
using MoviesLibrary;
using MovieApp.Manager.Repositories;
using System.Threading.Tasks;

namespace MovieApp.Manager
{
    public class MoviesManager : IMoviesManager
    {
        private readonly IStatusRepository _statusRepository;
        private readonly IMoviesRepository _moviesRepository;
        private readonly MovieDataSource _source;

        public MoviesManager(IStatusRepository statusRepository, IMoviesRepository moviesRepository)
        {
            _moviesRepository = moviesRepository;
            _statusRepository = statusRepository;
            _source = new MovieDataSource(); // this should be injected as well but the DLL should not be added to all projects
        }


        public async Task<IEnumerable<Movie>> GetAll(SortFields? sortField, SortDirection? sortDirection)
        {
            var movies = await _moviesRepository.GetAll(sortField, sortDirection);
            return movies.Select(m => Maps.MapToDto(m));
        }

        public Movie TryGet(int id)
        {
            var movie = _moviesRepository.Get(id);
            if (movie != null)
            {
                return Maps.MapToDto(movie);
            }

            return null;
        }

        public async Task<IEnumerable<Movie>> Search(string term)
        {
            var movies = await _moviesRepository.Find(term);
            return movies.Select(Maps.MapToDto);
        }

        public void AddNew(Movie movie)
        {
            movie.BuildSearchText();
            _moviesRepository.Save(Maps.MapToEntity(movie));
        }

        public bool Update(Movie movie)
        {
            var stored = _moviesRepository.Get(movie.Id);
            if (stored != null)
            {
                movie.BuildSearchText();

                stored.Genre = movie.Genre;
                stored.Classification = movie.Classification;
                stored.Rating = movie.Rating;
                stored.ReleaseDate = movie.ReleaseDate;
                stored.Title = movie.Title;
                stored.SearchText = movie.SearchText;

                // update cast here too

                _moviesRepository.Save(stored);
                return true;
            }

            return false;
        }

        private IList<Movie> Load()
        {
            var movies = _source.GetAllData();
            return movies.Select(x => Maps.MapToDto(x)).ToList();
        }

        public async void SyncDatabase()
        {
            var status = _statusRepository.GetLastRuntime();
            if (status == null || !status.LastUpdate.HasValue)
            {
                // we have nothing in the db, populate it from 3rd party data source
                var movies = Load();
                var entities = movies.Select(m => Maps.MapToEntity(m)).ToList();

                _moviesRepository.Save(entities);
                _statusRepository.UpdateLastRuntime();
            }
            else
            {
                // update latest movies
                using (var transaction = _moviesRepository.Begin())
                {
                    // this transaction is to avoid any concurrent updates from the website

                    var recentMovies = _moviesRepository.GetRecentMovies(status.LastUpdate.Value);
                    foreach (var movie in recentMovies)
                    {
                        var dto = Maps.MapToSource(movie);
                        if (movie.DateCreated >= status.LastUpdate.Value)
                        {
                            _source.Create(dto);
                        }
                        else
                        {
                            _source.Update(dto);
                        }
                    }

                    var movies = Load();
                    var storedMovies = await _moviesRepository.GetAll(null, null);

                    foreach(var movie in movies)
                    {
                        var storedMovie = storedMovies.FirstOrDefault(m => m.ReferenceId == movie.Id);
                        if (storedMovie == null)
                        {
                            storedMovies.Add(Maps.MapToEntity(movie));
                        } 
                        else
                        {
                            storedMovie.Genre = movie.Genre;
                            storedMovie.Classification = movie.Classification;
                            storedMovie.Rating = movie.Rating;
                            storedMovie.ReleaseDate = movie.ReleaseDate;
                            storedMovie.Title = movie.Title;
                            storedMovie.SearchText = movie.SearchText;
                            
                            // update cast as well
                        }
                    }

                    _moviesRepository.Commit(transaction);
                }
            }
        }
    }
}