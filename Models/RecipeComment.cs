using Cookit.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cookit.Models
{
    public class RecipeComment
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public string UserId { get; set; }

        public string Text { get; set; }

        public Recipe Recipe { get; set; }
        public CookitUser User { get; set; }
    }
}
