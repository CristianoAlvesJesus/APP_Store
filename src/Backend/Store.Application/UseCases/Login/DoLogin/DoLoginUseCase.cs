
using Store.Communication.Requests;
using Store.Communication.Responses;
using Store.Domain.Repositories.User;
using Store.Domain.Security.Tokens;
using Store.Domain.Security.Cryptography;
using Store.Exceptions.ExceptionBase;
using Store.Domain.Extensions;
using Store.Domain.Repositories.Token;
using Store.Domain.Repositories;


namespace Store.Application.UseCases.Login.DoLogin
{
    public class DoLoginUseCase : IDoLoginUseCase
    {
        private readonly IUserReadOnlyRepository _repository;
        private readonly IPasswordEncrypter _passwordEncrypter;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly ITokenRepository _tokenRepository;

        public DoLoginUseCase(IUserReadOnlyRepository repository,
                              IPasswordEncrypter passwordEncrypter,
                              IAccessTokenGenerator accessTokenGenerator,
                              IUnitOfWork unitOfWork,
                              IRefreshTokenGenerator refreshTokenGenerator,
                              ITokenRepository tokenRepository)
        {
            _repository = repository;
            _passwordEncrypter = passwordEncrypter;
            _accessTokenGenerator = accessTokenGenerator;
            _unitOfWork = unitOfWork;
            _refreshTokenGenerator = refreshTokenGenerator;
            _tokenRepository = tokenRepository;
        }

        public async Task<ResponseRegisterUserJson> Execute(RequestLoginJson request)
        {  
            var user = await _repository.GetByEmail(request.Email);

            if (user == null || _passwordEncrypter.IsValid(request.Password, user.Password).IsFalse())
                throw new InvalidLoginException();

            var refreshToken = await CreateAndSaveRefreshToken(user);

            return new ResponseRegisterUserJson
            {
                Name = user.Name,
                Tokens = new ResponseTokenJson
                {
                    AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier),
                    RefreshToken = refreshToken 
                }
            };
        }

        private async Task<string> CreateAndSaveRefreshToken(Domain.Entities.User user)
        {
            var refreshToken = new Domain.Entities.RefreshToken
            {
                Value = _refreshTokenGenerator.Generate(),
                UserId = user.Id
            };

            await _tokenRepository.SaveNewRefreshToken(refreshToken);

            await _unitOfWork.Commit();

            return refreshToken.Value;
        }
    }
}
