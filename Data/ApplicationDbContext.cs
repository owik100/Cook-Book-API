using System;
using System.Collections.Generic;
using System.Text;
using Cook_Book_API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Cook_Book_API.Data
{
    public class ApplicationDbContext : IdentityDbContext
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
        }
    }
}
