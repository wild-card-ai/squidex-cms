// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace Squidex.Pipeline;

public static class ValidationProblemDetailsFactory
{
    public static IActionResult Create(ModelStateDictionary modelState)
    {
        var errors = modelState
            .Where(e => e.Value?.Errors?.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value!.Errors.Select(x => x.ErrorMessage).ToArray()
            );

        var problemDetails = new ValidationProblemDetails(errors)
        {
            Status = 400,
            Title = "One or more validation errors occurred.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        return new BadRequestObjectResult(new { error = problemDetails });
    }
}
