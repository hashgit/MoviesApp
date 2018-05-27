using System.Collections.Generic;
using System.Web.Http;
using MovieApp.Manager;
using MovieApp.Models;
using MovieService.Model;
using System.Threading.Tasks;

namespace MovieService.Controllers
{
    public class MoviesController : ApiController
    {
        private readonly IMoviesManager _moviesManager;

        public MoviesController(IMoviesManager moviesManager)
        {
            _moviesManager = moviesManager;
        }

        [Route("api/Movies/")]
        [HttpGet]
        public async Task<IHttpActionResult> List([FromUri] SearchModel model)
        {
            if (model == null) model = new SearchModel();

            IEnumerable<Movie> movies;
            if (!string.IsNullOrWhiteSpace(model.Term))
            {
                movies = await _moviesManager.Search(model.Term);
            }
            else
            {
                movies = await _moviesManager.GetAll(model.Field, model.Direction);
            }

            return Ok(movies);
        }

        [Route("api/Movies/{id}")]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var movie = _moviesManager.TryGet(id);
            if (movie == null)
                return NotFound();

            return Ok(movie);
        }

        [Route("api/Movies/")]
        [HttpPut]
        public IHttpActionResult Add([FromBody] Movie movie)
        {
            _moviesManager.AddNew(movie);
            return Ok();
        }

        [Route("api/Movies/{id}")]
        [HttpPost]
        public IHttpActionResult Add(int id, [FromBody] Movie movie)
        {
            movie.Id = id;
            var result = _moviesManager.Update(movie);
            return Ok(result);
        }
    }
}
