using Cookit.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cookit.Models
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageFile { get; set; }

        /* Navigation Properties */
        public List<EquipmentRequirement> EquipmentRequirements { get; set; }
        public List<RecipeIngredient> RecipeIngredients{ get; set; }
        public List<Instruction> Instructions { get; set; }

        // Properties for User that Authored Recipe
        public string AuthorId { get; set; }
        public CookitUser Author { get; set; }
    }
}
