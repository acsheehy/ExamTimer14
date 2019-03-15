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
using System.Windows.Shapes;

namespace ExamTimer14
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private indSettings p_Settings;
        public String CentreRef { get { return p_Settings.CentreRef; } }
        public Boolean ShowSeconds { get { return p_Settings.ShowSeconds; } }
        public Boolean Pulse { get { return p_Settings.Pulse; } }
        public Boolean PlayAudio { get { return p_Settings.PlayAudio; } }
        public Boolean IsCancelled { get; set; }

        public Settings(String sCentreRef, Boolean bShowSeconds, Boolean bPulse, Boolean bPlayAudio)
        {
            InitializeComponent();
            // The audio component is being deprecated, however, the code still remains. Nevertheless - setting bPlayAudio to false removes 
            // all prevous settings where it is enabled.
            bPlayAudio = false;
            p_Settings = new indSettings() { CentreRef = sCentreRef, ShowSeconds = bShowSeconds, Pulse = bPulse, PlayAudio = bPlayAudio };
            DataContext = p_Settings;
            IsCancelled = true;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            IsCancelled = false;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            IsCancelled = true;
            Close();
        }

    }

    class indSettings
    {
        public String CentreRef { get; set; }
        public Boolean ShowSeconds { get; set; }
        public Boolean Pulse { get; set; }
        public Boolean PlayAudio { get; set; }
    }
}
