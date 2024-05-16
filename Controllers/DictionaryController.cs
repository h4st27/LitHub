using Microsoft.AspNetCore.Mvc;
using Libra.Dtos;
using Libra.Models;
using Libra.Models.ResponseModels;
using Libra.Services.ApiClient;
using Libra.Services.DictionaryService;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace Libra.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DictionaryController : ControllerBase
    {
        private readonly IApiClient _apiClient;
        private readonly IConfiguration _configuration;
        private readonly IDictionaryService _dictionaryService;
        public DictionaryController(IApiClient apiClient, IConfiguration configuration, IDictionaryService dictionaryService)
        {
            _apiClient = apiClient;
            _configuration = configuration;
            _dictionaryService = dictionaryService; 
        }
        [HttpGet("{word}"), Authorize]
        public async Task<ActionResult> GetDefinition(string word)
        {
            var wordToDefine = new WordDto() { Word = word };
            var response = new BaseResponse<DictionaryData>();
            try
            {
                var apiData = await _apiClient.GetAsync<DictionaryData>($"https://api.api-ninjas.com/v1/dictionary?word={wordToDefine.Word}&X-Api-Key={_configuration.GetSection("ApiKey").Value}");
                response.Data = apiData;
                response.Message = $"Definition of word '{wordToDefine.Word}'.";
                return Ok(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
        [HttpGet(), Authorize]
        public async Task<ActionResult> GetDictionary()
        {
            var response = new BaseResponse<List<DictionaryData>>();
            try
            {
                response.Data = _dictionaryService.GetDictionary();
                response.Message = $"Your dictionary.";
                return Ok(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
        [HttpPost("{word}"), Authorize]
        public async Task<ActionResult> PostDefinition(string word)
        {
            var wordToDefine = new WordDto() { Word = word };
            var response = new BaseResponse<DictionaryData>();
            try
            {
                var apiData = await _apiClient.GetAsync<DictionaryData>($"https://api.api-ninjas.com/v1/dictionary?word={wordToDefine.Word}&X-Api-Key={_configuration.GetSection("ApiKey").Value}");
                response.Data = apiData;
                if (_dictionaryService.AddDefinitionToWord(wordToDefine.Word, apiData))
                {
                    response.Message = $"Added definition.";
                    return Ok(response);
                }
                response.Message = "Wrong word.";
                return BadRequest(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
        [HttpDelete("{word}"),Authorize]
        public async Task<ActionResult> DeleteDefinition(string word)
        {
            var wordToDefine = new WordDto() { Word = word };
            var response = new BaseResponse<string>();
            response.Data = wordToDefine.Word;
            try
            {
                if (_dictionaryService.RemoveDefinitionFromWord(wordToDefine.Word))
                {
                    response.Message = $"Deleted.";
                    return Ok(response);
                }
                response.Message = "No such word to define.";
                return NotFound(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{word}"), Authorize]
        public async Task<ActionResult> UpdateWordDefinition(string word)
        {
            var wordToDefine = new WordDto() { Word = word };
            var response = new BaseResponse<DictionaryData>() { Data = new() };
            response.Data.Word = wordToDefine.Word;
            try
            {
                var apiData = await _apiClient.GetAsync<DictionaryData>($"https://api.api-ninjas.com/v1/dictionary?word={wordToDefine.Word}&X-Api-Key={_configuration.GetSection("ApiKey").Value}");
                if (_dictionaryService.ChangeDefinitionOfWord(wordToDefine.Word, apiData.Definition))
                {
                    response.Data.Definition = apiData.Definition;
                    response.Data.Valid = apiData.Valid;
                    response.Message = $"Changed.";
                    return Ok(response);
                }
                response.Message = "No such word to define.";
                return NotFound(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}
