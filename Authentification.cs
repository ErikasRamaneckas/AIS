using ais.Users;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace ais
{
    internal class Authentification
    {
        public static User Authenticate(string username, string password)
        {
            User user = null;
            try
            {
                string sql = @" SELECT 'administrator' AS role, administrators.id AS id, administrators.first_name AS first_name, administrators.last_name AS last_name FROM login
                JOIN administrators ON login.id = administrators.login_id
                WHERE login.username = @Username AND login.password = @Password
                UNION ALL
                SELECT 'lecturer' AS role, lecturers.id AS id, lecturers.first_name AS first_name, lecturers.last_name AS last_name FROM login
                JOIN lecturers ON login.id = lecturers.login_id
                WHERE login.username = @Username AND login.password = @Password
                UNION ALL
                SELECT 'student' AS role, students.id AS id, students.first_name AS first_name, students.last_name AS last_name FROM login
                JOIN students ON login.id = students.login_id
                WHERE login.username = @Username AND login.password = @Password
                UNION ALL
                SELECT '' AS role, -1 AS id, NULL AS first_name, NULL AS last_name
                WHERE NOT EXISTS (
                    SELECT 1 FROM login
                    WHERE login.username = @Username AND login.password = @Password);";

                DbConnection.Instance.openConnection();
                using (MySqlCommand cmd = new MySqlCommand(sql, DbConnection.Instance.getConnection()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@Username", MySqlDbType.VarChar).Value = username;
                    cmd.Parameters.Add("@Password", MySqlDbType.VarChar).Value = password;
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string role = reader.GetString("role");
                            int userID = reader.GetInt32("id");
                            string firstName = reader["first_name"] as string;
                            string lastName = reader["last_name"] as string;
                            switch (role)
                            {
                                case "administrator":
                                    user = new Admin(userID, firstName, lastName);
                                    break;
                                case "lecturer":
                                    user = new Lecturer(userID, firstName, lastName);
                                    break;
                                case "student":
                                    user = new Student(userID, firstName, lastName);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                DbConnection.Instance.closeConnection();
            }
            return user;
        }
    }
}
