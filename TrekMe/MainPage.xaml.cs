using System;
using System.Device.Location;
using System.Xml.Serialization;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Shell;
using NExtra.Geo;
using TrekMe.Resources;
using System.Collections.ObjectModel;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

namespace TrekMe
{   
    /// <summary>
    /// This app tracks movements by usage of GPS sensor.
    /// It records time of the walk/run/drive, calculates speed, average speed, etc.
    /// It also draws the path at the map.
    /// It also updates large live tile with details and medium and small tile with kilometers taken.
    /// </summary>
    /// <remarks>
    /// Author:     Zlatko Babic [zlatko.babic@outlook.com]
    /// License:    MIT license: http://www.opensource.org/licenses/mit-license.php
    /// Acknowledgments: 
    ///     Part of this app uses Geo portion of the NExtra class provided by Daniel Saidi [daniel.saidi@gmail.com]
    /// which is also licensed under MIT license.
    ///     Warenhouse Standard font is licensed under SIL Open Font License
    ///     Inspired by Colin Eberhardt's sample WP8Runner
    /// </remarks>
    public partial class MainPage : PhoneApplicationPage
    {
        private GeoCoordinateWatcher gps_watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);//define GPS position change watcher
        private MapPolyline trek_line; //draw line when position changes
        private GeoCoordinate coord = new GeoCoordinate();
        private DispatcherTimer trek_timer = new DispatcherTimer(); //measure time of the run
        private long walk_time_tick, pause_time_tick, total_pause;  //count ticks to remember them when having paused the walk
        private long trek_startTime, pause_start_time; //remember when the run is started and when pause is started
        private bool trek_started = false; //is the run started?
        private bool rotate_map = false; //display map that rotates when heading changes?
        private bool use_miles = false; //display distance in miles and alt in feet?
        private bool paused = false;//is the run paused?
        private bool was_started = false; //was it started before, then wait for GPS
        private bool draw_circle = false; //should I draw the circle?
        private bool redtrek_line = false;//every even run the line is red, for every odd number run it is blue
        private bool location = false; //introduced due to microsoft privacy policy, however app can't function without location services
        //private bool pause_tapped = false; //to draw only 1 circle for pause
        private int speed_count; //count number of measurement, for more accurate displaying of the speed
        //private double[] spd = new double[2]; //take 3 measurements of speed to have arithmetic median after every 3rd position measured
        TimeSpan runTime;
        private double trek_total_distance = 0.0;  //var to measure kilometers
        private long tick_previousPosition; //remmber previous position change time
        private double current_speed = 0.0, avg_speed; //remember current and average speed
        private Color colour = Colors.Green;
        private double map_pitch = 0.0;
        private double alt_start, alt_min, alt_max;
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings; //used to save settings
        string _dst = "km";
        string _spd = "mil";
        string _alt = "m";
        double _avg_speed = 0;
        double _curr_speed = 0;
        double _altitude=0, _alt_change_up=0, _alt_change_down=0;
        double _dist=0;
        MapLayer layerPin = new MapLayer();//layer for showing position on map long press
        Pushpin pushpin1 = new Pushpin();//pushpin that displays gps position on map
        MapOverlay overlayPin = new MapOverlay();//overlay for pin
        private bool center_map = true, delete_Runs = false;//center the map just on first location detection
        ObservableCollection<string> savedRuns = new ObservableCollection<string>(); //save each run in list that is saved in Isolated Storage for later use

        public MainPage()
        {
            InitializeComponent();
            // AppBar can't be localized as resource so this is workaround
            ApplicationBarIconButton appButtonSettings1 = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            ApplicationBarIconButton appButtonAbout1 = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
            ApplicationBarIconButton appButtonRecord1 = (ApplicationBarIconButton)ApplicationBar.Buttons[2];
            // event handler for tapping the pushpin
            pushpin1.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(PushpinTap);
            // event handler for tapping the map
            Map.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(MapTap);
            // load settings from isolated storage
            if (!settings.Contains("settingLocation"))
            {
                settings.Add("settingLocation", location);
            }
            else
                location = Convert.ToBoolean(settings["settingLocation"]);
            if (!settings.Contains("settingPitch"))
            {
                settings.Add("settingPitch", map_pitch);
            }
            else
            {
                map_pitch= Convert.ToDouble(settings["settingPitch"]);
            }
            if (!settings.Contains("settingRotate"))
            {
                settings.Add("settingRotate", rotate_map);
            }
            else
            {
                rotate_map = Convert.ToBoolean(settings["settingRotate"]);
            }
            if (!settings.Contains("settingMiles"))
            {
                settings.Add("settingMiles", use_miles);
            }
            else
            {
                use_miles = Convert.ToBoolean(settings["settingMiles"]);
            }
            // load saved runs - user data
            savedRuns = Deserialize("Treks");
            // detect culture and apply workaround for Application Bar buttons
            switch (System.Threading.Thread.CurrentThread.CurrentUICulture.Name.Substring(0, 2))
            {
                case "en":
                    appButtonSettings1.Text = "settings";
                    appButtonAbout1.Text = "about";
                    appButtonRecord1.Text = "runs";
                    break;
                case "sr":
                    appButtonSettings1.Text = "подеси";
                    appButtonAbout1.Text = "инфо";
                    appButtonRecord1.Text = "историја";
                    break;
                case "nb":
                    appButtonSettings1.Text = "innstillinger";
                    appButtonAbout1.Text = "om";
                    appButtonRecord1.Text = "løper";
                    break;
                case "de":
                    appButtonSettings1.Text = "Einstellungen";
                    appButtonAbout1.Text = "Info";
                    appButtonRecord1.Text = "Läufe";
                    break;
                case "fr":
                    appButtonSettings1.Text = "paramètres";
                    appButtonAbout1.Text = "environ";
                    appButtonRecord1.Text = "cours";
                    break;
                case "es":
                    appButtonSettings1.Text = "configuración";
                    appButtonAbout1.Text = "sobre";
                    appButtonRecord1.Text = "curso";
                    break;
                case "pt":
                    appButtonSettings1.Text = "configurações";
                    appButtonAbout1.Text = "sobre";
                    appButtonRecord1.Text = "curso";
                    break;
                case "pl":
                    appButtonSettings1.Text = "ustawienia";
                    appButtonAbout1.Text = "info";
                    appButtonRecord1.Text = "trasy";
                    break;
                case "it":
                    appButtonSettings1.Text = "impostazioni";
                    appButtonAbout1.Text = "su app";
                    appButtonRecord1.Text = "percorsi";
                    break;
                default:
                    appButtonSettings1.Text = "settings";
                    appButtonAbout1.Text = "about";
                    appButtonRecord1.Text = "runs";
                    break;
            }
            // draw a line for a run
            MapOverlay trekOverlay = new MapOverlay();
            MapLayer trekLayer = new MapLayer();
            trekOverlay.Content = trek_line;
            trekLayer.Add(trekOverlay);
            // Add the MapLayer to the Map.
            Map.Layers.Add(trekLayer);
            trek_line = new MapPolyline();
            //trek_line.StrokeColor = Colors.Transparent;
            trek_line.StrokeThickness = 5;
            Map.MapElements.Add(trek_line);
            //add handler for position change event
            gps_watcher.PositionChanged += GPS_PositionChanged;
            gps_watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(GPS_StatusChanged);
            //add timer handler to measure ticks when run starts
            trek_timer.Interval = TimeSpan.FromSeconds(1);
            trek_timer.Tick += Timer_Tick;
            //define boolean to let the app know if the run is paused, so that it doesn't count time, kilometres, calories.
            paused = false;
            //after installation, display location usage warning only once
            if (!location)
            {
                MessageBoxResult locOK = MessageBox.Show(AppResources.MessageLocation, AppResources.MessageLocationTitle, MessageBoxButton.OKCancel);
                if (locOK == MessageBoxResult.Cancel)
                {
                    MessageBox.Show(AppResources.MessageExit, AppResources.MessageExitTitle, MessageBoxButton.OK);
                    Application.Current.Terminate();
                }
                else
                {
                    location = true;
                }
            }
            StartButton.IsEnabled = true;//allow user to tap start
            PauseButton.IsEnabled = false;//disallow user to tap pause, because the run is not yet started
            //Start GPS watcher immediatelly, so that the map is displayed regardless of the fact that run is not yet started
            //otherwise, map would not be not displayed until the Start is tapped
            gps_watcher.MovementThreshold = 20; //reduce erratic data display when GPS signal is weak
            gps_watcher.Start();
        }
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            PhoneApplicationService.Current.State["location"] = location.ToString();
            PhoneApplicationService.Current.State["parameter"] = map_pitch.ToString();
            PhoneApplicationService.Current.State["rotate"] = rotate_map.ToString();
            PhoneApplicationService.Current.State["miles"] = use_miles.ToString();
            PhoneApplicationService.Current.State["runs"] = savedRuns;
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // the way to pass value from other xaml page - settings, parameter is defined there
            string parameterValue = "25";
            base.OnNavigatedTo(e);
            if (PhoneApplicationService.Current.State.ContainsKey("location"))
            {
                string loc = (string)PhoneApplicationService.Current.State["location"];
                location = Convert.ToBoolean(loc);
            }
            if (PhoneApplicationService.Current.State.ContainsKey("parameter"))
            {
                parameterValue = (string)PhoneApplicationService.Current.State["parameter"];
                map_pitch = Convert.ToDouble(parameterValue);
            }
            if (PhoneApplicationService.Current.State.ContainsKey("rotate"))
            {
                string rotate = (string)PhoneApplicationService.Current.State["rotate"];
                rotate_map = Convert.ToBoolean(rotate);
            }
            if (PhoneApplicationService.Current.State.ContainsKey("miles"))
            {
                string miles = (string)PhoneApplicationService.Current.State["miles"];
                use_miles = Convert.ToBoolean(miles);
            }
            //if signal sent from page Runs, then delete run history from user file
            if (PhoneApplicationService.Current.State.ContainsKey("deleteRuns"))
            {
                if (delete_Runs)
                {
                    savedRuns.Clear();
                    Serialize("Treks", savedRuns);
                }
            }

                // save settings section when navigated from Settings page
                settings["settingLocation"] = location;
            settings["settingPitch"] = map_pitch;
            settings["settingRotate"] = rotate_map;
            settings["settingMiles"] = use_miles;
            settings.Save();
            //check location status
            if (!location)
            {
                // The user has disabled the Location Service on Settings page.
                BannerInfo.Foreground = new SolidColorBrush(Colors.White);
                banner.Background = new SolidColorBrush(Colors.Blue);
                detailLabel.Foreground = new SolidColorBrush(Colors.Transparent);
                BannerInfo.Text = AppResources.WarningLocationDisabled;
                MessageBox.Show(AppResources.WarningLocationSettings, AppResources.MessageWarningTitle, MessageBoxButton.OK);
                
                StartButton.IsEnabled = false;
            }
            else if (!trek_started)
            {
                BannerInfo.Foreground = new SolidColorBrush(Colors.White);
                banner.Background = new SolidColorBrush(Colors.Blue);
                detailLabel.Foreground = new SolidColorBrush(Colors.Transparent);
                BannerInfo.Text = AppResources.LocationEnabled;
                StartButton.IsEnabled = true;
            }

        }
        private void pivotControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // show AppBar on Details pivot only
            switch (((Pivot)sender).SelectedIndex)
            {
                case 0:
                    ApplicationBar.IsVisible = true;
                    break;

                case 1:
                    ApplicationBar.IsVisible = false;
                    break;
            }
        }
        private void Timer_Tick(object sender, EventArgs e) //after every 1 second expires, this event is fired
        {
            if (!paused) //if not paused, measure run time and update screen values
            {
                //time measured = system time - start time - total pause measured
                walk_time_tick = System.Environment.TickCount - trek_startTime - total_pause;
                runTime = TimeSpan.FromMilliseconds(walk_time_tick);
                elapsedTime.Text = runTime.ToString(@"hh\:mm\:ss");
                elapsedTime2.Text = elapsedTime.Text;

            }
            else //Pause is tapped by user and pause time is measured
            {
                //current pause = system time - start time of the pause
                pause_time_tick = System.Environment.TickCount - pause_start_time;
                TimeSpan runTime = TimeSpan.FromMilliseconds(pause_time_tick);
                breakTime.Text = runTime.ToString(@"hh\:mm\:ss"); //update displayed pause time
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            //Start button tapped, set variables accordingly
            redtrek_line = !redtrek_line; //first run is marked with red line, next one blue, then again red, etc.
            trek_startTime = System.Environment.TickCount; //remember start time
            StartButton.IsEnabled = false; //disallow to start when it is started
            PauseButton.IsEnabled = true; //allow user to pause the run
            center_map = true;
            paused = false; //not paused
            walk_time_tick = 0; //set variables initial values
            pause_time_tick = 0;
            total_pause = 0;
            trek_started = true;  //boolean to know it is started
            trek_timer.Start(); //start measuring time
            trek_line = new MapPolyline(); //draw new line so that old lines are not deleted  !!!!!
            if (redtrek_line)   //determine if line color should be red or blue
                colour = Colors.Red;

            else
                colour = Colors.Blue;
            draw_circle = true;
            trek_line.StrokeColor = colour;
            Map.MapElements.Add(trek_line);
            distanceBox.Text = "0 ";
            speedBox.Text = "0 ";
            speedBox2.Text = "0 ";
            distanceBox2.Text = "0 ";
            caloriesLabel.Text = "0 kcal";
            avgSpeed.Text = "0 ";
            gps_watcher.Start(); //if not started, start GPS watcher
            if (was_started)
            {   //if the run had been started before, show GPS warning again
                BannerInfo.Foreground = new SolidColorBrush(Colors.White);
                banner.Background = new SolidColorBrush(Colors.Blue);
                detailLabel.Foreground = new SolidColorBrush(Colors.Transparent);
                BannerInfo.Text = AppResources.BannerInfo;
            }
            was_started = true;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (trek_started)
            {
                string currentDate = String.Format("{0:d}", System.DateTime.Now);
                savedRuns.Add(currentDate + " | " + distanceBox2.Text + " | " + elapsedTime2.Text + " | " + avgSpeed.Text+ " | ↑↓ " + (alt_max - alt_min).ToString() + _alt);
                Serialize("Treks", savedRuns);//save the last run
                                              //Stop is tapped so change display elements as they were set for start
                colour = Colors.Green; //last point of the run is green
                Ellipse myCircle = new Ellipse();  //draw a circle to mark stop point
                myCircle.Stroke = new SolidColorBrush(colour);
                myCircle.StrokeThickness = 10;
                myCircle.Height = 25;
                myCircle.Width = 25;
                myCircle.Opacity = .5;
                // Create a MapOverlay to contain the circle.
                MapOverlay myLocationOverlay = new MapOverlay();
                myLocationOverlay.Content = myCircle;
                myLocationOverlay.PositionOrigin = new Point(0.5, 0.5);
                myLocationOverlay.GeoCoordinate = coord;
                // Create a MapLayer to contain the MapOverlay.
                MapLayer myLocationLayer = new MapLayer();
                myLocationLayer.Add(myLocationOverlay);
                // Add the MapLayer to the Map.
                Map.Layers.Add(myLocationLayer);
                gps_watcher.Stop();
                trek_timer.Stop();
                StartButton.Background = new SolidColorBrush(Colors.Transparent); //make buttons normal color

                PauseButton.Background = new SolidColorBrush(Colors.Transparent);
                draw_circle = false;
                trek_started = false;
                paused = false;
                StartButton.IsEnabled = true;
                PauseButton.IsEnabled = false;

                walk_time_tick = 0;
                pause_time_tick = 0;
                total_pause = 0;
                ShellTile.ActiveTiles.First().Update(new IconicTileData() //update live tiles
                {
                    Title = "TrekMe",
                    Count = Math.Abs(Convert.ToInt16(_dist - 0.5)),
                    WideContent1 = string.Format("{0:f1} {1}", _dist, _dst),
                    WideContent2 = string.Format("avg: {0:f0} {1}", _avg_speed, _spd),
                    WideContent3 = string.Format("{0} {1} Alt:{2:f0}{3}", lon2.Text, lat2.Text, _altitude, _alt),
                    BackgroundColor = new Color { A = 255, R = 0, G = 0, B = 255 }
                });

                trek_total_distance = 0;
            }
        }

        private void Map_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            GeoCoordinate lokacija = Map.ConvertViewportPointToGeoCoordinate(e.GetPosition(Map));
            string pozicija = lokacija.Latitude.ToString("f6", CultureInfo.InvariantCulture) + ", " + lokacija.Longitude.ToString("f6", CultureInfo.InvariantCulture);
            Map.Layers.Remove(layerPin);
            layerPin.Remove(overlayPin);
            pushpin1.GeoCoordinate = lokacija;
            pushpin1.Content = pozicija;
            overlayPin.Content = pushpin1;
            overlayPin.GeoCoordinate = lokacija;
            layerPin.Add(overlayPin);

            Map.Layers.Add(layerPin);
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e) //pause can be tapped to start the pause as well to continue the run
        {
            try
            {
                colour = Colors.Gray;
                if (paused) //was paused so now continue again
                {
                    trek_line = new MapPolyline(); //draw new line so that old lines are not deleted
                    trek_line.StrokeThickness = 5;
                    if (redtrek_line) //determine color
                    {
                        colour = Colors.Red;
                    }
                    else
                    {
                        colour = Colors.Blue;
                    }
                    trek_line.StrokeColor = colour;
                    trek_line.StrokeThickness = 5;
                    draw_circle = false;
                    Map.MapElements.Add(trek_line);  //add line for run continuation
                    total_pause += pause_time_tick; //increase total pause with latest pause time
                    paused = false; //we continue the run
                    trek_timer.Stop();
                    trek_timer.Start(); //restart the timer to measure run time again
                    //StartButton.Content = "Started";  //let the user know by writing it on the screen
                    //PauseButton.Content = "Pause";
                    StartButton.IsEnabled = false;
                    StartButton.Background = (LinearGradientBrush)this.Resources["linearBrush"]; //started again so this button is painted
                    PauseButton.Background = new SolidColorBrush(Colors.Transparent);

                    trek_started = true;
                }
                else
                {   //pause is tapped, now measure pause and stop measuring the run
                    paused = true; //run is paused
                    Ellipse myCircle = new Ellipse();
                    myCircle.Stroke = new SolidColorBrush(colour);
                    myCircle.StrokeThickness = 10;
                    myCircle.Height = 25;
                    myCircle.Width = 25;
                    myCircle.Opacity = .5;
                    // Create a MapOverlay to contain the circle.
                    MapOverlay myCircleOverlay = new MapOverlay();
                    myCircleOverlay.Content = myCircle;
                    myCircleOverlay.PositionOrigin = new Point(0.5, 0.5);
                    myCircleOverlay.GeoCoordinate = coord;
                    // Create a MapLayer to contain the MapOverlay.
                    MapLayer myCircleLayer = new MapLayer();
                    myCircleLayer.Add(myCircleOverlay);
                    // Add the MapLayer to the Map.
                    Map.Layers.Add(myCircleLayer);
                    trek_line = new MapPolyline(); //draw new line for measuring movements during pause
                    trek_line.StrokeColor = colour;
                    trek_line.StrokeThickness = 2;
                    draw_circle = false;
                    Map.MapElements.Add(trek_line); //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                    trek_timer.Stop(); //restart timer to measure pause time
                    trek_timer.Start();
                    pause_start_time = System.Environment.TickCount; //remember pause start time
                    StartButton.Background = new SolidColorBrush(Colors.Transparent);
                    //StartButton.Foreground = new SolidColorBrush(color: SystemColors.ControlTextColor); 
                    PauseButton.Background = (LinearGradientBrush)this.Resources["linearBrush"];
                    trek_started = true;  //run is paused but still running
                    ShellTile.ActiveTiles.First().Update(new IconicTileData() //update live tiles
                    {
                        Title = "TrekMe",
                        Count = Math.Abs(Convert.ToInt16(_dist - 0.5)),
                        WideContent1 = string.Format("{0:f1} {1}", _dist, _dst),
                        WideContent2 = string.Format("P avg: {0:f0} {1}", _avg_speed, _spd),
                        WideContent3 = string.Format("{0} {1} Alt:{2:f0}{3}", lon2.Text, lat2.Text, _altitude, _alt),
                        BackgroundColor = new Color { A = 255, R = 255, G = 0, B = 0 }
                    });
                }
            }
            catch (Exception x)
            {
                string err = x.Message.ToString();
                string error = err;
            }
        }
        private void GPS_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    // The Location Service is disabled or unsupported.
                    // Check to see whether the user has disabled the Location Service.
                    if (gps_watcher.Permission == GeoPositionPermission.Denied)
                    {
                        // The user has disabled the Location Service on their device.
                        BannerInfo.Foreground = new SolidColorBrush(Colors.White);
                        banner.Background = new SolidColorBrush(Colors.Blue);
                        detailLabel.Foreground = new SolidColorBrush(Colors.Transparent);
                        BannerInfo.Text = AppResources.WarningLocationDisabled;
                        MessageBox.Show(AppResources.MessageWarningText1,AppResources.MessageWarningTitle, MessageBoxButton.OK);
                    }
                    else
                    {
                        BannerInfo.Foreground = new SolidColorBrush(Colors.White);
                        banner.Background = new SolidColorBrush(Colors.Blue);
                        detailLabel.Foreground = new SolidColorBrush(Colors.Transparent);
                        BannerInfo.Text = AppResources.WarningLocationNotSupported;
                        MessageBox.Show(AppResources.MessageWarningText2, AppResources.MessageWarningTitle, MessageBoxButton.OK);
                    }
                 break;
            }
        }

        private void GPS_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {   //event is fired every time GPS sensor detects position change, location coordinates are then fetched
            coord = new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude, e.Position.Location.Altitude, e.Position.Location.HorizontalAccuracy,
                e.Position.Location.VerticalAccuracy, e.Position.Location.Speed, e.Position.Location.Course);
            if (trek_line.Path.Count > 0) //calculate vars only after 1st position
            {
                var previousPoint = trek_line.Path.Last(); //to measure distance one must know previous position
                var distance = coord.GetDistanceTo(previousPoint); //measure distance from previous position
                var ticks = (System.Environment.TickCount - tick_previousPosition); //time elapsed since was at previous position
                if (ticks == 0) current_speed = 0.0; //we can measure only if time elapsed is > 0, to avoid dividing by zero

                if (!paused && trek_line.Path.Count > 0)  //if movement detected calculate vars
                {
                    /*
                    //to increase accuracy, 2 speed measurements are taken, then median is calculated
                    spd[speed_count] = (distance * 3600) / ticks; //one of 2 measurements for current speed
                    if (speed_count == 1) //if third measurement is taken, calculate median for current speed
                    {
                        current_speed = (spd[0] + spd[1]) / 2;
                        speed_count = -1; //prepare for taking another 2 measurements
                    }
                    speed_count++; //count how many speed measurements are taken
                    */
                    current_speed = 3.6 * coord.Speed;
                    trek_total_distance += distance / 1000.0; //add last measurement to the total distance covered
                    avg_speed = (trek_total_distance * 3600000) / walk_time_tick; //calc average speed so far

                    //set data to be displayed depending on units set
                    if (use_miles)
                    {
                        _dst = "mil"; // unit for distance
                        _spd = "mph"; // unit for speed

                        _avg_speed = avg_speed * 0.62137;
                        _curr_speed = current_speed * 0.62137;

                        _dist = trek_total_distance * 0.62137;
                    }
                    else
                    {
                        _dst = "km";
                        _spd = "km/h";

                        _avg_speed = avg_speed;
                        _curr_speed = current_speed;

                        _dist = trek_total_distance;
                    }
                    // set values on page with defined units
                    avgSpeed.Text = string.Format("{0:f1} {1}", _avg_speed, _spd);
                    distanceBox.Text = string.Format("{0:f1} {1}", _dist, _dst);
                    speedBox.Text = string.Format("{0:f0} {1}", _curr_speed, _spd);
                    speedBox2.Text = string.Format("{0:f1} {1}", _curr_speed, _spd);
                    distanceBox2.Text = string.Format("{0:f2} {1}", _dist, _dst);

                    caloriesLabel.Text = string.Format("{0:f0} kcal", trek_total_distance * 65);

                    trek_line.StrokeThickness = 5;
                }
                // if set in settings to rotate map with user changing heading
                if (rotate_map)
                {
                    PositionHandler handler = new PositionHandler();
                    var heading = handler.CalculateBearing(new Position(previousPoint), new Position(coord)); //set map orientation
                    Map.SetView(coord, Map.ZoomLevel, heading, MapAnimationKind.Parabolic);
                }
                // otherwise set map view to point to north
                else
                    Map.SetView(coord, Map.ZoomLevel, 0.0, MapAnimationKind.Linear);

                if (coord.Longitude < 0)
                    longitudelabel.Text = string.Format("{0:f2}⁰W ", (coord.Longitude * (-1.0))); //display latitude and longitude
                else
                    longitudelabel.Text = string.Format("{0:f2}⁰E ", coord.Longitude);
                if (coord.Latitude < 0)
                    latitudelabel.Text = string.Format("{0:f2}⁰S ", (coord.Latitude * (-1.0)));
                else
                    latitudelabel.Text = string.Format("{0:f2}⁰N ", coord.Latitude);
                // set new max and min altidute reached
                if (coord.Altitude > alt_max)
                    alt_max = coord.Altitude;
                else
                    if (coord.Altitude < alt_min)
                    alt_min = coord.Altitude;
                // define units to be displayed: feet or meters
                if (use_miles)
                {
                    _alt = "ft"; // altitude units
                    _altitude = coord.Altitude * 3.28084;
                    _alt_change_up = (alt_max - alt_start) * 3.28084; //update max reached alt
                    _alt_change_down = (alt_start - alt_min) * 3.28084; //update min reached alt
                }
                else
                {
                    _alt = "m";
                    _altitude = coord.Altitude;
                    _alt_change_up = alt_max - alt_start;
                    _alt_change_down = alt_start - alt_min;
                }
                // format labels to match format: meters or feet
                altLabel.Text = string.Format("{0:f0} {1}", _altitude, _alt);
                lon2.Text = longitudelabel.Text;
                lat2.Text = latitudelabel.Text;
                alt2.Text = string.Format("{0:f0} {1}", _altitude, _alt);
                altUpDown.Text = string.Format("↑{0:f0}  ↓{1:f0} {2}", _alt_change_up, _alt_change_down, _alt);

                if (trek_started && !paused)
                {

                    // update start tile with updated data
                    ShellTile.ActiveTiles.First().Update(new IconicTileData() //update live tiles
                    {
                        Title = "TrekMe",
                        Count = Math.Abs(Convert.ToInt16(_dist - 0.5)),
                        WideContent2 = string.Format("{0} | Avg: {1:f0} {2}", runTime.ToString(@"hh\:mm"), _avg_speed, _spd),
                        WideContent1 = string.Format("{0:f1} {1} | {2:f0} {3}", _dist, _dst, _curr_speed, _spd),
                        WideContent3 = string.Format("{0} {1} Alt:{2:f0}{3}", lon2.Text, lat2.Text, _altitude, _alt),
                        BackgroundColor = new Color { A = 255, R = 255, G = 0, B = 0 }
                    });

                }
            }
            else
            {   // when 1st position detected, center the map
                if (center_map)
                {
                    Map.Center = coord;
                    center_map = false;
                }
                //Map.Layers.Remove(layerPin); //remove pushpin when trek started
                //layerPin.Remove(overlayPin);

                BannerInfo.Foreground = new SolidColorBrush(Colors.Transparent); //remove warning about GPS data
                banner.Background = new SolidColorBrush(Colors.Transparent);
                detailLabel.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x9E, 0xFF));
                if (trek_started && !paused)   // if not paused, draw start circle
                {
                    StartButton.Background = (LinearGradientBrush)this.Resources["linearBrush"];
                    if (draw_circle)
                    {
                        Ellipse myCircle = new Ellipse();
                        myCircle.Stroke = new SolidColorBrush(colour);
                        myCircle.StrokeThickness = 10;
                        myCircle.Height = 25;
                        myCircle.Width = 25;
                        myCircle.Opacity = .5;
                        // Create a MapOverlay to contain the circle.
                        MapOverlay myCircleOverlay = new MapOverlay();
                        myCircleOverlay.Content = myCircle;
                        myCircleOverlay.PositionOrigin = new Point(0.5, 0.5);
                        myCircleOverlay.GeoCoordinate = coord;
                        // Create a MapLayer to contain the MapOverlay.
                        MapLayer myCircleLayer = new MapLayer();
                        myCircleLayer.Add(myCircleOverlay);
                        // Add the MapLayer to the Map.
                        Map.Layers.Add(myCircleLayer);
                    }
                    // define first detected altitude
                    alt_start = coord.Altitude;
                    alt_min = alt_start;
                    alt_max = alt_start;
                }
            }
            if (trek_started)
            {
                trek_line.Path.Add(coord); //draw line that displays the change from the last position
                //BannerInfo.Foreground = new SolidColorBrush(Colors.Transparent); //remove warning about GPS data
                //banner.Background = new SolidColorBrush(Colors.Transparent);
                //detailLabel.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x9E, 0xFF));
            }
            tick_previousPosition = System.Environment.TickCount; //remember time for the next position change
            Map.Pitch = map_pitch;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PivotButton_Click(object sender, RoutedEventArgs e)
        {   //when small red button is clicked on pivot item with a map, change the view to pivot item with run details
            pivotControl.SelectedIndex = 0;
        }

        private void CenterButton_Click(object sender, RoutedEventArgs e)
        {   // when small blue button is clicked on a map, center it
            Map.SetView(coord, Map.ZoomLevel, 0.0, MapAnimationKind.Parabolic); //heading = 0.0
            //Map.Layers.Remove(layerPin);
            //layerPin.Remove(overlayPin);
        }

        private void Map_Loaded(object sender, RoutedEventArgs e)
        {   // appId and appToken needed to use the map from Microsoft
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = "f519dba9-5601-4691-a614-2df33604452c";
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = "pvRxh3mdY1ETTVKB3SB6wA";
        }

        private void ApplicationBarSettings_Click(object sender, EventArgs e)
        {   // go to settings page
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }

        private void ApplicationBarAbout_Click(object sender, EventArgs e)
        {   // go to about page
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }

        private void appButtonRecord_Click(object sender, EventArgs e)
        {   // go to about page
            NavigationService.Navigate(new Uri("/Runs.xaml?object1=" + savedRuns, UriKind.Relative));
        }

        //Serializer to add or save user data (list) to xml file for later use
        private static void Serialize(string fileName, object source)
        {
            var userStore = IsolatedStorageFile.GetUserStoreForApplication();

            using (var stream = new IsolatedStorageFileStream(fileName, FileMode.Create, userStore))
            {
                XmlSerializer serializer = new XmlSerializer(source.GetType());
                serializer.Serialize(stream, source);
            }
        }
        //Deserializer to read saved data in form of the list that populates saved track data
        public static ObservableCollection<string> Deserialize(string filename)
        {
            //public static void Deserialize<T>(ObservableCollection<T> list, string filename)
            ObservableCollection<string> list = new ObservableCollection<string>();

            var userStore = IsolatedStorageFile.GetUserStoreForApplication();
            if (userStore.FileExists(filename))
            {
                using (var stream = new IsolatedStorageFileStream(filename, FileMode.Open, userStore))
                {
                    XmlSerializer serializer = new XmlSerializer(list.GetType());
                    var items = (ObservableCollection<string>)serializer.Deserialize(stream);

                    foreach (string item in items)
                    {
                        list.Add(item);
                    }
                }
                return list;
            }
            else return new ObservableCollection<string>();
        }

        private void PushpinTap(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Pushpin pushpin = sender as Pushpin;
            if (pushpin.Content != null)
            {
                int n_position = pushpin.Content.ToString().IndexOf('\n');
                if (n_position != -1)   //if it is already copied to clipboard, then remove that string before copying again
                    pushpin1.Content = pushpin.Content.ToString().Substring(0, n_position);
                Clipboard.SetText(pushpin.Content.ToString());
                pushpin1.Content = pushpin1.Content + "\n" + AppResources.CopiedToClipboard; //add info that it is copied to clipboard
            }
            // to stop the event from going to the parent map control
            e.Handled = true;
        }
        private void MapTap(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {   // remove previously added pin on short tap
            Map.Layers.Remove(layerPin);
            layerPin.Remove(overlayPin);
        }

    }
}