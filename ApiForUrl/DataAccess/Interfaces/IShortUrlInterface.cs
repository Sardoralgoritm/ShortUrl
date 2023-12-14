using ApiForUrl.DataAccess.Entities;

namespace ApiForUrl.DataAccess.Interfaces;

public interface IShortUrlInterface
{
    Task<ShortUrlModel> CreateLinkAsync(string link);
    Task<ShortUrlModel> GetByShortUrl(string link);
}
