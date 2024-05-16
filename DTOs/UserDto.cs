using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Security.Cryptography;
using System.ComponentModel;

namespace Libra.Dtos
{
    public class UserDto
    {
        [Required]
        [StringLength(15)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(15)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DefaultValue("2004-03-23")]
        public DateTime DateOfBirth { get; set; }

        private string _password;

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                _password = HashPassword(value);
            }
        }
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
