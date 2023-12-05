using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ais
{
    internal class DataCleaner
    {
        public static void CleanData(params ComboBox[] comboboxes)
        {
            comboboxes[0].Items.Clear();
            comboboxes[1].Items.Clear();
            comboboxes[2].Items.Clear();
        }
    }
}
