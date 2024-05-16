using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MyApp.DTOs
{
    public class DefinitionDto
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
        [Required(ErrorMessage = "Valid is required")]
        [DefaultValue(false)]
        public bool Valid { get; set; }
        [Required(ErrorMessage = "Definition is required")]
        [DefaultValue("definition")]
        public string Definition {  get; set; }
    }
}
