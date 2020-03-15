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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;
using Cook_Book_API.Models;

namespace Cook_Book_API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly IHostEnvironment _hostEnvironment;

        public RecipesController(ApplicationDbContext context, IConfiguration config, IHostEnvironment hostEnvironment)
        {
            _context = context;
            _config = config;
            _hostEnvironment = hostEnvironment;
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
        public async Task<ActionResult<RecipeAPIModel>> PostRecipes(RecipeAPIModel recipe)
        {
            Recipe recipeDb = new Recipe();

            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (recipe.Image != null)
            {


               // var filename = file.FileName;
               // var imagesPhotoPath = ConfigurationManager.AppSetting["ProductsImagePath"];
               // var rootFolderPath = _environment.WebRootPath;
               //var relativePath = imagesPhotoPath + filename;
              //  var path = rootFolderPath + relativePath;

                var filename = Guid.NewGuid() + ".jpeg";
                var imagesPhotoPath = _config["ImagePath"];
                var rootFolderPath = _hostEnvironment.ContentRootPath + "\\wwwroot";
                var relativePath = imagesPhotoPath + filename ;
                var path = rootFolderPath + relativePath;


                Save(path, recipe.Image);

                using (var photoFile = new FileStream(path, FileMode.Create))
                {
                    //recipe.Image.CopyTo(photoFile);
                }

                recipeDb.UserId = UserId;
                recipeDb.NameOfImage = filename.ToString();
                recipeDb.Name = recipe.Name;
                recipeDb.Instruction = recipe.Instruction;
                recipeDb.Ingredients = recipe.Ingredients;

          
            }


            _context.Recipes.Add(recipeDb);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostRecipes", new { id = recipeDb.RecipeId }, recipe);
        }

        private void Save(string path, string imageString)
        {
            try
            {

                //path = @"D:\Projects\AllInOne\Cook-Book\Cook-Book-API\wwwroot\images\kot.jpeg";

                var imageDataByteArray = Convert.FromBase64String(imageString);
                var imageDataStream = new MemoryStream(imageDataByteArray);
                imageDataStream.Position = 0;

                //using (FileStream file = new FileStream(path, FileMode.Create, System.IO.FileAccess.Write))
                //{
                //    byte[] bytes = new byte[imageDataStream.Length];
                //    imageDataStream.Read(bytes, 0, (int)imageDataStream.Length);
                //    file.Write(bytes, 0, bytes.Length);
                //    imageDataStream.Close();
                //}

                //using (FileStream file = new FileStream(path, FileMode.Create, System.IO.FileAccess.Write))
                //    imageDataStream.CopyTo(file);

                System.IO.File.WriteAllBytes(path, imageDataByteArray);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

          
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
        //GET /api/Recipes/CurrentUserRecipes
        public List<Recipe> GetUserRecipes()
        {
            List<Recipe> output = new List<Recipe>();

            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            output = _context.Recipes.Where(x => x.UserId == UserId).ToList();
            
            return output;
        }
    }
}

