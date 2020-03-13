using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cook_Book_API.Data.DbModels;
using Cook_Book_API.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cook_Book_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            try
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //_logger.LogInformation("User created a new account with password.");
                    return Ok();
                }
                else
                {
                    return BadRequest(new { message = string.Join("\n", result.Errors.Select(x => "Code " + x.Code + " Description" + x.Description))});
                }
            }
            catch (Exception ex)
            {
                if(ex.Message == "Sequence contains more than one element")
                {
                    return BadRequest(new { message = "Podany adres email już istnieje!" });
                }
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

