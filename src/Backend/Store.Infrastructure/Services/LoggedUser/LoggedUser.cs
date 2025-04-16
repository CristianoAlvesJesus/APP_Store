using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;
using Store.Domain.Security.Tokens;
using Store.Domain.Services.LoggedUser;
using Store.Infrastructure.DataAccess;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Store.Infrastructure.Services.LoggedUser;

public class LoggedUser : ILoggedUser
{
    private readonly StoreDbContext _context;
    private readonly ITokenProvider _tokenProvider;

    public LoggedUser(StoreDbContext context, ITokenProvider tokenProvider)
    {
        _context = context;
        _tokenProvider = tokenProvider;
    }
    public async Task<User> User()
    {
        var token = _tokenProvider.Value();

        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

        var userIdentifier = Guid.Parse(identifier);

        return await _context.Users.AsNoTracking()
            .FirstAsync(user => user.Active && user.UserIdentifier == userIdentifier);
    }
}
