using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExceptionHandlerController : ControllerBase
    {
        [HttpGet("NoException")]
        public ActionResult NoException()
        {
            return Ok(".net real world example");
        }

        [HttpGet("NotImplementedException")]
        public ActionResult GetNotImplementedException()
        {
            throw new NotImplementedException();
            return Ok();
        }

        [HttpGet("TimeoutException")]
        public ActionResult GetTimeoutException()
        {
            throw new TimeoutException();
            return Ok();
        }

    }
}
