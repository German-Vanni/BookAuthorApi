using System.ComponentModel.DataAnnotations;

namespace BookAuthor.Api.Util.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class ISBNAttribute : ValidationAttribute
    {
        private ISBNType _type;
        public ISBNAttribute(ISBNType type)
        {
            _type = type;
            ErrorMessage = $"Invalid {_type}";
        }

        public override bool IsValid(object value)
        {
            string isbn = (value as string).ToUpper();
            if (_type == ISBNType.ISBN10 && isbn.Length != 10 || _type == ISBNType.ISBN13 && isbn.Length != 13)
            {
                ErrorMessage = "Invalid length";
                return false;
            }
            char lastChar = isbn.ElementAt(isbn.Length - 1);
            string firstPart = isbn.Remove(isbn.Length - 1, 1);

            if (!firstPart.All(c => char.IsDigit(c))) return false;

            int sum = 0;
            if(_type == ISBNType.ISBN10)
            {
                var reversed = firstPart.Reverse().ToList();
                for (int i = 0; i< 9; i++)
                {
                    sum += int.Parse(reversed[i].ToString())* (2 + i); 
                }
                int remainder = sum % 11;
                int check = 11 - remainder;
                char checkChar;
                if (check == 10) checkChar = 'X';
                else
                {
                    checkChar = (char)('0' + check);
                }
                if (checkChar == lastChar) return true;
            }
            if (_type == ISBNType.ISBN13)
            {
                for (int i = 0; i < 12; i++)
                {
                    sum += int.Parse(isbn[i].ToString()) * ((i % 2) * 2 + 1);
                }
                int remainder = sum % 10;
                int check = 10 - remainder;
                if (check == 10) check = 0;
                char checkChar = (char)('0' + check);
                if (checkChar == lastChar) return true;
            }
            return false;
        }
    }

    public enum ISBNType
    {
        ISBN10 = 10,
        ISBN13 = 13,
    }
}
