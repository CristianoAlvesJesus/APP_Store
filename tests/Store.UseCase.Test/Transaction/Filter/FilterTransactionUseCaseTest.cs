using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Repositories;
using Store.Application.UseCases.Transaction.Filter;
using Store.Exceptions.ExceptionBase;
using Store.Exceptions;
using Store.Communication.Enums;

namespace Store.UseCase.Test.Transaction.Filter
{
    public class FilterTransactionUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestFilterTransactionJsonBuilder.Build();

            var transactions = TransactionBuilder.Collection(user);

            var useCase = CreateUseCase(user, transactions);

            var result = await useCase.Execute(request);

            Assert.NotNull(result);
            Assert.NotNull(result.Transactions);  
            Assert.NotEmpty(result.Transactions);
            Assert.Equal(result.Transactions.Count, transactions.Count);
        }

        [Fact]
        public async Task Error_CookingTime_Invalid()
        {
            (var user, _) = UserBuilder.Build();

            var transactions = TransactionBuilder.Collection(user);

            var request = RequestFilterTransactionJsonBuilder.Build();
            request.TransactionType = (TransactionType)100;

            var useCase = CreateUseCase(user, transactions);

            Func<Task> act = async () => { await useCase.Execute(request); };

            //Assert
            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);

            Assert.NotNull(exception);
            Assert.IsType<ErrorOnValidationException>(exception);
            Assert.True(exception.GetErrorMessages().Contains(ResourceMessageException.TRANSACTION_TYPE_NOT_SUPPORTED));
             
        }

        private static FilterTransactionUseCase CreateUseCase(
        Store.Domain.Entities.User user,
        IList<Store.Domain.Entities.Transaction> transactions)
        {
            var mapper = MapperBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);
            var repository = new TransactionReadOnlyRepositoryBuilder().Filter(user, transactions).Build();

            return new FilterTransactionUseCase(mapper, loggedUser, repository);
        }
    }
}
