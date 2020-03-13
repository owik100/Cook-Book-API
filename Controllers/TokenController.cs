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

        public TokenController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
        }

        [Route("/token")]
        [HttpPost]
        public async Task<IActionResult> Create(string username, string password, string grant_type)
        {
            if(await IsValidUsernameAndPassword(username, password))
            {
                return new ObjectResult(await GenerateToken(username));
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<bool> IsValidUsernameAndPassword(string username, string password)
        {
            var user = await _userManager.FindByEmailAsync(username);
            return await _userManager.CheckPasswordAsync(user, password);
        }

        private async Task<dynamic> GenerateToken(string username)
        {
            //var user = await _userManager.FindByEmailAsync(username);

            //var claims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Name, username),
            //    new Claim(ClaimTypes.NameIdentifier, user.Id),
            //    new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
            //    new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now.AddDays(10)).ToUnixTimeSeconds().ToString())
            //};

            //var token = new JwtSecurityToken(
            //    new JwtHeader(
            //        new SigningCredentials(
            //            new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecretKeyIsSecretDoNotTellAnymorePLISSS")),
            //        SecurityAlgorithms.HmacSha256)),
            //    new JwtPayload(claims));

            //var output = new
            //{
            //    Access_Token = new JwtSecurityTokenHandler().WriteToken(token),
            //    UserName = username
            //};

            //return output;


            var user = await _userManager.FindByEmailAsync(username);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config.GetValue<string>("Secrets:SecurityKey"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
    
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token

            var outpuy = new
            {
                Access_Token = tokenString,
                UserName = username
            };
            return outpuy;



        }
    }
}