/* Copyright 2015 Department of Computer Science and Systems Technology, University of Pannonia

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License. 
*/

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
        private static int instanceCounter = 0;
        private readonly int instanceId;
        public int UniqeId
        {
            get { return this.instanceId; }
        }

        public MutualExclusion(string name, List<OperatingUnit> opUnits)
        {
            this.instanceId = ++instanceCounter;
            this.ID = 0;
            this.Name = name;
            this.OpUnits = opUnits;
        }

        public MutualExclusion(List<OperatingUnit> opUnits)
        {
            this.instanceId = ++instanceCounter;
            this.ID = 0;
            this.OpUnits = opUnits;
            StringBuilder sb = new StringBuilder();
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

            this.Name = sb.ToString();

            Console.WriteLine(this.UniqeId.ToString());

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
            sb.Append(" ");
            sb.Append(this.UniqeId.ToString());

            this.Label = sb.ToString();
        }
    }
}
