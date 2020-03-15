using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cook_Book_API.Models
{
    public class RecipeAPIModel
    {
        public string Name { get; set; }
        public IEnumerable<string> Ingredients { get; set; }
        public string Instruction { get; set; }

        public string Image { get; set; }
    }
}
