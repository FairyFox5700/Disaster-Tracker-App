using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DisasterTrackerApp.Entities;

public class GoogleUser
{
    /// <summary>
    /// UUID of user
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public  Guid UserId { get; set; }
    
    /// <summary>
    /// Unique user identifier in Google products
    /// </summary>
    public string UserGoogleId { get; set; }
    
    /// <summary>
    /// Encrypted Access Token
    /// </summary>
    public  string AccessToken { get; set; }= null!;
    /// <summary>
    /// encrypted RefreshToken
    /// </summary>
    public  string RefreshToken { get; set; }= null!;
    /// <summary>
    /// calculate as  DateTimeOffset.UtcNow +long from expires in OAUTH2 response
    /// </summary>
    public  DateTimeOffset ExpiresIn { get; set; }
    /// <summary>
    /// Calculated bool field to check access token expiration
    /// </summary>
    [NotMapped]
    public bool IsExpired => DateTimeOffset.UtcNow >= ExpiresIn;

    public DateTime? LastVisit { get; set; } // todo rename to LastLoginDataUpdate 
    
    #region  FK
    public  List<GoogleCalendar>? Calendars { get; set; }
    #endregion
}