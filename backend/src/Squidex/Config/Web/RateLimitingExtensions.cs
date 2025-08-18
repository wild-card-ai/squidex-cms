// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace Squidex.Config.Web;

public static class RateLimitingExtensions
{
    public static IServiceCollection AddSquidexRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            // Global rate limiter for all API requests
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: GetClientIdentifier(httpContext),
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 1000,
                        Window = TimeSpan.FromMinutes(1)
                    }));

            // Authentication endpoints - stricter limits
            options.AddPolicy("Authentication", httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: GetClientIdentifier(httpContext),
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 5,
                        Window = TimeSpan.FromMinutes(1)
                    }));

            // User management endpoints - moderate limits
            options.AddPolicy("UserManagement", httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: GetClientIdentifier(httpContext),
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 20,
                        Window = TimeSpan.FromMinutes(1)
                    }));

            // Content modification endpoints - moderate limits
            options.AddPolicy("ContentModification", httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: GetClientIdentifier(httpContext),
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 100,
                        Window = TimeSpan.FromMinutes(1)
                    }));

            // Password reset and sensitive operations - very strict limits
            options.AddPolicy("SensitiveOperations", httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: GetClientIdentifier(httpContext),
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 3,
                        Window = TimeSpan.FromMinutes(5)
                    }));

            // Configure rejection response
            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = 429;
                context.HttpContext.Response.ContentType = "application/json";

                var response = new
                {
                    error = "rate_limit_exceeded",
                    message = "Too many requests. Please try again later.",
                    retryAfter = GetRetryAfterSeconds(context)
                };

                await context.HttpContext.Response.WriteAsync(
                    System.Text.Json.JsonSerializer.Serialize(response), token);
            };
        });

        return services;
    }

    private static string GetClientIdentifier(HttpContext httpContext)
    {
        // Try to get user ID first (for authenticated requests)
        var userId = httpContext.User?.FindFirst("sub")?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            return $"user:{userId}";
        }

        // Fall back to IP address for anonymous requests
        var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();
        if (!string.IsNullOrEmpty(ipAddress))
        {
            return $"ip:{ipAddress}";
        }

        // Fallback identifier
        return "unknown";
    }

    private static int GetRetryAfterSeconds(OnRejectedContext context)
    {
        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
        {
            return (int)retryAfter.TotalSeconds;
        }

        // Default retry after 60 seconds
        return 60;
    }
}