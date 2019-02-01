namespace Products.ViewModels
{
    using System;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Products.Models;
    using Products.Services;

    public class EditCategoryViewModel : BaseViewModel
    {
        #region Services
        DialogService dialogService;
        ApiService apiService;
        NavigationService navigationService;
        #endregion

        #region Attributes
        private bool isRunning;
        public bool isEnabled;
        Category category;
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
        public EditCategoryViewModel(Category category)
        {
            this.category = category;
            dialogService = new DialogService();
            apiService = new ApiService();
            navigationService = new NavigationService();

            Description = category.Description;
            this.IsEnabled = true;

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
        async void Save()
        {
            if (string.IsNullOrEmpty(Description))
            {
                await dialogService.ShowMessage(
                    "Error",
                    "You must enter a category description.");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }


            category.Description = Description;
            var mainViewModel = MainViewModel.GetInstance();

            var response = await apiService.Put(
                "https://productsapifab.azurewebsites.net",
                "/api",
                "/Categories",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken,
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

            var categoriesViewModel = CategoriesViewModel.GetInstance();
            categoriesViewModel.UpdateCategory(category);

            await navigationService.Back();

            IsRunning = false;
            IsEnabled = true;
        }
        #endregion
    }
}
