namespace ApiForUrl.DataAccess.Entities;

public class ShortUrlDto
{
    public string OrginalUrl { get; set; } = string.Empty;
    public string ShortUrl { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;

    public static implicit operator ShortUrlDto(ShortUrlModel shortUrlModel)
        => new ShortUrlDto
        {
            OrginalUrl = shortUrlModel.OrginalUrl,
            ShortUrl = shortUrlModel.ShortUrl,
            UserId = shortUrlModel.UserId
        };
}
