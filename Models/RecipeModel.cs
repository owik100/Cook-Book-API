using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cook_Book_API.Models
{
    public class RecipeModel : PaginationModel
    {
        public string RecipeId { get; set; }    
        public string Name { get; set; }
        public IEnumerable<string> Ingredients { get; set; }
        public string Instruction { get; set; }
        public string NameOfImage { get; set; }
        public bool IsPublic { get; set; }
        public string UserName { get; set; }

        public IFormFile Image { get; set; }
    }
}
