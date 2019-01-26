using System;
namespace Products.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Services;
    using System;
    using Products.Views;

    public class LoginViewModel : BaseViewModel
    {
        #region Attributes
        private string password;
        private bool isRunning;
        public bool isEnabled;
        public string email;
        #endregion

        #region Services
        DialogService dialogService;
        ApiService apiService;
        #endregion

        #region Properties
        public string Email
        {
            get { return this.email; }
            set { SetValue(ref this.email, value); }
        }
        public string Password
        {
            get { return this.password; }
            set { SetValue(ref this.password, value); }
        }
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }
        public bool IsRemembered
        {
            get;
            set;
        }
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        #endregion

        #region Commands
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await dialogService.ShowMessage(
                    "Error",
                    "You must enter an email");
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await dialogService.ShowMessage(
                    "Error",
                    "You must enter an password");
                return;
            }


            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = true;
                this.IsEnabled = false;

                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var response = await apiService.GetToken(
                "https://productsapifab.azurewebsites.net",
                Email,
                Password);

            if (response == null)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage(
                    "Error",
                    "The service is not available, please try latter.");
                Password = null;
                return;
            }

            if (string.IsNullOrEmpty(response.AccessToken))
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage(
                    "Error",
                    response.ErrorDescription);
                Password = null;
                Email = null;
                return;
            }

            await dialogService.ShowMessage(
                    "WELCOME",
                    "Ha ingresado");

            Email = null;
            Password = null;

            IsRunning = false;
            IsEnabled = true;

            MainViewModel.GetInstance().Token = response;
            MainViewModel.GetInstance().Categories = new CategoriesViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new CategoriesView());
        }
        #endregion

        #region Constructors
        public LoginViewModel()
        {
            dialogService = new DialogService();
            apiService = new ApiService();

            this.IsRemembered = true;
            this.IsEnabled = true;

            this.Email = "test";
            this.Password = "123";

            //http://restcountries.eu/rest/v2/all
        }
        #endregion
    }
}
