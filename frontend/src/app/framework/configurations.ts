/*
 * Squidex Headless CMS
 *
 * @license
 * Copyright (c) Squidex UG (haftungsbeschränkt). All rights reserved.
 */

export class UIOptions {
    constructor(
        public readonly value: any,
    ) {
    }
}

export class ApiUrlConfig {
    public readonly value: string;

    constructor(value: string) {
        if (value.indexOf('/', value.length - 1) < 0) {
            value = `${value}/`;
        }

        this.value = value;
    }

    public buildUrl(path: string) {
        if (path.indexOf('/') === 0) {
            path = path.substring(1);
        }

        return this.value + path;
    }
}

// Centralized status codes
export const STATUS_CODES = {
    OK: 200,
    CREATED: 201,
    ACCEPTED: 202,
    NO_CONTENT: 204,
    BAD_REQUEST: 400,
    UNAUTHORIZED: 401,
    FORBIDDEN: 403,
    NOT_FOUND: 404,
    CONFLICT: 409,
    INTERNAL_SERVER_ERROR: 500,
    // Add more as needed
};

// Centralized permissions
export const PERMISSIONS = {
    CREATE: 'create',
    READ: 'read',
    UPDATE: 'update',
    DELETE: 'delete',
    PUBLISH: 'publish',
    UNPUBLISH: 'unpublish',
    // Add more as needed
};

