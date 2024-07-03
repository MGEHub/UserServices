using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Users;

namespace User.Core.Users
{
    public class UserMessageService : IUserMessageService
    {
        public UserMessageService(IUserMessageService userEmailService, IUserMessageService userPhoneService)
        {
            UserEmailService = userEmailService;
            UserPhoneService = userPhoneService;
        }

        public IUserMessageService UserEmailService { get; }
        public IUserMessageService UserPhoneService { get; }

        public void Send(Email email, PhoneNumber phoneNumber, Password password)
        {
            var messageService = email != null ? UserEmailService : UserPhoneService;
            
            messageService.Send(email, phoneNumber, password);
        }
    }
}
