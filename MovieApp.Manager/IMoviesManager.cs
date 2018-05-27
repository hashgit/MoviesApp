using System.Collections.Generic;
using MovieApp.Models;
using System.Threading.Tasks;

namespace MovieApp.Manager
{
    public interface IMoviesManager
    {
        Task<IEnumerable<Movie>> GetAll(SortFields? sortField, SortDirection? sortDirection);
        Task<IEnumerable<Movie>> Search(string term);
        Movie TryGet(int id);
        void AddNew(Movie movie);
        bool Update(Movie movie);
        void SyncDatabase();
    }
}