using System.Net;

namespace Store.Exceptions.ExceptionBase;

public class ErrorOnValidationException: StoreException
{
    private readonly IList<string> _errorMessages;

    public ErrorOnValidationException(IList<string> listErrorMessages):base(string.Empty)
    {
        _errorMessages = listErrorMessages;
    }

    public override IList<string> GetErrorMessages() => _errorMessages;

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
}
