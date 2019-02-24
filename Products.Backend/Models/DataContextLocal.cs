namespace Products.Backend.Models
{
    using Products.Domain;
    using System.Data.Entity;

    public class DataContextLocal : DataContext
    {
        public System.Data.Entity.DbSet<Products.Domain.Customer> Customers { get; set; }
    }
}