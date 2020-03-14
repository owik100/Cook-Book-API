using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Cook_Book_API.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Cook_Book_API.Controllers
{
    public class TokenController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;

        private IdentityUser _user;

        public TokenController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
        }

        // /token
        [Route("/token")]
        [HttpPost]
        public async Task<IActionResult> GetToken(string username, string password)
        {
            if(await IsValidUsernameAndPassword(username, password))
            {
                return new ObjectResult(GenerateToken());
            }
            else
            {
                return BadRequest(new { message = "Podany login lub hasło jest błędne" });
            }
        }

        private async Task<bool> IsValidUsernameAndPassword(string username, string password)
        {
            _user = await _userManager.FindByNameAsync(username);
            return await _userManager.CheckPasswordAsync(_user, password);
        }

        private dynamic GenerateToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config.GetValue<string>("Secrets:SecurityKey"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
    
                    new Claim(ClaimTypes.Name, _user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, _user.Id),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var output = new
            {
                Access_Token = tokenString,
                UserName = _user.UserName
            };
            return output;
        }
    }
}