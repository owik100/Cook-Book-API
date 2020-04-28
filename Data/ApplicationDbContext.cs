using System;
using System.Collections.Generic;
using System.Text;
using Cook_Book_API.Data.DbModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Cook_Book_API.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Recipe> Recipes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var splitStringConverter = new ValueConverter<IEnumerable<string>, string>(v => string.Join(";", v), v => v.Split(new[] { ';' }));
            builder.Entity<Recipe>().Property(nameof(Recipe.Ingredients)).HasConversion(splitStringConverter);
            builder.Entity<ApplicationUser>().Property(nameof(ApplicationUser.FavouriteRecipes)).HasConversion(splitStringConverter);


            List<Recipe> recipes = new List<Recipe>
            {
                   new Recipe(){RecipeId =1, Name="Frytki", Ingredients = new List<string>{"Ziemniaki", "Sól"}, Instruction ="Pokrój i usmaż ziemniaki. Posól." },
                   new Recipe(){RecipeId =2, Name="Kanapka", Ingredients = new List<string>{"Chleb", "Masło"}, Instruction ="Posmaruj chleb masłem.", UserId="7aa6bef9-e1ec-4c38-b70b-89270d1e6a25" },
            };

            builder.Entity<Recipe>().HasData(recipes);
        }
    }
}
