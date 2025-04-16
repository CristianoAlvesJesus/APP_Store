using Microsoft.AspNetCore.Mvc;
using Store.API.Filter;

namespace Store.API.Attributes;

public class AuthenticatedUserAttribute : TypeFilterAttribute
{
    public AuthenticatedUserAttribute() : base(typeof(AuthenticatedUserFilter))
    {
    }
}