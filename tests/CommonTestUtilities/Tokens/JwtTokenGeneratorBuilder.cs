using Store.Domain.Security.Tokens;
using Store.Infrastructure.Security.Access.Generator;

namespace CommonTestUtilities.Tokens;

public class JwtTokenGeneratorBuilder
{
    public static IAccessTokenGenerator  Build() => new JwtTokenGenerator(expirationTimeMinutes: 5, signinKey: "wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww");
}