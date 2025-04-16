
using Store.Domain.Repositories.User;
using Store.Domain.Repositories;
using Store.Domain.Services.LoggedUser;
using Store.Domain.Services.ServiceBus;

namespace Store.Application.UseCases.User.Delete.Request;

public class RequestDeleteUserUseCase : IRequestDeleteUserUseCase
{
   // private readonly IDeleteUserQueue _queue;
    private readonly IUserUpdateOnlyRepository _userUpdateRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    public RequestDeleteUserUseCase(
        //IDeleteUserQueue queue,
        IUserUpdateOnlyRepository userUpdateRepository,
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork)
    {
        //_queue = queue;
        _userUpdateRepository = userUpdateRepository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute()
    {
        var loggedUser = await _loggedUser.User();

        var user = await _userUpdateRepository.GetById(loggedUser.Id);

        user.Active = false;
        _userUpdateRepository.Update(user);

        await _unitOfWork.Commit();

        //await _queue.SendMessage(loggedUser);
    }
}