using ApiForUrl.DataAccess.Entities;
using ApiForUrl.DataAccess.ViewModels;

namespace ApiForUrl.DataAccess.Interfaces;

public interface IShortUrlInterface
{
    Task<ShortUrlModel> CreateLinkAsync(AddViewModel addViewModel);
    Task<List<ShortUrlModel>> GetUsersUrls(string userId);
    Task<ShortUrlModel> GetByShortUrl(string link);
}
