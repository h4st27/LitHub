using Microsoft.AspNetCore.Mvc;
using Libra.Dtos;
using Libra.Models;
using Libra.Models.ResponseModels;
using Libra.Services.ApiClient;
using Libra.Services.JokesService;
using Libra.Services.WordsService;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace Libra.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public class JokesController : ControllerBase
    {
        private readonly IApiClient _apiClient;
        private readonly IConfiguration _configuration;
        private readonly IJokesService _jokesService;
        public JokesController(IApiClient apiClient, IConfiguration configuration, IJokesService jokesService)
        {
            _apiClient = apiClient;
            _configuration = configuration;
            _jokesService= jokesService;
        }

        [HttpGet("Random"), Authorize]
        public async Task<ActionResult> GetRandomJoke()
        {
            var response = new BaseResponse<JokeData>();
            var joke = new JokeDto();
            try
            {
                var apiData = await _apiClient.GetAsync<JokeData>($"https://api.api-ninjas.com/v1/chucknorris?&X-Api-Key={_configuration.GetSection("ApiKey").Value}");
                response.Data = apiData;
                response.Message = $"Your joke is '{joke.Joke}'.";
                return Ok(response);
            }
            catch {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult> GetJokeById(int id)
        {
            try
            {
                var response = new BaseResponse<string>();
                var joke = _jokesService.GetJokeById(id);
                response.Data = joke;
                if (joke==null)
                {
                    response.Message = $"There is no such joke in list.";
                    return NotFound(response);
                }
                response.Message = $"Your joke is '{joke}'.";
                return Ok(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet(), Authorize]
        public async Task<ActionResult> GetJokes()
        {
            try
            {
                var response = new BaseResponse<HashSet<string>>();
                var jokes = _jokesService.RetrieveJokes();
                response.Data = jokes;
                response.Message = $"Jokes retrieved.";
                return Ok(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPost(), Authorize]
        public async Task<ActionResult> PostWord([FromBody] JokeDto joke)
        {
            try
            {
                var response = new BaseResponse<string>();
                response.Data = joke.Joke;
                if (_jokesService.AddJoke(joke.Joke))
                {
                    response.Message = "The joke is successfully added.";
                    return Ok(response);
                }
                response.Message = "The joke already exists.";
                return BadRequest(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}"), Authorize]
        public async Task<ActionResult> DeleteWord(int id)
        {
            var joke = new JokeDto();
            try
            {
                var response = new BaseResponse<string>();
                joke.Joke = _jokesService.GetJokeById(id);
                if (_jokesService.RemoveJoke(id))
                {
                    response.Message = $"The joke is successfully deleted.";
                    response.Data = joke.Joke;
                    return (Ok(response));
                }
                response.Message = $"There is no such joke in list.";
                response.Data = joke.Joke;
                return NotFound(response);

            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}"), Authorize]
        public async Task<ActionResult> ChangeWord(int id, [FromBody] JokeDto joke)
        {
            try
            {
                var response = new BaseResponse<string>();
                response.Data = joke.Joke;
                if (_jokesService.ReplaceJoke(id, joke.Joke))
                {
                    response.Message = $"The joke with id '{id}' is successfully changed to {joke.Joke}.";
                    return Ok(response);
                }
                response.Message = $"There is no such joke in list or already exists.";
                return NotFound(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}
