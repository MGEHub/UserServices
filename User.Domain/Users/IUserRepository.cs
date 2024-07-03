using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Users
{
    public interface IUserRepository
    {
        Task<User> FindByEmail(string email);
        Task<User> FindByPhone(string phoneNumber);
        Task Create(User user);
    }
}
