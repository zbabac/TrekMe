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
    public partial class About : PhoneApplicationPage
    {
        public About()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        { 
            string aboutText = aboutTitle.Text;
            aboutTitle.Text = aboutText + " v"+ System.Reflection.Assembly.GetExecutingAssembly()
                    .FullName.Split('=')[1].Split(',')[0];
        }
        private void AboutBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}