using System.ComponentModel.DataAnnotations;

namespace BookAuthor.Api.Model.DTO
{
    public class RateBookDto
    {
        [Required]
        [Range(1, 5)]
        public int Score { get; set; }
    }
}
