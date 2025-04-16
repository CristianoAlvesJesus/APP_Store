using Store.Domain.Security.Cryptography;
using Store.Infrastructure.Security.Cryptography;

namespace CommonTestUtilities.Cryptography
{
    public class PasswordEncripterBuilder
    {
        public static IPasswordEncrypter Build() => new BCryptNet();
    }
}