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
        private string rotate = "false";
        string parameterValue = "0";
        public Settings()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {   // parameter value for map pitch passed from PitchList.xaml page
            
            base.OnNavigatedTo(e);
            // check if parameter value is passed
            if (PhoneApplicationService.Current.State.ContainsKey("parameter"))
            {   // if yes, then set Pitch
                parameterValue = (string)PhoneApplicationService.Current.State["parameter"];
                Pitch.Content = parameterValue;
            }
            if (PhoneApplicationService.Current.State.ContainsKey("rotate"))
            {   // if yes, then set check box accordingly
                rotate = (string)PhoneApplicationService.Current.State["rotate"];
                checkRotate.IsChecked = Convert.ToBoolean(rotate);
            }
            PhoneApplicationService.Current.State["parameter"] = parameterValue;
        }
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            PhoneApplicationService.Current.State["parameter"] = parameterValue;
            PhoneApplicationService.Current.State["rotate"] = rotate;
        }
        private void SettingsBack_Click(object sender, RoutedEventArgs e)
        {   // click on Back button (disabled, HW back is used)
            NavigationService.GoBack();
        }

        private void Pitch_Click(object sender, RoutedEventArgs e)
        {   // if user clicks pitch, go to PitchList.xaml page
            NavigationService.Navigate(new Uri("/PitchList.xaml", UriKind.Relative));
        }

        private void checkRotate_Click(object sender, RoutedEventArgs e)
        {
            if (checkRotate.IsChecked == true)
                rotate = "true";
            else rotate = "false";
            PhoneApplicationService.Current.State["rotate"] = rotate;
        }
    }
}