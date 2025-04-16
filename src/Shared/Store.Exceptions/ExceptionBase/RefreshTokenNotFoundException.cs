using System.Net;

namespace Store.Exceptions.ExceptionBase;

public class RefreshTokenNotFoundException : StoreException
{
    public RefreshTokenNotFoundException() : base(ResourceMessageException.EXPIRED_SESSION)
    {
    }
    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
