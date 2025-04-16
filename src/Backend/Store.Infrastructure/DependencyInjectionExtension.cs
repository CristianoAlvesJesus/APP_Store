using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Store.Domain.Repositories;
using Store.Domain.Repositories.Token;
using Store.Domain.Repositories.Transaction;
using Store.Domain.Repositories.User;
using Store.Domain.Security.Cryptography;
using Store.Domain.Security.Tokens;
using Store.Domain.Services.LoggedUser;
using Store.Infrastructure.DataAccess;
using Store.Infrastructure.DataAccess.Repositories;
using Store.Infrastructure.Extensions;
using Store.Infrastructure.Security;
using Store.Infrastructure.Security.Access.Generator;
using Store.Infrastructure.Security.Access.Validator;
using Store.Infrastructure.Security.Cryptography;
using Store.Infrastructure.Services.LoggedUser;
using System.Reflection;

namespace Store.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration )
    {

        AddPasswordEncripter(services);
        AddRepositories(services);
        AddTokens(services, configuration);
        AddLoggedUser(services);

        AddDbContext(services, configuration);
        AddFluentMigrator_SqlServer(services, configuration);
        if (configuration.IsUnitTestEnviroment())
            return;      
    }

    private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectioString = configuration.ConnectionString();

        services.AddDbContext<StoreDbContext>(dbContext =>
        {
            dbContext.UseSqlServer(connectioString);
        });
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();

        services.AddScoped<ITransactionWriteOnlyRepository, TransactionRepository>();
        services.AddScoped<ITransactionReadOnlyRepository, TransactionRepository>();

        services.AddScoped<ITokenRepository, TokenRepository>();

    }

    private static void AddFluentMigrator_SqlServer(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();

        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options
            .AddSqlServer()
            .WithGlobalConnectionString(connectionString)
            .ScanIn(Assembly.Load("Store.Infrastructure")).For.All();
        });
    }

    private static void AddTokens(IServiceCollection services, IConfiguration configuration)
    {
        var experationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

        services.AddScoped<IAccessTokenGenerator>(opt => new JwtTokenGenerator(experationTimeMinutes, signingKey!));
        services.AddScoped<IAccessTokenValidator>(opt => new JwtTokenValidator(signingKey!));

        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
    }

    private static void AddPasswordEncripter(IServiceCollection services)
    { 
        services.AddScoped<IPasswordEncrypter, BCryptNet>();
    }
}
