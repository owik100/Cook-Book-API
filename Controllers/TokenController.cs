using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Cook_Book_API.Data;
using Cook_Book_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Cook_Book_API.Controllers
{
    public class TokenController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;
        private readonly ILogger _logger;
        private IdentityUser _user;

        public TokenController(UserManager<IdentityUser> userManager, IConfiguration config, ILogger<TokenController> logger)
        {
            _userManager = userManager;
            _config = config;
            _logger = logger;
        }

        //POST  /token
        [Route("/token")]
        [HttpPost]
        public async Task<ActionResult<TokenModel>> GetToken(string username, string password)
        {
            if (await IsValidUsernameAndPassword(username, password))
            {
                _logger.LogInformation($"UserName {username}: Login attempt successful");
                return GenerateToken();
            }
            else
            {
                _logger.LogWarning($"UserName {username}: Login attempt failed");
                return BadRequest(new { message = "Podany login lub hasło jest błędne" });
            }
        }

        private async Task<bool> IsValidUsernameAndPassword(string username, string password)
        {
            _user = await _userManager.FindByNameAsync(username);
            return await _userManager.CheckPasswordAsync(_user, password);
        }

        private TokenModel GenerateToken()
        {
            TokenModel output = new TokenModel();
            try
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

                output.Access_Token = tokenString;
                output.UserName = _user.UserName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Got exception.");
                throw;
            }

            return output;
        }
    }
}