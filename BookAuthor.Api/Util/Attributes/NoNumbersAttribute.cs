using System.ComponentModel.DataAnnotations;

namespace BookAuthor.Api.Util.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class NoNumbersAttribute : ValidationAttribute
    {
        public NoNumbersAttribute()
        {
            ErrorMessage = "Numbers are not allowed";
        }

        public override bool IsValid(object value)
        {
            if (value is null)
            {
                //Required attribute should be used to ensure that value is not null
                return true;
            }
            string text = value as string;
            return !text.Any(c => char.IsDigit(c));
        }

    }
}
