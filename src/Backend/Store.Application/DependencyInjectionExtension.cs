using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sqids;
using Store.Application.Services.AutoMapper;
using Store.Application.UseCases.Login.DoLogin;
using Store.Application.UseCases.Token.RefreshToken;
using Store.Application.UseCases.Transaction.Filter;
using Store.Application.UseCases.Transaction.Gets;
using Store.Application.UseCases.Transaction.Register;
using Store.Application.UseCases.User.ChangePassword;
using Store.Application.UseCases.User.Delete.Request;
using Store.Application.UseCases.User.Profile;
using Store.Application.UseCases.User.Register;
using Store.Application.UseCases.User.Update;

namespace Store.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        
        AutoMapper(services, configuration);
        AddEncoder(services, configuration);
        AddUseCases(services);
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();

        services.AddScoped<IRegisterTransactionUseCase, RegisterTransactionUseCase>();
        services.AddScoped<IFilterTransactionUseCase, FilterTransactionUseCase>();
        services.AddScoped<IGetTransactionByIdUseCase, GetTransactionByIdUseCase>();
        services.AddScoped<IUseRefreshTokenUseCase, UseRefreshTokenUseCase>();
        services.AddScoped<IRequestDeleteUserUseCase, RequestDeleteUserUseCase>();

    }
    private static void AutoMapper(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(option => new AutoMapper.MapperConfiguration(autoMapperOptions =>
        {
            var sqids = option.GetService<SqidsEncoder<long>>()!;

            autoMapperOptions.AddProfile(new AutoMapping(sqids));
        }).CreateMapper());
    }

    private static void AddEncoder(IServiceCollection services, IConfiguration configuration)
    {
        var sqids = new SqidsEncoder<long>(new()
        {
            MinLength = 3,
            Alphabet = configuration.GetValue<string>("Settings:IdCryptographyAlphabet")!
        });

        services.AddSingleton(sqids);
    }
}
