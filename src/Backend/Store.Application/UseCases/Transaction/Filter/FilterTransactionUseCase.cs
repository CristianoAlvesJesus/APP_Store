using AutoMapper;
using Store.Application.Extensions;
using Store.Communication.Requests;
using Store.Communication.Responses;
using Store.Domain.Extensions;
using Store.Domain.Repositories.Transaction;
using Store.Domain.Services.LoggedUser;
using Store.Exceptions.ExceptionBase;

namespace Store.Application.UseCases.Transaction.Filter;

public class FilterTransactionUseCase : IFilterTransactionUseCase
{
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    private readonly ITransactionReadOnlyRepository _repository;

    public FilterTransactionUseCase(
        IMapper mapper,
        ILoggedUser loggedUser,
        ITransactionReadOnlyRepository repository)
    {
        _mapper = mapper;
        _loggedUser = loggedUser;
        _repository = repository;
    }
    public async Task<ResponseTransactionsJson> Execute(RequestFilterTransactionJson request)
    {
        Validate(request);

        var loggedUser = await _loggedUser.User();
        var filters = new Domain.Dtos.FilterTransactionsDto
        {
            Description = request.Description,
            TransactionType = (Domain.Enums.TransactionType)request.TransactionType,
            Amount = request.Amount 
        };

        var transations   = await _repository.Filter(loggedUser, filters);
        return new ResponseTransactionsJson
        {
            Transactions = await transations.MapToShortTransactionsJson(loggedUser, _mapper)
        }; 
    }
    private static void Validate(RequestFilterTransactionJson request)
    {
        var validator = new FilterTransactionValidator();

        var result = validator.Validate(request);

        if (result.IsValid.IsFalse())
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).Distinct().ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}