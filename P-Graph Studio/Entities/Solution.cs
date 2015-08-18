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
    public class Solution
    {
        public Dictionary<string, MaterialProperty> Materials;
        public Dictionary<string, OperatingUnitProperty> OperatingUnits;

        public int Index = 0;
        
        public string Title = "";

        public double OptimalValue { get; set; }
        public string AlgorithmUsed { get; set; }

        public Solution(int index, string title)
        {
            Materials = new Dictionary<string, MaterialProperty>();
            OperatingUnits = new Dictionary<string, OperatingUnitProperty>();
            Index = index;
            Title = title;
        }

        public Solution(int index)
        {
            Materials = new Dictionary<string, MaterialProperty>();
            OperatingUnits = new Dictionary<string, OperatingUnitProperty>();
            Index = index;
            Title = "Unknown solution #" + index;
        }

        public void AddMaterial(string name, double flow)
        {
            if (Materials.ContainsKey(name) == false)
            {
                MaterialProperty prop = new MaterialProperty(name, flow, 0);
                Materials[name] = prop;
            }
        }

        //TODO ezt kell majd használni a végén
        public void AddMaterial(string name, double flow, double cost)
        {
            if (Materials.ContainsKey(name) == false)
            {
                MaterialProperty prop = new MaterialProperty(name, flow, cost);
                Materials[name] = prop;
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
                OperatingUnitProperty prop = new OperatingUnitProperty(name, size, 0, new List<MaterialProperty>(), new List<MaterialProperty>());
                OperatingUnits[name] = prop;
            }
        }

        //TODO ezt kell majd használni a végén
        public void AddOperatingUnit(string name, double size, double cost, List<MaterialProperty> input, List<MaterialProperty> output)
        {
            if (OperatingUnits.ContainsKey(name) == false)
            {
                OperatingUnitProperty prop = new OperatingUnitProperty(name, size, cost, input, output);
                OperatingUnits[name] = prop;
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

    public class MaterialProperty
    {
        public string Name { get; set; }
        public double Flow { get; set; }
        public double Cost { get; set; }

        public MaterialProperty(string name, double flow, double cost)
        {
            this.Name = name;
            this.Flow = flow;
            this.Cost = cost;
        }
    }

    public class OperatingUnitProperty
    {
        public string Name { get; set; }
        public double Size { get; set; }
        public double Cost { get; set; }
        public List<MaterialProperty> Input { get; set; }
        public List<MaterialProperty> Output { get; set; }

        public OperatingUnitProperty(string name, double size, double cost, List<MaterialProperty> input, List<MaterialProperty> output)
        {
            this.Name = name;
            this.Size = size;
            this.Cost = cost;
            this.Input = input;
            this.Output = output;
        }
    }
}
