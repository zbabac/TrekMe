﻿#pragma checksum "C:\Users\ba100065\Documents\Visual Studio 2013\Projects\TrekMe\TrekMe\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "453F6CB017843E3CD6229AC95F91B2DC"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
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
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal Microsoft.Phone.Controls.Pivot pivotControl;
        
        internal System.Windows.Controls.TextBlock detailLabel;
        
        internal System.Windows.Controls.Grid banner;
        
        internal System.Windows.Controls.TextBlock BannerInfo;
        
        internal System.Windows.Controls.TextBlock distanceBox2;
        
        internal System.Windows.Controls.TextBlock elapsedTime2;
        
        internal System.Windows.Controls.TextBlock caloriesLabel;
        
        internal System.Windows.Controls.TextBlock breakTime;
        
        internal System.Windows.Controls.TextBlock lat2;
        
        internal System.Windows.Controls.TextBlock lon2;
        
        internal System.Windows.Controls.TextBlock alt2;
        
        internal System.Windows.Controls.Button StartButton;
        
        internal System.Windows.Controls.Button PauseButton;
        
        internal System.Windows.Controls.Button StopButton;
        
        internal System.Windows.Controls.TextBlock speedBox2;
        
        internal System.Windows.Controls.TextBlock avgSpeed;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal Microsoft.Phone.Maps.Controls.Map Map;
        
        internal System.Windows.Controls.TextBlock longitudelabel;
        
        internal System.Windows.Controls.TextBlock latitudelabel;
        
        internal System.Windows.Controls.TextBlock altLabel;
        
        internal System.Windows.Controls.TextBlock speedBox;
        
        internal System.Windows.Controls.Button CenterButton;
        
        internal System.Windows.Controls.TextBlock distanceBox;
        
        internal System.Windows.Controls.TextBlock elapsedTime;
        
        internal System.Windows.Controls.Button PivotButton;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/TrekMe;component/MainPage.xaml", System.UriKind.Relative));
            this.pivotControl = ((Microsoft.Phone.Controls.Pivot)(this.FindName("pivotControl")));
            this.detailLabel = ((System.Windows.Controls.TextBlock)(this.FindName("detailLabel")));
            this.banner = ((System.Windows.Controls.Grid)(this.FindName("banner")));
            this.BannerInfo = ((System.Windows.Controls.TextBlock)(this.FindName("BannerInfo")));
            this.distanceBox2 = ((System.Windows.Controls.TextBlock)(this.FindName("distanceBox2")));
            this.elapsedTime2 = ((System.Windows.Controls.TextBlock)(this.FindName("elapsedTime2")));
            this.caloriesLabel = ((System.Windows.Controls.TextBlock)(this.FindName("caloriesLabel")));
            this.breakTime = ((System.Windows.Controls.TextBlock)(this.FindName("breakTime")));
            this.lat2 = ((System.Windows.Controls.TextBlock)(this.FindName("lat2")));
            this.lon2 = ((System.Windows.Controls.TextBlock)(this.FindName("lon2")));
            this.alt2 = ((System.Windows.Controls.TextBlock)(this.FindName("alt2")));
            this.StartButton = ((System.Windows.Controls.Button)(this.FindName("StartButton")));
            this.PauseButton = ((System.Windows.Controls.Button)(this.FindName("PauseButton")));
            this.StopButton = ((System.Windows.Controls.Button)(this.FindName("StopButton")));
            this.speedBox2 = ((System.Windows.Controls.TextBlock)(this.FindName("speedBox2")));
            this.avgSpeed = ((System.Windows.Controls.TextBlock)(this.FindName("avgSpeed")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.Map = ((Microsoft.Phone.Maps.Controls.Map)(this.FindName("Map")));
            this.longitudelabel = ((System.Windows.Controls.TextBlock)(this.FindName("longitudelabel")));
            this.latitudelabel = ((System.Windows.Controls.TextBlock)(this.FindName("latitudelabel")));
            this.altLabel = ((System.Windows.Controls.TextBlock)(this.FindName("altLabel")));
            this.speedBox = ((System.Windows.Controls.TextBlock)(this.FindName("speedBox")));
            this.CenterButton = ((System.Windows.Controls.Button)(this.FindName("CenterButton")));
            this.distanceBox = ((System.Windows.Controls.TextBlock)(this.FindName("distanceBox")));
            this.elapsedTime = ((System.Windows.Controls.TextBlock)(this.FindName("elapsedTime")));
            this.PivotButton = ((System.Windows.Controls.Button)(this.FindName("PivotButton")));
        }
    }
}

