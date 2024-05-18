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

        //API ��������� ������� ����� ����� (���� �������� � ������ API)
        [HttpGet("HomonymInBook")]
        public async Task<ActionResult> GetHomonyms([FromQuery] string book)
        {
            var response = new BaseResponse<BookHomonymsData>();
            try
            {
                if (!_booksService.ValidateBook(book))
                {
                    response.Message = $"Wrong validation of book {book.Trim().ToLower()}";
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return StatusCode((int)response.StatusCode, response);
                }
                BookHomonymsData? apiData = await _apiClient.GetAsync<BookHomonymsData>($"https://api.api-ninjas.com/v1/thesaurus?word={book.Trim().ToLower()}&X-Api-Key={_configuration.GetSection("ApiKey").Value}");
                response.Data = apiData;
                response.Message = $"Retrieved homonyms in book {book.Trim().ToLower()}";
                response.StatusCode = HttpStatusCode.OK;
                return StatusCode((int)response.StatusCode, response);
            }
            catch {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
        //API ��������� ���������� ����� � ���������� ������ (���� �� �������)
        [HttpGet("Random")]
        public async Task<ActionResult> GetRandom()
        {
            try
            {
                var response = new BaseResponse<string>();
                var book = _booksService.GetRandomBook();
                response.StatusCode = HttpStatusCode.OK;
                response.Data = book;
                response.Message = $"Your book is {book.Trim().ToLower()}";
                return StatusCode((int)response.StatusCode, response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
        //API ��������� ���� ���������� ������ (���� �� �������)
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
        //API ������� ����� �� ���������� ������ (���� �� �������)
        [HttpPost()]
        public async Task<ActionResult> PostBook([FromBody] BookDTO bookDTO)
        {
            try
            {
                var response = new BaseResponse<string>();
                if (!_booksService.ValidateBook(bookDTO.Book))
                {
                    response.Message = "Invalid data";
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }
                if (_booksService.AddBook(bookDTO.Book))
                {
                    response.Message = "Data received successfully";
                    response.StatusCode = HttpStatusCode.OK;
                    return Ok(response);
                }
                response.Message = "The book already exists.";
                response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
        //API ������� ��������� ���������� ������ (���� �� �������)
        [HttpDelete("{book}")]
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
                    response.Message = $"Book '{book.Trim().ToLower()}' was deleted successfully.";
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
        //API ���� �������� � ���������� ������ (���� �� �������)
        [HttpPut("{oldBook}")]
        public IActionResult ChangeBook(string oldBook, [FromBody] BookDTO bookDTO)
        {
            try
            {
                var response = new BaseResponse<string>();
                if (!_booksService.ValidateBook(bookDTO.Book))
                {
                    response.Message = "Invalid data";
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }
                if (_booksService.ReplaceBook(oldBook, bookDTO.Book))
                {
                    response.Message = $"Book '{oldBook.Trim().ToLower()}' changed to '{bookDTO.Book.Trim().ToLower()}' successfully.";
                    response.StatusCode = HttpStatusCode.OK;
                    return Ok(response);
                }
                response.Message = $"There is no '{oldBook.Trim().ToLower()}' in list.";
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
