using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace ais
{
    internal class DataLoader
    {
        public static void LoadComboBoxesForLecturer(int userID, params ComboBox[] comboBoxes)
        {
            LoadSubjectsForLecturer(userID, comboBoxes[0]);
            LoadSubjectsForLecturer(userID, comboBoxes[1]);
            LoadLecturersStudents(comboBoxes[2]);
            LoadLecturersStudents(comboBoxes[3]);
        }

        public static void LoadSubjectsForLecturer(int userID, ComboBox comboBox)
        {
            string sql = "SELECT subjects.id, subjects.subject_name " +
                         "FROM subjects " +
                         "JOIN lecturers ON subjects.lecturer_id = lecturers.id " +
                         "WHERE lecturers.id = @UserID";
            try
            {
                DbConnection.Instance.openConnection();
                using (MySqlCommand cmd = new MySqlCommand(sql, DbConnection.Instance.getConnection()))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string displayName = reader["id"].ToString();
                            string subjectName = reader["subject_name"].ToString();
                            displayName = $"{displayName}. {subjectName}";
                            comboBox.Items.Add(displayName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                DbConnection.Instance.closeConnection();
            }
        }

        public static void LoadLecturersStudents(ComboBox comboBox)
        {
            string sql = "SELECT DISTINCT students.id, students.first_name, students.last_name " +
                             "FROM students " +
                             "JOIN `groups` ON students.group_id = `groups`.id " +
                             "JOIN subjects ON `groups`.id = subjects.group_id " +
                             "JOIN lecturers ON subjects.lecturer_id = lecturers.id";
            LoadComboBoxItems(comboBox, sql, "id", "first_name", "last_name");
        }

        public static void LoadUsers(ComboBox comboBox)
        {
            string sql = "SELECT id, first_name, last_name FROM lecturers " +
                         "UNION ALL " +
                         "SELECT id, first_name, last_name FROM students";
            LoadComboBoxItems(comboBox, sql, "id", "first_name", "last_name");
        }

        public static void LoadLecturers(ComboBox comboBox)
        {
            string sql = "SELECT id, first_name, last_name FROM lecturers";
            LoadComboBoxItems(comboBox, sql, "id", "first_name", "last_name");
        }

        public static void LoadStudents(ComboBox comboBox)
        {
            string sql = "SELECT id, first_name, last_name FROM students";
            LoadComboBoxItems(comboBox, sql, "id", "first_name", "last_name");
        }

        public static void LoadSubjects(ComboBox comboBox)
        {
            string sql = "SELECT id, subject_name FROM subjects";
            LoadComboBoxItems(comboBox, sql, "id", "subject_name");
        }

        public static void LoadSubjectsIntoComboBoxes(params ComboBox[] comboBoxes)
        {
            LoadSubjects(comboBoxes[0]);
            LoadSubjects(comboBoxes[1]);
            LoadSubjects(comboBoxes[2]);
        }

        public static void LoadGroupsIntoComboBoxes(params ComboBox[] comboBoxes)
        {
            LoadGroups(comboBoxes[0]);
            LoadGroups(comboBoxes[1]);
            LoadGroups(comboBoxes[2]);
        }
        public static void LoadGroups(ComboBox comboBox)
        {
            string sql = "SELECT id, group_name FROM `groups`";
            LoadComboBoxItems(comboBox, sql, "id", "group_name");
        }
        public static void LoadGradeValues(ComboBox comboBox)
        {
            string sql = "SELECT id FROM grade_values";
            LoadComboBoxItems(comboBox, sql, "id");
        }

        public static void LoadGrades(string selectedStudentId, string selectedSubjectId, ComboBox combobox)
        {
            try
            {
                string sql = $"SELECT id, grade_id FROM grades WHERE student_id = '{selectedStudentId}' AND subject_id = '{selectedSubjectId}'";
                DbConnection.Instance.openConnection();
                using (MySqlCommand cmd = new MySqlCommand(sql, DbConnection.Instance.getConnection()))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        combobox.Items.Clear();
                        while (reader.Read())
                        {
                            string gradeId = reader["grade_id"].ToString();
                            string id = reader["id"].ToString();
                            string displayName = $"{id}. {gradeId}";
                            combobox.Items.Add(displayName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                DbConnection.Instance.closeConnection();
            }
        }
        public static void LoadComboBoxItems(ComboBox comboBox, string sql, params string[] displayColumnNames)
        {
            try
            {
                DbConnection.Instance.openConnection();
                using (MySqlCommand cmd = new MySqlCommand(sql, DbConnection.Instance.getConnection()))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string displayName = reader[displayColumnNames[0]].ToString();
                            string remainingColumns = string.Join(" ", displayColumnNames.Skip(1).Select(col => reader[col].ToString()));
                            displayName = $"{displayName}. {remainingColumns}";
                            comboBox.Items.Add(displayName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                DbConnection.Instance.closeConnection();
            }
        }
    }
}
