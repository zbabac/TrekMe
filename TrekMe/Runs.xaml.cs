using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;

namespace TrekMe
{
    public partial class Runs : PhoneApplicationPage
    {
        ObservableCollection<string> lista = new ObservableCollection<string>();
        
        public Runs()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {   // parameter value for map pitch passed from PitchList.xaml page

            base.OnNavigatedTo(e);
            // check if parameter value is passed
            if (PhoneApplicationService.Current.State.ContainsKey("runs"))
            {   // if yes, fill the list
                lista = (ObservableCollection<string>)PhoneApplicationService.Current.State["runs"];
                Lista.ItemsSource = lista;
                Lista.UpdateLayout();
            }
        }
    }
}