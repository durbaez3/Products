using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Products.Views
{
    public partial class MasterView : MasterDetailPage
    {
        public MasterView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.Navigator = Navigator;
            App.Master = this;
        }
    }
}
