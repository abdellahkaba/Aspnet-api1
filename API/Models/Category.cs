using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        // Propriété de navigation pour les produits
        public ICollection<Product>? Products { get; set; }
    }
}
