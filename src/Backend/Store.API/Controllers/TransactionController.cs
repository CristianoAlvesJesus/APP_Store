using Microsoft.AspNetCore.Mvc;
using Store.API.Attributes;
using Store.API.Binder;
using Store.Application.UseCases.Transaction.Filter;
using Store.Application.UseCases.Transaction.Gets;
using Store.Application.UseCases.Transaction.Register;
using Store.Communication.Requests;
using Store.Communication.Responses;

namespace Store.API.Controllers;

[AuthenticatedUser]
public class TransactionController : StoreBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegiteredTransactionJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
    [FromServices] IRegisterTransactionUseCase useCase,
    [FromForm] RequestRegisterTransactionFormData request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpPost("filter")]
    [ProducesResponseType(typeof(ResponseTransactionsJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Filter(
        [FromServices] IFilterTransactionUseCase useCase,
        [FromBody] RequestFilterTransactionJson request)
    {
        var response = await useCase.Execute(request);

        if (response.Transactions.Any())
            return Ok(response);

        return NoContent();
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseTransactionJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromServices] IGetTransactionByIdUseCase useCase,
        [FromRoute][ModelBinder(typeof(TransactionIdBinder))] long id)
    {
        var response = await useCase.Execute(id);

        return Ok(response);
    }
}
