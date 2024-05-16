using Microsoft.AspNetCore.Mvc;
using Libra.Dtos;
using Libra.Models;
using Libra.Models.ResponseModels;
using Libra.Services.ApiClient;
using Libra.Services.WordsService;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Libra.Controllers
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

        [HttpGet("HomonymTo"), Authorize]
        public async Task<ActionResult> GetHomonyms(string wordToFind)
        {
            var response = new BaseResponse<HomonymsData>();
            var word = new WordDto();
            word.Word = wordToFind;
            try
            {
                HomonymsData? apiData = await _apiClient.GetAsync<HomonymsData>($"https://api.api-ninjas.com/v1/thesaurus?word={word.Word}&X-Api-Key={_configuration.GetSection("ApiKey").Value}");
                response.Data = apiData;
                response.Message = $"Retrieved homonyms of word '{word.Word}'.";
                return Ok(response);
            }
            catch {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet("Random"), Authorize]
        public async Task<ActionResult> GetRandomWord()
        {
            try
            {
                var response = new BaseResponse<string>();
                var word = _wordsService.GetRandomWord();
                response.Data = word;
                response.Message = $"Your word is '{word}'.";
                return Ok(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet(), Authorize]
        public async Task<ActionResult> GetWords()
        {
            try
            {
                var response = new BaseResponse<HashSet<string>>();
                var words = _wordsService.RetrieveWords();
                response.Data = words;
                response.Message = $"Words retrieved.";
                return Ok(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPost(), Authorize]
        public async Task<ActionResult> PostWord([FromBody] WordDto word)
        {
            try
            {
                var response = new BaseResponse<string>();
                response.Data = word.Word;
                if (_wordsService.AddWord(word.Word))
                {
                    response.Message = "The word is successfully added.";
                    return Ok(response);
                }
                response.Message = "The word already exists.";
                return BadRequest(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{wordToFind}"), Authorize]
        public async Task<ActionResult> DeleteWord(string wordToFind)
        {
            var word = new WordDto();
            word.Word = wordToFind;
            try
            {
                var response = new BaseResponse<string>();
                response.Data = word.Word;
                if (_wordsService.RemoveWord(word.Word))
                {
                    response.Message = $"The word '{word.Word}' is successfully deleted.";
                    response.Data = word.Word;
                    return (Ok(response));
                }
                response.Message = $"There is no '{word.Word}' in list.";
                response.Data = word.Word;
                return NotFound(response);

            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{wordToFind}"), Authorize]
        public async Task<ActionResult> ChangeWord(string wordToFind, [FromBody] WordDto word)
        {
            var oldWord = new WordDto();
            oldWord.Word = wordToFind;
            try
            {
                var response = new BaseResponse<string>();
                response.Data = word.Word;
                if (_wordsService.ReplaceWord(oldWord.Word, word.Word))
                {
                    response.Message = $"The word '{oldWord.Word}' is successfully changed to {word.Word}.";
                    return Ok(response);
                }
                response.Message = $"There is no '{oldWord.Word}' in list.";
                response.Data = oldWord.Word;
                return NotFound(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}
