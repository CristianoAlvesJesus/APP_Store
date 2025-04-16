using AutoMapper;
using Store.Communication.Requests;
using Store.Communication.Responses;
using Store.Domain.Extensions;
using Store.Domain.Repositories;
using Store.Domain.Repositories.Transaction;
using Store.Domain.Services.LoggedUser;
using Store.Exceptions.ExceptionBase;

namespace Store.Application.UseCases.Transaction.Register;

public class RegisterTransactionUseCase : IRegisterTransactionUseCase
{
    private readonly ITransactionWriteOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper; 

    public RegisterTransactionUseCase(
        ILoggedUser loggedUser,
        ITransactionWriteOnlyRepository repository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _loggedUser = loggedUser; 
    }
    public async Task<ResponseRegiteredTransactionJson> Execute(RequestRegisterTransactionFormData request)
    {
        Validate(request); 

        var loggedUser = await _loggedUser.User();

        var transaction = _mapper.Map<Domain.Entities.Transaction>(request);
        transaction.UserId = loggedUser.Id;

        await _repository.Add(transaction);

        await _unitOfWork.Commit();

        return _mapper.Map<ResponseRegiteredTransactionJson>(transaction);
    }

    private static void Validate(RequestRegisterTransactionFormData request)
    {
        var result = new TransactionValidator().Validate(request);

        if (result.IsValid.IsFalse())
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
    }
}
