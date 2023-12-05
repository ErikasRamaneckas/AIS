using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ais.Users;
using MySql.Data.MySqlClient;

namespace ais
{
    internal class DbCommands
    {
        public static void AddUser(string firstName, string lastName, int role)
        {
            string loginSql = "INSERT INTO login (username, password) VALUES (@username, @password)";
            string userSql = "";
            switch (role)
            {
                case 2:
                    userSql = "INSERT INTO lecturers (first_name, last_name, role_id, login_id) VALUES (@firstName, @lastName, @role_id, @login_id)";
                    break;
                case 3:
                    userSql = "INSERT INTO students (first_name, last_name, role_id, login_id) VALUES (@firstName, @lastName, @role_id, @login_id)";
                    break;
                default:
                    MessageBox.Show("Tokios rolės nėra.");
                    return;
            }
            try
            {
                DbConnection.Instance.openConnection();
                using (MySqlCommand loginCmd = new MySqlCommand(loginSql, DbConnection.Instance.getConnection()))
                {
                    loginCmd.CommandType = CommandType.Text;
                    loginCmd.Parameters.Add("@username", MySqlDbType.VarChar).Value = firstName;
                    loginCmd.Parameters.Add("@password", MySqlDbType.VarChar).Value = lastName; 
                    loginCmd.ExecuteNonQuery();
                    int loginId = (int)loginCmd.LastInsertedId;
                    using (MySqlCommand userCmd = new MySqlCommand(userSql, DbConnection.Instance.getConnection()))
                    {
                        userCmd.CommandType = CommandType.Text;
                        userCmd.Parameters.Add("@firstName", MySqlDbType.VarChar).Value = firstName;
                        userCmd.Parameters.Add("@lastName", MySqlDbType.VarChar).Value = lastName;
                        userCmd.Parameters.Add("@role_id", MySqlDbType.Int32).Value = role;
                        userCmd.Parameters.Add("@login_id", MySqlDbType.Int32).Value = loginId;
                        userCmd.ExecuteNonQuery();
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

        public static void DeleteUser(string firstName, string lastName)
        {
            try
            {
                DbConnection.Instance.openConnection();
                string[] userTables = { "lecturers", "students" };

                foreach (var userTable in userTables)
                {
                    string userSql = $"DELETE FROM {userTable} WHERE first_name = @firstName AND last_name = @lastName";
                    string loginSql = $"DELETE FROM login WHERE username = @firstName AND password = @lastName";
                    using (MySqlCommand cmd = new MySqlCommand(userSql, DbConnection.Instance.getConnection()))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@firstName", MySqlDbType.VarChar).Value = firstName;
                        cmd.Parameters.Add("@lastName", MySqlDbType.VarChar).Value = lastName;
                        cmd.ExecuteNonQuery();
                            using (MySqlCommand loginCmd = new MySqlCommand(loginSql, DbConnection.Instance.getConnection()))
                            {
                                loginCmd.CommandType = CommandType.Text;
                                loginCmd.Parameters.Add("@firstName", MySqlDbType.VarChar).Value = firstName;
                                loginCmd.Parameters.Add("@lastName", MySqlDbType.VarChar).Value = lastName;
                                loginCmd.ExecuteNonQuery();
                            return;
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

        public static void AddGroup(string groupName)
        {
            try
            {
                DbConnection.Instance.openConnection();
                string sql = "INSERT INTO `groups` VALUES(id, @groupName)";
                MySqlCommand cmd = new MySqlCommand(sql, DbConnection.Instance.getConnection());
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@groupName", MySqlDbType.VarChar).Value = groupName;
                cmd.ExecuteNonQuery();
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
        
        public static void DeleteGroup(int groupID)
        {
            try
            {
                DbConnection.Instance.openConnection();
                string studentSql = "UPDATE students SET group_id = NULL WHERE group_id = @groupID";
                MySqlCommand cmd2 = new MySqlCommand(studentSql, DbConnection.Instance.getConnection());
                cmd2.CommandType = CommandType.Text;
                cmd2.Parameters.Add("@groupID", MySqlDbType.Int32).Value = groupID;
                cmd2.ExecuteNonQuery();

                string sql = "DELETE FROM `groups` WHERE id = @groupID";
                MySqlCommand cmd = new MySqlCommand(sql, DbConnection.Instance.getConnection());
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@groupID", MySqlDbType.Int32).Value = groupID;
                cmd.ExecuteNonQuery();
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

        public static void AddSubject(string subject_name)
        {
            try
            {
                DbConnection.Instance.openConnection();
                string sql = "INSERT INTO subjects (subject_name) VALUES (@subject_name)";
                MySqlCommand cmd = new MySqlCommand(sql, DbConnection.Instance.getConnection());
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@subject_name", MySqlDbType.VarChar).Value = subject_name;
                cmd.ExecuteNonQuery();
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

        public static void DeleteSubject(int subjectID)
        {
            try
            {
                DbConnection.Instance.openConnection();
                string sql = "DELETE FROM subjects WHERE id = @ID";
                MySqlCommand cmd = new MySqlCommand(sql, DbConnection.Instance.getConnection());
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@ID", MySqlDbType.VarChar).Value = subjectID;
                cmd.ExecuteNonQuery();
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

        public static void AssignLecturerToSubject(int lecturerID, int subjectID)
        {
            try
            {
                DbConnection.Instance.openConnection();
                string sql = "UPDATE subjects SET lecturer_id = @lecturerID WHERE id = @subjectID";
                MySqlCommand cmd = new MySqlCommand(sql, DbConnection.Instance.getConnection());
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@subjectID", MySqlDbType.Int32).Value = subjectID;
                cmd.Parameters.Add("@lecturerID", MySqlDbType.Int32).Value = lecturerID;
                cmd.ExecuteNonQuery();
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

        public static void AssignStudentToGroup(int studentID, int groupID)
        {
            try
            {
                DbConnection.Instance.openConnection();
                string sql = "UPDATE students SET group_id = @groupID WHERE id = @studentID";
                MySqlCommand cmd = new MySqlCommand(sql, DbConnection.Instance.getConnection());
                cmd.Parameters.Add("@studentID", MySqlDbType.Int32).Value = studentID;
                cmd.Parameters.Add("@groupID", MySqlDbType.Int32).Value = groupID;
                cmd.ExecuteNonQuery();
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

        public static void AssignSubjectToGroup(int subjectID, int groupID)
        {
            try
            {
                DbConnection.Instance.openConnection();
                string sql = "UPDATE subjects SET group_id = @groupID WHERE id = @subjectID";
                MySqlCommand cmd = new MySqlCommand(sql, DbConnection.Instance.getConnection());
                cmd.Parameters.Add("@subjectID", MySqlDbType.Int32).Value = subjectID;
                cmd.Parameters.Add("@groupID", MySqlDbType.Int32).Value = groupID;
                cmd.ExecuteNonQuery();
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

        public static void AddGrade(int studentID, int subjectID, int grade)
        {
            try
            {
                DbConnection.Instance.openConnection();
                string sql = "INSERT INTO grades VALUES(id, @studentID, @gradeID, @subjectID)";
                MySqlCommand cmd = new MySqlCommand(sql, DbConnection.Instance.getConnection());
                cmd.Parameters.Add("@studentID", MySqlDbType.Int32).Value = studentID;
                cmd.Parameters.Add("@subjectID", MySqlDbType.Int32).Value = subjectID;
                cmd.Parameters.Add("@gradeID", MySqlDbType.Int32).Value = grade;
                cmd.ExecuteNonQuery();
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

        public static void UpdateGrade(int newGrade,int selectedGradeId)
        {
            try
            {
                string sql = $"UPDATE grades SET grade_id = '{newGrade}' WHERE id = '{selectedGradeId}'";
                DbConnection.Instance.openConnection();
                using (MySqlCommand cmd = new MySqlCommand(sql, DbConnection.Instance.getConnection()))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating grade: {ex.Message}");
            }
            finally
            {
                DbConnection.Instance.closeConnection();
            }
        }

        public static DataTable ViewGrades(int studentID)
        {
            DataTable table = new DataTable();
            try
            {
                DbConnection.Instance.openConnection();
                string sql = "SELECT subjects.subject_name AS 'Dalyko Pavadinimas', grades.grade_id AS 'Pažymys' FROM grades JOIN subjects on grades.subject_id = subjects.id WHERE grades.student_id = @studentID";
                MySqlCommand cmd = new MySqlCommand(sql, DbConnection.Instance.getConnection());
                cmd.Parameters.Add("@studentID", MySqlDbType.Int32).Value = studentID;
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(table);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                DbConnection.Instance.closeConnection();
            }
            return table;
        }
    }
}
