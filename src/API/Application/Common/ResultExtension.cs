using Microsoft.AspNetCore.Mvc;

namespace GameService.Application.Common;

public static class ResultExtension
{
    public static IActionResult ToHttpResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(result.Value!);

        return new ObjectResult(result.Error)
        {
            StatusCode = result.StatusCode ?? 400
        };
    }
}