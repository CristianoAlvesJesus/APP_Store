using AutoMapper;
using Store.Communication.Responses;
using Store.Domain.Services.LoggedUser;

namespace Store.Application.UseCases.User.Profile;

public class GetUserProfileUseCase : IGetUserProfileUseCase
{
    private readonly ILoggedUser _loggerdUser;
    private readonly IMapper _mapper;
    public GetUserProfileUseCase(ILoggedUser loggerdUser, IMapper mapper)
    {
        _loggerdUser = loggerdUser;
        _mapper = mapper;
    }
    public async Task<ResponseUserProfileJson> Execute()
    {
        var user = await _loggerdUser.User();

        return _mapper.Map<ResponseUserProfileJson>(user);
    }
}

