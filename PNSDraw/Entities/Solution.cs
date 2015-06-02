using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNSDraw.Entities
{
    public class Solution
    {
        public Dictionary<string, double> Materials;
        public Dictionary<string, double> OperatingUnits;

        public int Index = 0;
        
        public string Title = "";

        public double OptimalValue { get; set; }
        public string AlgorithmUsed { get; set; }

        public Solution(int index, string title)
        {
            Materials = new Dictionary<string, double>();
            OperatingUnits = new Dictionary<string, double>();
            Index = index;
            Title = title;
        }


        public void AddMaterial(string name, double flow)
        {
            if (Materials.ContainsKey(name) == false)
            {
                Materials[name] = flow;
            }
        }


        public void RemoveMaterial(string name)
        {
            if (Materials.ContainsKey(name) == true)
            {
                Materials.Remove(name);
            }
        }

        public void AddOperatingUnit(string name, double size)
        {
            if (OperatingUnits.ContainsKey(name) == false)
            {
                OperatingUnits[name] = size;
            }
        }


        public void RemoveOperatingUnit(string name)
        {
            if (OperatingUnits.ContainsKey(name) == true)
            {
                OperatingUnits.Remove(name);
            }
        }

    }

    class Solutions : List<Solution> { }
}
