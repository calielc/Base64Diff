using System;
using Microsoft.AspNetCore.Mvc;

namespace Caliel.Base64Diff.Api.v1.Controllers {
    [Route("v1/ping")]
    public class PingController : Controller {
        [HttpGet]
        public ActionResult Get() {
            return Ok(DateTime.UtcNow);
        }
    }
}
