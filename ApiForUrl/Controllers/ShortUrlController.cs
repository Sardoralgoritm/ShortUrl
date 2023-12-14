using ApiForUrl.DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiForUrl.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ShortUrlController(IShortUrlInterface shortUrlInterface) : ControllerBase
{
    private readonly IShortUrlInterface shortUrlInterface = shortUrlInterface;
    [HttpPost("create")]
    public async Task<IActionResult> CreateUrl([FromBody]string link)
    {
        var result = await shortUrlInterface.CreateLinkAsync(link);
        return Ok(result.ShortUrl);
    }

    [HttpGet("get")]
    public async Task<IActionResult> Go()
    {
        return Ok("Finally");
    }
}
