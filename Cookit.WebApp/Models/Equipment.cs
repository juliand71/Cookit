using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cookit.WebApp.Models
{
    public class Equipment
    {
        public int EquipmentID { get; set; }
        public string Name { get; set; }

        public ICollection<RecipeEquipment> RecipeEquipment { get; set; }
    }
}
