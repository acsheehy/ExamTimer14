using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExamTimer14
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ExInfo MyInfo;

        public MainWindow()
        {
            InitializeComponent();
            MyInfo = new ExInfo();
            LoadSettings();
            MainClock.ShowingSeconds = MyInfo.ShallIShowSeconds();
            DataContext = MyInfo;
            MyInfo.ShowSecondsEvent += MyInfo_ShowSecondsEvent;
        }

        void MyInfo_ShowSecondsEvent(object sender, EventArgs e)
        {
            MainClock.ShowingSeconds = MyInfo.ShallIShowSeconds();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            MyInfo.CreateNewExam();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            Settings SettingsForm = new Settings(MyInfo.CentreNumber, MyInfo.ShowSeconds, MyInfo.Pulse, MyInfo.PlayAudio);
            SettingsForm.ShowDialog();
            if (!SettingsForm.IsCancelled)
            {
                MyInfo.CentreNumber = SettingsForm.CentreRef;
                MyInfo.ShowSeconds = SettingsForm.ShowSeconds;
                MyInfo.Pulse = SettingsForm.Pulse;
                MyInfo.PlayAudio = SettingsForm.PlayAudio;
                MainClock.ShowingSeconds = MyInfo.ShallIShowSeconds();
                SaveSettings();
            }
        }

        private void btnExam1Del_Click(object sender, RoutedEventArgs e)
        {
            MyInfo.DeleteExam(0);
        }

        private void btnExam1Edit_Click(object sender, RoutedEventArgs e)
        {
            MyInfo.EditExam(0);
        }

        private void btnExam2Del_Click(object sender, RoutedEventArgs e)
        {
            MyInfo.DeleteExam(1);
        }

        private void btnExam2Edit_Click(object sender, RoutedEventArgs e)
        {
            MyInfo.EditExam(1);
        }

        private void btnExam4Del_Click(object sender, RoutedEventArgs e)
        {
            MyInfo.DeleteExam(3);
        }

        private void btnExam4Edit_Click(object sender, RoutedEventArgs e)
        {
            MyInfo.EditExam(3);
        }

        private void btnExam5Del_Click(object sender, RoutedEventArgs e)
        {
            MyInfo.DeleteExam(4);
        }

        private void btnExam5Edit_Click(object sender, RoutedEventArgs e)
        {
            MyInfo.EditExam(4);
        }

        private void btnExam6Del_Click(object sender, RoutedEventArgs e)
        {
            MyInfo.DeleteExam(5);
        }

        private void btnExam6Edit_Click(object sender, RoutedEventArgs e)
        {
            MyInfo.EditExam(5);
        }

        private void btnExam3Del_Click(object sender, RoutedEventArgs e)
        {
            MyInfo.DeleteExam(2);
        }

        private void btnExam3Edit_Click(object sender, RoutedEventArgs e)
        {
            MyInfo.EditExam(2);
        }

        private void LoadSettings()
        {
            MyInfo.ShowSeconds = Properties.Settings.Default.ShowSeconds;
            MyInfo.CentreNumber = Properties.Settings.Default.CentreRef;
            MyInfo.Pulse = Properties.Settings.Default.Pulse;
            MyInfo.PlayAudio = Properties.Settings.Default.PlayAudio;
            // This forceably disables all audio so that it cannot be enabled.
            MyInfo.PlayAudio = false;
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.ShowSeconds = MyInfo.ShowSeconds;
            Properties.Settings.Default.CentreRef = MyInfo.CentreNumber;
            Properties.Settings.Default.Pulse = MyInfo.Pulse;
            // Properties.Settings.Default.PlayAudio = MyInfo.PlayAudio;
            // The feature for audio is being deprecated but not removed from the code
            // This setting ensures that the audio is disabled in registry.
            Properties.Settings.Default.PlayAudio = false;
            Properties.Settings.Default.Save();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveSettings();
        }

    }
}
