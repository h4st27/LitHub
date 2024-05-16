using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Libra.Dtos
{
    public class WordDto
    {
        private string _word;
        [Required(ErrorMessage = "Word is required")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Word must contain only alphabetic characters")]
        [DefaultValue("word")]
        public string Word
        {
            get => _word;
            set => _word = value?.Trim().ToLower();
        }
    }
}
