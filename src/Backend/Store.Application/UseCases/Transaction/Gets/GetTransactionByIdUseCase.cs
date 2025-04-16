using AutoMapper;
using Store.Communication.Responses;
using Store.Domain.Repositories.Transaction;
using Store.Domain.Services.LoggedUser;
using Store.Exceptions;
using Store.Exceptions.ExceptionBase;

namespace Store.Application.UseCases.Transaction.Gets;

public class GetTransactionByIdUseCase : IGetTransactionByIdUseCase
{
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;
    private readonly ITransactionReadOnlyRepository _repository;
    public GetTransactionByIdUseCase( IMapper mapper,
                                      ILoggedUser loggedUser,
                                      ITransactionReadOnlyRepository repository
                                    )
    {
        _repository = repository;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }
    public async  Task<ResponseTransactionJson> Execute(long transactionId)
    {
        var loggedUser = await _loggedUser.User();

        var transaction = await _repository.GetTransationById(loggedUser, transactionId);

        if (transaction is null)
            throw new NotFoundException(ResourceMessageException.TRANSACTION_NOT_FOUND);

        return _mapper.Map<ResponseTransactionJson>(transaction); 
    }
}
