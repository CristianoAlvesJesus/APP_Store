using Microsoft.AspNetCore.Http;

namespace Store.Communication.Requests;

public class RequestRegisterTransactionFormData : RequestTransactionJson
{
    public IFormFile? Image { get; set; }
}
