using Microsoft.Extensions.Configuration;

namespace DisasterTrackerApp.Models.Configuration;

public class GoogleOAuthClientCredentials
{
    public const string Section = "OAuthCredentials";

    [ConfigurationKeyName("client_id")] 
    public string ClientId { get; set; } = null!;

    [ConfigurationKeyName("project_id")]
    public string ProjectId { get; set; } = null!;

    [ConfigurationKeyName("auth_uri")]
    public string AuthUri { get; set; } = null!;

    [ConfigurationKeyName("token_uri")]
    public string TokenUri { get; set; } = null!;

    [ConfigurationKeyName("auth_provider_x509_cert_url")]
    public string AuthProviderX509CertUrl { get; set; } = null!;

    [ConfigurationKeyName("client_secret")]
    public string ClientSecret { get; set; } = null!;
    
    public string RedirectUri { get; set; } = null!;
}