using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Users;

namespace User.Application.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IUserHashingService userHashingService;
        private readonly IUserMessageService userMessageService;

        public UserService(IUserRepository userRepository, 
            IUserHashingService userHashingService, IUserMessageService userMessageService)
        {
            this.userRepository = userRepository;
            this.userHashingService = userHashingService;
            this.userMessageService = userMessageService;
        }

        public async Task<Guid> Create(Email email, PhoneNumber phoneNumber, string clearTextPassword)
        {
            var id = Guid.NewGuid();
            
            await userRepository.Create(
                new Domain.Users.User(id, email, phoneNumber, clearTextPassword, 
                userHashingService, userMessageService));

            return id;
        }

        public async Task<Domain.Users.User?> GetUserByEmailOrPhoneNumber(string emailOrPhoneNumber)
        {
            if (Email.IsAddressValid(emailOrPhoneNumber))
                return await userRepository.FindByEmail(emailOrPhoneNumber);

            return await userRepository.FindByPhone(new PhoneNumber(emailOrPhoneNumber).ToString());
        }
    }
}
