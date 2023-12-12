using Microsoft.AspNetCore.Identity;

namespace ApiForUrl.DataAccess.Entities;

public class User : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<ShortUrlModel> ShortUrls { get; set; } = new List<ShortUrlModel>();
}
