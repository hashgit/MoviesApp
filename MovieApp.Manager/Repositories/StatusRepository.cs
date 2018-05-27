using MovieApp.Manager.DataContext;
using MovieApp.Manager.Entities;
using System;
using System.Linq;

namespace MovieApp.Manager.Repositories
{
    public interface IStatusRepository
    {
        Status GetLastRuntime();
        void UpdateLastRuntime();
    }

    public class StatusRepository : IStatusRepository
    {
        private readonly MoviesDbContext _dbContext;

        public StatusRepository(MoviesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Status GetLastRuntime()
        {
            return _dbContext.Status.OrderByDescending(s => s.LastUpdate).FirstOrDefault();
        }

        public void UpdateLastRuntime()
        {
            var status = new Status()
            {
                LastUpdate = DateTime.Now
            };

            _dbContext.Status.Add(status);
            _dbContext.SaveChanges();
        }
    }
}
