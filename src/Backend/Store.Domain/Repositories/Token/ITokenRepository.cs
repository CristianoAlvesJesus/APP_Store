namespace Store.Domain.Repositories.Token;

public interface ITokenRepository
{
    Task<Domain.Entities.RefreshToken?> Get(string refreshToken);
    Task SaveNewRefreshToken(Domain.Entities.RefreshToken refreshToken);
}