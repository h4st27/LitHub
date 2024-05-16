using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Libra.Dtos
{
    public class JokeDto
    {
        private string _joke;
        [Required(ErrorMessage = "Joke is required")]
        [DefaultValue("I love baNaNas")]
        public string Joke
        {
            get => _joke;
            set => _joke = value?.Trim();
        }
    }
}
