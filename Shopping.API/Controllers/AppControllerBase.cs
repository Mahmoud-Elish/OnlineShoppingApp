using Microsoft.AspNetCore.Mvc;
using Shopping.Shared;
using System.Net;

namespace Shopping.API;
[ApiController]
public class AppControllerBase : ControllerBase
{
    protected IActionResult FromOperationResult(ResultDto result)
    {
        var response = new { success = result.Success, message = result.Message };

        return result.Code switch
        {
            HttpStatusCode.OK => Ok(response),
            HttpStatusCode.Created => Created(string.Empty, response),
            HttpStatusCode.NotFound => NotFound(response),
            HttpStatusCode.BadRequest => BadRequest(response),
            _ => StatusCode((int)result.Code, response)
        };
    }
}
