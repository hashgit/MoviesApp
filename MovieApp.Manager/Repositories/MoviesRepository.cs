using MovieApp.Manager.DataContext;
using MovieApp.Manager.Entities;
using MovieApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Movie = MovieApp.Manager.Entities.Movie;

namespace MovieApp.Manager.Repositories
{
    public interface IMoviesRepository
    {
        void Save(IList<Movie> movies);
        void Save(Movie movies);
        IList<Movie> GetRecentMovies(DateTime lastRun);
        DbContextTransaction Begin();
        void Commit(DbContextTransaction transaction);
        Task<IList<Movie>> GetAll(SortFields? sortField, SortDirection? sortDirection);
        Movie Get(int id);
        Task<IList<Movie>> Find(string term);
    }

    public class MoviesRepository : IMoviesRepository
    {
        private readonly MoviesDbContext _dbContext;

        public MoviesRepository(MoviesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Save(IList<Movie> movies)
        {
            _dbContext.Movies.AddRange(movies);
            _dbContext.SaveChanges();
        }

        public void Save(Entities.Movie movies)
        {
            _dbContext.Movies.Add(movies);
            _dbContext.SaveChanges();
        }

        public IList<Entities.Movie> GetRecentMovies(DateTime lastRun)
        {
            return _dbContext.Movies
                .Where(m => m.DateCreated >= lastRun || m.DateUpdated >= lastRun)
                .ToList();
        }

        public async Task<IList<Entities.Movie>> Find(string term)
        {
            return await _dbContext.Movies.Where(m => m.SearchText.Contains(term)).ToListAsync();
        }

        public async Task<IList<Movie>> GetAll(SortFields? sortField, SortDirection? sortDirection)
        {
            var movies = _dbContext.Movies.AsQueryable();
            if (sortField.HasValue)
            {
                switch(sortField.Value)
                {
                    case SortFields.Classification:
                        movies = sortDirection == SortDirection.Descending ? movies.OrderByDescending(m => m.Classification)
                            : movies.OrderBy(m => m.Classification);
                        break;
                    case SortFields.Genre:
                        movies = sortDirection == SortDirection.Descending ? movies.OrderByDescending(m => m.Genre)
                            : movies.OrderBy(m => m.Genre);
                        break;
                    case SortFields.Rating:
                        movies = sortDirection == SortDirection.Descending ? movies.OrderByDescending(m => m.Rating)
                            : movies.OrderBy(m => m.Rating);
                        break;
                    case SortFields.Title:
                        movies = sortDirection == SortDirection.Descending ? movies.OrderByDescending(m => m.Title)
                            : movies.OrderBy(m => m.Title);
                        break;
                    case SortFields.ReleaseDate:
                        movies = sortDirection == SortDirection.Descending ? movies.OrderByDescending(m => m.ReleaseDate)
                            : movies.OrderBy(m => m.ReleaseDate);
                        break;
                }
            }

            return await movies.ToListAsync();
        }

        public Movie Get(int id)
        {
            return _dbContext.Movies.FirstOrDefault(m => m.ReferenceId == id);
        }

        public DbContextTransaction Begin()
        {
            return _dbContext.Database.BeginTransaction();
        }

        public void Commit(DbContextTransaction transaction)
        {
            transaction.Commit();
        }
    }
}
