using AutoMapper;
using Store.Communication.Requests;
using Store.Communication.Responses;
using Store.Domain.Extensions;
using Store.Domain.Repositories;
using Store.Domain.Repositories.Token;
using Store.Domain.Repositories.User;
using Store.Domain.Security.Cryptography;
using Store.Domain.Security.Tokens;
using Store.Exceptions;
using Store.Exceptions.ExceptionBase;

namespace Store.Application.UseCases.User.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IUserWriteOnlyRepository _writeOnlyRepository;
        private readonly IUserReadOnlyRepository _readOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordEncrypter _passwordEncryption;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        private readonly ITokenRepository _tokenRepository;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;

        public RegisterUserUseCase(IUserWriteOnlyRepository writeOnlyRepository,
                                  IUserReadOnlyRepository readOnlyRepository,
                                  IMapper mapper,
                                  IPasswordEncrypter passwordEncryption,
                                   IUnitOfWork unitOfWork,
                                   IAccessTokenGenerator accessTokenGenerator,
                                    ITokenRepository tokenRepository,
                                    IRefreshTokenGenerator refreshTokenGenerator)
        {
            _writeOnlyRepository = writeOnlyRepository;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
            _passwordEncryption = passwordEncryption;
            _unitOfWork = unitOfWork;
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _tokenRepository = tokenRepository;

        }
        public async Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request)
        {
            await Validate(request);

            var user = _mapper.Map<Domain.Entities.User>(request);

            user.Password = _passwordEncryption.Encrypt(request.Password);
            user.UserIdentifier = Guid.NewGuid();

            await _writeOnlyRepository.Add(user);
            await _unitOfWork.Commit();

            var refreshToken = await CreateAndSaveRefreshToken(user);

            return new ResponseRegisterUserJson
            {
                Name = request.Name,
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
        private async Task Validate(RequestRegisterUserJson request)
        {
            var validator = new RegisterUserValidator();

            var result = validator.Validate(request);

            var emailExistActive = await _readOnlyRepository.ExistActiveUserWithEmail(request.Email);

            if (emailExistActive)
            {
                result.Errors.Add(
                    new FluentValidation.Results.ValidationFailure(
                        string.Empty,
                        ResourceMessageException.EMAIL_ALREADY_REGISTERED));
            }

            if (result.IsValid.IsFalse())
            {
                var errorMessages = result.Errors.Select(_ => _.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
