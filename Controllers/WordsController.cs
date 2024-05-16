using Microsoft.AspNetCore.Mvc;
using MyApp.DTOs;
using MyApp.Models;
using MyApp.Models.ResponseModels;
using MyApp.Services.ApiClient;
using MyApp.Services.WordsService;
using System.Net;
using System.Text.RegularExpressions;

namespace MyApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WordsController : ControllerBase
    {
        private readonly IApiClient _apiClient;
        private readonly IConfiguration _configuration;
        private readonly IWordsService _wordsService;
        public WordsController(IApiClient apiClient, IConfiguration configuration, IWordsService wordsService )
        {
            _apiClient = apiClient;
            _configuration = configuration;
            _wordsService= wordsService; 
        }

        //API отримання омонімів певного слова (Дані отримані з іншого API)
        [HttpGet("HomonymTo")]
        public async Task<ActionResult> GetHomonyms([FromQuery] string word)
        {
            var response = new BaseResponse<HomonymsData>();
            try
            {
                if (!_wordsService.ValidateWord(word))
                {
                    response.Message = $"Wrong validation of word {word.Trim().ToLower()}";
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return StatusCode((int)response.StatusCode, response);
                }
                HomonymsData? apiData = await _apiClient.GetAsync<HomonymsData>($"https://api.api-ninjas.com/v1/thesaurus?word={word.Trim().ToLower()}&X-Api-Key={_configuration.GetSection("ApiKey").Value}");
                response.Data = apiData;
                response.Message = $"Retrieved homonyms of word {word.Trim().ToLower()}";
                response.StatusCode = HttpStatusCode.OK;
                return StatusCode((int)response.StatusCode, response);
            }
            catch {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
        //API отримання рандомного слова зі статичного списку (Дані із серверу)
        [HttpGet("Random")]
        public async Task<ActionResult> GetRandom()
        {
            try
            {
                var response = new BaseResponse<string>();
                var word = _wordsService.GetRandomWord();
                response.StatusCode = HttpStatusCode.OK;
                response.Data = word;
                response.Message = $"Your word is {word.Trim().ToLower()}";
                return StatusCode((int)response.StatusCode, response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
        //API отримання слів статичного списку (Дані із серверу)
        [HttpGet()]
        public async Task<ActionResult> Get()
        {
            try
            {
                var response = new BaseResponse<List<string>>();
                var words = _wordsService.RetrieveWords();
                response.StatusCode = HttpStatusCode.OK;
                response.Data = words;
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
        public async Task<ActionResult> PostWord([FromBody] WordDTO wordDTO)
        {
            try
            {
                var response = new BaseResponse<string>();
                if (!_wordsService.ValidateWord(wordDTO.Word))
                {
                    response.Message = "Invalid data";
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }
                if (_wordsService.AddWord(wordDTO.Word))
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
        public async Task<ActionResult> DeleteWord(string word)
        {
            try
            {
                var response = new BaseResponse<string>();
                if (!_wordsService.ValidateWord(word))
                {
                    response.Message = "Invalid data";
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }
                if (_wordsService.RemoveWord(word))
                {
                    response.Message = $"Word '{word.Trim().ToLower()}' was deleted successfully.";
                    response.StatusCode = HttpStatusCode.OK;
                    return Ok(response);
                }
                response.Message = $"There is no '{word.Trim().ToLower()}' in list.";
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
        public IActionResult ChangeWord(string oldWord, [FromBody] WordDTO wordDTO)
        {
            try
            {
                var response = new BaseResponse<string>();
                if (!_wordsService.ValidateWord(wordDTO.Word))
                {
                    response.Message = "Invalid data";
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }
                if (_wordsService.ReplaceWord(oldWord, wordDTO.Word))
                {
                    response.Message = $"Word '{oldWord.Trim().ToLower()}' changed to '{wordDTO.Word.Trim().ToLower()}' successfully.";
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
