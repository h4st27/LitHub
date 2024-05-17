using Microsoft.AspNetCore.Mvc;
using LitHub.DTOs;
using LitHub.Models;
using LitHub.Models.ResponseModels;
using LitHub.Services.ApiClient;
using LitHub.Services.MangasService;
using LitHub.Services.BooksService;
using System.Net;

namespace LitHub.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MangasController : ControllerBase
    {
        private readonly IApiClient _apiClient;
        private readonly IConfiguration _configuration;
        private readonly IMangasService _mangasService;
        public MangasController(IApiClient apiClient, IConfiguration configuration, IMangasService mangasService)
        {
            _apiClient = apiClient;
            _configuration = configuration;
            _mangasService= mangasService;
        }

        [HttpGet("Random")]
        public async Task<ActionResult> GetRandomManga()
        {
            var response = new BaseResponse<MangaData>();
            var manga = new MangaDto();
            try
            {
                var apiData = await _apiClient.GetAsync<MangaData>($"https://api.api-ninjas.com/v1/chucknorris?&X-Api-Key={_configuration.GetSection("ApiKey").Value}");
                response.Data = apiData;
                response.Message = $"Your manga is '{manga.Manga}'.";
                return Ok(response);
            }
            catch {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetMangaById(int id)
        {
            try
            {
                var response = new BaseResponse<string>();
                var manga = _mangasService.GetMangaById(id);
                response.Data = manga;
                if (manga==null)
                {
                    response.Message = $"There is no such manga in list.";
                    return NotFound(response);
                }
                response.Message = $"Your manga is '{manga}'.";
                return Ok(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet()]
        public async Task<ActionResult> GetMangas()
        {
            try
            {
                var response = new BaseResponse<HashSet<string>>();
                var mangas = _mangasService.RetrieveMangas();
                response.Data = mangas;
                response.Message = $"Mangas retrieved.";
                return Ok(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPost()]
        public async Task<ActionResult> PostBook([FromBody] MangaDto manga)
        {
            try
            {
                var response = new BaseResponse<string>();
                response.Data = manga.Manga;
                if (_mangasService.AddManga(manga.Manga))
                {
                    response.Message = "The manga is successfully added.";
                    return Ok(response);
                }
                response.Message = "The manga already exists.";
                return BadRequest(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            var manga = new MangaDto();
            try
            {
                var response = new BaseResponse<string>();
                manga.Manga = _mangasService.GetMangaById(id);
                if (_mangasService.RemoveManga(id))
                {
                    response.Message = $"The manga is successfully deleted.";
                    response.Data = manga.Manga;
                    return (Ok(response));
                }
                response.Message = $"There is no such manga in list.";
                response.Data = manga.Manga;
                return NotFound(response);

            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> ChangeBook(int id, [FromBody] MangaDto manga)
        {
            try
            {
                var response = new BaseResponse<string>();
                response.Data = manga.Manga;
                if (_mangasService.ReplaceManga(id, manga.Manga))
                {
                    response.Message = $"The manga with id '{id}' is successfully changed to {manga.Manga}.";
                    return Ok(response);
                }
                response.Message = $"There is no such manga in list or already exists.";
                return NotFound(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}
