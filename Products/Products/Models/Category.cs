namespace Products.Models
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Products.Services;
    using Products.ViewModels;

    public class Category
    {
        #region Services
        NavigationService navigationService;
        #endregion
        #region Properties
        public int CategoryId { get; set; }

        public string Description { get; set; }

        public List<Product> Products { get; set; }
        #endregion

        #region Commands
        public ICommand SelectCategoryCommand 
        { 
            get
            {
                return new RelayCommand(SelectCategory)
;            }
        }

        async void SelectCategory()
        {
            MainViewModel.GetInstance().Products = new ProductsViewModel(Products);
            await navigationService.Navigate("ProductsView");
        }
        #endregion

        #region Constructors
        public Category()
        {
            navigationService = new NavigationService();
        }
        #endregion
    }
}
