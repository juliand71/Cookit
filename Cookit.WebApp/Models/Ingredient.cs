using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cookit.WebApp.Models
{
    public class Ingredient
    {
        public int IngredientID { get; set; }
        public string Name { get; set; }

        public List<IngredientAmount> IngredientAmounts { get; set; }
    }
}
