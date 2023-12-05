using System;
using System.Collections.Generic;
using System.Data;
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

namespace ais
{
    /// <summary>
    /// Interaction logic for StudentWindow.xaml
    /// </summary>
    public partial class StudentWindow : Window
    {
        public StudentWindow()
        {
            InitializeComponent();
            LoadGrades();
        }

        private void LoadGrades()
        {
            int userID = SessionManager.LoggedInUserID;
            DataTable gradesTable = DbCommands.ViewGrades(userID);
            gradesDataGrid.ItemsSource = gradesTable.DefaultView;
        }

        private void LogoutClick(object sender, RoutedEventArgs e)
        {
            WindowManager.OpenMainWindow();
            Close();
        }
    }
}
