//-----------------------------------------------------------------------
// <copyright file="MainPage.xaml.cs" company="Jay Bautista Mendoza">
//     Copyright (c) Jay Bautista Mendoza. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PowerState.Views
{
    using System;
    using System.Diagnostics;
    using System.Windows.Controls;
    using System.Windows.Threading;

    /// <summary>Interaction logic for MainPage.</summary>
    public partial class MainPage : Page
    {
        #region FIELDS │ PRIVATE │ STATIC │ NON-READONLY

        /// <summary>Countdown counter.</summary>
        private static int counter;

        /// <summary>Selected state string.</summary>
        private static string selectedState;

        #endregion

        #region FIELDS │ PRIVATE │ NON-STATIC │ NON-READONLY

        /// <summary>Declare an instance pf ProcessStartInfo for the 7-ZIP process.</summary>
        private ProcessStartInfo processInfo;

        /// <summary>Declare an instance pf DispatcherTimer.</summary>
        private DispatcherTimer dispatcherTimer;

        #endregion

        #region CONSTRUCTORS │ PUBLIC │ NON-STATIC

        /// <summary>Initializes a new instance of the <see cref="MainPage" /> class.</summary>
        public MainPage()
        {
            this.InitializeComponent();
            this.processInfo = new ProcessStartInfo();
            this.processInfo.WindowStyle = ProcessWindowStyle.Hidden;

            this.dispatcherTimer = new DispatcherTimer();
            this.dispatcherTimer.Tick += this.DispatcherTimer_Tick;
            this.dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            this.MainTimeControl.Visibility = System.Windows.Visibility.Collapsed;
            
            this.ScreenOffButton.Focus();
        }

        #endregion

        #region METHODS (EVENTS) │ PRIVATE │ NON-STATIC

        /// <summary>Click event for "Cancel" Button.</summary>
        /// <param name="sender">Object "sender".</param>
        /// <param name="e">RoutedEventArgs "e".</param>
        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ResetToInitialState();
        }

        /// <summary>Click event for Power State Button.</summary>
        /// <param name="sender">Object "sender".</param>
        /// <param name="e">RoutedEventArgs "e".</param>
        private void PowerStateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button senderButton = sender as Button;
            MainPage.selectedState = senderButton.Content.ToString();
            this.processInfo = GetProcessByName(MainPage.selectedState);

            if (!(bool)this.TimedCheckBox.IsChecked)
            {
                MainPage.counter = 2;
            }
            else
            {
                int hours = this.MainTimeControl.Hour * 3600;
                int minutes = this.MainTimeControl.Minute * 60;
                int seconds = this.MainTimeControl.Second;

                int totalSeconds = hours + minutes + seconds;

                if (this.MainTimeControl.FreeTime)
                {
                    MainPage.counter = totalSeconds;
                }
                else
                {
                    DateTime timeNow = DateTime.Now;
                    int hoursNow = timeNow.Hour * 3600;
                    int minutesNow = timeNow.Minute * 60;
                    int secondsNow = timeNow.Second;

                    int totalSecondsFromNow = hoursNow + minutesNow + secondsNow;

                    MainPage.counter = totalSeconds - totalSecondsFromNow;
                }
            }
            this.CancelButton.Content = string.Concat(MainPage.selectedState, " │ ", (MainPage.counter + 1).ToString());

            this.dispatcherTimer.Start();
            this.ButtonsGrid.Visibility = System.Windows.Visibility.Collapsed;
            this.CancelButton.Visibility = System.Windows.Visibility.Visible;
            this.CancelButton.Focus();
        }

        /// <summary>Tick event for the countdown timer.</summary>
        /// <param name="sender">Object "sender".</param>
        /// <param name="e">EventArgs "e".</param>
        private void DispatcherTimer_Tick(object sender, System.EventArgs e)
        {
            this.CancelButton.Content = string.Concat(MainPage.selectedState, " │ ", counter.ToString());

            if (counter == 0)
            {
                Process process = Process.Start(this.processInfo);
                process.WaitForExit();
                this.ResetToInitialState();
            }

            counter--;
        }
        #endregion

        #region METHODS │ PRIVATE │ NON-STATIC
        
        /// <summary>Reset the program to initial state.</summary>
        private void ResetToInitialState()
        {
            this.dispatcherTimer.Stop();
            this.ButtonsGrid.Visibility = System.Windows.Visibility.Visible;
            this.CancelButton.Visibility = System.Windows.Visibility.Collapsed;

            this.GetButtonByName(MainPage.selectedState).Focus();
        }

        private Button GetButtonByName(string name)
        {
            switch (name.ToUpper())
            {
                case "SCREEN OFF": return this.ScreenOffButton;
                case "SCREEN LOCK": return this.ScreenLockButton;
                case "SIGN OUT": return this.LogOffButton;
                case "SUSPEND": return this.SuspendButton;
                case "RESTART": return this.RestartButton;
                case "SHUT DOWN": return this.ShutDownButton;
                default: return this.ScreenOffButton;
            }
        }

        private ProcessStartInfo GetProcessByName(string name)
        {
            switch (name.ToUpper())
            {
                case "SCREEN OFF": return new ProcessStartInfo(@"ScreenOffProgram.exe");
                case "SCREEN LOCK": return new ProcessStartInfo(@"rundll32.exe", "User32.dll,LockWorkStation");
                case "SIGN OUT": return new ProcessStartInfo(@"shutdown.exe", "-l -f");
                case "SUSPEND": return new ProcessStartInfo(@"rundll32.exe", "powrprof.dll,SetSuspendState");
                case "RESTART": return new ProcessStartInfo(@"shutdown.exe", "-r -f -t 00");
                case "SHUT DOWN": return new ProcessStartInfo(@"shutdown.exe", "-s -f -t 00");
                default: return new ProcessStartInfo(@"ScreenOffProgram.exe");
            }
        }

        #endregion

        private void TimedCheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.MainTimeControl.Visibility = System.Windows.Visibility.Visible;
        }

        private void TimedCheckBox_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.MainTimeControl.Visibility = System.Windows.Visibility.Collapsed;
        }

        
    }
}
