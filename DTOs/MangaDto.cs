using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LitHub.DTOs
{
    public class MangaDto
    {
        private string _manga;
        [Required(ErrorMessage = "Manga is required")]
        [DefaultValue("Shingeki no Kyojin")]
        public string Manga
        {
            get => _manga;
            set => _manga = value?.Trim();
        }
    }
}
