using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Shared;

public static class OperationResultExtensions
{
    public static ResultDto ToOperationResult(this int result, string successMessage = null, string failureMessage = null)
       => result switch
       {
           > 0 => OperationResult.Success(successMessage),
           0 => OperationResult.NotFound(failureMessage ?? "Record not found"),
           -1 => OperationResult.BadRequest(failureMessage),
       };
    public static ResultDto ToOperationResult(this bool result, string successMessage = null, string failureMessage = null)
        => result
            ? OperationResult.Success(successMessage)
            : OperationResult.NotFound(failureMessage ?? "Record not found");

    public static ResultDto ToOperationResult(this bool result, HttpStatusCode status, string data)
    => result
            ? OperationResult.Success(data)
            : OperationResult.StatusMap(status, data);

}

public static class OperationResult
{
    public static ResultDto Success(string message = null)
        => new(true, HttpStatusCode.OK, message ?? "Operation completed successfully");

    public static ResultDto NotFound(string message = null)
        => new(false, HttpStatusCode.NotFound, message ?? "Record not found");

    public static ResultDto BadRequest(string message = null)
        => new(false, HttpStatusCode.BadRequest, message ?? "Invalid request");

    public static ResultDto Created(string message = null)
        => new(true, HttpStatusCode.Created, message ?? "Record created successfully");

    public static ResultDto StatusMap(HttpStatusCode status,string message = null)
       => new(false,status, message ?? "An error occurred ");
}
