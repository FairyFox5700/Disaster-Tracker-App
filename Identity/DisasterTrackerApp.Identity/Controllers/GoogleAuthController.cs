using Google.Apis.Auth;
using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace DisasterTrackerApp.Identity.Controllers;

public  class GoogleAuthController:ControllerBase
{
     /// <summary>
        /// Fetches and shows the Google OAuth2 tokens that are currently active for the logged in user.
        /// Specifying the <see cref="AuthorizeAttribute"/> will guarantee that the code executes only if the
        /// user is authenticated. Once the user is authenticated the tokens are stored locally, in a cookie,
        /// and we can inspect them.
        /// </summary>
        [Authorize]
        public async Task<IActionResult> ShowTokens()
        {
            // The user is already authenticated, so this call won't trigger authentication.
            // But it allows us to access the AuthenticateResult object that we can inspect
            // to obtain token related values.
            AuthenticateResult auth = await HttpContext.AuthenticateAsync();
            string idToken = auth.Properties.GetTokenValue(OpenIdConnectParameterNames.IdToken);
            string idTokenValid, idTokenIssued, idTokenExpires;
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
                idTokenValid = "true";
                idTokenIssued = new DateTime(1970, 1, 1).AddSeconds(payload.IssuedAtTimeSeconds.Value).ToString();
                idTokenExpires = new DateTime(1970, 1, 1).AddSeconds(payload.ExpirationTimeSeconds.Value).ToString();
            }
            catch (Exception e)
            {
                idTokenValid = $"false: {e.Message}";
                idTokenIssued = "invalid";
                idTokenExpires = "invalid";
            }
            string accessToken = auth.Properties.GetTokenValue(OpenIdConnectParameterNames.AccessToken);
            string refreshToken = auth.Properties.GetTokenValue(OpenIdConnectParameterNames.RefreshToken);
            string accessTokenExpiresAt = auth.Properties.GetTokenValue("expires_at");
            string cookieIssuedUtc = auth.Properties.IssuedUtc?.ToString() ?? "<missing>";
            string cookieExpiresUtc = auth.Properties.ExpiresUtc?.ToString() ?? "<missing>";

            return Ok(new []
            {
                $"Id Token: '{idToken}'",
                $"Id Token valid: {idTokenValid}",
                $"Id Token Issued UTC: '{idTokenIssued}'",
                $"Id Token Expires UTC: '{idTokenExpires}'",
                $"Access Token: '{accessToken}'",
                $"Refresh Token: '{refreshToken}'",
                $"Access token expires at: '{accessTokenExpiresAt}'",
                $"Cookie Issued UTC: '{cookieIssuedUtc}'",
                $"Cookie Expires UTC: '{cookieExpiresUtc}'",
            });
        }

     /// <summary>
        /// Forces the refresh of the OAuth access token.
        /// Specifying the <see cref="AuthorizeAttribute"/> will guarantee that the code executes only if the
        /// user is authenticated.
        /// </summary>
        /// <param name="auth">The Google authorization provider.
        /// This can also be injected on the controller constructor.</param>
        [Authorize]
        public async Task<IActionResult> ForceTokenRefresh([FromServices] IGoogleAuthProvider auth)
        {
            // Obtain OAuth related values before the refresh.
            AuthenticateResult authResult0 = await HttpContext.AuthenticateAsync();
            string accessToken0 = authResult0.Properties.GetTokenValue(OpenIdConnectParameterNames.AccessToken);
            string refreshToken0 = authResult0.Properties.GetTokenValue(OpenIdConnectParameterNames.RefreshToken);
            string issuedUtc0 = authResult0.Properties.IssuedUtc?.ToString() ?? "<missing>";
            string expiresUtc0 = authResult0.Properties.ExpiresUtc?.ToString() ?? "<missing>";

            // Force token refresh by specifying a too-long refresh window.
            GoogleCredential cred = await auth.GetCredentialAsync(TimeSpan.FromHours(24));

            // Obtain OAuth related values after the refresh.
            AuthenticateResult authResult1 = await HttpContext.AuthenticateAsync();
            string accessToken1 = authResult1.Properties.GetTokenValue(OpenIdConnectParameterNames.AccessToken);
            string refreshToken1 = authResult1.Properties.GetTokenValue(OpenIdConnectParameterNames.RefreshToken);
            string issuedUtc1 = authResult1.Properties.IssuedUtc?.ToString() ?? "<missing>";
            string expiresUtc1 = authResult1.Properties.ExpiresUtc?.ToString() ?? "<missing>";

            // As demonstration compare the old values with the new ones and check that everything is
            // as it should be.
            string credAccessToken = await cred.UnderlyingCredential.GetAccessTokenForRequestAsync();

            bool accessTokenChanged = accessToken0 != accessToken1;
            bool credHasCorrectAccessToken = credAccessToken == accessToken1;

            bool pass = accessTokenChanged && credHasCorrectAccessToken;

            var model = new ForceTokenRefreshModel
            {
                Results = new[]
                {
                    $"Before Access Token: '{accessToken0}'",
                    $"Before Refresh Token: '{refreshToken0}'",
                    $"Before Issued UTC: '{issuedUtc0}'",
                    $"Before Expires UTC: '{expiresUtc0}'",
                    $"After Access Token: '{accessToken1}'",
                    $"After Refresh Token: '{refreshToken1}'",
                    $"After Issued UTC: '{issuedUtc1}'",
                    $"After Expires UTC: '{expiresUtc1}'",
                    $"Access token changed: {accessTokenChanged}",
                    $"Credential has correct access token: {credHasCorrectAccessToken}",
                    $"Pass: {pass}"
                },
                AccessToken = accessToken1
            };
            return Ok(model);
        }
     
    /// <summary>
    /// Checks that the access token is the expected one.
    /// Specifying the <see cref="AuthorizeAttribute"/> will guarantee that the code executes only if the
    /// user is authenticated.
    /// This method is used from the Force Refresh sample to show that the refreshed token is persisted.
    /// </summary>
    /// <param name="auth">The Google authorization provider.
    /// This can also be injected on the controller constructor.</param>
    /// <param name="expectedAccessToken">The expected token.</param>
    [Authorize]
    [HttpPost]
    public async Task<string[]> ForceTokenRefreshCheckPersisted([FromServices] IGoogleAuthProvider auth, [FromForm] string expectedAccessToken)
    {
        // Check that the refreshed access token is correctly persisted across requests.
        var cred = await auth.GetCredentialAsync();
        var credAccessToken = await cred.UnderlyingCredential.GetAccessTokenForRequestAsync();
        var pass = credAccessToken == expectedAccessToken;
        return new[]
        {
            $"Expected access token: '{expectedAccessToken}'",
            $"Credential access token: '{credAccessToken}'",
            $"Pass: {pass}"
        };
    }
}