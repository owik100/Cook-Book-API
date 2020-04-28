using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Cook_Book_API.Data;
using Cook_Book_API.Data.DbModels;
using Cook_Book_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;

        public UserController(ApplicationDbContext context, ILogger<UserController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        //GET api/User/
        public LoggedUserModel GetUserInfo()
        {
            LoggedUserModel output = new LoggedUserModel();

            try
            {
                //Znajdź "zalogowanego" usera (token) po ID.
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _context.Users.Find(userId);

                output = _mapper.Map<LoggedUserModel>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Got exception.");
            }

            return output;
        }

        // PUT: api/User/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, [FromForm]LoggedUserModel loggedUserModel)
        {
            try
            {
                if (id.ToString() != loggedUserModel.Id)
                {
                    _logger.LogWarning($"User: {id} not found");
                    return BadRequest();
                }

                string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _context.Users.Find(UserId) as ApplicationUser;

                user.FavouriteRecipes = loggedUserModel.FavouriteRecipes;

                _context.Entry(user).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!UserExists(id.ToString()))
                    {
                        _logger.LogError(ex, "Got exception.");
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(ex, "Got exception.");
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Got exception.");
                throw;
            }

            return Ok();
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}