using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace Products.Models
{
    public class Menu
    {
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

        private void Navigate()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
