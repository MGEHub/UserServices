namespace User.Domain.Users
{
    public interface IUserMessageService
    {
        void Send(Email email, PhoneNumber phoneNumber, Password password);
    }
}