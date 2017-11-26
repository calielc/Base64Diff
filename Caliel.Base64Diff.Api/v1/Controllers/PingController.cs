using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Caliel.Base64Diff.Api.v1.Controllers {
    [Route("v1/ping")]
    public class PingController : Controller {
        [HttpGet]
        [ProducesResponseType(typeof(DateTime), (int)HttpStatusCode.OK)]
        public ActionResult Get() {
            return Ok(DateTime.UtcNow);
        }
    }
}
