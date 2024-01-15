namespace ApiForUrl.DataAccess.ViewModels;

public class AuthResultVM
{
    public string UserId { get; set; } = string.Empty;
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; } 
}
