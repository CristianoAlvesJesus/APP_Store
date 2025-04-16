using System.Net;

namespace Store.Exceptions.ExceptionBase;

public  abstract class StoreException : SystemException
{
    public StoreException(string message) : base(message) { }

    public abstract IList<string> GetErrorMessages();
    public abstract HttpStatusCode GetStatusCode();
}
