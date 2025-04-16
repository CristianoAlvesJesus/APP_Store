using Microsoft.AspNetCore.Mvc;
using Store.API.Attributes;
using Store.Application.UseCases.User.ChangePassword;
using Store.Application.UseCases.User.Delete.Request;
using Store.Application.UseCases.User.Profile;
using Store.Application.UseCases.User.Register;
using Store.Application.UseCases.User.Update;
using Store.Communication.Requests;
using Store.Communication.Responses;

namespace Store.API.Controllers;

public class UserController : StoreBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromServices] IRegisterUserUseCase registerUserUseCase, 
        [FromBody] RequestRegisterUserJson request)
    {
        var result = await registerUserUseCase.Execute(request);

        return Created(string.Empty, result);
    } 

    [HttpGet]
    [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
    [AuthenticatedUserAttribute]
    public async Task<IActionResult> GetUserProfile([FromServices] IGetUserProfileUseCase useCase)
    {
        var result = await useCase.Execute();

        return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType( StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status400BadRequest)]
    [AuthenticatedUserAttribute]
    public async Task<IActionResult> Update([FromServices] IUpdateUserUseCase useCase, [FromBody] RequestUpdateUserJson request)
    {
        await useCase.Execute(request);

        return NoContent();
    }

    [HttpPut("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [AuthenticatedUserAttribute]
    public async Task<IActionResult> ChangePassword(
        [FromServices] IChangePasswordUseCase useCase,
        [FromBody] RequestChangePasswordJson request)
    {
        await useCase.Execute(request);

        return NoContent();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [AuthenticatedUser]
    public async Task<IActionResult> Delete([FromServices] IRequestDeleteUserUseCase useCase)
    {
        await useCase.Execute();

        return NoContent();
    }
}