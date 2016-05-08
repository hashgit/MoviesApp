using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieApp.Manager;
using MovieApp.Models;

namespace MoviesApp.Test
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void CanReadAllMovies()
        {
            // just a sample test case
            var movies = new MoviesManager().GetAll();
            Assert.IsNotNull(movies);
            Assert.AreEqual(80, movies.Count());
        }

        [TestMethod]
        public void CanSortMovies()
        {
            var manager = new MoviesManager();
            var movies = manager.GetAll();
            Assert.IsNotNull(movies);
            Assert.AreEqual(80, movies.Count());
            Assert.AreEqual(1, movies.First().Id);

            var sorted = manager.Sort(movies, SortFields.Id, SortDirection.Descending);
            Assert.AreEqual(80, sorted.First().Id);
        }

        [TestMethod]
        public void CanSortMoviesByGenre()
        {
            var manager = new MoviesManager();
            var movies = manager.GetAll();
            Assert.IsNotNull(movies);
            Assert.AreEqual(80, movies.Count());
            Assert.AreEqual(1, movies.First().Id);

            var sorted = manager.Sort(movies, SortFields.Genre, SortDirection.Descending);
            Assert.AreEqual(68, sorted.First().Id);
        }
    }
}
