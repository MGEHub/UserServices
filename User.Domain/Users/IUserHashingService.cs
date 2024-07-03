namespace User.Domain.Users
{
    public interface IUserHashingService
    {
        string Hash(Guid id, string password);
    }
}