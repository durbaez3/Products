
namespace Products.ViewModels
{
    using System;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Products.Models;
    using Products.Services;
    public class EditProductViewModel : BaseViewModel
    {

        #region Services
        DialogService dialogService;
        ApiService apiService;
        NavigationService navigationService;
        #endregion

        #region Attributes
        private bool isRunning;
        public bool isActive;
        Product product;
        #endregion

        #region Properties
        public string Description
        {
            get; set;
        }
        public decimal Price
        {
            get; set;
        }
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public bool IsActive
        {
            get { return this.isActive; }
            set { SetValue(ref this.isActive, value); }
        }
        public DateTime LastPurchase
        {
            get; set;
        }
        public double Stock
        {
            get; set;
        }
        public string Remarks
        {
            get; set;
        }
        #endregion

        #region Constructor
        public EditProductViewModel(Product product)
        {
            this.product = product;
            dialogService = new DialogService();
            apiService = new ApiService();
            navigationService = new NavigationService();

            Description = product.Description;
            Price = product.Price;
            Stock = product.Stock;
            Remarks = product.Remarks;
            IsActive = product.IsActive;
            LastPurchase = product.LastPurchase;
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

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRunning = false;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            product.CategotyId = MainViewModel.GetInstance().Category.CategotyId;
            product.Description = Description;
            product.Price = Price;
            product.Remarks = Remarks;
            product.Stock = Stock;
            product.IsActive = IsActive;
            product.LastPurchase = DateTime.Now;

            var mainViewModel = MainViewModel.GetInstance();

            var response = await apiService.Put(
                "https://productsapifab.azurewebsites.net",
                "/api",
                "/Products",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken,
                product);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            var productsViewModel = ProductsViewModel.GetInstance();
            productsViewModel.UpdateProduct(product);

            await navigationService.Back();

            IsRunning = false;
        }
        #endregion
    }
}
