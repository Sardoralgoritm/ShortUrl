using ApiForUrl.DataAccess;
using ApiForUrl.DataAccess.Entities;
using ApiForUrl.DataAccess.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ApiForUrl.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController(UserManager<User> user,
                                      RoleManager<IdentityRole> roleManager,
                                      AppDbContext context,
                                      IConfiguration configuration) : ControllerBase
{
    private readonly UserManager<User> _userManager = user;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly AppDbContext _context = context;
    private readonly IConfiguration _configuration = configuration;

    [HttpPost("register-user")]
    public async Task<IActionResult> Register([FromBody]RegisterVM registerVM)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Please, provide all the required fields");
        }

        var userExists = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
        if (userExists != null)
        {
            return BadRequest($"User {registerVM.EmailAddress} is already exist");
        }

        User newUser = new()
        {
            FirstName = registerVM.FirstName,
            LastName = registerVM.LastName,
            Email = registerVM.EmailAddress,
            UserName = registerVM.UserName,
            SecurityStamp = Guid.NewGuid().ToString(),
        };

        var result = await _userManager.CreateAsync(newUser, registerVM.Password);


        if (result.Succeeded) return Ok("User created");
        return BadRequest("User could not be created");
    }

    [HttpPost("login-user")]
    public async Task<IActionResult> Login([FromBody] LoginVM loginVM)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Please, provide all the required fields");
        }

        var userExists = await _userManager.FindByEmailAsync(loginVM.EmailAddress);

        if (userExists != null && await _userManager.CheckPasswordAsync(userExists, loginVM.Password))
        {
            var tokenValue = await GenerateJWTTokenAsync(userExists);
            return Ok(tokenValue);
        }

        return Unauthorized();
    }

    private async Task<AuthResultVM> GenerateJWTTokenAsync(User user)
    {
        var authClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            expires: DateTime.UtcNow.AddMinutes(1),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        var response = new AuthResultVM()
        {
            Token = jwtToken,
            ExpiresAt = token.ValidTo
        };

        return response;
    }
}
