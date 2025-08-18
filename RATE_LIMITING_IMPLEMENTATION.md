# Rate Limiting Implementation for Squidex CMS

## Overview

This implementation adds comprehensive rate limiting to sensitive API endpoints in Squidex CMS to prevent abuse and protect against various attack vectors including brute force attacks, API abuse, and resource exhaustion.

## Implementation Details

### 1. Rate Limiting Configuration

**File**: `/backend/src/Squidex/Config/Web/RateLimitingExtensions.cs`

The implementation uses .NET 8's built-in rate limiting middleware with the following policies:

- **Global Rate Limiter**: 1000 requests per minute per client
- **Authentication**: 5 requests per minute (login, logout, etc.)
- **UserManagement**: 20 requests per minute (user CRUD operations)
- **ContentModification**: 100 requests per minute (content creation/updates)
- **SensitiveOperations**: 3 requests per 5 minutes (password reset, API key management)

### 2. Client Identification

The rate limiter identifies clients using:
1. **User ID** for authenticated requests (preferred)
2. **IP Address** for anonymous requests (fallback)
3. **"unknown"** as final fallback

### 3. Rate Limiting Attributes

**File**: `/backend/src/Squidex.Web/Pipeline/RateLimitAttribute.cs`

Custom attributes for easy application:
- `[RateLimitPolicies.Authentication]`
- `[RateLimitPolicies.UserManagement]`
- `[RateLimitPolicies.ContentModification]`
- `[RateLimitPolicies.SensitiveOperations]`

### 4. Applied Controllers

Rate limiting has been applied to the following sensitive controllers:

#### Authentication Controllers
- **AccountController** (`[RateLimitPolicies.Authentication]`)
  - Login/logout endpoints
  - Account management

#### User Management Controllers
- **UsersController** (`[RateLimitPolicies.UserManagement]`)
  - User queries and profile updates
- **UserManagementController** (`[RateLimitPolicies.UserManagement]`)
  - Administrative user operations
- **AppsController** (`[RateLimitPolicies.UserManagement]`)
  - App creation and management

#### Sensitive Operations Controllers
- **ProfileController** (`[RateLimitPolicies.SensitiveOperations]`)
  - Password changes
  - Profile modifications
- **AppClientsController** (`[RateLimitPolicies.SensitiveOperations]`)
  - API key management
  - Client credentials

#### Content Controllers
- **ContentsController** (`[RateLimitPolicies.ContentModification]`)
  - Content creation, updates, and deletion

### 5. Error Response Format

When rate limits are exceeded, the API returns:

```json
{
  "error": "rate_limit_exceeded",
  "message": "Too many requests. Please try again later.",
  "retryAfter": 60
}
```

**HTTP Status Code**: 429 (Too Many Requests)

### 6. Integration Points

#### Startup Configuration
**File**: `/backend/src/Squidex/Startup.cs`

```csharp
// In ConfigureServices
services.AddSquidexRateLimiting();

// In Configure (after authentication, before authorization)
app.UseSquidexRateLimiting();
```

#### Middleware Extension
**File**: `/backend/src/Squidex/Config/Web/WebExtensions.cs`

```csharp
public static IApplicationBuilder UseSquidexRateLimiting(this IApplicationBuilder app)
{
    app.UseRateLimiter();
    return app;
}
```

## Security Benefits

1. **Brute Force Protection**: Limits authentication attempts
2. **API Abuse Prevention**: Prevents excessive API calls
3. **Resource Protection**: Protects against resource exhaustion
4. **DoS Mitigation**: Helps mitigate denial-of-service attacks
5. **Fair Usage**: Ensures fair resource allocation among users

## Configuration Flexibility

The rate limiting policies can be easily adjusted by modifying the values in `RateLimitingExtensions.cs`:

- **PermitLimit**: Number of requests allowed
- **Window**: Time window for the limit
- **AutoReplenishment**: Whether to automatically reset the counter

## Monitoring and Observability

The implementation includes:
- Proper error responses with retry-after headers
- Client identification for tracking
- Configurable policies for different endpoint types

## Testing

To test the rate limiting:

1. **Authentication Endpoints**: Make more than 5 login attempts within a minute
2. **User Management**: Perform more than 20 user operations within a minute
3. **Sensitive Operations**: Attempt more than 3 password changes within 5 minutes
4. **Content Operations**: Create more than 100 content items within a minute

Expected result: HTTP 429 with appropriate error message and retry-after header.

## Future Enhancements

Potential improvements could include:
- Redis-based distributed rate limiting for multi-instance deployments
- Dynamic rate limit adjustment based on user roles
- Rate limiting metrics and monitoring
- Whitelist/blacklist functionality
- Custom rate limiting rules per tenant/app

## Compliance

This implementation helps meet security requirements for:
- OWASP API Security Top 10
- General data protection regulations
- Industry security standards
- Rate limiting best practices