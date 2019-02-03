
namespace Products.ViewModels
{
    using System;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Products.Models;
    using Products.Services;

    public class NewProductViewModel : BaseViewModel
    {
        #region Services
        DialogService dialogService;
        ApiService apiService;
        NavigationService navigationService;
        #endregion

        #region Attributes
        private bool isRunning;
        public bool isActive;
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

        #region Constructors
        public NewProductViewModel()
        {
            dialogService = new DialogService();
            apiService = new ApiService();
            navigationService = new NavigationService();
            this.IsActive = true;
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
            if (String.IsNullOrEmpty(Description))
            {
                await dialogService.ShowMessage(
                                "Error",
                                 "You must enter a Product Description.");
                return;
            }

            IsRunning = true;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;

                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var product = new Product
            {
                CategotyId = MainViewModel.GetInstance().Category.CategotyId,
                Description = Description,
                Price = Price,
                Stock = Stock,
                Remarks = Remarks,
                IsActive = IsActive,
                LastPurchase = DateTime.Now,
            };

            this.IsRunning = false;

            var mainViewModel = MainViewModel.GetInstance();

            var response = await apiService.Post(
                "https://productsapifab.azurewebsites.net"
                , "/api"
                , "/Products",
                mainViewModel.Token.TokenType
                , mainViewModel.Token.AccessToken,
                product);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);

                return;
            }
            //Retrnar para actualizar nuevo registro
            //actualizamos solo el nuevo regsitro
            var listProduct = MainViewModel.GetInstance().Products;
            product = (Product)response.Result;
            var productViewModel = ProductsViewModel.GetInstance();
            productViewModel.AddProduct(product);

            await navigationService.Back();

            IsRunning = false;
        }
        #endregion
    }
}
