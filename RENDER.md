# Render Deployment

Deploy this directory as the `insurance-api-gateway` repository after the Identity, Customer, and Policy services have deployed.

Configure the Render web service with:

- Dockerfile path: `Dockerfile`
- Docker build context: `.`
- Health check path: `/health`
- `ASPNETCORE_ENVIRONMENT=Production`

Set these environment variables to the public HTTPS URLs assigned by Render. Include the trailing slash for service URLs. Set `Cors__AllowedOrigins__0` to the exact deployed frontend origin, with no trailing slash.

```text
ReverseProxy__Clusters__identity__Destinations__primary__Address=https://insurance-identity-service.onrender.com/
ReverseProxy__Clusters__customers__Destinations__primary__Address=https://insurance-customer-service-mxdj.onrender.com/
ReverseProxy__Clusters__policies__Destinations__primary__Address=https://insurance-policy-service.onrender.com/
ReverseProxy__Clusters__notifications__Destinations__primary__Address=https://notificationservice-9ko7.onrender.com/
ReverseProxy__Clusters__premium__Destinations__primary__Address=https://premiumservice-bagg.onrender.com/
ReverseProxy__Clusters__payments__Destinations__primary__Address=https://paymentservice-zfth.onrender.com/
ReverseProxy__Clusters__claims__Destinations__primary__Address=https://insurance-claim-service-mq7u.onrender.com/
ReverseProxy__Clusters__reporting__Destinations__primary__Address=https://insurance-reporting-service.onrender.com/
ReverseProxy__Clusters__ai-assistant__Destinations__primary__Address=https://insurance-ai-assistant-service.onrender.com/
Cors__AllowedOrigins__0=https://your-frontend.onrender.com
```

The supplied service URLs are already the Production defaults in `appsettings.Production.json`; setting them in Render is optional but recommended when a URL changes, because environment variables override file configuration. Add `Cors__AllowedOrigins__1`, `Cors__AllowedOrigins__2`, and so on only for additional approved frontend origins.

Before deploying, verify each backend service's public `/health` endpoint. After deployment, verify `GET /health` on the gateway, then send an authenticated request through each of `/identity`, `/customers`, `/policies`, `/notifications`, `/premium`, `/payments`, `/claims`, `/reporting`, and `/ai-assistant`.