using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cook_Book_API.Models
{
    public class LoggedUserModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }
        public List<string> FavouriteRecipes { get; set; }
    }
}
