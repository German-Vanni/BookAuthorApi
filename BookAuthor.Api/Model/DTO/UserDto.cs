using System.ComponentModel.DataAnnotations;

namespace BookAuthor.Api.Model.DTO
{
    public class UserDto
    {
        public string Email { get; set; }
    }

    public class UserDtoForCreation
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [StringLength(32, ErrorMessage = "Password length invalid")]
        public string Password { get; set; }

    }

    public class UserDtoForLogin
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

}
