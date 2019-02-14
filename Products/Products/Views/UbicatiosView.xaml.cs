using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Products.Views
{
    using System.Threading.Tasks;
    using Services;
    using ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Maps;
    public partial class UbicatiosView : ContentPage
    {
        public UbicatiosView()
        {
            InitializeComponent();
            InitializeComponent();

            geolocatorService = new GeolocatorService();

            MoveMapToCurrentPosition();
        }

        #region Services
        GeolocatorService geolocatorService;
        #endregion
        #region Methods
        async void MoveMapToCurrentPosition()
        {
            await geolocatorService.GetLocation();
            if (geolocatorService.Latitude != 0 ||
                geolocatorService.Longitude != 0)
            {
                var position = new Position(
                    geolocatorService.Latitude,
                    geolocatorService.Longitude);
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                    position,
                    Distance.FromKilometers(.5)));
            }

            ///await LoadPins();
        }

        //async Task LoadPins()
        //{
          //  var ubicationsViewModel = UbicationsViewModel.GetInstance();
            //await ubicationsViewModel.LoadPins();
            //foreach (var pin in ubicationsViewModel.Pins)
            //{
             //   MyMap.Pins.Add(pin);
            //}
        //}
        #endregion
    }
}
