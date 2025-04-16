using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Store.Application.UseCases.User.Register;
using Store.Domain.Extensions;
using Store.Exceptions;
using Store.Exceptions.ExceptionBase;
using Xunit; 

namespace Store.UseCase.Test.User.Register
{
    public class RegisterUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            var useCase = CreateUseCase();

            var result =  await useCase.Execute(request);

            Assert.NotEmpty(result.Name);
            Assert.Equal(result.Name, request.Name);
            Assert.NotNull(result.Tokens);
            Assert.NotEmpty(result.Tokens.AccessToken);
        }

        [Fact]
        public async Task Error_Email_Already_Registered()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            var useCase = CreateUseCase(request.Email);

            Func<Task> act = async () => await useCase.Execute(request);

            //Assert
            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
            Assert.NotNull(exception);
            Assert.IsType<ErrorOnValidationException>(exception);
            Assert.True(exception.GetErrorMessages().Contains( ResourceMessageException.EMAIL_ALREADY_REGISTERED));
        }

        [Fact]
        public async Task Error_Name_Empty()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;

            var useCase = CreateUseCase();

            Func<Task> act = async () => await useCase.Execute(request);

            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
            Assert.NotNull(exception);
            Assert.IsType<ErrorOnValidationException>(exception);
            Assert.True(exception.GetErrorMessages().Contains(ResourceMessageException.NAME_EMPTY)); 
        }

        private static RegisterUserUseCase CreateUseCase(string? email = null)
        {
            var mapper = MapperBuilder.Build();
            var passwordEncripter = PasswordEncripterBuilder.Build();
            var writeOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();

            var readOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();

            var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

            var refreshTokenGenerator = RefreshTokenGeneratorBuilder.Build();
            var tokenRepository = new TokenRepositoryBuilder().Build();

            if (string.IsNullOrEmpty(email).IsFalse()) 
                readOnlyRepositoryBuilder.ExistActiveUserWithEmail(email!);
             


            return new RegisterUserUseCase(writeOnlyRepository,
                                           readOnlyRepositoryBuilder.Build(),
                                           mapper,
                                           passwordEncripter,
                                           unitOfWork,
                                           accessTokenGenerator,
                                           tokenRepository, 
                                           refreshTokenGenerator);
        }
    }
}