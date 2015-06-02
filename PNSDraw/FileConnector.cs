using PNSDraw.Entities;
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
                file_str.Append(mat.ReqFlowProp.Value != Default.flow_rate_lower_bound && mat.ReqFlowProp.Value != -1? ", flow_rate_lower_bound=" + mat.ReqFlowProp.Value : "");
                file_str.Append(mat.MaxFlowProp.Value != Default.flow_rate_upper_bound && mat.MaxFlowProp.Value != -1 ? ", flow_rate_upper_bound=" + mat.MaxFlowProp.Value : "");
                file_str.Append(mat.PriceProp.Value != Default.price && mat.PriceProp.Value != -1 ? ", price=" + mat.PriceProp.Value : "");

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
                file_str.Append(ou.CapacityLowerProp.Value != Default.capacity_lower_bound ? " capacity_lower_bound=" + ou.CapacityLowerProp.Value + "," : "");
                file_str.Append(ou.CapacityUpperProp.Value != Default.capacity_upper_bound ? " capacity_upper_bound=" + ou.CapacityUpperProp.Value + "," : "");
                double fix_cost = ou.InvestmentCostFixProp.Value + ou.OperatingCostFixProp.Value;
                file_str.Append(fix_cost != Default.fix_cost ? " fix_cost=" + fix_cost + "," : "");
                double prop_cost = ou.InvestmentCostPropProp.Value + ou.OperatingCostPropProp.Value;
                file_str.Append(prop_cost != Default.prop_cost ? " proportional_cost=" + prop_cost + "," : "");
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
                file_str.Append("mutually_exclusive_sets_of_operating_units:");
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
            Console.WriteLine(path);
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
        private static Dictionary<string, Dictionary<Material, double>> GetOpUnitBeginEnd(OperatingUnit ou, PGraph graph)
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
    }
}
