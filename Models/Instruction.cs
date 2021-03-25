namespace Cookit.Models
{
    public class Instruction
    {
        public int Id { get; set; }
        public int Step { get; set; }
        public string Text { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}