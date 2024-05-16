using Libra.Models.ResponseModels;
using Libra.Services.RandomDataService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Libra.Controllers.RandomDataController.v2
{
    [ApiVersion("2.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1.2")]
    public class RandomDataController : Controller
    {
        private readonly IRandomDataService _service;

        public RandomDataController(IRandomDataService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        [MapToApiVersion("2")]
        public async Task<IActionResult> GetRandomData()
        {
            try
            {
                var response = new BaseResponse<string>();
                string text = _service.GetTextValue();
                response.Data = text;
                response.Message = "Success";
                return Ok(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}
