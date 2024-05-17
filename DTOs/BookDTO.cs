using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyApp.DTOs
{
    public class BookDTO
    {
        private string _book;
        [Required(ErrorMessage = "Book is required")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Book must contain only alphabetic characters")]
        [DefaultValue("book")]
        public string Book
        {
            get => _book;
            set => _book = value?.Trim().ToLower();
        }
    }
}
