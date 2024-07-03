using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Users;

namespace User.Core.Users
{
    public class UserHashingService : IUserHashingService
    {
        public string Hash(Guid id, string password)
        {
            // assume that the password has been hashed.
            return password;
        }
    }
}
