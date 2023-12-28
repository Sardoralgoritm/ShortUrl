using ApiForUrl.DataAccess.Entities;
using ApiForUrl.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ApiForUrl.DataAccess.Repositories;

public class ShortUrlRepository(AppDbContext appDbContext) : IShortUrlInterface
{
    private readonly AppDbContext _DbContext = appDbContext;

    public async Task<ShortUrlModel> CreateLinkAsync(string link)
    {
        var oldUrl = _DbContext.shortUrlModels.FirstOrDefault(x => x.OrginalUrl == link);

        if (oldUrl != null)
        {
            return oldUrl;
        }

        start:
        string shortUrl = GenerateUrl();

        if (IsNotExist(shortUrl))
        {
            ShortUrlModel model = new ShortUrlModel()
            {
                OrginalUrl = link,
                ShortUrl = "https://brogrammers.fn1.uz/" + shortUrl,
                UserId = "1"
            };
            _DbContext.shortUrlModels.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }
        else
        {
            goto start;
        }
    }

    public async Task<ShortUrlModel> GetByShortUrl(string link)
    {
        string shortUrl = link.Replace("%2F", "/");
        var url = await _DbContext.shortUrlModels.FirstOrDefaultAsync(x => x.ShortUrl == shortUrl);
        if (url != null)
        {
            return url;
        }
        return new ShortUrlModel();
    }

    private string GenerateUrl()
    {
        string letters = "MASDFGHJKLQWERTYUIOPzxcvbnmasdfghjklqwertyuiop-123456789";
        StringBuilder result = new StringBuilder();
        Random random = new Random();
        for (int i = 0; i < 4; i++)
        {
            result.Append(letters[random.Next(0, 50)]);
        }

        return result.ToString();
    }

    private bool IsNotExist(string shortUrl)
        => !_DbContext.shortUrlModels.Any(i => i.ShortUrl == shortUrl);
}
