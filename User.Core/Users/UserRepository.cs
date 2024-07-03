using User.Domain.Users;

namespace User.Core.Users
{
    public class UserRepository : IUserRepository
    {
        private Dictionary<Guid, Domain.Users.User> users = new();

        public async Task Create(Domain.Users.User user)
        {
            if (users.GetValueOrDefault(user.Id) == null)
                users.Add(user.Id, user);

            await Task.CompletedTask;
        }

        public async Task<Domain.Users.User> FindByEmail(string email)
        {
            var user = users
                .Where(u => u.Value.Email.Address == email)
                .Single()
                .Value;

            return await Task.FromResult(user);
        }

        public async Task<Domain.Users.User> FindByPhone(string phoneNumber)
        {
            var user = users
                .Where(u => u.Value.PhoneNumber.ToString() == phoneNumber)
                .Single()
                .Value;

            return await Task.FromResult(user);
        }
    }
}