namespace Store.Domain.Security.Tokens;

public interface IRefreshTokenGenerator
{
    string Generate();
}
