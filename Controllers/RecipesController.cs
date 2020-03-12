using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cook_Book_API.Data;
using Cook_Book_API.Models;
using System.Security.Claims;

namespace Cook_Book_API.Controllers
{
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
        public async Task<ActionResult<IEnumerable<Recipes>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/Recipes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Recipes>> GetRecipes(int id)
        {
            var recipes = await _context.Products.FindAsync(id);

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
        public async Task<IActionResult> PutRecipes(int id, Recipes recipes)
        {
            if (id != recipes.RecipesId)
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
        public async Task<ActionResult<Recipes>> PostRecipes(Recipes recipes)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            recipes.UserId = UserId;
            _context.Products.Add(recipes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecipes", new { id = recipes.RecipesId }, recipes);
        }

        // DELETE: api/Recipes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Recipes>> DeleteRecipes(int id)
        {
            var recipes = await _context.Products.FindAsync(id);
            if (recipes == null)
            {
                return NotFound();
            }

            _context.Products.Remove(recipes);
            await _context.SaveChangesAsync();

            return recipes;
        }

        private bool RecipesExists(int id)
        {
            return _context.Products.Any(e => e.RecipesId == id);
        }

        [HttpGet]
        [Route("CurrentUserRecipes")]
        //GET api/User/
        public List<Recipes> GetUserRecipes()
        {
            List<Recipes> output = new List<Recipes>();

            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier); //RequestContext.Principal.Identity.GetUserId();



            output = _context.Products.Where(x => x.UserId == UserId).ToList();
            

            return output;
        }
    }
}

