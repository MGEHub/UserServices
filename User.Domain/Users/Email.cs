using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Users
{
    public class Email : ValueObject
    {
        public string Address { get; private set; }
        public Email(string address)
        {
            ValidateAddress(address);

            Address = address;
        }

        private void ValidateAddress(string address)
        {
            if (String.IsNullOrEmpty(address) || address.Length > 256 || !IsAddressValid(address))
            {
                throw new ArgumentException("Invalid email address.");
            }
        }

        public static bool IsAddressValid(string address)
        {
            try
            {
                var addr = new MailAddress(address);
                return addr.Address == address;
            }
            catch
            {
                return false;
            }
        }

    }
}
