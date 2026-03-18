namespace Infrastructure.Exceptions
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException()
            : base("Username or Password is incorrect")
        {
        }
    }
}
