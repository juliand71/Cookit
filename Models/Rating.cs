﻿using Cookit.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cookit.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public string UserId { get; set; }

        [Range(1, 5)]
        public int Score { get; set; }

        public Recipe Recipe { get; set; }
        public CookitUser User { get; set; }
    }
}
