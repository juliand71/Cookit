namespace Cookit.WebApp.Models
{
    public class RecipeEquipment
    {
        public int RecipeEquipmentID { get; set; }
        public int RecipeID { get; set; }
        public int EquipmentID { get; set; }

        public Recipe Recipe { get; set; }
        public Equipment Equipment { get; set; }
    }
}