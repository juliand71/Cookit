using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cookit.Models
{
    public class RecipeIngredient
    {
        public int Id { get; set; }
        public string Unit { get; set; }
        public double Amount { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

    }
}
