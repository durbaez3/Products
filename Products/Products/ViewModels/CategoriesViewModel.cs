

namespace Products.ViewModels
{
    using Products.Models;
    using Products.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;

    public class CategoriesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        #region Attributes
        List<Category> categories; 
         ObservableCollection<Category> _categories;
         #endregion

        #region Services
        DialogService dialogService;
        ApiService apiService;
        #endregion

        #region Properties
        public ObservableCollection<Category> Categories
        {
            get
            {
                return _categories;
            }
            set
            {
                if (_categories != value)
                {
                    _categories = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Categories)));
                }
            }
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
        async void LoadCategories()
        {
            apiService = new ApiService();
            dialogService = new DialogService();

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
    }
}
