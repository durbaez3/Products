using System;
namespace Products.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastPurchase { get; set; }
        public double Stock { get; set; }
        public string Remarks { get; set; }
        public string ImageFullPath 
        {
            get
            {
                if (!string.IsNullOrEmpty(Image))
                {
                    return string.Format("https://productsbackendfab.azurewebsites.net/{0}",
                    Image.Substring(1));
                }
                return Image;
            } 
        }

    }
}
