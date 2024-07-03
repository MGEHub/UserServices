using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace User.Domain.Users
{
    public class Password : ValueObject
    {
        public string Hashed { get; private set; }
        public Password((Guid id, string clearText) password, 
            IUserHashingService userHashingService)
        {
            ValidateClearText(password.clearText);

            Hashed = userHashingService.Hash(password.id, password.clearText);
        }

        private void ValidateClearText(string clearText)
        {
            if (String.IsNullOrWhiteSpace(clearText) || clearText.Length < 8)
            {
                throw new ArgumentException("Password must be at least 8 characters long.");
            }

            if (!Regex.IsMatch(clearText, @"[a-z]"))
            {
                throw new ArgumentException("Password must contain at least one lowercase character.");
            }

            if (!Regex.IsMatch(clearText, @"[A-Z]"))
            {
                throw new ArgumentException("Password must contain at least one uppercase character.");
            }

            if (!Regex.IsMatch(clearText, @"\d.*\d"))
            {
                throw new ArgumentException("Password must contain at least two digits.");
            }

            if (!Regex.IsMatch(clearText, @"[!@#$%^&*(),.?\"":{ }|<>]"))
            {
                throw new ArgumentException("Password must contain at least one symbol.");
            }
        }


    }
}
