using System.ComponentModel.DataAnnotations;

namespace MyApp.DTOs
{
    public class WordDTO
    {
        [Required]
        public string Word { get; set; }
    }
}
