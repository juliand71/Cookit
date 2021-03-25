using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cookit.Models
{
    public class EquipmentRequirement
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public int EquipmentId { get; set; }
        public Equipment Equipment { get; set; }

    }
}
