﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cook_Book_API.Data;
using Cook_Book_API.Data.DbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            
            
                var Usert = _context.Users.Find(UserId);

                output.Email = Usert.Email;
                output.Id = Usert.Id;
                output.UserName = Usert.UserName;
            

            return output;
        }
    }
}