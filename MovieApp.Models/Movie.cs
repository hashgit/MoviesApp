using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Classification { get; set; }
        public string Genre { get; set; }
        public int Rating { get; set; }
        public int ReleaseDate { get; set; }
        public string[] Cast { get; set; }

        private string _searchText;

        public void BuildSearchText()
        {
            _searchText = string.Format("{0}, {1}, {2}, {3}, {4}, {5}",
                Title == null ? string.Empty : Title.ToLower(),
                Classification == null ? string.Empty : Classification.ToLower(),
                Genre == null ? string.Empty : Genre.ToLower(),
                Rating.ToString(),
                ReleaseDate.ToString(),
                Cast == null ? string.Empty : string.Join(" ", Cast.Select(c => c == null ? string.Empty : c.ToLower())));
        }

        public bool ContainsTerm(string term)
        {
            return _searchText.Contains(term.ToLower());
        }
    }
}
