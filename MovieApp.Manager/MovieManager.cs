using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MovieApp.Models;
using MoviesLibrary;

namespace MovieApp.Manager
{
    public class MoviesManager : IMoviesManager
    {
        private readonly ConcurrentDictionary<int, Movie> _dictionary;
        private readonly object _padlock = new object();
        // have to keep object in the class otherwise will lose the session
        // in reality we should create this instance when required
        private readonly MovieDataSource _source;
        private DateTime _lastUpdated = DateTime.MinValue;

        public MoviesManager()
        {
            _dictionary = new ConcurrentDictionary<int, Movie>();
            _source = new MovieDataSource();
        }


        public IEnumerable<Movie> GetAll()
        {
            CheckLastUpdated();
            return _dictionary.OrderBy(t => t.Key).Select(t => t.Value).ToList();
        }

        public Movie TryGet(int id)
        {
            CheckLastUpdated();
            Movie movie;
            if (_dictionary.TryGetValue(id, out movie)) return movie;

            // not found in the cache, try in the source
            var source = _source.GetDataById(id);
            if (source == null) return null;

            var data = MapToDto(source);
            _dictionary.TryAdd(data.Id, data);
            return data;
        }

        public IEnumerable<Movie> Search(string term)
        {
            CheckLastUpdated();
            return _dictionary.Values.Where(d => d.ContainsTerm(term)).ToList();
        }

        public int AddNew(Movie movie)
        {
            CheckLastUpdated();
            var result = _source.Create(MapToSource(movie));

            if (result > 0)
            {
                movie.Id = result;
                movie.BuildSearchText();
                // don't care if that fails, it will fix itself up
                _dictionary.TryAdd(result, movie);
                return result;
            }

            return 0;
        }

        public bool Update(Movie movie)
        {
            CheckLastUpdated();
            var source = MapToSource(movie);
            source.MovieId = movie.Id;

            movie.BuildSearchText();
            _source.Update(source);

            _dictionary.AddOrUpdate(movie.Id, movie, (i, movie1) => movie);
            return true;
        }

        private void LoadMovies()
        {
            var movies = _source.GetAllData();
            if (movies == null || movies.Count == 0) return;

            movies.ForEach(movie =>
            {
                var movieDto = MapToDto(movie);
                _dictionary.AddOrUpdate(movie.MovieId, movieDto, (i, movie1) => movieDto);
            });
        }

        private void CheckLastUpdated()
        {
            // updated today
            if (DateTime.Now - _lastUpdated < TimeSpan.FromHours(24)) return;
            lock (_padlock)
            {
                if (DateTime.Now - _lastUpdated < TimeSpan.FromHours(24)) return;
                LoadMovies();
                _lastUpdated = DateTime.Now;
            }
        }

        private static MovieData MapToSource(Movie movie)
        {
            var obj = new MovieData
            {
                Classification = movie.Classification,
                Genre = movie.Genre,
                Rating = movie.Rating,
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate
            };

            if (movie.Cast != null) obj.Cast = movie.Cast.ToArray();
            return obj;
        }

        private static Movie MapToDto(MovieData obj)
        {
            var dto = new Movie
            {
                Id = obj.MovieId,
                Classification = obj.Classification,
                Genre = obj.Genre,
                Rating = obj.Rating,
                Title = obj.Title,
                ReleaseDate = obj.ReleaseDate
            };

            if (obj.Cast != null) dto.Cast = obj.Cast.ToArray();
            dto.BuildSearchText();
            return dto;
        }
    }
}