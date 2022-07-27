using Microsoft.AspNetCore.Identity;

namespace BookAuthor.Api.Model
{
    public class ApiUser : IdentityUser
    {
        //So if i need to add fields to this class there will be no need to refactor dbcontext
        public ICollection<BookScore> Ratings { get; set; }
    }
}
