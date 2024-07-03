using User.Core.Users;
using User.Domain.Users;

namespace User.Tests.Users
{
    internal class UserEmailServiceFake : IUserMessageService
    {
        public (Email email, PhoneNumber phoneNumber, Password password) SendReturn { get; set; }

        public UserEmailServiceFake()
        {
        }

        public void Send(Email email, PhoneNumber phoneNumber, Password password)
        {
            SendReturn = (email, phoneNumber, password);
        }
    }
}