using Microsoft.AspNetCore.Mvc;
using MyApp.DTOs;
using MyApp.Models;
using MyApp.Models.ResponseModels;
using MyApp.Services.ApiClient;
using MyApp.Services.BooksService;
using System.Net;

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

        [HttpGet("TypesTo")]
        public async Task<ActionResult> GetTypes(string bookToFind)
        {
            var response = new BaseResponse<TypeData>();
            var book = new BookDTO();
            book.Book = bookToFind;
            try
            {
                TypeData? apiData = await _apiClient.GetAsync<TypeData>($"https://api.api-ninjas.com/v1/thesaurus?word={book.Book}&X-Api-Key={_configuration.GetSection("ApiKey").Value}");
                response.Data = apiData;
                response.Message = $"Retrieved author of book '{book.Book}'.";
                return Ok(response);
            }
            catch {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet("Random")]
        public async Task<ActionResult> GetRandomBook()
        {
            try
            {
                var response = new BaseResponse<string>();
                var book = _booksService.GetRandomBook();
                response.Data = book;
                response.Message = $"Your book is '{book}'.";
                return Ok(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet()]
        public async Task<ActionResult> GetBook()
        {
            try
            {
                var response = new BaseResponse<HashSet<string>>();
                var books = _booksService.RetrieveBooks();
                response.Data = books;
                response.Message = $"Books retrieved.";
                return Ok(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPost()]
        public async Task<ActionResult> PostWord([FromBody] BookDTO book)
        {
            try
            {
                var response = new BaseResponse<string>();
                response.Data = book.Book;
                if (_booksService.AddBook(book.Book))
                {
                    response.Message = "The book is successfully added.";
                    return Ok(response);
                }
                response.Message = "The book already exists.";
                return BadRequest(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{bookToFind}")]
        public async Task<ActionResult> DeleteBook(string bookToFind)
        {
            var book = new BookDTO();
            book.Book = bookToFind;
            try
            {
                var response = new BaseResponse<string>();
                response.Data = book.Book;
                if (_booksService.RemoveBook(book.Book))
                {
                    response.Message = $"The book '{book.Book}' is successfully deleted.";
                    response.Data = book.Book;
                    return (Ok(response));
                }
                response.Message = $"There is no '{book.Book}' in list.";
                response.Data = book.Book;
                return NotFound(response);

            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{bookToFind}")]
        public async Task<ActionResult> ChangeBook(string bookToFind, [FromBody] BookDTO book)
        {
            var oldBook = new BookDTO();
            oldBook.Book = bookToFind;
            try
            {
                var response = new BaseResponse<string>();
                response.Data = book.Book;
                if (_booksService.ReplaceBook(oldBook.Book, book.Book))
                {
                    response.Message = $"The book '{oldBook.Book}' is successfully changed to {book.Book}.";
                    return Ok(response);
                }
                response.Message = $"There is no '{oldBook.Book}' in list.";
                response.Data = oldBook.Book;
                return NotFound(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}
