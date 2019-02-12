﻿using System;


namespace Products.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Products.Models;
    using Services;

    public class MainViewModel
    {
        #region Properties

        public ObservableCollection<Menu> MyMenu
        {
            get;
            set;
        }

        public Category Category 
        { 
            get; set; 
        }
        public LoginViewModel Login
        {
            get; set;
        }

        public CategoriesViewModel Categories
        {
            get; set;
        }

        public NewCategoryViewModel NewCategory
        {
            get; set;
        }

        public NewProductViewModel NewProduct
        {
            get; set;
        }

        public EditCategoryViewModel EditCategory
        {
            get; set;
        }

        public EditProductViewModel EditProduct
        {
            get; set;
        }

        public ProductsViewModel Products
        {
            get; set;
        }

        public TokenResponse Token 
        { 
            get; set; 
        }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;
            this.Login = new LoginViewModel();

            navigationService = new NavigationService();
            LoadMenu();
        }
        #endregion

        #region Methods
        void LoadMenu()
        {
            MyMenu = new ObservableCollection<Menu>();

            MyMenu.Add(new Menu
            {
                Icon = "ic_settings",
                PageName = "MyProfileView",
                Title = "My Profile",
            });

            MyMenu.Add(new Menu
            {
                Icon = "ic_map",
                PageName = "UbicationsView",
                Title = "Ubications",
            });

            MyMenu.Add(new Menu
            {
                Icon = "ic_exit_to_app",
                PageName = "LoginView",
                Title = "Close sesion",
            });
        }
        #endregion
        #region Singleton
        public static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }
            return instance;
        }
        #endregion

        #region Services
        NavigationService navigationService;
        #endregion
        #region Commands
        public ICommand NewCategoryCommand
        {
            get
            {
                return new RelayCommand(GoNewCategory);
            }
        }

        async void GoNewCategory()
        {
            NewCategory = new NewCategoryViewModel();
            await navigationService.NavigateOnMaster("NewCategoryView");
        }

        public ICommand NewProductCommand
        {
            get
            {
                return new RelayCommand(GoNewProduct);
            }
        }

        async void GoNewProduct()
        {
            NewProduct = new NewProductViewModel();
            await navigationService.NavigateOnMaster("NewProductView");
        }
        #endregion
    }
}
