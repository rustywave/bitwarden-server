﻿using Bit.Core.Settings;
using IdentityServer4.Models;

namespace Bit.Core.IdentityServer
{
    public class ApiClient : Client
    {
        public ApiClient(
            GlobalSettings globalSettings,
            string id,
            int refreshTokenSlidingDays,
            int accessTokenLifetimeHours,
            string[] scopes = null)
        {
            ClientId = id;
            AllowedGrantTypes = new[] { GrantType.ResourceOwnerPassword, GrantType.AuthorizationCode };
            RefreshTokenExpiration = TokenExpiration.Sliding;
            RefreshTokenUsage = TokenUsage.ReUse;
            SlidingRefreshTokenLifetime = 86400 * refreshTokenSlidingDays;
            AbsoluteRefreshTokenLifetime = 0; // forever
            UpdateAccessTokenClaimsOnRefresh = true;
            AccessTokenLifetime = 3600 * accessTokenLifetimeHours;
            AllowOfflineAccess = true;

            RequireConsent = false;
            RequirePkce = true;
            RequireClientSecret = false;
            if (id == "web")
            {
                RedirectUris = new[] { $"{globalSettings.BaseServiceUri.Vault}/sso-connector.html" };
                PostLogoutRedirectUris = new[] { globalSettings.BaseServiceUri.Vault };
                AllowedCorsOrigins = new[] { globalSettings.BaseServiceUri.Vault };
            }
            else if (id == "desktop")
            {
                RedirectUris = new[] { "bitwarden://sso-callback" };
                PostLogoutRedirectUris = new[] { "bitwarden://logged-out" };
            }
            else if (id == "connector")
            {
                var connectorUris = new List<string>();
                for (var port = 8065; port <= 8070; port++)
                {
                    connectorUris.Add(string.Format("http://localhost:{0}", port));
                }
                RedirectUris = connectorUris.Append("bwdc://sso-callback").ToList();
                PostLogoutRedirectUris = connectorUris.Append("bwdc://logged-out").ToList();
            }
            else if (id == "browser")
            {
                RedirectUris = new[] { $"{globalSettings.BaseServiceUri.Vault}/sso-connector.html" };
                PostLogoutRedirectUris = new[] { globalSettings.BaseServiceUri.Vault };
                AllowedCorsOrigins = new[] { globalSettings.BaseServiceUri.Vault };
            }
            else if (id == "cli")
            {
                var cliUris = new List<string>();
                for (var port = 8065; port <= 8070; port++)
                {
                    cliUris.Add(string.Format("http://localhost:{0}", port));
                }
                RedirectUris = cliUris;
                PostLogoutRedirectUris = cliUris;
            }
            else if (id == "mobile")
            {
                RedirectUris = new[] { "bitwarden://sso-callback" };
                PostLogoutRedirectUris = new[] { "bitwarden://logged-out" };
            }

            if (scopes == null)
            {
                scopes = new string[] { "api" };
            }
            AllowedScopes = scopes;
        }
    }
}
