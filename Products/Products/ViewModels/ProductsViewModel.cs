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

        #region Constructor
        public ProductsViewModel(List<Product> produts)
        {
            this.products = produts;
            Products = new ObservableCollection<Product>(produts.OrderBy(p => p.Description));
        }
        #endregion
    }
}
