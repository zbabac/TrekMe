using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace TrekMe
{
    public partial class Settings : PhoneApplicationPage
    {
        public Settings()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string parameterValue = "25";
            base.OnNavigatedTo(e);
            if (PhoneApplicationService.Current.State.ContainsKey("parameter"))
            {
                parameterValue = (string)PhoneApplicationService.Current.State["parameter"];
                Pitch.Content = parameterValue;
            }
            PhoneApplicationService.Current.State["parameter"] = parameterValue;
        }
        private void SettingsBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Pitch_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PitchList.xaml", UriKind.Relative));
        }
    }
}