namespace API.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }

        // Clé étrangère pour Category
        public int CategoryId { get; set; }

        // Propriété de navigation vers Category
        public Category? Category { get; set; }
    }
}
