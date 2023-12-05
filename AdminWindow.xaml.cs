using ais.Users;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            DataLoader.LoadUsers(comboboxUser);
            DataLoader.LoadSubjectsIntoComboBoxes(comboboxSubject, comboboxSubjectsAssign, comboboxSubjectAssign2);
            DataLoader.LoadGroupsIntoComboBoxes(comboboxGroup, comboboxGroupAssign, comboboxGroupAssign2);
            DataLoader.LoadLecturers(comboboxLecturer);
            DataLoader.LoadStudents(comboboxStudentAssign);
        }

        private void LogoutClick(object sender, RoutedEventArgs e)
        {
            WindowManager.OpenMainWindow();
            Close();
        }
        private void AddUserClick(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedComboBoxItem = roleInput.SelectedItem as ComboBoxItem;
            string username = usernameInput.Text;
            string password = passwordInput.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || selectedComboBoxItem == null)
            {
                MessageBox.Show("Užpildykite tuščius laukelius!");
                return;
            }

            if (int.TryParse(selectedComboBoxItem.Tag?.ToString(), out int selectedRoleID))
            {
                string firstName = username;
                string lastName = password;
                DbCommands.AddUser(username, password, selectedRoleID);
                MessageBox.Show("Naudotoja sukurtas.");
                DataCleaner.CleanData(comboboxUser, comboboxStudentAssign, comboboxLecturer);
                DataLoader.LoadUsers(comboboxUser);
                DataLoader.LoadStudents(comboboxStudentAssign);
                DataLoader.LoadLecturers(comboboxLecturer);
            }
        }

        private void DeleteUserClick(object sender, RoutedEventArgs e)
        {
            string selectedFullName = comboboxUser.SelectedItem as string;

            if (string.IsNullOrEmpty(selectedFullName))
            {
                MessageBox.Show("Pasirinkite naudotoją!");
                return;
            }

            string[] names = selectedFullName.Split(' ');

            if (names.Length != 3)
            {
                MessageBox.Show("Netinkamas vardo formatas.");
                return;
            }

            string firstName = names[1];
            string lastName = names[2];

            DbCommands.DeleteUser(firstName, lastName);
            MessageBox.Show("Naudotojas ištrintas");
            DataCleaner.CleanData(comboboxUser, comboboxStudentAssign, comboboxLecturer);
            DataLoader.LoadUsers(comboboxUser);
            DataLoader.LoadStudents(comboboxStudentAssign);
            DataLoader.LoadLecturers(comboboxLecturer);
        }

        private void AddGroupClick(object sender, RoutedEventArgs e)
        {
            string newGroup = groupNameInput.Text;
            if (string.IsNullOrEmpty(newGroup))
            {
                MessageBox.Show("Užpildykite tuščius laukelius!");
                return;
            }
            DbCommands.AddGroup(newGroup);
            MessageBox.Show("Grupė sukurta.");
            DataCleaner.CleanData(comboboxGroup, comboboxGroupAssign, comboboxGroupAssign2);
            DataLoader.LoadGroupsIntoComboBoxes(comboboxGroup, comboboxGroupAssign, comboboxGroupAssign2);
        }

        private void DeleteGroupClick(object sender, RoutedEventArgs e)
        {
            string selectedGroup = comboboxGroup.SelectedItem as string;
            if (string.IsNullOrEmpty(selectedGroup))
            {
                MessageBox.Show("Pasirinkite grupę!");
                return;
            }
            int groupID = Extraction.GetIDFromFullName(selectedGroup);
            DbCommands.DeleteGroup(groupID);
            MessageBox.Show("Grupė ištrinta.");
            DataCleaner.CleanData(comboboxGroup, comboboxGroupAssign, comboboxGroupAssign2);
            DataLoader.LoadGroupsIntoComboBoxes(comboboxGroup, comboboxGroupAssign, comboboxGroupAssign2);
        }

        private void AssignLecturerToSubjectClick(object sender, RoutedEventArgs e)
        {
            string selectedLecturer = comboboxLecturer.SelectedItem as string;
            string selectedSubject = comboboxSubjectsAssign.SelectedItem as string;
            if (string.IsNullOrEmpty(selectedLecturer) || string.IsNullOrEmpty(selectedSubject))
            {
                MessageBox.Show("Pasirinkite visus reikiamus laukus!");
                return;
            }
            int lecturerID = Extraction.GetIDFromFullName(selectedLecturer);
            int subjectID = Extraction.GetIDFromFullName(selectedSubject);
            DbCommands.AssignLecturerToSubject(lecturerID, subjectID);
            MessageBox.Show("Dėstytojas priskirtas dalykui.");
        }

        private void AssignStudentToGroupClick(object sender, RoutedEventArgs e)
        {
            string selectedStudent = comboboxStudentAssign.SelectedItem as string;
            string selectedGroup = comboboxGroupAssign.SelectedItem as string;
            if (string.IsNullOrEmpty(selectedStudent) || string.IsNullOrEmpty(selectedGroup))
            {
                MessageBox.Show("Pasirinkite visus reikiamus laukus!");
                return;
            }
            int studentID = Extraction.GetIDFromFullName(selectedStudent);
            int groupID = Extraction.GetIDFromFullName(selectedGroup);
            DbCommands.AssignStudentToGroup(studentID, groupID);
            MessageBox.Show("Studentas priskirtas grupei.");
        }

        private void AssignSubjectToGroupClick(Object sender, RoutedEventArgs e)
        {
            string selectedSubject = comboboxSubjectAssign2.SelectedItem as string;
            string selectedGroup = comboboxGroupAssign2.SelectedItem as string;
            if (string.IsNullOrEmpty(selectedSubject) || string.IsNullOrEmpty(selectedGroup))
            {
                MessageBox.Show("Pasirinkite visus reikiamus laukus!");
                return;
            }
            int subjectID = Extraction.GetIDFromFullName(selectedSubject);
            int groupID = Extraction.GetIDFromFullName(selectedGroup);
            DbCommands.AssignSubjectToGroup(subjectID, groupID);
            MessageBox.Show("Dalykas priskirtas grupei.");
        }

        private void AddSubjectClick(object sender, RoutedEventArgs e)
        {
            string newSubject = subjectInput.Text;
            if (string.IsNullOrEmpty(newSubject))
            {
                MessageBox.Show("Užpildykite tuščius laukelius!");
                return;
            }
            DbCommands.AddSubject(newSubject);
            MessageBox.Show("Dalykas sukurtas.");
            DataCleaner.CleanData(comboboxSubject, comboboxSubjectsAssign, comboboxSubjectAssign2);
            DataLoader.LoadSubjectsIntoComboBoxes(comboboxSubject, comboboxSubjectsAssign, comboboxSubjectAssign2);
        }

        private void DeleteSubjectClick(object sender, RoutedEventArgs e)
        {
            string selectedSubject = comboboxSubject.SelectedItem as string;
            if (string.IsNullOrEmpty(selectedSubject))
            {
                MessageBox.Show("Pasirinkite dalyką.");
                return;
            }
            int subjectID = Extraction.GetIDFromFullName(selectedSubject);
            DbCommands.DeleteSubject(subjectID);
            MessageBox.Show("Dalykas ištrintas.");
            DataCleaner.CleanData(comboboxSubject, comboboxSubjectsAssign, comboboxSubjectAssign2);
            DataLoader.LoadSubjectsIntoComboBoxes(comboboxSubject, comboboxSubjectsAssign, comboboxSubjectAssign2);
        }
    }
}
