using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ExamTimer14
{
    /// <summary>
    /// Interaction logic for ExamClock.xaml
    /// </summary>
    public partial class ExamClock : UserControl
    {
        

        private ClockInfo MyClock;

        public Boolean ShowingSeconds { get { return MyClock.ShowSeconds; } set { if (value) { MyClock.EnableSeconds(); } else { MyClock.DisableSeconds(); }; } }

        public ExamClock()
        {
            InitializeComponent();
            MyClock = new ClockInfo();
            DataContext = MyClock;
            MyClock.LastTime = DateTime.Now;
            MyClock.EnableSeconds();

        }

        

    }

    public class ClockInfo : INotifyPropertyChanged
    {

        public DateTime LastTime { get; set; }
        public bool ShowSeconds { get; set; }
        public bool Ticker { get; set; }
        public bool TickOn { get; set; }
        public Visibility TickVisible { get { if (TickOn) { return Visibility.Visible; } else { return Visibility.Hidden; } } }
        private System.Timers.Timer Quartseq;
        private System.Windows.Threading.Dispatcher WindowsDispatcher;

        public String H1 { get; set; }
        public String H2 { get; set; }
        public String M1 { get; set; }
        public String M2 { get; set; }
        public String S1 { get; set; }
        public String S2 { get; set; }

        private GridLength glZero = new GridLength(0, GridUnitType.Pixel);
        private GridLength glOne = new GridLength(1, GridUnitType.Star);
        private GridLength glOnePoint = new GridLength(1.5, GridUnitType.Star);

        public GridLength coldefColonWidth { get { if (!ShowSeconds) { return glZero; } else { return glOne; } } }
        public GridLength coldefSecWidth { get { if (!ShowSeconds) { return glZero; } else { return glOnePoint; } } }

        public event PropertyChangedEventHandler PropertyChanged;

        public ClockInfo()
        {
            WindowsDispatcher = Dispatcher.CurrentDispatcher;
            ShowTime(DateTime.Now);
            TickOn = true;
            ShowSeconds = true;
            Ticker = false;
            Quartseq = new System.Timers.Timer(250);
            Quartseq.Elapsed +=Quartseq_Elapsed;
            Quartseq.Enabled = true;
        }

        public void DisableSeconds()
        {
            ShowSeconds = false;
            Ticker = true;
            RaiseSecondUpdates();
        }

        public void EnableSeconds()
        {
            ShowSeconds = true;
            Ticker = false;
            RaiseSecondUpdates();
        }

        private void RaiseSecondUpdates()
        {
            RaisePropertyChanged("coldefSecWidth");
            RaisePropertyChanged("coldefColonWidth");
        }

        void Quartseq_Elapsed(object sender, ElapsedEventArgs e)
        {
            WindowsDispatcher.BeginInvoke(DispatcherPriority.Normal, (System.Threading.ThreadStart)(delegate()
            {
                ShowTime(DateTime.Now);
            }));
        }

        protected void RaisePropertyChanged(String info)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        private void ShowTime(DateTime dTime)
        {
            if (LastTime.AddMilliseconds(500) < dTime)
            {
                string str = dTime.ToString("HHmmss");
                H1 = str.Substring(0, 1);
                H2 = str.Substring(1, 1);
                M1 = str.Substring(2, 1);
                M2 = str.Substring(3, 1);
                S1 = str.Substring(4, 1);
                S2 = str.Substring(5, 1);
                LastTime = dTime;
                if (Ticker)
                { if (TickOn) { TickOn = false; } else { TickOn = true; } }
                else { TickOn = true; }
                RaisePropertyChanged("H1");
                RaisePropertyChanged("H2");
                RaisePropertyChanged("M1");
                RaisePropertyChanged("M2");
                RaisePropertyChanged("S1");
                RaisePropertyChanged("S2");
                RaisePropertyChanged("TickOn");
                RaisePropertyChanged("TickVisible");
            }
        }

    }

}
