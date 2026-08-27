# API Gateway Routing Contract

## Ownership

The API Gateway is the public entry point for platform APIs. It owns route-prefix configuration and forwards requests without exposing internal service routes.

## Routes

| Service | Public prefix | Local service URL |
| --- | --- | --- |
| Identity Service | `/identity` | `http://localhost:5178` |
| Customer Service | `/customers` | `http://localhost:5180` |
| Policy Service | `/policies` | `http://localhost:5182` |

The gateway removes the public prefix before forwarding. For example, `GET /policies/api/policies/{policyId}` is forwarded as `GET /api/policies/{policyId}`.

## Deployment

Use `GET /health` to check gateway availability. In Render, configure the `ReverseProxy__Clusters__*__Destinations__primary__Address` environment variables with each deployed service URL, including its trailing slash.

## Change Rules

Add a new service route before consumers depend on it. Preserve existing prefixes and review route or OpenAPI changes with affected service teams.