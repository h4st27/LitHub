using System.ComponentModel.DataAnnotations;

namespace Libra.Dtos
{
    public class AuthPayloadDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
