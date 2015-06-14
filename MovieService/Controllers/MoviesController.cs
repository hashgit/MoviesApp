using System.Web.Http;
using System.Web.UI.WebControls;
using MovieApp.Manager;
using MovieApp.Models;

namespace MovieService.Controllers
{
    public class MoviesController : ApiController
    {
        [Route("api/Movies/")]
        [HttpGet]
        public IHttpActionResult List(string term = null)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                var allMovies = MovieManager.Instance.GetAll();
                return Ok(allMovies);
            }

            return Ok(MovieManager.Instance.Search(term));
        }

        [Route("api/Movies/{id}")]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var movie = MovieManager.Instance.TryGet(id);
            if (movie == null)
                return NotFound();

            return Ok(movie);
        }

        [Route("api/Movies/")]
        [HttpPut]
        public IHttpActionResult Add([FromBody] Movie movie)
        {
            var result = MovieManager.Instance.AddNew(movie);
            return Ok(result);
        }

        [Route("api/Movies/{id}")]
        [HttpPost]
        public IHttpActionResult Add(int id, [FromBody] Movie movie)
        {
            movie.Id = id;
            var result = MovieManager.Instance.Update(movie);
            return Ok(result);
        }
    }
}
