using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Corbli.Services
{
    public class MagicUser { }
    public class LoginService
    {
        public static Byte[] HashPassword(string password)
        {
            PasswordHasher<MagicUser> hash = new PasswordHasher<MagicUser>();
            hash.HashPassword(new MagicUser(), "password");
            throw new NotImplementedException();
        }

        public static bool TryLogin(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
