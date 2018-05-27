using MovieApp.Manager.Entities;
using System.Data.Entity;

namespace MovieApp.Manager.DataContext
{
    public class MoviesDbContext : DbContext
    {
        public DbSet<Status> Status { get; set; }
        public DbSet<Movie> Movies { get; set; }

        public MoviesDbContext()
            : base("MovieDatabase")
        {
        }
    }
}
