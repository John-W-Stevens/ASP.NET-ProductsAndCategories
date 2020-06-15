using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductsAndCategories.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        // One-To-Many (One-side) nav property goes here <<

        // One-To-Many (Many-side) nav property goes here <<


        public List<Association> Products { get; set; }
        // Many-To-Many nav property goes here <<

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }
    }
}

