using ais.Users;
using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ais
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /*

        private void AdminWindow_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow();
            adminWindow.Show();
            Close();
        }
        */

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            User user = Authentification.Authenticate(username, password);
            if (user != null)
            {
                SessionManager.SetLoggedInUserID(user.ID);
                WindowManager.OpenWindowBasedOnRole(user);
                Close();
            }
            else
            {
                MessageBox.Show("Prisijungti nepavyko! Patikrinkite savo prisijungimo duomenis.");
            }
        }
    }
}