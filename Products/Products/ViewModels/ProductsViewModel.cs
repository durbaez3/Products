using System;


namespace Products.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Models;
    using Products.Services;

    public class ProductsViewModel : BaseViewModel
    {
       
        #region Attributes
        public ObservableCollection<Product> _produts;
        bool _isRefreshing;
        List<Product> products;
        #endregion

        #region Services
        DialogService dialogService;
        ApiService apiService;
        #endregion

        #region Properties
        public ObservableCollection<Product> Products
        {
            get { return this._produts; }
            set { SetValue(ref this._produts, value); }
        }

        public bool IsRefreshing
        {
            get { return this._isRefreshing; }
            set { SetValue(ref this._isRefreshing, value); }
        }
        #endregion

        #region Methods
        public void AddProduct(Product product)
        {
            products.Add(product);
            Products = new ObservableCollection<Product>(
                products.OrderBy(c => c.Description));
        }

        public void UpdateProduct(Product product)
        {
            IsRefreshing = true;
            var oldProduct = products.
                Where(c => c.ProductId == product
                .ProductId).FirstOrDefault();


            oldProduct = product;
            Products = new ObservableCollection<Product>(
                products.OrderBy(c => c.Description));
            IsRefreshing = false;
        }

        public async Task DeleteProduct(Product product)
        {
            IsRefreshing = true;
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var mainViewModel = MainViewModel.GetInstance();

            var response = await apiService.Delete(
                "https://productsapifab.azurewebsites.net",
                "/api",
                "/Products",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken,
                product);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            products.Remove(product);
            Products = new ObservableCollection<Product>(
                products.OrderBy(c => c.Description));

            IsRefreshing = false;
        }
        #endregion
        #region Constructor
        public ProductsViewModel(List<Product> produts)
        {
            instance = this;
            apiService = new ApiService();
            dialogService = new DialogService();
            this.products = produts;
            Products = new ObservableCollection<Product>(produts.OrderBy(p => p.Description));
        }

        public ProductsViewModel()
        {
            instance = this;
        }
        #endregion

        #region Singleton
        public static ProductsViewModel instance;

        public static ProductsViewModel GetInstance()
        {
            if (instance == null)
            {
                return new ProductsViewModel();
            }
            return instance;
        }
        #endregion
    }
}
