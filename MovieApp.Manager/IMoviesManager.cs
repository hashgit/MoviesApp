using System.Collections.Generic;
using MovieApp.Models;

namespace MovieApp.Manager
{
    public interface IMoviesManager
    {
        IEnumerable<Movie> GetAll();
        IEnumerable<Movie> Search(string term);
        Movie TryGet(int id);
        int AddNew(Movie movie);
        bool Update(Movie movie);
    }
}