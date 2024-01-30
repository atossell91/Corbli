namespace Corbli.Services
{
    public class LoginService
    {
        public static Byte[] HashPassword(string password)
        {
            throw new NotImplementedException();
        }

        public static bool TryLogin(string username, string password)
        {
            var passHash = HashPassword(password);

            return false;
        }
    }
}
