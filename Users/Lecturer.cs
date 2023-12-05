using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ais.Users
{
    internal class Lecturer : User
    {
        public Lecturer(int id, string firstName, string lastName)
        {
            ID = id;
            FirstName = firstName;
            LastName = lastName;
            Role = "lecturer";
        }
    }
}
