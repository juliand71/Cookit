using Cookit.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public DateTime DatePosted { get; set; }

        /* Navigation Properties */
        public List<EquipmentRequirement> EquipmentRequirements { get; set; }
        public List<RecipeIngredient> RecipeIngredients{ get; set; }
        public List<Instruction> Instructions { get; set; }

        // Properties for User that Authored Recipe
        public string AuthorId { get; set; }
        public CookitUser Author { get; set; }

        // properties for ratings
        public List<Rating> Ratings { get; set; }

        [NotMapped]
        public double AverageScore
        {
            get
            {
                double sumOfRatings = 0;
                double average = 0;
                if (Ratings == null)
                {
                    return average;
                }
                else
                {
                    foreach(var rating in Ratings)
                    {
                        sumOfRatings += rating.Score;
                    }
                    average = sumOfRatings / Ratings.Count;
                    return average;
                }
            }
        }

        // properties for comments
        public List<RecipeComment> Comments { get; set; }
    }
}
