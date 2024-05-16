using Libra.Models.ResponseModels;
using Libra.Services.RandomDataService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;

namespace Libra.Controllers.RandomDataController.v3
{
    [ApiVersion("3.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1.3")]
    public class RandomDataController : Controller
    {
        private readonly IRandomDataService _service;

        public RandomDataController(IRandomDataService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        [MapToApiVersion("3")]
        public async Task<IActionResult> GetRandomData()
        {
            try
            {
                byte[] fileBytes = _service.GenerateExcelFile();
                string contentType = "application/octet-stream";
                return File(fileBytes, contentType, "generated_file.xlsx");
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}
