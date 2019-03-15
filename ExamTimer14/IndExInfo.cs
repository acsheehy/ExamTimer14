using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;


namespace ExamTimer14
{
    public class IndExInfo : INotifyPropertyChanged
    {
        public String ExamName { get; set; }
        public Boolean Editing { get; set; }

        public Boolean ExamInProgress
        {
            get
            {
                if ((p_StartTime - DateTime.Now).TotalSeconds < 0)
                {
                    if (p_ExtraFiftyEnabled)
                    {
                        if ((p_ExtraFifty - DateTime.Now).TotalSeconds < 0) {return false;} else {return true;}
                    }
                    if (p_ExtraTFiveEnabled)
                    {
                        if ((p_ExtraTFive - DateTime.Now).TotalSeconds<0) {return false;} else {return true;}
                    }
                    if ((p_FinishTime - DateTime.Now).TotalSeconds < 0) return false; 
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public String StartTimeString { get { return p_StartTime.ToString("HH:mm"); } }
        public Brush StartTimeColour { get { return Brushes.Black; } }
        public DateTime StartTime { get { return p_StartTime; } set { p_StartTime = value; } }
        public TimeSpan StartTimeSpan
        {
            get { return p_StartTime.TimeOfDay; }
            set { p_StartTime = Convert.ToDateTime(value.ToString()); Calculate(); RaiseAllProperties(); }
        }
        
        public String FinishTimeString { get { return p_FinishTime.ToString("HH:mm"); } }
        public Brush FinishTimeColour 
        { 
            get 
            {
                if ((p_FinishTime - DateTime.Now).TotalSeconds < 0) return Brushes.DarkRed;
                if ((p_FinishTime - DateTime.Now).TotalSeconds < 300) return p_YellowBrush;
                return Brushes.DarkGreen;
            } 
        }
        public DateTime FinishTime { get { return p_FinishTime; } }
        public Visibility FinishTimeVisibility
        {
            get
            {
                if (!Pulse) return Visibility.Visible;
                int SecsToEnd = (int)(p_FinishTime - DateTime.Now).TotalSeconds;
                if ((SecsToEnd < 0) && (SecsToEnd > -30))
                {
                    if (p_EndPulse) { p_EndPulse = false; return Visibility.Hidden; } else { p_EndPulse = true; return Visibility.Visible; }
                }
                return Visibility.Visible;
            }
        }
        
        public String PlusTFiveString { 
            get 
            { 
                if (p_ExtraTFiveEnabled) 
                { return p_ExtraTFive.ToString("HH:mm"); } 
                else { return ""; } 
            } 
        }
        public Brush PlusTFiveColour 
        { 
            get 
            {
                if ((p_ExtraTFive - DateTime.Now).TotalSeconds < 0) return Brushes.DarkRed;
                if ((p_ExtraTFive - DateTime.Now).TotalSeconds < 300) return p_YellowBrush;
                return Brushes.DarkGreen;
            }
        }
        public Boolean ExtraTFiveEnabled { get { return p_ExtraTFiveEnabled; } set { p_ExtraTFiveEnabled = value; RaiseTFive(); } }
        public DateTime ExtraTFive { get { return p_ExtraTFive; } }
        public Visibility ExtraTFiveVisible
        {
            get
            {
                if (!Pulse) return Visibility.Visible;
                int SecsToEnd = (int)(p_ExtraTFive - DateTime.Now).TotalSeconds;
                if ((SecsToEnd < 0) && (SecsToEnd > -60))
                {
                    if (p_TFivePulse) { p_TFivePulse = false; return Visibility.Hidden; } else { p_TFivePulse = true; return Visibility.Visible; }
                } else
                    return Visibility.Visible;
            }
        }

        public String PFiftyString { get { if (p_ExtraFiftyEnabled) { return p_ExtraFifty.ToString("HH:mm"); } else { return ""; } } }
        public Brush PlusFiftyColour
        {
            get
            {
                if ((p_ExtraFifty - DateTime.Now).TotalSeconds < 0) return Brushes.DarkRed;
                if ((p_ExtraFifty - DateTime.Now).TotalSeconds < 300) return p_YellowBrush;
                return Brushes.DarkGreen;
            }
        }
        public Boolean ExtraFiftyEnabled { get { return p_ExtraFiftyEnabled; } set { p_ExtraFiftyEnabled = value; RaiseFifty(); } }
        public DateTime ExtraFifty { get { return p_ExtraFifty; } }
        public Visibility ExtraFiftyVisible
        {
            get
            {
                if (!Pulse) return Visibility.Visible;
                int SecsToEnd = (int)(p_ExtraFifty - DateTime.Now).TotalSeconds;
                if ((SecsToEnd < 0) && (SecsToEnd > -60))
                {
                    if (p_FiftyPulse) { p_FiftyPulse = false; return Visibility.Hidden; } else { p_FiftyPulse = true; return Visibility.Visible; }
                } else
                    return Visibility.Visible;
            }
        }

        public int Duration
        {
            get
            {
                return p_Duration;
            }
            set
            {
                p_Duration = value; Calculate(); RaiseAllProperties();
            }
        }
        
        public Boolean Pulse { get; set; }

        public Boolean PlayAudio { get; set; }

        public Visibility ButtonsVisibility
        {
            get
            {
                if (p_HideButtons)
                { return Visibility.Hidden; }
                else { return Visibility.Visible; }
            }
        }

        private bool p_Visible;

        private int p_Duration;
        private DateTime p_StartTime;
        private DateTime p_FinishTime;
        private DateTime p_ExtraTFive;
        private DateTime p_ExtraFifty;
        private bool p_ExtraTFiveEnabled;
        private bool p_ExtraFiftyEnabled;

        private Brush p_YellowBrush = (Brush)new BrushConverter().ConvertFromString("#9a8532");

        private bool p_EndPulse = true;
        private bool p_TFivePulse = true;
        private bool p_FiftyPulse = true;

        private bool p_HideButtons = false;

        private System.Timers.Timer p_enTimer;
        private Dispatcher p_WindowsDispatcher;
        private int p_MoveTimer;

        public event PropertyChangedEventHandler PropertyChanged;

        public IndExInfo()
        {
            DateTime Midday = DateTime.Today.AddHours(12);
            Editing = false;
            if (Midday > DateTime.Now)
            { p_StartTime = DateTime.Today.AddMinutes(570); }
            else { p_StartTime = DateTime.Today.AddHours(14); }
            ExamName = "Exam Name";
            p_WindowsDispatcher = Dispatcher.CurrentDispatcher;
            p_Duration = 60;
            Calculate();
            p_ExtraTFiveEnabled = false;
            p_ExtraFiftyEnabled = false;
            p_Visible = true;
            p_enTimer = new System.Timers.Timer(500);
            p_enTimer.Elapsed += p_enTimer_Elapsed;
            p_enTimer.Enabled = true;
        }

        void p_enTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Editing) return;
            p_WindowsDispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate()
            {
                UpdateDetails();
            });
            if (p_MoveTimer <= 0)
            { p_HideButtons = true; }
            else { p_MoveTimer--; }
        }

        public void Calculate()
        {
            p_FinishTime = p_StartTime.AddMinutes(Duration);
            p_ExtraTFive = p_StartTime.AddMinutes((Duration * 125 / 100));
            p_ExtraFifty = p_StartTime.AddMinutes((Duration * 150 / 100));
        }

        public void RaiseAllProperties()
        {
            RaisePropertyChanged("ExamName");
            RaisePropertyChanged("ButtonsVisibility");
            RaiseStart();
            RaiseFinish();
            RaiseTFive();
            RaiseFifty();
        }

        public void RaiseStart()
        {
            RaisePropertyChanged("StartTimeString");
            RaisePropertyChanged("StartTimeColour");
        }

        public void RaiseFinish()
        {
            RaisePropertyChanged("FinishTime");
            RaisePropertyChanged("FinishTimeString");
            RaisePropertyChanged("FinishTimeColour");
            RaisePropertyChanged("FinishTimeVisibility");
        }

        public void RaiseTFive()
        {
            RaisePropertyChanged("ExtraTFive");
            RaisePropertyChanged("PlusTFiveString");
            RaisePropertyChanged("PlusTFiveColour");
            RaisePropertyChanged("ExtraTFiveVisible");
        }

        public void RaiseFifty()
        {
            RaisePropertyChanged("ExtraFifty");
            RaisePropertyChanged("PFiftyString");
            RaisePropertyChanged("PlusFiftyColour");
            RaisePropertyChanged("ExtraFiftyVisible");
        }

        protected void RaisePropertyChanged(String info)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        private void UpdateDetails()
        {
            if (p_Visible)
            {
                int ToEndSecs = (int)Math.Round((double)(FinishTime - DateTime.Now).TotalSeconds,1,MidpointRounding.ToEven);
                int ToTEndSecs = (int)Math.Round((double)(ExtraTFive - DateTime.Now).TotalSeconds,1,MidpointRounding.ToEven);
                int ToTFiveSecs = (int)Math.Round((double)(ExtraFifty - DateTime.Now).TotalSeconds, 1, MidpointRounding.ToEven);

                // Check Finish Time to see if it is time to make noise
                // and change colour
                if ((ToEndSecs < 0) && (ToEndSecs > -40)) RaisePropertyChanged("FinishTimeColour"); 
                if (ToEndSecs == 0) PlayEndSound();
                if (ToEndSecs == 300) PlayFiveMinuteSound();
                if (ExtraTFiveEnabled)
                {
                    if ((ToTEndSecs < 0) && (ToTEndSecs > -70)) RaisePropertyChanged("PlusTenColour");
                    if (ToTEndSecs == 0) PlayEndSound();
                    if (ToTEndSecs == 300) PlayFiveMinuteSound();
                }
                if (ExtraFiftyEnabled)
                {
                    if ((ToTFiveSecs < 0) && (ToTFiveSecs > -70)) RaisePropertyChanged("PlusTFiveColour");
                    if (ToTFiveSecs == 0) PlayEndSound();
                    if (ToTFiveSecs == 300) PlayFiveMinuteSound();
                }
                RaiseAllProperties();
            }
        }

        private void PlayEndSound()
        {
            if (PlayAudio)
            {
                SoundPlayer EndSound = new SoundPlayer { Stream = Properties.Resources.examover };
                EndSound.Play();
            }
        }

        private void PlayFiveMinuteSound()
        {
            if (PlayAudio)
            {
                SoundPlayer FiveSound = new SoundPlayer { Stream = Properties.Resources._5minutes };
                FiveSound.Play();
            }
        }
    }

    [ValueConversion(typeof(int), typeof(String))]
    public class IntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int integer = (int)value;
            return integer.ToString();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;
            int resultInteger;
            if (int.TryParse(strValue, out resultInteger))
            {
                return resultInteger;
            }
            return DependencyProperty.UnsetValue;
        }
    }
}
