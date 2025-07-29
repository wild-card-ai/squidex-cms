// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

namespace Squidex.Areas.Api.Controllers;

public sealed class ContributorsMetadata
    public static class ContributorsMetadataDefaults
    {
        public const string IsInvitedTrue = "true";
    }

{
    /// <summary>
    /// Indicates whether the user has been invited.
    /// </summary>
    public string IsInvited { get; set; }
}
