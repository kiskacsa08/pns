﻿using Pns.PnsSolver;
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
            Console.WriteLine(palg);
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
