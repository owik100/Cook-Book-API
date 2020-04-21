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
using Microsoft.Extensions.Logging;
using Cook_Book_API.Interfaces;
using AutoMapper;

namespace Cook_Book_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly IImageHelper _imageHelper;
        private readonly IMapper _mapper;

        public RecipesController(ApplicationDbContext context, ILogger<RecipesController> logger, IImageHelper imageHelper, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _imageHelper = imageHelper;
            _mapper = mapper;
        }

        // GET: api/Recipes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeAPIModel>> GetRecipe(int id)
        {
            var output = new RecipeAPIModel();
            try
            {
                string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var recipes = await _context.Recipes.FindAsync(id);

                if (recipes == null)
                {
                    return NotFound();
                }

                else if (recipes.UserId != UserId)
                {
                    return NotFound();
                }

                output = _mapper.Map<RecipeAPIModel>(recipes);

                string userName = _context.Users
                   .Where(x => x.Id == UserId)
                   .Select(y => y.UserName)
                   .SingleOrDefault();

                output.UserName = userName;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Got exception.");
                throw;
            }

            return output;
        }

        // GET: api/Recipes/GetPublicRecipes
        [HttpGet]
        [Route("GetPublicRecipes")]
        public  ActionResult<List<RecipeAPIModel>> GetPublicRecipes()
        {

             List<RecipeAPIModel> output = new List<RecipeAPIModel>();

            try
            {
                var recipesDB = _context.Recipes.Where(x => x.IsPublic == true).ToList();

                foreach (var item in recipesDB)
                {

                    RecipeAPIModel singleRecipeModel = _mapper.Map<RecipeAPIModel>(item);


                    string userName = _context.Users
                   .Where(x => x.Id == item.UserId)
                   .Select(y => y.UserName)
                   .SingleOrDefault();

                    singleRecipeModel.UserName = userName;

                    output.Add(singleRecipeModel);

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Got exception.");
                throw;
            }

            return output;
        }

        //GET /api/Recipes/CurrentUserRecipes
        [HttpGet]
        [Route("CurrentUserRecipes")]
        public ActionResult<List<RecipeAPIModel>> GetUserRecipes()
        {
            List<RecipeAPIModel> output = new List<RecipeAPIModel>();

            try
            {
                string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var recipesDB = _context.Recipes.Where(x => x.UserId == UserId).ToList();

                string userName = _context.Users
                  .Where(x => x.Id == UserId)
                  .Select(y => y.UserName)
                  .SingleOrDefault();

                foreach (var item in recipesDB)
                {

                    RecipeAPIModel singleRecipeModel = _mapper.Map<RecipeAPIModel>(item);

                    singleRecipeModel.UserName = userName;

                    output.Add(singleRecipeModel);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Got exception.");
                throw;
            }

            return output;
        }

        // POST: api/Recipes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<RecipeAPIModel>> PostRecipes([FromForm]RecipeAPIModel recipe)
        {
            Recipe recipeDb = new Recipe();
            try
            {
               
                recipeDb = _mapper.Map<Recipe>(recipe);

                string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                recipeDb.UserId = UserId;

                if (recipe.Image != null)
                {
                    var extension = Path.GetExtension(recipe.Image.FileName);
                    var filename = Guid.NewGuid() + extension;
                    string path = _imageHelper.GetImagePath(filename);
                    await SaveImage(path, recipe.Image);

                    recipeDb.NameOfImage = filename.ToString();
                }
                
                await _context.Recipes.AddAsync(recipeDb);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Got exception.");
                throw;
            }

            return Ok();
        }

        // PUT: api/Recipes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipes(int id, [FromForm]RecipeAPIModel recipe)
        {
            try
            {
                if (id.ToString() != recipe.RecipeId)
                {
                    _logger.LogWarning($"Recipe: {id} not found");
                    return BadRequest();
                }

                string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var oldRecipe = _context.Recipes.Where(x => x.RecipeId.ToString() == recipe.RecipeId).FirstOrDefault();

                if (!string.IsNullOrEmpty(recipe.NameOfImage))
                {
                    if (recipe.NameOfImage != oldRecipe.NameOfImage)
                    {
                        //Usun stare zdjecie - Jest nowe
                        DeleteImage(oldRecipe.NameOfImage);



                        var extension = Path.GetExtension(recipe.Image.FileName);
                        var filename = Guid.NewGuid() + extension;
                        string path = _imageHelper.GetImagePath(filename);
                        await SaveImage(path, recipe.Image);

                        oldRecipe.NameOfImage = filename.ToString();
                    }


                }

                if (string.IsNullOrEmpty(recipe.NameOfImage) && oldRecipe.NameOfImage != null)
                {
                    //Usun stare zdjecie - Usunieto je
                    DeleteImage(oldRecipe.NameOfImage);
                    oldRecipe.NameOfImage = null;
                }

                oldRecipe.RecipeId = id;
                oldRecipe.UserId = UserId;
                oldRecipe.Name = recipe.Name;
                oldRecipe.Instruction = recipe.Instruction;
                oldRecipe.Ingredients = recipe.Ingredients;
                oldRecipe.IsPublic = recipe.IsPublic;

                _context.Entry(oldRecipe).State = EntityState.Modified;
                //_context.Recipes.Update(oldRecipe);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!RecipesExists(id))
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

        // DELETE: api/Recipes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRecipes(int id)
        {
            try
            {
                var recipes = await _context.Recipes.FindAsync(id);
                if (recipes == null)
                {
                    _logger.LogWarning($"Not found recipe: {id}");
                    return NotFound();
                }

                _context.Recipes.Remove(recipes);
                await _context.SaveChangesAsync();

                if (recipes.NameOfImage != null)
                {
                    DeleteImage(recipes.NameOfImage);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Got exception.");
                throw;
            }

            return Ok();
        }

        //GET /api/Recipes/GetPhoto/abc15
        [HttpGet]
        [Route("GetPhoto/{id}")]
        public IActionResult GetPhoto(string id)
        {
            try
            {
                //Znajdz foto
                string photoName = _context.Recipes.Where(x => x.NameOfImage == id).Select(y => y.NameOfImage).FirstOrDefault();

                if (!string.IsNullOrEmpty(photoName))
                {

                    string path = _imageHelper.GetImagePath(id);

                    var stream = new FileStream(path, FileMode.Open);
                    return File(stream, "image/jpeg", photoName);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Got exception.");
                throw;
            }

            _logger.LogWarning($"Not found image: {id}");
            return NotFound();

        }

        private async Task SaveImage(string path, IFormFile image)
        {
            try
            {
                if (image.Length > 0)
                {
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Got exception.");
                throw;
            }
        }

        private void DeleteImage(string nameOfImage)
        {
            try
            {
                string path = _imageHelper.GetImagePath(nameOfImage);

                if ((System.IO.File.Exists(path)))
                {
                    System.IO.File.Delete(path);
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Got exception.");
                throw;
            }
        }

        private bool RecipesExists(int id)
        {
            return _context.Recipes.Any(e => e.RecipeId == id);
        }

        #region Unused
        //// GET: api/Recipes
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Recipe>>> GetProducts()
        //{
        //    return await _context.Recipes.ToListAsync();
        //}
        #endregion
    }
}

