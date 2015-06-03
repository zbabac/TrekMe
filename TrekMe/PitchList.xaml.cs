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
    public partial class PitchList : PhoneApplicationPage
    {
        public PitchList()
        {
            InitializeComponent();
        }

        private void PitchListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            ListBoxItem lbi = ((sender as ListBox).SelectedItem as ListBoxItem);
            PhoneApplicationService.Current.State["parameter"] = lbi.Content.ToString();
            NavigationService.GoBack();
        }
    }
}