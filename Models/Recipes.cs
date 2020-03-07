﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cook_Book_API.Models
{
    public class Recipes
    {
        public int RecipesId { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Ingredients { get; set; }
        public string Instruction { get; set; }
        public string NameOfImage { get; set; }
    }
}
