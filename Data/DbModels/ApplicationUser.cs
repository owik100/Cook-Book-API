using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cook_Book_API.Data.DbModels
{
    public class ApplicationUser : IdentityUser
    {
        public List<Recipe> Recipes { get; set; }
    }
}
