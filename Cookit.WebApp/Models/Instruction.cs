using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cookit.WebApp.Models
{
    public class Instruction
    {
        public int InstructionID { get; set; }
        public int Step { get; set; }
        public string Description { get; set; }
        public int RecipeID { get; set; }

        /* Navigation Property */
        public Recipe Recipe { get; set; }
    }
}
