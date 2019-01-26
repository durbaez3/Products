using System;


namespace Products.ViewModels
{
    using Products.Models;

    public class MainViewModel
    {
        #region Properties
        public LoginViewModel Login
        {
            get; set;
        }

        public CategoriesViewModel Categories
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
    }
}
