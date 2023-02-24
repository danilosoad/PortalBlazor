using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PortalBlazor.Core.Enum;
using PortalBlazor.Core.Models;

using PortalBlazor.Server.Util.Token;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using System.Text;

namespace PortalBlazor.Server.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _sigInManager;
        private readonly IConfiguration _configuration;

        public LoginController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> sigInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _sigInManager = sigInManager;
            _configuration = configuration;
        }

        [HttpGet]
        public string Get()
        {
            return $"Login Controller -- {DateTime.Now.ToShortTimeString()}";
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserInfo model)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (model.Role == (int)RoleEnum.Admin)
                    await _userManager.AddToRoleAsync(user, "Admin");
                else
                    await _userManager.AddToRoleAsync(user, "User");

                return Ok(await GenerateToken(model));
            }
            else
                return BadRequest(new { message = "invalid credentials" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserInfo model)
        {
            var result = await _sigInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
                return Ok(await GenerateToken(model));
            else
                return BadRequest(new { message = "invalid credentials" });
        }

        private async Task<UserToken> GenerateToken(UserInfo model)
        {
            var user = await _sigInManager.UserManager.FindByEmailAsync(model.Email);
            var roles = await _sigInManager.UserManager.GetRolesAsync(user);
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, user.Email));

            foreach (var item in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JwtKey").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddHours(2);
            var message = "JWT Token Created";

            var token = new JwtSecurityToken(
                            issuer: null,
                            audience: null,
                            claims: claims,
                            expires: expiration,
                            signingCredentials: creds);

            return new UserToken() { Token = new JwtSecurityTokenHandler().WriteToken(token), Expiration = expiration, Message = message };
        }
    }
}