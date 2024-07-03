using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Users
{
    public class User
    {
        public User(Guid id, Email email, PhoneNumber phoneNumber, string clearTextPassword,
            IUserHashingService hashingService, 
            IUserMessageService messageService)
        {
            Id = id;
            Email = email;
            PhoneNumber = phoneNumber;
            Password = clearTextPassword != null ? new Password((id, clearTextPassword), hashingService) : null;

            messageService.Send(email, phoneNumber, Password);
        }
        public Guid Id { get; private set; }
        public Email Email { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public Password Password { get; private set; }
    }
}
