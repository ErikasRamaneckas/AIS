using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ais.Users
{
    internal abstract class User
    {
        public int ID { get; set; }
        public string? FirstName {  get; set; }
        public string? LastName { get; set; }
        public string? Role { get; set; }  
        public int LoginID {  get; set; }
    }
}
