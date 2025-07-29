/*
 * Squidex Headless CMS
 *
 * @license
 * Copyright (c) Squidex UG (haftungsbeschränkt). All rights reserved.
 */

export interface Resource {
    _links: ResourceLinks;

    _meta?: Metadata | any;
}

export type ResourceLinks = { [rel: string]: ResourceLink };
export type ResourceLink = { href: string; method: ResourceMethod | string; metadata?: string };

export type Metadata = { [rel: string]: string };

export function getLinkUrl(value: Resource | ResourceLinks, ...rels: ReadonlyArray<string>) {
    if (!value) {
        return false;
    }

    const links = (value._links || value) as ResourceLinks;

    for (const rel of rels) {
        const link = links[rel];

        if (link && link.method && link.href) {
            return link.href;
        }
    }

    return undefined;
}

export function hasAnyLink(value: Resource | ResourceLinks, ...rels: ReadonlyArray<string>) {
    return !!getLinkUrl(value, ...rels);
}

// Helper to get a specific link object (not just URL)
export function getLink(value: Resource | ResourceLinks, rel: string): ResourceLink | undefined {
    if (!value) {
        return undefined;
    }
    const links = (value._links || value) as ResourceLinks;
    return links[rel];
}

// Helper to get all available link rels
export function getAvailableLinks(value: Resource | ResourceLinks): string[] {
    if (!value) {
        return [];
    }
    const links = (value._links || value) as ResourceLinks;
    return Object.keys(links);
}


export type ResourceMethod =
    'GET' |
    'DELETE' |
    'PATCH' |
    'POST' |
    'PUT';
