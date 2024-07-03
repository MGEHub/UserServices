using User.Domain.Users;

namespace User.Application.Users
{
    public interface IUserService
    {
        Task<Guid> Create(Email email, PhoneNumber phoneNumber, string clearTextPassword);
        Task<User.Domain.Users.User?> GetUserByEmailOrPhoneNumber(string emailOrPhoneNumber);
    }
}