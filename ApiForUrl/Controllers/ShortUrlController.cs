using ApiForUrl.DataAccess.Entities;
using ApiForUrl.DataAccess.Interfaces;
using ApiForUrl.DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiForUrl.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class ShortUrlController(IShortUrlInterface shortUrlInterface) : ControllerBase
{
    private readonly IShortUrlInterface shortUrlInterface = shortUrlInterface;
    [HttpPost("create")]
    public async Task<IActionResult> CreateUrl([FromBody]string link)
    {
        var result = await shortUrlInterface.CreateLinkAsync(link);
        return Ok(result.ShortUrl);
    }

    [HttpGet("get/{url}")]
    public async Task<IActionResult> GetUrl(string url)
    {
        var urlModel = await shortUrlInterface.GetByShortUrl(url);
        string orginalUrl = urlModel.OrginalUrl;
        return Ok(orginalUrl);
    }
}