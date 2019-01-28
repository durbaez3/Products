using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Products.Models;
using Products.Services;

namespace Products.ViewModels
{
    public class NewCategoryViewModel : BaseViewModel
    {

        #region Services
        DialogService dialogService;
        ApiService apiService;
        NavigationService navigationService;
        #endregion

        #region Attributes
        private bool isRunning;
        public bool isEnabled;
        #endregion 

        #region Properties

        public string Description 
        { 
            get; set; 
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        #endregion

        #region Constructors
        public NewCategoryViewModel()
        {
            dialogService = new DialogService();
            apiService = new ApiService();
            navigationService = new NavigationService();
            this.IsEnabled = true;


            //http://restcountries.eu/rest/v2/all
        }
        #endregion

        #region Commands
        public ICommand SaveCommand 
        {
            get
            {
                return new RelayCommand(Save);
            }
        }
        async void  Save()
        {
            if (String.IsNullOrEmpty(Description))
            {
                await dialogService.ShowMessage(
                                "Error",
                                 "You must enter a Category Description.");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var category = new Category
            {
                Description = Description
            };

            this.IsRunning = false;
            this.IsEnabled = true;

            var mainViewModel = MainViewModel.GetInstance();

            var response = await apiService.Post(
                "https://productsapifab.azurewebsites.net"
                ,"/api"
                ,"/Categories",
                mainViewModel.Token.TokenType
                ,mainViewModel.Token.AccessToken,
                category);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);

                return;
            }
            //Retrnar para actualizar nuevo registro
            //actualizamos solo el nuevo regsitro
            category = (Category)response.Result;
            var categoriesViewModel= CategoriesViewModel.GetInstance();
            categoriesViewModel.AddCategory(category);

            await navigationService.Back();

            IsRunning = false;
            IsEnabled = true;
        }
        #endregion

    }
}
