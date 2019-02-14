


namespace Products.Models
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Products.Services;
    using ViewModels;

    public class Menu
    {
        #region Services
        NavigationService navigation;
        #endregion

        #region Constructors
        public Menu()
        {
            navigation = new NavigationService();
        }
        #endregion
        #region Properties
        public string Icon { get; set; }

        public string Title { get; set; }

        public string PageName { get; set; }
        #endregion

        #region Commands
        public ICommand NavigateCommand
        {
            get
            {
                return new RelayCommand(Navigate);
            }

        }

        async void Navigate()
        {
            switch (PageName)
            {
                case "LoginView":
                    MainViewModel.GetInstance().Login = new LoginViewModel();
                    navigation.SetMainPage("LoginView");
                    break;
                case "UbicatiosView":
                    MainViewModel.GetInstance().Ubications =
                    new UbicationsViewModel();
                    await navigation.NavigateOnMaster("UbicatiosView");
                    break;
            }
        }
        #endregion
    }
}
