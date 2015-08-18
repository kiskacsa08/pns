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
    class Problem
    {
        public string name { get; set; }
        public string algorithm { get; set; }
        public int limit { get; set; }
        public int threadNum { get; set; }
        public PGraph graph { get; set; }
        public string status { get; set; }
        public Int32 timestamp { get; set; }

        public Problem()
        {            
            algorithm = "";
            graph = new PGraph();
            threadNum = 4;
            limit = 10;
            status = "pending";
            timestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            name = "Graph_" + timestamp.ToString();
        }

        public Problem(string palg, PGraph pgraph, int pthread = 4, int plimit=100)
        {
            switch (palg)
            {
                case "INSIDEOUT":
                    algorithm = "INSIDE_OUT";
                    break;
                case "SSG":
                    algorithm = "SSG";
                    break;
                default:
                    algorithm = "SSG";
                    break;
            }
            graph = pgraph;
            threadNum = pthread;
            limit = plimit;
            status = "pending";
            timestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            name = "Graph_" + timestamp.ToString();
         }
    }
}
