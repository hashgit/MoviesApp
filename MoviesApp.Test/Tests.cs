using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieApp.Manager;

namespace MoviesApp.Test
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void CanReadAllMovies()
        {
            // just a sample test case

            var movies = MovieManager.Instance.GetAll();
            Assert.IsNotNull(movies);
            Assert.AreEqual(80, movies.Count());
        }
    }
}
