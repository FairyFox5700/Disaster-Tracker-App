using System.Net.Http.Headers;
using DisasterTrackerApp.BL.HttpClients.Contract;
using DisasterTrackerApp.Models.Configuration;
using DisasterTrackerApp.Models.GoogleOAuth;
using DisasterTrackerApp.Utils.Extensions;
using Microsoft.Extensions.Options;

namespace DisasterTrackerApp.BL.HttpClients.Implementation;

public class GoogleOAuthHttpClient : IGoogleOAuthHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly GoogleOAuthClientCredentials _credentials;

    public GoogleOAuthHttpClient(HttpClient httpClient, IOptions<GoogleOAuthClientCredentials> credentialOptions)
    {
        _httpClient = httpClient;
        _credentials = credentialOptions.Value;
    }

    public async Task<AuthTokenResponse?> ExchangeCode(string code, CancellationToken cancellationToken)
    {
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
        _httpClient.DefaultRequestHeaders.Add("Accept", "*/*");

        var parameters = new List<KeyValuePair<string, string>>
        {
            new("client_id", _credentials.ClientId),
            new("client_secret", _credentials.ClientSecret),
            new("code", code),
            new("grant_type", GrantType.AuthorizationCode),
            new("redirect_uri", _credentials.RedirectUri)
        };
        var request = new HttpRequestMessage(HttpMethod.Post, _credentials.TokenUri)
        {
            Content = new FormUrlEncodedContent(parameters)
        };
        using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        
        return !response.IsSuccessStatusCode ? null :
            HttpClientExtensions.DeserializeJsonFromStream<AuthTokenResponse>(stream);
    }

    public async Task<RefreshAccessTokenResponse?> RefreshAccessToken(string refreshToken)
    {
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
        _httpClient.DefaultRequestHeaders.Add("Accept", "*/*");

        var parameters = new List<KeyValuePair<string, string>>
        {
            new("client_id", _credentials.ClientId),
            new("client_secret", _credentials.ClientSecret),
            new("refresh_token", refreshToken),
            new("grant_type", GrantType.RefreshToken)
        };
        var request = new HttpRequestMessage(HttpMethod.Post, _credentials.TokenUri)
        {
            Content = new FormUrlEncodedContent(parameters)
        };
        using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        var stream = await response.Content.ReadAsStreamAsync();
        
        return !response.IsSuccessStatusCode ? null :
            HttpClientExtensions.DeserializeJsonFromStream<RefreshAccessTokenResponse>(stream);
    }
}