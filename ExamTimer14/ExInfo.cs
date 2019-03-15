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
    public class ExInfo : INotifyPropertyChanged
    {


        public String CentreNumber { get { return p_CentreNumber; } set { p_CentreNumber = value; RaisePropertyChanged("CentreHeading"); } }
        public String CentreHeading { get { return "Centre Number : " + CentreNumber; } }
        public String ExamDate { get { return DateTime.Now.ToShortDateString(); } } //DateTime.Now.ToString("ddd dd MMM yyyy"); } }
        public IndExInfos Exams { get { return p_Exams; } set { p_Exams = value; } }
        public Boolean ShowSeconds { get { return p_ShowSecondsInExams; } set { p_ShowSecondsInExams = value; } }


        public Boolean PlayAudio { get { return p_PlayAudio; } set { p_PlayAudio = value; SetAudio(); } }
        public Boolean Pulse { get { return p_Pulse; } set { p_Pulse = value; SetPulse(); } }

        public IndExInfo Exam1 { get { if (Exams.Count > 0) { return Exams[0]; } else { return null; } } }
        public IndExInfo Exam2 { get { if (Exams.Count > 1) { return Exams[1]; } else { return null; } } }
        public IndExInfo Exam3 { get { if (Exams.Count > 2) { return Exams[2]; } else { return null; } } }
        public IndExInfo Exam4 { get { if (Exams.Count > 3) { return Exams[3]; } else { return null; } } }
        public IndExInfo Exam5 { get { if (Exams.Count > 4) { return Exams[4]; } else { return null; } } }
        public IndExInfo Exam6 { get { if (Exams.Count > 5) { return Exams[5]; } else { return null; } } }

        public Visibility Exam1Visibility { get { if (Exams.Count > 0) { return Visibility.Visible; } else { return Visibility.Hidden; } } }
        public Visibility Exam2Visibility { get { if (Exams.Count > 1) { return Visibility.Visible; } else { return Visibility.Hidden; } } }
        public Visibility Exam3Visibility { get { if (Exams.Count > 2) { return Visibility.Visible; } else { return Visibility.Hidden; } } }
        public Visibility Exam4Visibility { get { if (Exams.Count > 3) { return Visibility.Visible; } else { return Visibility.Hidden; } } }
        public Visibility Exam5Visibility { get { if (Exams.Count > 4) { return Visibility.Visible; } else { return Visibility.Hidden; } } }
        public Visibility Exam6Visibility { get { if (Exams.Count > 5) { return Visibility.Visible; } else { return Visibility.Hidden; } } }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler ShowSecondsEvent;

        private IndExInfos p_Exams;
        private string p_CentreNumber;
        private bool p_Pulse;
        private bool p_PlayAudio;
        private bool p_ShowSecondsInExams;
        private bool p_CurrentlyShowingSeconds = true;
        private System.Timers.Timer p_SecTimer;

        private Dispatcher WindowsDispatcher;

        public ExInfo()
        {
            // CentreNumber = "10287";
            p_Exams = new IndExInfos();
            WindowsDispatcher = Dispatcher.CurrentDispatcher;
            p_SecTimer = new System.Timers.Timer(5000);
            p_SecTimer.Elapsed += p_SecTimer_Elapsed;
            p_SecTimer.Enabled = true;
        }

        void p_SecTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            WindowsDispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate()
            {
                if (p_CurrentlyShowingSeconds != ShallIShowSeconds())
                {
                    // raise other event
                    p_CurrentlyShowingSeconds = ShallIShowSeconds();
                    if (ShowSecondsEvent != null) ShowSecondsEvent(this, null);
                }
            });
        }

        

        public void CreateNewExam()
        {
            IndExInfo NewExam = new IndExInfo();
            NewExam.Editing = true;
            NewExam.Pulse = p_Pulse;
            NewExam.PlayAudio = p_PlayAudio;
            EditExam ExamForm = new EditExam(NewExam);
            ExamForm.ShowDialog();
            if (!ExamForm.IsCancelled)
            {
                ExamForm.EditedExam.Editing = false;
                p_Exams.Add(ExamForm.EditedExam);
                RaiseAllExams();
            }
        }

        public void DeleteExam(int FromPosition)
        {
            if (MessageBox.Show("Are you sure?","Delete Exam",MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (Exams.Count > FromPosition)
                {
                    Exams.RemoveAt(FromPosition);
                    RaiseAllExams();
                }
            }
        }

        public void EditExam(int InPosition)
        {
            if (Exams.Count>InPosition)
            {
                EditExam ExamForm = new EditExam(Exams[InPosition]);
                ExamForm.EditedExam.Editing = true;
                ExamForm.ShowDialog();
                if (!ExamForm.IsCancelled)
                {
                    ExamForm.EditedExam.Editing = false;
                    Exams[InPosition] = ExamForm.EditedExam;
                    RaiseAllExams();
                }
            }
        }

        private void RaiseAllExams()
        {
            RaisePropertyChanged("Exam1");
            RaisePropertyChanged("Exam2");
            RaisePropertyChanged("Exam3");
            RaisePropertyChanged("Exam4");
            RaisePropertyChanged("Exam5");
            RaisePropertyChanged("Exam6");
            RaisePropertyChanged("Exam1Visibility");
            RaisePropertyChanged("Exam2Visibility");
            RaisePropertyChanged("Exam3Visibility");
            RaisePropertyChanged("Exam4Visibility");
            RaisePropertyChanged("Exam5Visibility");
            RaisePropertyChanged("Exam6Visibility");
        }

        protected void RaisePropertyChanged(String info)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        private void SetPulse()
        {
            if (p_Exams.Count > 0)
            {
                foreach (IndExInfo Exam in p_Exams)
                {
                    Exam.Pulse = p_Pulse;
                }
            }
        }

        private void SetAudio()
        {
            if (p_Exams.Count > 0)
            {
                foreach (IndExInfo Exam in p_Exams)
                {
                    Exam.PlayAudio = p_PlayAudio;
                }
            }
        }

        public bool ShallIShowSeconds()
        {
            if (p_ShowSecondsInExams) return true;
            if (p_Exams.Count > 0)
            {
                foreach (IndExInfo item in p_Exams)
                {
                    if (item.ExamInProgress) return false;
                }
            }
            return true;
        }

    }

}
