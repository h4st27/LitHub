using System.ComponentModel.DataAnnotations;

namespace MyApp.DTOs
{
    public class BookDTO
    {
        [Required]
        public string Book { get; set; }
    }
}
