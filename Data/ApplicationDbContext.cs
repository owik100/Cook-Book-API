using System;
using System.Collections.Generic;
using System.Text;
using Cook_Book_API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Cook_Book_API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Recipes> Products { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var splitStringConverter = new ValueConverter<IEnumerable<string>, string>(v => string.Join(";", v), v => v.Split(new[] { ';' }));
            builder.Entity<Recipes>().Property(nameof(Recipes.Ingredients)).HasConversion(splitStringConverter);


            List<Recipes> recipes = new List<Recipes>
            {
                   new Recipes(){RecipesId =1, Name="Frytki", Ingredients = new List<string>{"Ziemniaki", "Sól"}, Instruction ="Pokrój i usmaż ziemniaki. Posól." },
                   new Recipes(){RecipesId =2, Name="Kanapka", Ingredients = new List<string>{"Chleb", "Masło"}, Instruction ="Posmaruj chleb masłem." },
            };

            builder.Entity<Recipes>().HasData(recipes);
        }
    }
}
