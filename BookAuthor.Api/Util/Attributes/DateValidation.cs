using System.ComponentModel.DataAnnotations;

namespace BookAuthor.Api.Util.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class DateBeforeNowAttribute : ValidationAttribute
    {
        bool _allowNull;
        public DateBeforeNowAttribute(bool allowNull = false)
        {
            _allowNull = allowNull;
            ErrorMessage = "Future dates are not allowed";
        }

        public override bool IsValid(object value)
        {
            if (value is null)
            {
                return _allowNull;
            }

            DateTime dateValue = (DateTime)value; 
            return dateValue < DateTime.Today; 
        }

    }
}
