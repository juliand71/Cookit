﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cookit.WebApp.Models
{
    public class Recipe
    {
        public int RecipeID { get; set; }

        // Owner - name for the User that Submitted the Recipe
        public string Owner { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageFileName { get; set; }
        public string ImageCaption { get; set; }


        [DisplayName("Ingredients")]
        public List<IngredientAmount> IngredientAmounts { get; set; }
        public List<Instruction> Instructions { get; set; }
        [DisplayName("Equipment")]
        public List<RecipeEquipment> RecipeEquipment { get; set; }
    }
}
