using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cookit.WebApp.Models
{
    public class IngredientAmount
    {
        public int IngredientAmountID { get; set; }
        public int IngredientID { get; set; }
        public int RecipeID { get; set; }
        public string Unit { get; set; }
        public int Amount { get; set; }

        /* Navigation */
        public Recipe Recipe { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
