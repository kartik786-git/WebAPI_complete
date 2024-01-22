using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common;
using WebAPI.VeiwModel;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericModelValidatorController : ControllerBase
    {
        [HttpPost("validate")]
        public IActionResult ValidateModel([FromBody] MyValidatorModel model)
        {
            var errors = ModelValidator.Validate(model,
                skipProperties: new[] { "Name" },
                emailProperties: new[] {"Email", "OneNestedValidatorModel.Email" } );
            if (errors.Count > 0)
            {
                return BadRequest(errors);
            }

            return Ok("Model is valid.");
        }
    }
}
