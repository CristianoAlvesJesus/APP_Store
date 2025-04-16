using System.Net;

namespace Store.Exceptions.ExceptionBase;

public class InvalidLoginException : StoreException
{
    public InvalidLoginException() : base(ResourceMessageException.EMAIL_OR_PASSWORD_INVALID)
    {
    }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}