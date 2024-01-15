using ApiForUrl.DataAccess.Entities;
using ApiForUrl.DataAccess.Interfaces;
using ApiForUrl.DataAccess.ViewModels;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateUrl(AddViewModel viewModel)
    {
        try
        {
            var result = await shortUrlInterface.CreateLinkAsync(viewModel);
            return Ok(result.ShortUrl);
        }
        catch(ArgumentNullException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetUrl([FromQuery] ViewModel viewModel)
    {
        try
        {
            var urlModel = await shortUrlInterface.GetByShortUrl(viewModel.Link);
            string orginalUrl = urlModel.OrginalUrl;
            return Ok(orginalUrl);
        }
        catch(Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("get-users-url")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetUsersUrls([FromQuery] ViewModelForUserId viewModel)
    {
        try
        {
            var urls = await shortUrlInterface.GetUsersUrls(viewModel.UserId);
            var result = urls.Select(i => (ShortUrlDto)i).ToList();
            return Ok(result);
        }
        catch(ArgumentNullException ex)
        {
            return NotFound(ex.Message);
        }
    }
}