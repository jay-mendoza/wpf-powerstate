//-----------------------------------------------------------------------
// <copyright file="TimeTextBox.xaml.cs" company="Jay Bautista Mendoza">
//     Copyright (c) Jay Bautista Mendoza. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PowerState.Views
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>Interaction logic for TimeTextBox.</summary>
    public partial class TimeControl : UserControl
    {
        #region FIELDS │ PRIVATE │ STATIC │ NON-READONLY
                
        /// <summary></summary>
        private static int currentHour;
        
        /// <summary></summary>
        private static int currentMinute;

        /// <summary></summary>
        private static int currentSecond;

        /// <summary></summary>
        private static TimeMode selectedTimeMode;

        #endregion

        #region FIELDS │ PRIVATE │ NON-STATIC │ NON-READONLY

        /// <summary>This control's hour value.</summary>
        private int hour;

        /// <summary>This control's minute value.</summary>
        private int minute;

        /// <summary>This control's second value.</summary>
        private int second;

        /// <summary>This control's time mode value.</summary>
        private TimeMode timeMode;

        #endregion
        
        #region CONSTRUCTORS │ PUBLIC │ NON-STATIC
        
        /// <summary>Initializes a new instance of the <see cref="TimeTextBox" /> class.</summary>
        public TimeControl()
        {
            this.InitializeComponent();
            TimeControl.selectedTimeMode = TimeMode.HR24;

            TimeControl.currentHour = DateTime.Now.Hour + 1;
            TimeControl.currentMinute = 0;
            TimeControl.currentSecond = 0;

            this.SetControlTime(TimeControl.currentHour, TimeControl.currentMinute, TimeControl.currentSecond);
        }

        #endregion

        #region ENUMS │ PUBLIC
        /// <summary>Enumertaions of various Time Modes.</summary>
        public enum TimeMode
        {
            /// <summary>12-Hour ante meridiem.</summary>
            AM12,

            /// <summary>12-Hour post meridiem.</summary>
            PM12,

            /// <summary>24-Hour mode.</summary>
            HR24,

            /// <summary>Free hour mode.</summary>
            Free
        }
        #endregion

        #region #region PROPERTIES │ PUBLIC │ NON-STATIC

        /// <summary>Gets this control's hour value.</summary>
        public int Hour { get { return this.hour; } }

        /// <summary>Gets this control's minute value.</summary>
        public int Minute { get { return this.minute; } }

        /// <summary>Gets this control's second value.</summary>
        public int Second { get { return this.second; } }

        /// <summary>Gets this control's second value.</summary>
        public bool FreeTime
        {
            get
            {
                return this.timeMode == TimeMode.Free;
            }
        }

        #endregion

        #region METHODS │ PRIVATE │ NON-STATIC
        /// <summary>Sets the time on the textboxes.</summary>
        /// <param name="hour">Hour (0-23).</param>
        /// <param name="minute">Minute (0-60).</param>
        /// <param name="second">Second (0-60).</param>
        private void SetControlTime(int hour, int minute, int second)
        {
            switch (TimeControl.selectedTimeMode)
            {
                case TimeMode.HR24:
                    hour = hour > 23 ? 23 : hour;
                    TimeControl.currentHour = hour;
                    this.HourTextBox.Text = hour.ToString("00");
                    this.MinuteTextBox.Text = minute.ToString("00");
                    this.SecondTextBox.Text = minute.ToString("00");
                    this.SaveCurrentTime();
                    return;
                case TimeMode.AM12:
                    hour = hour > 12 ? hour - 12 : (hour == 0 ? 12 : hour);
                    TimeControl.currentHour = hour;
                    this.HourTextBox.Text = hour.ToString("00");
                    this.MinuteTextBox.Text = minute.ToString("00");
                    this.SecondTextBox.Text = minute.ToString("00");
                    this.SaveCurrentTime();
                    return;
                case TimeMode.PM12:
                    this.HourTextBox.Text = hour.ToString("00");
                    this.MinuteTextBox.Text = minute.ToString("00");
                    this.SecondTextBox.Text = minute.ToString("00");

                    hour = hour <= 12 ? hour + 12 : (hour == 23 ? 12 : hour);
                    TimeControl.currentHour = hour;
                    this.SaveCurrentTime();
                    return;
                case TimeMode.Free:
                    TimeControl.currentHour = hour;
                    this.HourTextBox.Text = hour.ToString("00");
                    this.MinuteTextBox.Text = minute.ToString("00");
                    this.SecondTextBox.Text = minute.ToString("00");
                    this.SaveCurrentTime();
                    return;
                default: return;

            }
        }

        /// <summary>Saves the current time to the exposed fields and properties.</summary>
        private void SaveCurrentTime()
        {
            this.hour = TimeControl.currentHour;
            this.minute = TimeControl.currentMinute;
            this.second = TimeControl.currentSecond;
            this.timeMode = TimeControl.selectedTimeMode;
        }
        #endregion

        #region METHODS (EVENTS) │ PRIVATE │ NON-STATIC

        /// <summary>Click event for Switch Mode Button.</summary>
        /// <param name="sender">Object "sender".</param>
        /// <param name="e">RoutedEventArgs "e".</param>
        private void TimeModeSelectButton_Click(object sender, RoutedEventArgs e)
        {
            TextBlock textBox = (TextBlock)this.TimeModeSelectButton.Template.FindName("ModeText", this.TimeModeSelectButton);

            int hour = int.Parse(this.HourTextBox.Text);
            int minute = int.Parse(this.MinuteTextBox.Text);
            int second = int.Parse(this.SecondTextBox.Text);

            switch (TimeControl.selectedTimeMode)
            {
                case TimeMode.HR24:
                    TimeControl.selectedTimeMode = TimeMode.AM12;
                    textBox.Text = "AM";
                    break;
                case TimeMode.AM12:
                    TimeControl.selectedTimeMode = TimeMode.PM12;
                    textBox.Text = "PM";
                    break;
                case TimeMode.PM12:
                    TimeControl.selectedTimeMode = TimeMode.Free;
                    textBox.Text = "--";
                    break;
                case TimeMode.Free:
                    TimeControl.selectedTimeMode = TimeMode.HR24;
                    textBox.Text = "24";
                    break;
                default: break;
            }

            this.SetControlTime(hour, minute, second);
        }

        /// <summary>Lost Focus event for Time TextBoxes (Hour, Second, Minute).</summary>
        /// <param name="sender">Object "sender".</param>
        /// <param name="e">RoutedEventArgs "e".</param>
        private void TimeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Text.Length == 1)
            {
                textBox.Text = string.Concat("0", textBox.Text);
            }
            else if (textBox.Text.Length == 0)
            {
                textBox.Text = "00";
            }
        }

        /// <summary>Text changed event for Hour TextBox.</summary>
        /// <param name="sender">Object "sender".</param>
        /// <param name="e">TextChangedEventArgs "e".</param>
        private void HourTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int hour = 0;
            bool parseable = int.TryParse(this.HourTextBox.Text, out hour);
            bool outOfRange = TimeControl.selectedTimeMode == TimeMode.Free ? false :
                (TimeControl.selectedTimeMode == TimeMode.HR24 ? hour >= 24 || hour < 0 : hour > 12 || hour <= 0);


            if (outOfRange || !parseable)
            {
                this.HourTextBox.Text = TimeControl.currentHour.ToString("00");
                this.HourTextBox.SelectAll();
                return;
            }
                        
            TimeControl.currentHour = hour;
            this.SaveCurrentTime();
        }

        /// <summary>Text changed event for Minute TextBox.</summary>
        /// <param name="sender">Object "sender".</param>
        /// <param name="e">TextChangedEventArgs "e".</param>
        private void MinuteTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int minute = 0;
            bool parseable = int.TryParse(this.MinuteTextBox.Text, out minute);
            bool outOfRange = minute >= 60 || minute < 0;

            if (outOfRange || !parseable)
            {
                this.MinuteTextBox.Text = TimeControl.currentMinute.ToString("00");
                this.MinuteTextBox.SelectAll();
                return;
            }
            
            TimeControl.currentMinute = minute;
            this.SaveCurrentTime();
        }

        /// <summary>Text changed event for Second TextBox.</summary>
        /// <param name="sender">Object "sender".</param>
        /// <param name="e">TextChangedEventArgs "e".</param>
        private void SecondTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int second = 0;
            bool parseable = int.TryParse(this.SecondTextBox.Text, out second);

            if (second >= 60 || second < 0 || !parseable)
            {
                this.SecondTextBox.Text = TimeControl.currentSecond.ToString("00");
                this.SecondTextBox.SelectAll();
            }

            TimeControl.currentSecond = second;
            this.SaveCurrentTime();
        }

        #endregion
    }
}
