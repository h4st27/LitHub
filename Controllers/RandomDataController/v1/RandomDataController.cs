using DocumentFormat.OpenXml.Spreadsheet;
using Libra.Models;
using Libra.Models.ResponseModels;
using Libra.Services.RandomDataService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Libra.Controllers.RandomDataController.v1
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1.1")]
    [Obsolete("This version is deprecated.")]
    public class RandomDataController : Controller
    {
        private readonly IRandomDataService _service;

        public RandomDataController(IRandomDataService service)
        {
            _service = service;
        }
        [HttpGet]
        [Authorize]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetRandomData()
        {
            try
            {
                var response = new BaseResponse<int>();
                int intValue = _service.GetIntegerValue();
                response.Data = intValue;
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
