using ApiForUrl.DataAccess.Entities;
using ApiForUrl.DataAccess.Interfaces;
using ApiForUrl.DataAccess.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ApiForUrl.DataAccess.Repositories;

public class ShortUrlRepository(AppDbContext appDbContext,
                                UserManager<User> user) : IShortUrlInterface
{
    private readonly AppDbContext _DbContext = appDbContext;
    private readonly UserManager<User> _userManager = user;

    public async Task<ShortUrlModel> CreateLinkAsync(AddViewModel addViewModel)
    {
        var oldUrl = _DbContext.shortUrlModels.FirstOrDefault(x => x.OrginalUrl == addViewModel.Link);

        if (oldUrl != null)
        {
            return oldUrl;
        }
        var check = await _userManager.Users.FirstOrDefaultAsync(i => i.Id == addViewModel.UserId);
        if (check == null)
        {
            throw new ArgumentNullException("User not found!");
        }
        start:
        string shortUrl = GenerateUrl();

        if (IsNotExist(shortUrl))
        {
            ShortUrlModel model = new ShortUrlModel()
            {
                OrginalUrl = addViewModel.Link,
                ShortUrl = "https://brogrammers.fn1.uz/" + shortUrl,
                UserId = addViewModel.UserId,
                User = null
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
        throw new Exception("This url not found!");
    }
    public async Task<List<ShortUrlModel>> GetUsersUrls(string userId)
    {
        var urls = await _DbContext.shortUrlModels.Where(i => i.UserId == userId).ToListAsync();
        if (urls.Count() == 0)
        {
            throw new ArgumentNullException("Urls not found!");
        }

        return urls;
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
