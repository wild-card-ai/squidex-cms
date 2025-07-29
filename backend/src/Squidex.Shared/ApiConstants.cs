// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

namespace Squidex.Shared;

public static class ApiConstants
{
    // Route templates
    public const string UserManagementRoute = "user-management/";
    public const string UserManagementIdRoute = "user-management/{id}/";
    public const string UserManagementLockRoute = "user-management/{id}/lock/";
    public const string UserManagementUnlockRoute = "user-management/{id}/unlock/";

    // Link relation names
    public const string RelLock = "lock";
    public const string RelUnlock = "unlock";
    public const string RelDelete = "delete";
    public const string RelUpdate = "update";
    public const string RelPicture = "picture";
    public const string RelCreate = "create";

    // Error keys
    public const string ErrorLockYourself = "users.lockYourselfError";
    public const string ErrorUnlockYourself = "users.unlockYourselfError";
    public const string ErrorDeleteYourself = "users.deleteYourselfError";
}
