using System.Net;
using Caliel.Base64Diff.Api.v1.Models;
using Caliel.Base64Diff.Domain.Diff;
using Microsoft.AspNetCore.Mvc;

namespace Caliel.Base64Diff.Api.v1.Controllers {
    [Produces("application/json")]
    [Route("v1/diff/{id}")]
    public class DiffController : Controller {
        private readonly IDiffService _service;

        public DiffController(IDiffService service) {
            _service = service;
        }

        /// <summary>
        /// Set Left side of Id
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="input">content</param>
        /// <returns>received Id</returns>
        /// <response code="400">Base64 data is missing or invalid</response>
        [HttpPost]
        [Route("left")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public ActionResult Left(string id, [FromBody] DiffInputModel input) {
            if (input.IsValid() == false) {
                return BadRequest(id);
            }

            _service.LoadOrCreate(id).SetLeft(input.GetBytes()).Save();

            return Ok(id);
        }

        /// <summary>
        /// Set right side of Id
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="input">content</param>
        /// <returns>received Id</returns>
        /// <response code="400">Base64 data is missing or invalid</response>
        [HttpPost]
        [Route("right")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public ActionResult Right(string id, [FromBody] DiffInputModel input) {
            if (input.IsValid() == false) {
                return BadRequest(id);
            }

            _service.LoadOrCreate(id).SetRight(input.GetBytes()).Save();

            return Ok(id);
        }

        /// <summary>
        /// Return diff-ed of Id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        /// <response code="204">Id was found but one side is missing</response>
        /// <response code="404">Id was not found</response>        
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(DiffViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        public ActionResult Get(string id) {
            var model = _service.Load(id);
            if (model is null) {
                return NotFound(id);
            }

            var similarity = model.Similarity;
            if (similarity is null) {
                return NoContent();
            }

            return Ok(new DiffViewModel {
                Similarity = similarity.Similarity,
                Diffs = similarity.Diffs
            });
        }
    }
}