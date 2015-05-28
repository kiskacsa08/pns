using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNSDraw.Entities
{
    public class MutualExclusion
    {
        public string Name { get; set; }
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
            sb.Append("[");
            int i = 0;
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

            this.Name = sb.ToString();
            
        }
    }
}
