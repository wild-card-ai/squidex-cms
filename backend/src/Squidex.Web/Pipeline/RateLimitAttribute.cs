// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using Microsoft.AspNetCore.RateLimiting;

namespace Squidex.Web.Pipeline;

/// <summary>
/// Attribute to apply rate limiting to controllers or actions.
/// </summary>
public sealed class RateLimitAttribute : EnableRateLimitingAttribute
{
    public RateLimitAttribute(string policyName) : base(policyName)
    {
    }
}

/// <summary>
/// Predefined rate limiting attributes for common scenarios.
/// </summary>
public static class RateLimitPolicies
{
    /// <summary>
    /// Rate limiting for authentication endpoints (login, logout, etc.).
    /// </summary>
    public sealed class AuthenticationAttribute : RateLimitAttribute
    {
        public AuthenticationAttribute() : base("Authentication")
        {
        }
    }

    /// <summary>
    /// Rate limiting for user management endpoints.
    /// </summary>
    public sealed class UserManagementAttribute : RateLimitAttribute
    {
        public UserManagementAttribute() : base("UserManagement")
        {
        }
    }

    /// <summary>
    /// Rate limiting for content modification endpoints.
    /// </summary>
    public sealed class ContentModificationAttribute : RateLimitAttribute
    {
        public ContentModificationAttribute() : base("ContentModification")
        {
        }
    }

    /// <summary>
    /// Rate limiting for sensitive operations like password reset.
    /// </summary>
    public sealed class SensitiveOperationsAttribute : RateLimitAttribute
    {
        public SensitiveOperationsAttribute() : base("SensitiveOperations")
        {
        }
    }
}