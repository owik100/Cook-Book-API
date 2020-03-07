using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cook_Book_API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Recipes> Recipes { get; set; }
    }
}
