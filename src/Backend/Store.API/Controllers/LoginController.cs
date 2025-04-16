using Microsoft.AspNetCore.Mvc;
using Store.Application.UseCases.Login.DoLogin;
using Store.Communication.Requests;
using Store.Communication.Responses;

namespace Store.API.Controllers
{
 
    public class LoginController : StoreBaseController
    {

        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromServices] IDoLoginUseCase useCase, [FromBody] RequestLoginJson request)
            {
            var response = await useCase.Execute(request);

            return Ok(response);
        }
    }
}
