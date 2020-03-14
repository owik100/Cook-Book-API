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


        public UserController(ApplicationDbContext context)
        {
            _context = context;

        }

        [HttpGet]
        //GET api/User/
        public ApplicationUser GetById()
        {
            ApplicationUser output = new ApplicationUser();


            //Znajdź "zalogowanego" usera (token) po ID.
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = _context.Users.Find(userId);

            output.Email = user.Email;
            output.Id = user.Id;
            output.UserName = user.UserName;

            return output;
        }
    }
}