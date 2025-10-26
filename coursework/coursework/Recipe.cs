using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coursework
{
    public class Recipe
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Category { get; set; }
        public string CookingTime { get; set; }
        public string Ingredients { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
