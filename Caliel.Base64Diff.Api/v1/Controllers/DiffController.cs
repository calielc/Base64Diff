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

        [HttpPost]
        [Route("left")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public ActionResult Left(string id, [FromBody] DiffInputModel inputModel) {
            if (inputModel.IsValid() == false) {
                return BadRequest(id);
            }

            _service.LoadOrCreate(id).SetLeft(inputModel.GetBytes()).Save();

            return Ok(id);
        }

        [HttpPost]
        [Route("right")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public ActionResult Right(string id, [FromBody] DiffInputModel inputModel) {
            if (inputModel.IsValid() == false) {
                return BadRequest(id);
            }

            _service.LoadOrCreate(id).SetRight(inputModel.GetBytes()).Save();

            return Ok(id);
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(DiffViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public ActionResult Get(string id) {
            var similarity = _service.Load(id)?.Similarity;
            if (similarity is null) {
                return NotFound(id);
            }

            return Ok(new DiffViewModel {
                Similarity = similarity.Similarity,
                Diffs = similarity.Diffs
            });
        }
    }
}