using System.ComponentModel.DataAnnotations;

namespace ApiForUrl.DataAccess.Entities;

public class ShortUrlModel : BaseModel
{
    public string OrginalUrl { get; set; } = string.Empty;
    public string ShortUrl { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public User User { get; set; } = new User();
}
