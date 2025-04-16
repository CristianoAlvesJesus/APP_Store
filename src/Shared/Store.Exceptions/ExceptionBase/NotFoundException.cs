using System.Net;

namespace Store.Exceptions.ExceptionBase;

public class NotFoundException : StoreException
{
    public NotFoundException(string message) : base(message)
    {
    }
    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
}