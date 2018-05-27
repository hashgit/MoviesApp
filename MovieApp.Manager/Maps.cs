using MovieApp.Models;
using MoviesLibrary;
using System;
using System.Linq;

namespace MovieApp.Manager
{
    class Maps
    {
        public static MovieData MapToSource(Entities.Movie movie)
        {
            var obj = new MovieData
            {
                Classification = movie.Classification,
                Genre = movie.Genre,
                Rating = movie.Rating,
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate
            };

            if (movie.Cast != null) obj.Cast = movie.Cast.Select(c => c.Name).ToArray();
            return obj;
        }

        public static MovieData MapToSource(Movie movie)
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

        public static Movie MapToDto(MovieData obj)
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

        public static Movie MapToDto(Entities.Movie obj)
        {
            var dto = new Movie
            {
                Id = obj.ReferenceId,
                Classification = obj.Classification,
                Genre = obj.Genre,
                Rating = obj.Rating,
                Title = obj.Title,
                ReleaseDate = obj.ReleaseDate
            };

            if (obj.Cast != null) dto.Cast = obj.Cast.Select(c => c.Name).ToArray();
            return dto;
        }

        public static Entities.Movie MapToEntity(Movie m)
        {
            return new Entities.Movie()
            {
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Classification = m.Classification,
                Genre = m.Genre,
                Rating = m.Rating,
                ReferenceId = m.Id,
                ReleaseDate = m.ReleaseDate,
                SearchText = m.SearchText,
                Title = m.Title,
                Cast = m.Cast?.Select(c => new Entities.Cast()
                {
                    Name = c,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now
                }).ToList(),
            };
        }
    }
}
