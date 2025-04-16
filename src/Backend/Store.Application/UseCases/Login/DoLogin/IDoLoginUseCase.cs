using Store.Communication.Requests;
using Store.Communication.Responses;

namespace Store.Application.UseCases.Login.DoLogin;

public interface IDoLoginUseCase
{
      Task<ResponseRegisterUserJson> Execute(RequestLoginJson request);
}
