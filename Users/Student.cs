using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ais.Users
{
    internal class Student : User
    {
        public int GroupID { get; set; }
        public Student(int id, string firstName, string lastName)
        {
            ID = id;
            FirstName = firstName;
            LastName = lastName;
            Role = "student";
        }
    }
}
