using System.Net.Http.Headers;
using DisasterTrackerApp.Models.ApiModels;
using DisasterTrackerApp.Models.Auth;
using DisasterTrackerApp.Models.Disaster;
using DisasterTrackerApp.Utils.Extensions;
using Google.Apis.Auth.OAuth2;

namespace DisasterTrackerApp.WebApi.HttpClients.Implementation;

public class GoogleAuthClient
{
    private const string CredentialsPath = "cleint_credentional.json";
    private readonly HttpClient _httpClient;
    public static readonly string GRANT_TYPE = "authorization_code";
    public static readonly string REDIRECT_URI = "http://localhost:5000/oauth/google-auth/connect";
    public static readonly string LINK_GET_CODE = "https://accounts.google.com/o/oauth2/auth";
    public static readonly string LINK_GET_TOKEN = "https://accounts.google.com/o/oauth2/token";
    private ClientSecrets? _clientSecrets;

    public async ValueTask<string> GetRedirectLink()
    {
        var clientCredentionals = await GetClientSecrets().ConfigureAwait(false);
        string redirect = LINK_GET_CODE
                          + "?scope=email profile"
                          + "&redirect_uri=" + REDIRECT_URI
                          + "&response_type=code"
                          + "&client_id=" + clientCredentionals.ClientId;
        return redirect;
    }
    
    public GoogleAuthClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AuthenticationToken?> GetAccessToken(string code, CancellationToken cancellationToken)
    {  
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            _httpClient.DefaultRequestHeaders.Add("Accept", "*/*");

            var clientCredentials = await GetClientSecrets().ConfigureAwait(false);
            var parameters = new List<KeyValuePair<string, string>>
            {
                new("client_id", clientCredentials.ClientId),
                new("client_secret", clientCredentials.ClientSecret),
                new("code", code),
                new("grant_type", GRANT_TYPE),
                new("redirect_uri", REDIRECT_URI)
            };
            var request = new HttpRequestMessage(HttpMethod.Post, LINK_GET_TOKEN)
            {
                Content = new FormUrlEncodedContent(parameters)
            };
            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

            return !response.IsSuccessStatusCode ? null :
                HttpClientExtensions.DeserializeJsonFromStream<AuthenticationToken>(stream);
    }

    #region  private_access
    private async ValueTask<ClientSecrets> GetClientSecrets()
    {
        if (_clientSecrets != null)
        {
            return _clientSecrets;
        }

        await using var stream =
            new FileStream(CredentialsPath, FileMode.Open, FileAccess.Read);
        _clientSecrets = (await GoogleClientSecrets.FromStreamAsync(stream)).Secrets;
        return _clientSecrets;
    }
    #endregion
}