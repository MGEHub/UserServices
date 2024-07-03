using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Users
{
    public class PhoneNumber : ValueObject
    {
        public string CountryCode { get; private set; }
        public string Number { get; private set; }
        public PhoneNumber(string phoneNumber)
        {
            var parsedPhoneNumber = ParsePhoneNumber(phoneNumber);

            CountryCode = parsedPhoneNumber.CountryCode;
            Number = parsedPhoneNumber.Number;
        }
        
        public static (string CountryCode, string Number) ParsePhoneNumber(string phoneNumber)
        {
            ValidatePhoneNumber(phoneNumber);

            string countryCode = phoneNumber.Substring(0, phoneNumber.Length - 9);
            string number = phoneNumber.Substring(phoneNumber.Length - 9);

            countryCode = CorrectCountryCode(countryCode);
            
            return (countryCode, number);
        }

        public override string ToString()
        {
            return CountryCode + Number;
        }

        private static void ValidatePhoneNumber(string phoneNumber)
        {
            bool valid = true;

            if (String.IsNullOrEmpty(phoneNumber))
                valid = false;

            var plusTrimmed = phoneNumber.TrimStart('+');
            if (plusTrimmed.Length < 9 || plusTrimmed.Any(c => !Char.IsDigit(c)))
                valid = false;

            if(!valid) { throw new ArgumentException("Invalid phone number."); }
        }

        private static string CorrectCountryCode(string countryCode)
        {
            if (countryCode.StartsWith("00"))
                countryCode = "+" + countryCode.TrimStart("00".ToCharArray());

            if (String.IsNullOrWhiteSpace(countryCode) || countryCode == "+")
                countryCode = "+46";

            return countryCode;
        }


    }
}
