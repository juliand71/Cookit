using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Cookit.WebApp.Models
{
    public class Recipe
    {
        public int RecipeID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [DisplayName("Ingredients")]
        public ICollection<IngredientAmount> IngredientAmounts { get; set; }
        public ICollection<Instruction> Instructions { get; set; }
        public ICollection<RecipeEquipment> RecipeEquipment { get; set; }
    }
}
