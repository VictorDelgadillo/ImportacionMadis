using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siged_Spatial_ImportExportData
{
    public class Division
    {
        public string Value;
        public string Text;

        public Division(string val, string text)
        {
            Value = val;
            Text = text;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
