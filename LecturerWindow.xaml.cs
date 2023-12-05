using ais.Users;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
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
    /// Interaction logic for LecturerWindow.xaml
    /// </summary>
    public partial class LecturerWindow : Window
    {
        public LecturerWindow()
        {
            InitializeComponent();
            int userID = SessionManager.LoggedInUserID;
            DataLoader.LoadComboBoxesForLecturer(userID, comboboxSubject, comboboxSubject2, comboboxStudent, comboboxStudent2);
            DataLoader.LoadGradeValues(comboboxGradeValues);
            DataLoader.LoadGradeValues(comboboxGrades);
            comboboxStudent2.SelectionChanged += ComboBoxStudent_SelectionChanged;
            comboboxSubject2.SelectionChanged += ComboBoxSubject_SelectionChanged;
        }

        private void LogoutClick(object sender, RoutedEventArgs e)
        {
            WindowManager.OpenMainWindow();
            Close();
        }

        private void WriteGradeClick(object sender, RoutedEventArgs e)
        {
            string selectedStudent = comboboxStudent.SelectedItem as string;
            string selectedSubject = comboboxSubject.SelectedItem as string;
            string selectedGradeValue = comboboxGradeValues.SelectedItem as string;
            if (selectedStudent == null || selectedSubject == null || selectedGradeValue == null)
            {
                MessageBox.Show("Pasirinkite visus reikiamus laukus!");
                return;
            } 
            int studentID = Extraction.GetIDFromFullName(selectedStudent);
            int subjectID = Extraction.GetIDFromFullName(selectedSubject);
            int gradeID = Extraction.GetIDFromFullName(selectedGradeValue);
            DbCommands.AddGrade(studentID, subjectID, gradeID);
            MessageBox.Show("Pažymys įrašytas.");
            string selectedStudentId2 = comboboxStudent2.SelectedItem as string;
            string selectedSubjectId2 = comboboxSubject2.SelectedItem as string;
            DataLoader.LoadGrades(selectedStudentId2, selectedSubjectId2, comboboxOldGrades);
        }

        private void UpdateGradeClick(object sender, RoutedEventArgs e)
        {
            string selectedStudentId = comboboxStudent2.SelectedItem as string;
            string selectedSubjectId = comboboxSubject2.SelectedItem as string;
            string selectedGradeId = comboboxOldGrades.SelectedItem as string;
            string selectedNewGradeId = comboboxGrades.SelectedItem as string;
            if (comboboxGrades.SelectedItem == null || comboboxStudent2.SelectedItem == null || comboboxSubject2.SelectedItem == null)
            {
                MessageBox.Show("Pasirinkite visus reikiamus laukus!");
                return;
            }
                int gradeID = Extraction.GetIDFromFullName(selectedGradeId);
                int newGrade = Extraction.GetIDFromFullName(selectedNewGradeId);
                DbCommands.UpdateGrade(newGrade, gradeID);
                MessageBox.Show("Pažymys pakeistas.");
                DataLoader.LoadGrades(selectedStudentId, selectedSubjectId, comboboxOldGrades);
        }

        private void ComboBoxStudent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboboxStudent2.SelectedItem != null && comboboxSubject2.SelectedItem != null)
            {
                string selectedStudentId = comboboxStudent2.SelectedItem.ToString();
                string selectedSubjectId = comboboxSubject2.SelectedItem.ToString();
                DataLoader.LoadGrades(selectedStudentId, selectedSubjectId, comboboxOldGrades);
            }
        }

        private void ComboBoxSubject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboboxStudent2.SelectedItem != null && comboboxSubject2.SelectedItem != null)
            {
                string selectedStudentId = comboboxStudent2.SelectedItem.ToString();
                string selectedSubjectId = comboboxSubject2.SelectedItem.ToString();
                DataLoader.LoadGrades(selectedStudentId, selectedSubjectId, comboboxOldGrades);
            }
        }
    }
}

