using System.ComponentModel.DataAnnotations;

namespace BookAuthor.Api.Util.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class DateBeforeNowAttribute : ValidationAttribute
    {
        public DateBeforeNowAttribute()
        {
            ErrorMessage = "Future dates are not allowed";
        }

        public override bool IsValid(object value)
        {
            if (value is null)
            {
                //Required attribute should be used to ensure that value is not null
                return true;
            }

            DateTime dateValue = (DateTime)value; 
            return dateValue < DateTime.Today; 
        }

    }
}
