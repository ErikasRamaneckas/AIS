using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace ais
{
    internal class DbConnection
    {
        private static DbConnection instance;
        private static MySqlConnection conn;

        /*
        private static string server = "mysql6008.site4now.net";
        private static string database = "db_aa2681_erikasr";
        private static string username = "aa2681_erikasr";
        private static string password = "password123";
        */
        /*
        private static string server = "192.168.0.159";
        private static string database = "ais_duomenu_baze";
        private static string username = "erikasAIS";
        private static string password = "123ais";
        */
        private static string server = "127.0.0.1";
        private static string database = "ais_duomenu_baze";
        private static string username = "root";
        private static string password = "";
        private static string constring = $"SERVER={server};DATABASE={database};UID={username};PASSWORD={password};";

        public DbConnection()
        {
            conn = new MySqlConnection(constring);
        }

        #region Singleton
        public static DbConnection Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DbConnection();
                }
                return instance;
            }
        }
        #endregion

        public void openConnection()
        {
            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();
        }
        public void closeConnection()
        {
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();
        }

        public MySqlConnection getConnection()
        {
            return conn;
        }
    }
}
