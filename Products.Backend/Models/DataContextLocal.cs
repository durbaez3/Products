namespace Products.Backend.Models
{
    using Products.Domain;
    using System.Data.Entity;

    public class DataContextLocal : DataContext
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }
    }
}