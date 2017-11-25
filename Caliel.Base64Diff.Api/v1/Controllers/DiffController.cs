using System.Collections.Generic;
using System.Net;
using Caliel.Base64Diff.Api.Bussiness;
using Caliel.Base64Diff.Api.v1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Caliel.Base64Diff.Api.v1.Controllers {
    [Produces("application/json")]
    [Route("v1/diff/{id}")]
    public class DiffController : Controller {
        private static readonly Dictionary<string, byte[]> LeftData = new Dictionary<string, byte[]>();
        private static readonly Dictionary<string, byte[]> RightData = new Dictionary<string, byte[]>();

        [HttpPost]
        [Route("left")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public ActionResult Left(string id, [FromBody] DiffInputModel inputModel) {
            if (inputModel.IsValid() == false) {
                return BadRequest(id);
            }

            LeftData[id] = inputModel.GetBytes();

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

            RightData[id] = inputModel.GetBytes();

            return Ok(id);
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(DiffViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public ActionResult Get(string id) {
            if (LeftData.ContainsKey(id) == false || RightData.ContainsKey(id) == false) {
                return NotFound(id);
            }

            var arrayComparison = new BytesSimilarity(LeftData[id], RightData[id]);

            return Ok(new DiffViewModel {
                Similarity = arrayComparison.Similarity,
                Diffs = arrayComparison.Diffs
            });
        }
    }
}