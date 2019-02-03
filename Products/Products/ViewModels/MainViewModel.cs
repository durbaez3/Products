using System;


namespace Products.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Products.Models;
    using Services;

    public class MainViewModel
    {
        #region Properties

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

        public EditCategoryViewModel EditCategory
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
            await navigationService.Navigate("NewCategoryView");
        }
        #endregion
    }
}
