using System;


namespace Products.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using Models;

    public class ProductsViewModel : INotifyPropertyChanged
    {
        List<Product> products;
        public event PropertyChangedEventHandler PropertyChanged;

        #region Attributes
        public ObservableCollection<Product> _produts;

        #endregion

        #region Properties
        public ObservableCollection<Product> Products
        {
            get
            {
                return _produts;
            }
            set
            {
                if (_produts != value)
                {
                    _produts = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Products)));
                }
            }
        }
        #endregion

        #region Methods
        public void AddProduct(Product product)
        {
            products.Add(product);
            Products = new ObservableCollection<Product>(
                products.OrderBy(c => c.Description));
        }

        public void UpdateProduct(Product product)
        {
            //IsRefreshing = true;
            var oldProduct = products.
                Where(c => c.ProductId == product
                .ProductId).FirstOrDefault();


            oldProduct = product;
            Products = new ObservableCollection<Product>(
                products.OrderBy(c => c.Description));
            //IsRefreshing = false;
        }
        #endregion
        #region Constructor
        public ProductsViewModel(List<Product> produts)
        {
            instance = this;
            this.products = produts;
            Products = new ObservableCollection<Product>(produts.OrderBy(p => p.Description));
        }

        public ProductsViewModel()
        {
            instance = this;
        }
        #endregion

        #region Singleton
        public static ProductsViewModel instance;

        public static ProductsViewModel GetInstance()
        {
            if (instance == null)
            {
                return new ProductsViewModel();
            }
            return instance;
        }
        #endregion
    }
}
