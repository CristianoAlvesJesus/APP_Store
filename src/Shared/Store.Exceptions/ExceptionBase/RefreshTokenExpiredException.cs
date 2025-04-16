using System.Net;

namespace Store.Exceptions.ExceptionBase;

public class RefreshTokenExpiredException : StoreException
{
    public RefreshTokenExpiredException() : base(ResourceMessageException.INVALID_SESSION)
    {
    }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
