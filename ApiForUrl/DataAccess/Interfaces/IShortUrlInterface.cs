using ApiForUrl.DataAccess.Entities;

namespace ApiForUrl.DataAccess.Interfaces;

public interface IShortUrlInterface
{
    Task<List<ShortUrlModel>> GetAllUrlAsync();
    Task<ShortUrlModel> GetByIdUrlAsync(int id);
    Task AddUrlAsync(ShortUrlModel model);
    Task UpdateUrlAsync(ShortUrlModel model);
    Task DeleteUrlAsync(ShortUrlModel model);
}
