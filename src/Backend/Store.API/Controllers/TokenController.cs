using Microsoft.AspNetCore.Mvc;
using Store.Application.UseCases.Token.RefreshToken;
using Store.Communication.Requests;
using Store.Communication.Responses;

namespace Store.API.Controllers;

public class TokenController : StoreBaseController
{
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ResponseTokensJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken(
        [FromServices] IUseRefreshTokenUseCase useCase,
        [FromBody] RequestNewTokenJson request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }
}