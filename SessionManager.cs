using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ais
{
    public static class SessionManager
    {
        public static int LoggedInUserID { get; private set; }
        public static void SetLoggedInUserID(int userID)
        {
            LoggedInUserID = userID;
        }
    }
}
