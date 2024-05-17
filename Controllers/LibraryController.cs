using Microsoft.AspNetCore.Mvc;
using LitHub.DTOs;
using LitHub.Models;
using LitHub.Models.ResponseModels;
using LitHub.Services.ApiClient;
using LitHub.Services.LibraryService;
using System.Net;

namespace LitHub.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LibraryController : ControllerBase
    {
        private readonly IApiClient _apiClient;
        private readonly IConfiguration _configuration;
        private readonly ILibraryService _libraryService;
        public LibraryController(IApiClient apiClient, IConfiguration configuration, ILibraryService libraryService)
        {
            _apiClient = apiClient;
            _configuration = configuration;
            _libraryService = libraryService; 
        }
        [HttpGet("{book}")]
        public async Task<ActionResult> GetDefinition(string book)
        {
            var bookToDefine = new BookDTO() { Book = book };
            var response = new BaseResponse<LibraryData>();
            try
            {
                var apiData = await _apiClient.GetAsync<LibraryData>($"https://api.api-ninjas.com/v1/dictionary?word={bookToDefine.Book}&X-Api-Key={_configuration.GetSection("ApiKey").Value}");
                response.Data = apiData;
                response.Message = $"Definition of book '{bookToDefine.Book}'.";
                return Ok(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
        [HttpGet()]
        public async Task<ActionResult> GetDictionary()
        {
            var response = new BaseResponse<List<LibraryData>>();
            try
            {
                response.Data = _libraryService.GetLibrary();
                response.Message = $"Your dictionary.";
                return Ok(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
        [HttpPost("{book}")]
        public async Task<ActionResult> PostDefinition(string book)
        {
            var bookToDefine = new BookDTO() { Book = book };
            var response = new BaseResponse<LibraryData>();
            try
            {
                var apiData = await _apiClient.GetAsync<LibraryData>($"https://api.api-ninjas.com/v1/dictionary?word={bookToDefine.Book}&X-Api-Key={_configuration.GetSection("ApiKey").Value}");
                response.Data = apiData;
                if (_libraryService.AddDefinitionToBook(bookToDefine.Book, apiData))
                {
                    response.Message = $"Added definition.";
                    return Ok(response);
                }
                response.Message = "Wrong book.";
                return BadRequest(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
        [HttpDelete("{book}")]
        public async Task<ActionResult> DeleteDefinition(string book)
        {
            var bookToDefine = new BookDTO() { Book = book };
            var response = new BaseResponse<string>();
            response.Data = bookToDefine.Book;
            try
            {
                if (_libraryService.RemoveDefinitionFromBook(bookToDefine.Book))
                {
                    response.Message = $"Deleted.";
                    return Ok(response);
                }
                response.Message = "No such book to define.";
                return NotFound(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{book}")]
        public async Task<ActionResult> UpdateBookDefinition(string book)
        {
            var bookToDefine = new BookDTO() { Book = book };
            var response = new BaseResponse<LibraryData>() { Data = new() };
            response.Data.Book = bookToDefine.Book;
            try
            {
                var apiData = await _apiClient.GetAsync<LibraryData>($"https://api.api-ninjas.com/v1/dictionary?word={bookToDefine.Book}&X-Api-Key={_configuration.GetSection("ApiKey").Value}");
                if (_libraryService.ChangeDefinitionOfBook(bookToDefine.Book, apiData.Definition))
                {
                    response.Data.Definition = apiData.Definition;
                    response.Data.Valid = apiData.Valid;
                    response.Message = $"Changed.";
                    return Ok(response);
                }
                response.Message = "No such book to define.";
                return NotFound(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}
