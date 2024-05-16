using Microsoft.AspNetCore.Mvc;
using MyApp.DTOs;
using MyApp.Models;
using MyApp.Models.ResponseModels;
using MyApp.Services.ApiClient;
using MyApp.Services.BooksService;
using System.Net;
using System.Text.RegularExpressions;

namespace MyApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IApiClient _apiClient;
        private readonly IConfiguration _configuration;
        private readonly IBooksService _booksService;
        public BooksController(IApiClient apiClient, IConfiguration configuration, IBooksService booksService )
        {
            _apiClient = apiClient;
            _configuration = configuration;
            _booksService= booksService; 
        }

        //API отримання омонімів певної книги (Дані отримані з іншого API)
        [HttpGet("HomonymTo")]
        public async Task<ActionResult> GetHomonyms([FromQuery] string book)
        {
            var response = new BaseResponse<HomonymsData>();
            try
            {
                if (!_booksService.ValidateBook(book))
                {
                    response.Message = $"Wrong validation of book {book.Trim().ToLower()}";
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return StatusCode((int)response.StatusCode, response);
                }
                HomonymsData? apiData = await _apiClient.GetAsync<HomonymsData>($"https://api.api-ninjas.com/v1/thesaurus?word={book.Trim().ToLower()}&X-Api-Key={_configuration.GetSection("ApiKey").Value}");
                response.Data = apiData;
                response.Message = $"Retrieved homonyms of book {book.Trim().ToLower()}";
                response.StatusCode = HttpStatusCode.OK;
                return StatusCode((int)response.StatusCode, response);
            }
            catch {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
        //API отримання рандомного книги зі статичного списку (Дані із серверу)
        [HttpGet("Random")]
        public async Task<ActionResult> GetRandom()
        {
            try
            {
                var response = new BaseResponse<string>();
                var book = _booksService.GetRandomBook();
                response.StatusCode = HttpStatusCode.OK;
                response.Data = book;
                response.Message = $"Your word is {book.Trim().ToLower()}";
                return StatusCode((int)response.StatusCode, response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
        //API отримання книг статичного списку (Дані із серверу)
        [HttpGet()]
        public async Task<ActionResult> Get()
        {
            try
            {
                var response = new BaseResponse<List<string>>();
                var books = _booksService.RetrieveBooks();
                response.StatusCode = HttpStatusCode.OK;
                response.Data = books;
                response.Message = $"OK";
                return StatusCode((int)response.StatusCode, response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
        //API додання слова до статичного списку (Дані із серверу)
        [HttpPost()]
        public async Task<ActionResult> PostBook([FromBody] BookDTO wordDTO)
        {
            try
            {
                var response = new BaseResponse<string>();
                if (!_booksService.ValidateBook(wordDTO.Book))
                {
                    response.Message = "Invalid data";
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }
                if (_booksService.AddBook(wordDTO.Book))
                {
                    response.Message = "Data received successfully";
                    response.StatusCode = HttpStatusCode.OK;
                    return Ok(response);
                }
                response.Message = "The word already exists.";
                response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
        //API додання видалення статичного списку (Дані із серверу)
        [HttpDelete("{word}")]
        public async Task<ActionResult> DeleteBook(string book)
        {
            try
            {
                var response = new BaseResponse<string>();
                if (!_booksService.ValidateBook(book))
                {
                    response.Message = "Invalid data";
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }
                if (_booksService.RemoveBook(book))
                {
                    response.Message = $"Word '{book.Trim().ToLower()}' was deleted successfully.";
                    response.StatusCode = HttpStatusCode.OK;
                    return Ok(response);
                }
                response.Message = $"There is no '{book.Trim().ToLower()}' in list.";
                response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
        //API зміни елементу зі статичного списку (Дані із серверу)
        [HttpPut("{oldWord}")]
        public IActionResult ChangeWord(string oldWord, [FromBody] BookDTO wordDTO)
        {
            try
            {
                var response = new BaseResponse<string>();
                if (!_booksService.ValidateBook(wordDTO.Book))
                {
                    response.Message = "Invalid data";
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }
                if (_booksService.ReplaceBook(oldWord, wordDTO.Book))
                {
                    response.Message = $"Word '{oldWord.Trim().ToLower()}' changed to '{wordDTO.Book.Trim().ToLower()}' successfully.";
                    response.StatusCode = HttpStatusCode.OK;
                    return Ok(response);
                }
                response.Message = $"There is no '{oldWord.Trim().ToLower()}' in list.";
                response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}
