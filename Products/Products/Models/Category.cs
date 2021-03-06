﻿namespace Products.Models
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
        DialogService dialogService;
        NavigationService navigationService;
        #endregion
        #region Properties
        public int CategotyId { get; set; }

        public string Description { get; set; }

        public List<Product> Products { get; set; }
        #endregion

        #region Commands
        public ICommand DeleteCommand 
        { 
            get
            {
                return new RelayCommand(Delete);
            }

        }

        async void Delete()
        {
            var response = await dialogService.ShowConfirm(
                "Confirm",
                "Area you Sure to delete this record?");
            if (!response)
            {
                return;
            }
            await CategoriesViewModel.GetInstance().DeleteCategory(this);
        }

        public ICommand EditCommand
        {
            get
            {
                return new RelayCommand(Edit);
            }
        }

        async void Edit()
        {
            MainViewModel.GetInstance().EditCategory = new  EditCategoryViewModel(this);
            await navigationService.NavigateOnMaster("EditCategoryView");
        }

        public ICommand SelectCategoryCommand 
        { 
            get
            {
                return new RelayCommand(SelectCategory);            }
        }

        async void SelectCategory()
        {
            MainViewModel.GetInstance().Products = new ProductsViewModel(Products);
            MainViewModel.GetInstance().Category = this;
            await navigationService.NavigateOnMaster("ProductsView");
        }
        #endregion

        #region Constructors
        public Category()
        {
            navigationService = new NavigationService();
            dialogService = new DialogService();
        }
        #endregion

        #region Methods
        public override int GetHashCode()
        {
            return CategotyId;
        }
        #endregion
    }
}
