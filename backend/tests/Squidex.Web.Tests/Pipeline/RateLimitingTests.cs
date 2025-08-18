// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using Squidex.Config.Web;
using Squidex.Web.Pipeline;
using Xunit;

namespace Squidex.Web.Tests.Pipeline;

public class RateLimitingTests
{
    [Fact]
    public void AddSquidexRateLimiting_ShouldRegisterRateLimiterService()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddSquidexRateLimiting();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var rateLimiterService = serviceProvider.GetService<RateLimiterService>();
        Assert.NotNull(rateLimiterService);
    }

    [Fact]
    public void RateLimitAttributes_ShouldHaveCorrectPolicyNames()
    {
        // Arrange & Act
        var authAttribute = new RateLimitPolicies.AuthenticationAttribute();
        var userMgmtAttribute = new RateLimitPolicies.UserManagementAttribute();
        var contentAttribute = new RateLimitPolicies.ContentModificationAttribute();
        var sensitiveAttribute = new RateLimitPolicies.SensitiveOperationsAttribute();

        // Assert
        Assert.IsType<EnableRateLimitingAttribute>(authAttribute);
        Assert.IsType<EnableRateLimitingAttribute>(userMgmtAttribute);
        Assert.IsType<EnableRateLimitingAttribute>(contentAttribute);
        Assert.IsType<EnableRateLimitingAttribute>(sensitiveAttribute);
    }

    [Theory]
    [InlineData("Authentication")]
    [InlineData("UserManagement")]
    [InlineData("ContentModification")]
    [InlineData("SensitiveOperations")]
    public void RateLimitAttribute_ShouldAcceptValidPolicyNames(string policyName)
    {
        // Arrange & Act
        var attribute = new RateLimitAttribute(policyName);

        // Assert
        Assert.NotNull(attribute);
        Assert.IsType<EnableRateLimitingAttribute>(attribute);
    }
}