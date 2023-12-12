using ApiForUrl.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiForUrl.DataAccess.Entities.Repositories;

public class ShortUrlRepository(AppDbContext appDbContext) : IShortUrlInterface
{
    private readonly AppDbContext _DbContext = appDbContext;

    public async Task AddUrlAsync(ShortUrlModel model)
    {
        _DbContext.Add(model);
        await _DbContext.SaveChangesAsync();
    }

    public async Task DeleteUrlAsync(ShortUrlModel model)
    {
        _DbContext.Remove(model);
        await _DbContext.SaveChangesAsync();
    }

    public async Task<List<ShortUrlModel>> GetAllUrlAsync()
        => await _DbContext.shortUrlModels.ToListAsync();

    public async Task<ShortUrlModel> GetByIdUrlAsync(int id)
        => await _DbContext.shortUrlModels.FirstOrDefaultAsync(i => i.Id == id) ?? new ShortUrlModel();

    public async Task UpdateUrlAsync(ShortUrlModel model)
    {
        _DbContext.Update(model);
        await _DbContext.SaveChangesAsync();
    }
}
