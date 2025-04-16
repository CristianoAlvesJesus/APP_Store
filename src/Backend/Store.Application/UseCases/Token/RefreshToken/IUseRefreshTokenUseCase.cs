using Store.Communication.Requests;
using Store.Communication.Responses;

namespace Store.Application.UseCases.Token.RefreshToken;

public interface IUseRefreshTokenUseCase
{
    Task<ResponseTokensJson> Execute(RequestNewTokenJson request);
}
