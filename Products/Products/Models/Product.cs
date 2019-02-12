using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Products.Services;
using Products.ViewModels;

namespace Products.Models
{
    public class Product
    {
        #region Services
        DialogService dialogService;
        NavigationService navigationService;
        #endregion

        #region Properties
        public int ProductId { get; set; }
        public int CategotyId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastPurchase { get; set; }
        public double Stock { get; set; }
        public string Remarks { get; set; }
        public string ImageFullPath
        {
            get
            {
                if (!string.IsNullOrEmpty(Image))
                {
                    return string.Format("https://productsapifab.azurewebsites.net/{0}",
                    Image.Substring(1));
                }
                return "noimage";
            }
        }
        #endregion

        #region Commands
        public ICommand EditCommand
        {
            get
            {
                return new RelayCommand(Edit);
            }
        }

        async void Edit()
        {
            MainViewModel.GetInstance().EditProduct = new EditProductViewModel(this);
            await navigationService.NavigateOnMaster("EditProductView");
        }

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
            await ProductsViewModel.GetInstance().DeleteProduct(this);
        }


        public Product()
        {
            dialogService = new DialogService();
            navigationService = new NavigationService();
        }
        #endregion

        #region Methods
        public override int GetHashCode()
        {
            return ProductId;
        }
        #endregion
    }
}
