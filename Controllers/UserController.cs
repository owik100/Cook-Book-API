using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cook_Book_API.Data;
using Cook_Book_API.Data.DbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cook_Book_API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public UserController(ApplicationDbContext context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        //GET api/User/
        public ApplicationUser GetUserInfo()
        {
            ApplicationUser output = new ApplicationUser();

            try
            {
                //Znajdź "zalogowanego" usera (token) po ID.
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _context.Users.Find(userId);

                output.Email = user.Email;
                output.Id = user.Id;
                output.UserName = user.UserName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Got exception.");
            }

            return output;
        }
    }
}