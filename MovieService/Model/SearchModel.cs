using MovieApp.Models;

namespace MovieService.Model
{
    public class SearchModel
    {
        public string Term { get; set; }
        public SortFields? Field { get; set; }
        public SortDirection? Direction { get; set; }
    }
}