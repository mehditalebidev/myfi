using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using MyFi.Api.Common.Results;

namespace MyFi.Api.Common.Api;

public static class ProblemDetailsExtensions
{
    public static ActionResult<TValue> ToActionResult<TValue>(
        this Result<TValue> result,
        ControllerBase controller)
    {
        if (result.IsSuccess)
        {
            return controller.Ok(result.Value);
        }

        return controller.ToProblem(result.Error!);
    }

    public static ObjectResult ToProblem(this ControllerBase controller, Error error)
    {
        var factory = controller.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
        var problemDetails = factory.CreateProblemDetails(
            controller.HttpContext,
            statusCode: error.StatusCode,
            title: error.Title,
            detail: error.Detail,
            type: $"https://httpstatuses.com/{error.StatusCode}");

        problemDetails.Extensions["code"] = error.Code;

        return new ObjectResult(problemDetails)
        {
            StatusCode = error.StatusCode
        };
    }
}
