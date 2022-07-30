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
            string text = value as string;
            return !text.Any(c => char.IsDigit(c));
        }

    }
}
