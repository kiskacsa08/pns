using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNSDraw
{
    public class MutualExclusion
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public List<OperatingUnit> OpUnits { get; set; }
        public int ID { get; set; }

        public MutualExclusion(string name, List<OperatingUnit> opUnits)
        {
            ID = 0;
            this.Name = name;
            this.OpUnits = opUnits;
        }

        public MutualExclusion(List<OperatingUnit> opUnits)
        {
            ID = 0;
            this.OpUnits = opUnits;
            StringBuilder sb = new StringBuilder();
            //sb.Append("[");
            int i = 0;
            foreach (OperatingUnit ou in this.OpUnits)
            {
                if (i < 3)
                {
                    sb.Append(ou.Name);
                    sb.Append("");
                }
                i++;
            }
            sb.Remove(sb.Length - 1, 1);
            Int32 timestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            sb.Append(timestamp.ToString());
            //sb.Append("]");

            this.Name = sb.ToString();

            sb.Clear();
            sb.Append("[");
            i = 0;
            foreach (OperatingUnit ou in this.OpUnits)
            {
                if (i < 3)
                {
                    sb.Append(ou.Name);
                    sb.Append(";");
                }
                i++;
            }
            sb.Remove(sb.Length - 1, 1);
            if (i > 3)
            {
                sb.Append(";...");
            }
            sb.Append("]");

            this.Label = sb.ToString();
        }
    }
}
