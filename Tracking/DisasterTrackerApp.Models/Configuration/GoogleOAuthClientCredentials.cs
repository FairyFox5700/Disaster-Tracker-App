using Microsoft.Extensions.Configuration;

namespace DisasterTrackerApp.Models.Configuration;

public class GoogleOAuthClientCredentials
{
    public const string OAuthCredentials = "OAuthCredentials";
    
    [ConfigurationKeyName("client_id")]
    public string ClientId { get; set; }

    [ConfigurationKeyName("project_id")]
    public string ProjectId { get; set; }

    [ConfigurationKeyName("auth_uri")]
    public string AuthUri { get; set; }

    [ConfigurationKeyName("token_uri")]
    public string TokenUri { get; set; }

    [ConfigurationKeyName("auth_provider_x509_cert_url")]
    public string AuthProviderX509CertUrl { get; set; }

    [ConfigurationKeyName("client_secret")]
    public string ClientSecret { get; set; }
    
    public string RedirectUri { get; set; }
}