using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNSDraw
{
    class FileConnector
    {
        public static void ProblemToSolverInput(Problem problem, string filename)
        {
            StringBuilder file_str = new StringBuilder();
            
            // Fejléc
            file_str.Append("file_type=PNS_problem_v1\n");
            file_str.Append("file_name=");
            file_str.Append(filename);
            file_str.Append("\n\n");

            //Mértékegységek
            file_str.Append("measurement_units:\n");
            file_str.Append("mass_unit=");
            file_str.Append(Default.mass_mu);
            file_str.Append("\n");
            file_str.Append("time_unit=");
            file_str.Append(Default.time_mu);
            file_str.Append("\n");
            file_str.Append("money_unit=");
            file_str.Append(Default.money_mu);
            file_str.Append("\n\n");

            //Alapértelmezett értékek
            file_str.Append("defaults:");
            file_str.Append("\n");
            file_str.Append("material_type=");
            switch (Default.type)
            {
                case Globals.MaterialTypes.Raw:
                    file_str.Append("raw_material");
                    break;
                case Globals.MaterialTypes.Intermediate:
                    file_str.Append("intermediate");
                    break;
                case Globals.MaterialTypes.Product:
                    file_str.Append("product");
                    break;
                default:
                    break;
            }
            file_str.Append("\n");
            file_str.Append("material_flow_rate_lower_bound=");
            file_str.Append(Default.flow_rate_lower_bound);
            file_str.Append("\n");
            file_str.Append("material_flow_rate_upper_bound=");
            file_str.Append(Default.flow_rate_upper_bound);
            file_str.Append("\n");
            file_str.Append("material_price=");
            file_str.Append(Default.price);
            file_str.Append("\n");
            file_str.Append("operating_unit_capacity_lower_bound=");
            file_str.Append(Default.capacity_lower_bound);
            file_str.Append("\n");
            file_str.Append("operating_unit_capacity_upper_bound=");
            file_str.Append(Default.capacity_upper_bound);
            file_str.Append("\n");
            file_str.Append("operating_unit_fix_cost=");
            file_str.Append(Default.fix_cost);
            file_str.Append("\n");
            file_str.Append("operating_unit_proportional_cost=");
            file_str.Append(Default.prop_cost);
            file_str.Append("\n\n");

            // Materialok
            file_str.Append("materials:");
            file_str.Append("\n");
            foreach (Material mat in problem.graph.Materials)
            {
                file_str.Append(mat.Name);
                file_str.Append(": ");
                switch (mat.Type)
                {
                    case Globals.MaterialTypes.Raw:
                        file_str.Append("raw_material");
                        break;
                    case Globals.MaterialTypes.Intermediate:
                        file_str.Append("intermediate");
                        break;
                    case Globals.MaterialTypes.Product:
                        file_str.Append("product");
                        break;
                    default:
                        break;
                }
                file_str.Append(mat.ReqFlowProp.Value != Default.flow_rate_lower_bound && mat.ReqFlowProp.Value != mat.ParameterList["reqflow"].NonValue? ", flow_rate_lower_bound=" + mat.ReqFlowProp.Value : "");
                file_str.Append(mat.MaxFlowProp.Value != Default.flow_rate_upper_bound && mat.MaxFlowProp.Value != mat.ParameterList["maxflow"].NonValue ? ", flow_rate_upper_bound=" + mat.MaxFlowProp.Value : "");
                file_str.Append(mat.PriceProp.Value != Default.price && mat.PriceProp.Value != mat.ParameterList["price"].NonValue ? ", price=" + mat.PriceProp.Value : "");

                file_str.Append("\n");
            }
            file_str.Append("\n");

            //Operating unitok
            file_str.Append("operating_units:");
            file_str.Append("\n");
            foreach (OperatingUnit ou in problem.graph.OperatingUnits)
            {
                file_str.Append(ou.Name);
                file_str.Append(":");
                file_str.Append(ou.CapacityLowerProp.Value != Default.capacity_lower_bound && ou.CapacityLowerProp.Value != ou.ParameterList["caplower"].NonValue ? " capacity_lower_bound=" + ou.CapacityLowerProp.Value + "," : "");
                file_str.Append(ou.CapacityUpperProp.Value != Default.capacity_upper_bound && ou.CapacityUpperProp.Value != ou.ParameterList["capupper"].NonValue ? " capacity_upper_bound=" + ou.CapacityUpperProp.Value + "," : "");
                double fix_cost = ou.InvestmentCostFixProp.Value / (ou.PayoutPeriodProp.Value * ou.WorkingHourProp.Value) + ou.OperatingCostFixProp.Value;
                file_str.Append(fix_cost != Default.fix_cost ? " fix_cost=" + DoubleToGBString(fix_cost) + "," : "");
                double prop_cost = ou.InvestmentCostPropProp.Value / (ou.PayoutPeriodProp.Value * ou.WorkingHourProp.Value) + ou.OperatingCostPropProp.Value;
                file_str.Append(prop_cost != Default.prop_cost ? " proportional_cost=" + DoubleToGBString(prop_cost) + "," : "");
                file_str.Remove(file_str.Length - 1, 1);
                file_str.Append("\n");
            }
            file_str.Append("\n");

            // Élek
            file_str.Append("material_to_operating_unit_flow_rates:");
            file_str.Append("\n");
            foreach (OperatingUnit ou in problem.graph.OperatingUnits)
            {
                Dictionary<Material, double> inputMaterials = GetOpUnitBeginEnd(ou,problem.graph)["input"];
                Dictionary<Material, double> outputMaterials = GetOpUnitBeginEnd(ou, problem.graph)["output"];

                file_str.Append(ou.Name);
                file_str.Append(":");
                foreach (KeyValuePair<Material, double> mat in inputMaterials)
                {
                    file_str.Append(mat.Value != 1 ? " " + DoubleToGBString(mat.Value) : "");
                    file_str.Append(" " + mat.Key.Name);
                    file_str.Append(" +");
                }
                if (inputMaterials.Count != 0)
                {
                    file_str.Remove(file_str.Length - 1, 1);
                }
                else
                {
                    file_str.Append(" ");
                }
                file_str.Append("=>");
                foreach (KeyValuePair<Material, double> mat in outputMaterials)
                {
                    file_str.Append(mat.Value != 1 ? " " + DoubleToGBString(mat.Value) : "");
                    file_str.Append(" " + mat.Key.Name);
                    file_str.Append(" +");
                }
                file_str.Remove(file_str.Length - 2, 2);
                file_str.Append("\n");
            }
            
            // Mutual exclusionok
            if (problem.graph.MutualExclusions.Count != 0)
            {
                file_str.Append("\n");
                file_str.Append("mutually_exlcusive_sets_of_operating_units:");
                file_str.Append("\n");
                foreach (MutualExclusion me in problem.graph.MutualExclusions)
                {
                    file_str.Append(me.Name);
                    file_str.Append(":");
                    foreach (OperatingUnit ou in me.OpUnits)
                    {
                        file_str.Append(" " + ou.Name + ",");
                    }
                    file_str.Remove(file_str.Length - 1, 1);
                    file_str.Append("\n");
                }
            }
            file_str.Append("\n");
            //file_str.Remove(file_str.Length - 1, 1);

            // Fájlba írás
            string path = Path.GetTempPath() + filename + ".in";
            //Console.WriteLine(path);
            StreamWriter outfile = new StreamWriter(path);
            outfile.Write(file_str.ToString());
            outfile.Close();
        }

        // Double értékeknél a tizedesvessző helyett tizedespontot rak
        private static string DoubleToGBString(double value)
        {
            return value.ToString(CultureInfo.GetCultureInfo("en-GB"));
        }

        //Egy Operating Unitnak az input és az output materialjait adja vissza
        public static Dictionary<string, Dictionary<Material, double>> GetOpUnitBeginEnd(OperatingUnit ou, PGraph graph)
        {
            Dictionary<string, Dictionary<Material, double>> beginEnd = new Dictionary<string, Dictionary<Material, double>>();
            Dictionary<Material, double> inputMats = new Dictionary<Material, double>();
            Dictionary<Material, double> outputMats = new Dictionary<Material, double>();
            foreach (Edge edge in graph.Edges)
            {
                if (edge.begin.GetType().Equals(typeof(OperatingUnit)))
                {
                    if (((OperatingUnit)edge.begin).Name.Equals(ou.Name))
                    {
                        outputMats.Add((Material)edge.end, edge.Rate);
                    }
                }

                if (edge.end.GetType().Equals(typeof(OperatingUnit)))
                {
                    if (((OperatingUnit)edge.end).Name.Equals(ou.Name))
                    {
                        inputMats.Add((Material)edge.begin, edge.Rate);
                    }
                }
            }

            beginEnd.Add("input", inputMats);
            beginEnd.Add("output", outputMats);

            return beginEnd;
        }

        public static void ParseSolution(string str, PGraph graph)
        {
            graph.Solutions.Clear();

            int begin = 0, pos = 0, index = 0;

            //---------------Maximal Structure-------------------
            if ((begin = str.IndexOf("Maximal Structure", begin)) != -1)
            {
                Solution sol = new Solution(index++);
                // Title
                pos = str.IndexOf(":", begin);
                sol.Title = str.Substring(begin, pos - begin);
                // Materials
                begin = str.IndexOf("Materials", begin);
                begin = str.IndexOf("\n", begin);
                begin++;
                pos = str.IndexOf("Operating units", begin);
                string[] mats = (str.Substring(begin, pos-begin)).Split(',');
                foreach (string mat in mats)
                {
                    sol.AddMaterial(mat.Trim(), 0);
                }
                // Operating units
                begin = pos;
                begin = str.IndexOf("\n", begin);
                begin++;
                string[] ops;
                if ((pos = str.IndexOf("Solution structure",begin)) != -1)
                {
                    ops = (str.Substring(begin, pos - begin)).Split(',');
                }
                else if ((pos = str.IndexOf("Feasible structure", begin)) != -1)
                {
                    ops = (str.Substring(begin, pos - begin)).Split(',');
                }
                else
                {
                    pos = str.IndexOf("End", begin);
                    ops = (str.Substring(begin, pos - begin)).Split(',');
                }

                foreach (string op in ops)
                {
                    sol.AddOperatingUnit(op.Trim(), 0);
                }
                graph.Solutions.Add(sol);
            }

            //---------------SSG-----MSG---------------------------
            if (str.IndexOf("Solution structure", 0) != -1)
            {
                while ((begin = str.IndexOf("Solution structure", begin)) != -1)
                {
                    Solution sol = new Solution(index++, "Solution structure #" + (index-1).ToString());
                    // Materials
                    begin = str.IndexOf("Materials", begin);
                    begin = str.IndexOf("\n", begin);
                    begin++;
                    pos = str.IndexOf("Operating units", begin);
                    string[] mats = (str.Substring(begin, pos - begin)).Split(',');
                    foreach (string mat in mats)
                    {
                        sol.AddMaterial(mat.Trim(), 0);
                    }
                    // Operating units
                    begin = pos;
                    begin = str.IndexOf("\n", begin);
                    begin++;
                    string[] ops;
                    if ((pos = str.IndexOf("Solution structure", begin)) != -1)
                    {
                        ops = (str.Substring(begin, pos - begin)).Split(',');
                    }
                    else
                    {
                        pos = str.IndexOf("End", begin);
                        ops = (str.Substring(begin, pos - begin)).Split(',');
                    }

                    foreach (string op in ops)
                    {
                        sol.AddOperatingUnit(op.Trim(), 0);
                    }
                    graph.Solutions.Add(sol);
                }
            }
            //------------------------------------------------------

            //-------------------ABB---SSG+LP-----------------------
            if (str.IndexOf("Feasible structure", 0) != -1)
            {
                while ((begin = str.IndexOf("Feasible structure", begin)) != -1)
                {
                    Solution sol = new Solution(index++, "Feasible structure #" + (index - 1).ToString());
                    //Materials
                    begin = str.IndexOf("Materials", begin);
                    begin = str.IndexOf("\n", begin);
                    begin++;
                    pos = str.IndexOf("Operating units", begin);
                    string[] mats = (str.Substring(begin, pos - begin)).Split('\n');
                    foreach (string mat in mats)
                    {
                        string matStr = mat.Trim();
                        if (matStr.Length == 0)
                        {
                            continue;
                        }
                        string name;
                        double cost = 0;
                        double rate;
                        bool costSpecified = false;
                        int matBegin = 0, matPos = 0;
                        if (matStr.IndexOf("(", matBegin) != -1)
                        {
                            costSpecified = true;
                            matPos = matStr.IndexOf(" ", matBegin);
                            name = matStr.Substring(matBegin, matPos - matBegin);
                            matBegin = matStr.IndexOf("(", matBegin);
                            matBegin++;
                            matPos = matStr.IndexOf(" ", matBegin);
                            //Console.WriteLine(matStr.Substring(matBegin, matPos - matBegin));
                            cost = Convert.ToDouble(matStr.Substring(matBegin, matPos - matBegin), CultureInfo.GetCultureInfo("en-GB"));
                            //Console.WriteLine(cost);
                        }
                        else
                        {
                            matPos = matStr.IndexOf(":", matBegin);
                            name = matStr.Substring(matBegin, matPos - matBegin);
                        }
                        //Console.WriteLine(name);
                        matBegin = matStr.IndexOf(":", matBegin);
                        matBegin += 2;
                        matPos = matStr.Length;
                        //Console.WriteLine("Hossz: " + matPos);
                        //Console.WriteLine(matStr.Substring(matBegin, matPos - matBegin));
                        if (matStr.Substring(matBegin, matPos - matBegin).Equals("balanced"))
                        {
                            rate = 0;
                        }
                        else
                        {
                            string rateStr = matStr.Substring(matBegin, matPos - matBegin);
                            int beginRate = 0;
                            int posRate = rateStr.IndexOf(" ", beginRate);
                            rate = Convert.ToDouble(rateStr.Substring(beginRate, posRate - beginRate), CultureInfo.GetCultureInfo("en-GB"));
                            //Console.WriteLine("Rate: " + rate);
                        }
                        //Console.WriteLine(matStr);
                        if (costSpecified)
                        {
                            sol.AddMaterial(name, rate, cost);
                        }
                        else
                        {
                            sol.AddMaterial(name, rate);
                        }
                    }
                    //Operating units
                    begin = pos;
                    begin = str.IndexOf("\n", begin);
                    begin++;
                    pos = str.IndexOf("Total annual cost", begin);
                    string opStr = str.Substring(begin, pos - begin);
                    //Console.WriteLine(opStr);
                    string[] opsLines = opStr.Split('\n');
                    for (int i = 0; i < opsLines.Length; i++)
                    {
                        if (opsLines[i].Length > 0)
                        {
                            string opsLine = opsLines[i].Trim();
                            string checkChar = opsLine.Substring(0, 1);
                            int tmp;
                            //Console.WriteLine(opsLine);
                            if (int.TryParse(checkChar, out tmp) == true)
                            {
                                string name;
                                double size;
                                double cost;
                                List<MaterialProperty> inputMats = new List<MaterialProperty>();
                                List<MaterialProperty> outputMats = new List<MaterialProperty>();

                                for (int j = i+1; j < opsLines.Length; j++)
                                {
                                    if (opsLines[j].Length > 0)
                                    {
                                        checkChar = opsLines[j].Trim().Substring(0, 1);
                                        //Console.WriteLine(checkChar);
                                        if (int.TryParse(checkChar, out tmp) == false)
                                        {
                                            opsLine = opsLine + " " + opsLines[j].Trim();
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                                int opsBegin = 0, opsPos;
                                opsPos = opsLine.IndexOf("*", opsBegin);
                                size = Convert.ToDouble(opsLine.Substring(opsBegin, opsPos - opsBegin), CultureInfo.GetCultureInfo("en-GB"));
                                //Console.WriteLine(size);
                                opsBegin = opsPos + 1;
                                opsPos = opsLine.IndexOf(" ", opsBegin);
                                name = opsLine.Substring(opsBegin, opsPos - opsBegin);
                                //Console.WriteLine(name);
                                opsBegin = opsLine.IndexOf("(", opsPos);
                                opsBegin++;
                                opsPos = opsLine.IndexOf(" ", opsBegin);
                                cost = Convert.ToDouble(opsLine.Substring(opsBegin, opsPos - opsBegin), CultureInfo.GetCultureInfo("en-GB"));
                                //Console.WriteLine(cost);
                                opsBegin = opsLine.IndexOf(":", opsPos);
                                opsPos = opsLine.IndexOf("=>", opsBegin);
                                string opsInMats = opsLine.Substring(opsBegin, opsPos - opsBegin).Trim();
                                //Console.WriteLine(opsInMats);
                                if (opsInMats.Length > 1)
                                {
                                    int matBegin;
                                    int matPos = 0;
                                    int count = opsInMats.Split('(').Length - 1;
                                    for (int j = 0; j < count; j++)
                                    {
                                        matBegin = matPos + 2;
                                        matPos = opsInMats.IndexOf("(", matBegin) - 1;
                                        string matName = opsInMats.Substring(matBegin, matPos - matBegin);
                                        matBegin = matPos + 2;
                                        matPos = opsInMats.IndexOf(" ", matBegin);
                                        //Console.WriteLine(opsInMats.Substring(matBegin, matPos - matBegin));
                                        double matCost = Convert.ToDouble(opsInMats.Substring(matBegin, matPos - matBegin), CultureInfo.GetCultureInfo("en-GB"));
                                        //Console.WriteLine(matCost);
                                        matPos = opsInMats.IndexOf(")", matPos);
                                        inputMats.Add(new MaterialProperty(matName, matCost, 0));
                                    }
                                }
                                //Console.WriteLine(opsLine);
                                opsBegin = opsPos;
                                opsPos = opsLine.Length;
                                //Console.WriteLine("opsBegin: " + opsBegin + ", opsPos: " + opsPos);
                                string opsOutMats = opsLine.Substring(opsBegin, opsPos - opsBegin).Trim();
                                //Console.WriteLine(opsOutMats);
                                if (opsOutMats.Length > 2)
                                {
                                    int matBegin;
                                    int matPos = 0;
                                    int count = opsOutMats.Split('(').Length - 1;
                                    //Console.WriteLine(count);
                                    for (int j = 0; j < count; j++)
                                    {
                                        matBegin = matPos + 2;
                                        matPos = opsOutMats.IndexOf("(", matBegin) - 1;
                                        string matName = opsOutMats.Substring(matBegin, matPos - matBegin).Trim();
                                        matBegin = matPos + 2;
                                        matPos = opsOutMats.IndexOf(" ", matBegin);
                                        //Console.WriteLine(matName);
                                        double matCost = Convert.ToDouble(opsOutMats.Substring(matBegin, matPos - matBegin), CultureInfo.GetCultureInfo("en-GB"));
                                        //Console.WriteLine(matCost);
                                        matPos = opsOutMats.IndexOf(")", matPos);
                                        outputMats.Add(new MaterialProperty(matName, matCost, 0));
                                    }
                                }
                                sol.AddOperatingUnit(name, size, cost, inputMats, outputMats);
                            }
                        }
                        
                    }
                    // Total annual cost
                    begin = pos;
                    begin = str.IndexOf("= ", begin);
                    begin = begin + 2;
                    pos = str.IndexOf(" ", begin);
                    double totalCost = Convert.ToDouble(str.Substring(begin, pos - begin), CultureInfo.GetCultureInfo("en-GB"));
                    sol.OptimalValue = totalCost;

                    graph.Solutions.Add(sol);
                }
            }
            
        }
    }
}
