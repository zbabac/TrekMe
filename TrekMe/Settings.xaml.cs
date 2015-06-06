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
        {   // parameter value for map pitch passed from PitchList.xaml page
            string parameterValue = "25";
            base.OnNavigatedTo(e);
            // check if parameter value is passed
            if (PhoneApplicationService.Current.State.ContainsKey("parameter"))
            {   // if yes, then set Pitch
                parameterValue = (string)PhoneApplicationService.Current.State["parameter"];
                Pitch.Content = parameterValue;
            }
            PhoneApplicationService.Current.State["parameter"] = parameterValue;
        }
        private void SettingsBack_Click(object sender, RoutedEventArgs e)
        {   // click on Back button (disabled, HW back is used)
            NavigationService.GoBack();
        }

        private void Pitch_Click(object sender, RoutedEventArgs e)
        {   // if user clicks pitch, go to PitchList.xaml page
            NavigationService.Navigate(new Uri("/PitchList.xaml", UriKind.Relative));
        }
    }
}