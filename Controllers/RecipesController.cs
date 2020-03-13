using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cook_Book_API.Data;
using Cook_Book_API.Data.DbModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Cook_Book_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RecipesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Recipes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetProducts()
        {
            return await _context.Recipes.ToListAsync();
        }

        // GET: api/Recipes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipes(int id)
        {
            var recipes = await _context.Recipes.FindAsync(id);

            if (recipes == null)
            {
                return NotFound();
            }

            return recipes;
        }

        // PUT: api/Recipes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipes(int id, Recipe recipes)
        {
            if (id != recipes.RecipeId)
            {
                return BadRequest();
            }

            _context.Entry(recipes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Recipes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Recipe>> PostRecipes(Recipe recipes)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            recipes.UserId = UserId;
            _context.Recipes.Add(recipes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecipes", new { id = recipes.RecipeId }, recipes);
        }

        // DELETE: api/Recipes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Recipe>> DeleteRecipes(int id)
        {
            var recipes = await _context.Recipes.FindAsync(id);
            if (recipes == null)
            {
                return NotFound();
            }

            _context.Recipes.Remove(recipes);
            await _context.SaveChangesAsync();

            return recipes;
        }

        private bool RecipesExists(int id)
        {
            return _context.Recipes.Any(e => e.RecipeId == id);
        }

        [HttpGet]
        [Route("CurrentUserRecipes")]
        //GET api/User/
        public List<Recipe> GetUserRecipes()
        {
            List<Recipe> output = new List<Recipe>();

            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier); //RequestContext.Principal.Identity.GetUserId();



            output = _context.Recipes.Where(x => x.UserId == UserId).ToList();
            

            return output;
        }
    }
}

