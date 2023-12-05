using ais.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ais
{
    internal class WindowManager
    {
        public static void OpenWindowBasedOnRole(User user)
        {
            if(user == null)
            {
                MessageBox.Show("Naudotojas neturi rolės!");
                return;
            }

            if (user is Admin)
            {
                AdminWindow adminWindow = new AdminWindow();
                adminWindow.Show();
            }
            else if (user is Lecturer)
            {
                LecturerWindow lecturerWindow = new LecturerWindow();
                lecturerWindow.Show();
            }
            else if (user is Student)
            {
                StudentWindow studentWindow = new StudentWindow();
                studentWindow.Show();
            }
        }

        public static void OpenMainWindow()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
