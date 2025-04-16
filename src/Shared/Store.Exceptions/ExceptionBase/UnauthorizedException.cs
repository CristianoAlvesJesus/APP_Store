using Store.Exceptions.ExceptionBase;
using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionsBase;
public class UnauthorizedException : StoreException
{
    public UnauthorizedException(string message) : base(message)
    {
    }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}