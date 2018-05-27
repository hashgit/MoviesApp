using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieApp.Manager;
using Moq;
using MovieApp.Manager.Repositories;
using MovieApp.Manager.Entities;
using System.Collections.Generic;

namespace MoviesApp.Test
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void CanPopulateTheDatabaseWhenNoStatusFound()
        {
            // just a sample test case
            var mockStatusRepository = new Mock<IStatusRepository>();
            var mockMoviesRepository = new Mock<IMoviesRepository>();

            var movies = new MoviesManager(mockStatusRepository.Object, mockMoviesRepository.Object);

            mockStatusRepository.Setup(x => x.GetLastRuntime()).Returns<Status>(null);
            movies.SyncDatabase();
            mockMoviesRepository.Verify(x => x.Save(It.IsAny<List<MovieApp.Manager.Entities.Movie>>()));
            mockStatusRepository.Verify(x => x.UpdateLastRuntime());
        }
    }
}
