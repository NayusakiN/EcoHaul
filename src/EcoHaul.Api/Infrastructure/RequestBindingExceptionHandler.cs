using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;

namespace EcoHaul.Api.Infrastructure;

public sealed class RequestBindingExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not BadHttpRequestException badRequest)
        {
            return false;
        }

        await Results.Problem(
            title: "Bad Request",
            detail: Describe(badRequest),
            statusCode: badRequest.StatusCode,
            type: "https://tools.ietf.org/html/rfc9110#section-15.5.1").ExecuteAsync(httpContext);

        return true;
    }

    private static string Describe(BadHttpRequestException exception)
    {
        var message = exception.Message;

        if (message.Contains("\"Guid customerId\"", StringComparison.Ordinal))
        {
            return message.Contains("was not provided", StringComparison.Ordinal)
                ? "The X-Customer-Id header is required."
                : "The X-Customer-Id header must be a GUID.";
        }

        if (message.Contains("PickupStatus", StringComparison.Ordinal))
        {
            return "Status must be Draft, Scheduled, or Collected.";
        }

        if (message.Contains("requestedFrom", StringComparison.Ordinal) &&
            message.Contains("DateOnly", StringComparison.Ordinal))
        {
            return "Requested from must be a date in yyyy-MM-dd format.";
        }

        if (message.Contains("requestedTo", StringComparison.Ordinal) &&
            message.Contains("DateOnly", StringComparison.Ordinal))
        {
            return "Requested to must be a date in yyyy-MM-dd format.";
        }

        if (exception.InnerException is JsonException jsonException)
        {
            return DescribeJson(jsonException);
        }

        return message;
    }

    private static string DescribeJson(JsonException exception)
    {
        if (!exception.Message.Contains("could not be converted", StringComparison.Ordinal))
        {
            return exception.Message;
        }

        return exception.Path switch
        {
            "$.material" => "Material must be Paper, Plastic, Glass, or Electronics.",
            "$.weightKg" => "Weight must be a number.",
            "$.requestedOn" => "Requested on must be a date in yyyy-MM-dd format.",
            _ => "The request body contains a value that could not be read."
        };
    }
}
