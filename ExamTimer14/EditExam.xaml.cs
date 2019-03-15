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
    /// Interaction logic for EditExam.xaml
    /// </summary>
    public partial class EditExam : Window
    {

        private IndExInfo p_IndExam;
        private bool p_Cancelled = true;

        public IndExInfo EditedExam { get { return p_IndExam; } set { p_IndExam = value; } }
        public Boolean IsCancelled { get { return p_Cancelled; } }

        public EditExam(IndExInfo ExamToEdit)
        {
            InitializeComponent();
            p_IndExam = ExamToEdit;
            DataContext = p_IndExam;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            p_Cancelled = false;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            p_Cancelled = true;
            Close();
        }
    }
}
