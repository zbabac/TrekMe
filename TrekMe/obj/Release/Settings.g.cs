﻿#pragma checksum "C:\Users\ba100065\Documents\Visual Studio 2015\Projects\TrekMe\TrekMe\Settings.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3E5D52301268889827E9BD4B7306541B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace TrekMe {
    
    
    public partial class Settings : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.Button Pitch;
        
        internal System.Windows.Controls.CheckBox checkRotate;
        
        internal System.Windows.Controls.CheckBox checkMiles;
        
        internal System.Windows.Controls.CheckBox checkLocation;
        
        internal System.Windows.Controls.Button SettingsBack;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/TrekMe;component/Settings.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.Pitch = ((System.Windows.Controls.Button)(this.FindName("Pitch")));
            this.checkRotate = ((System.Windows.Controls.CheckBox)(this.FindName("checkRotate")));
            this.checkMiles = ((System.Windows.Controls.CheckBox)(this.FindName("checkMiles")));
            this.checkLocation = ((System.Windows.Controls.CheckBox)(this.FindName("checkLocation")));
            this.SettingsBack = ((System.Windows.Controls.Button)(this.FindName("SettingsBack")));
        }
    }
}

