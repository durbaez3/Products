

namespace Products.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Products.Models;
    using Products.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;

    public class CategoriesViewModel : BaseViewModel
    {
        //public event PropertyChangedEventHandler PropertyChanged;
        #region Attributes
        List<Category> categories; 
        ObservableCollection<Category> _categories;
        bool _isRefreshing;
        #endregion

        #region Services
        DialogService dialogService;
        ApiService apiService;
        #endregion

        #region Properties
        public ObservableCollection<Category> Categories
        {
            get { return this._categories; }
            set { SetValue(ref this._categories, value); }
        }

        public bool IsRefreshing
        {
            get { return this._isRefreshing; }
            set { SetValue(ref this._isRefreshing, value); }
        }
        #endregion

        #region Constructor
        public CategoriesViewModel()
        {
            LoadCategories();
            instance = this;
        }
        #endregion

        #region Methods
        public void AddCategory(Category category)
        {
            categories.Add(category);
            Categories = new ObservableCollection<Category>(
                categories.OrderBy(c => c.Description));
        }

        public void UpdateCategory(Category category)
        {
            IsRefreshing = true;
            var oldCategory = categories.
                Where(c => c.CategoryId == category
                .CategoryId).FirstOrDefault();

            oldCategory = category;
            //categories.Add(category);
            Categories = new ObservableCollection<Category>(
                categories.OrderBy(c => c.Description));
            IsRefreshing = false;
        }

        async void LoadCategories()
        {
            apiService = new ApiService();
            dialogService = new DialogService();
            IsRefreshing = true;
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                //this.IsRunning = true;
                //this.IsEnabled = false;

                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var mainViewModel = MainViewModel.GetInstance();
            var response = await apiService.GetList<Category>(
                "https://productsapifab.azurewebsites.net", "/api", "/Categories",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken);

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            categories = (List<Category>)response.Result;
            Categories = new ObservableCollection<Category>(
               categories.OrderBy(c => c.Description));
            IsRefreshing = false;
        }
        #endregion


        #region Singleton
        public static CategoriesViewModel instance;

        public static CategoriesViewModel GetInstance()
        {
            if (instance == null)
            {
                return new CategoriesViewModel();
            }
            return instance;
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand  
        { 
            get
            {
                return new RelayCommand(LoadCategories);
            }
        }
        #endregion
    }
}
