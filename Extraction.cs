using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ais
{
    internal class Extraction
    {
        public static int GetIDFromFullName(string fullName)
        {
            if(fullName != null)
            {
                string[] parts = fullName.Split('.');
                if (parts.Length > 0 && int.TryParse(parts[0], out int id))
                {
                    return id;
                }
            }
            return -1;
        }
    }
}
