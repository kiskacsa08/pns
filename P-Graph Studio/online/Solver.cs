﻿/* Copyright 2015 Department of Computer Science and Systems Technology, University of Pannonia

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
using System.Runtime.Serialization;

using MongoDB.Bson;
using System.ComponentModel;
using System.Windows.Forms;

using PNSDraw.Configuration;

namespace PNSDraw.online
{
    class Solver
    {
        Problem problem;
        string problemID;

        MongoHandler mh;
        SolverSocket socket;

        public Solver(Problem problem)
        {
            this.problem = problem;
            mh = new MongoHandler();
            socket = new SolverSocket();
        }

        public void Stop()
        {
            if (socket.IsConnected())
            {
                BsonDocument document = socket.ToBson("{\"HEAD\":{\"status\":\"REQUEST\"},\"BODY\":{\"data\":{\"request\":{\"query\":\"STOP\",\"params\":{\"mongoID\":\"" + problemID + "\"}}}}}");
                socket.Send(document.ToString(), false);
            }
            else
            {
                MessageBox.Show("Online solver not connected!");
            }
        }

        public Problem Run(BackgroundWorker worker)
        {
            try
            {
                socket.Connect(Config.Instance.SolverSettings.OnlineSolverHost, Config.Instance.SolverSettings.OnlineSolverPort);
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to establish connection to online solver, please use the offline solver!", "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return problem;
            }

            worker.ReportProgress(10);

            BsonDocument document = socket.ToBson("{\"HEAD\":{\"status\":\"LOGIN\"},\"BODY\":{\"data\":{\"request\":{\"query\":\"LOGIN\",\"params\":{\"password\":\"Kepler-37b\",\"username\":\"Jaffa\"}}}}}");

            string resp = socket.Send(document.ToString());

            worker.ReportProgress(30);

            if (resp.Contains("LOGGEDIN"))
            {
                resp = Work(worker);
            }
            BsonDocument response = socket.ToBson(resp);

            worker.ReportProgress(80);
            try
            {
                if (response["HEAD"]["status"].Equals("DONE"))
                {
                    ParseSolution(response["BODY"]["data"]["response"]["body"]["mongoID"].ToString());
                }
                else if (response["HEAD"]["status"].Equals("ERROR"))
                {
                    MessageBox.Show(response["HEAD"]["statusNote"].AsString, "ERROR",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (response["HEAD"]["status"].Equals("STOPPED"))
                {
                    MessageBox.Show("Online solver stopped!");
                }

                worker.ReportProgress(100);
            }
            catch (KeyNotFoundException ex)
            {
                MessageBox.Show("Something went wrong!\nThe solver connection is broken!", "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return problem;
        }

        private string Work(BackgroundWorker worker)
        {
            problemID = SaveGraph();

            BsonDocument document = socket.ToBson("{\"HEAD\":{\"status\":\"REQUEST\"},\"BODY\":{\"data\":{\"request\":{\"query\":\"RUN\",\"params\":{\"mongoID\":\""+problemID+"\"}}}}}");

            string resp = socket.Send(document.ToJson());

            if (resp.Contains("SUCCESS"))
            {
                worker.ReportProgress(40);
                resp = socket.Done();
            }

            socket.Close();

            return resp;
        }

        private string SaveGraph()
        {
            List<Material> materials = problem.graph.Materials;
            List<OperatingUnit> operatings = problem.graph.OperatingUnits;
            List<Edge> connections = problem.graph.Edges;
            List<MutualExclusion> mutualExclusions = problem.graph.MutualExclusions;

            BsonArray materialArray = new BsonArray();
            List<BsonDocument> raw = new List<BsonDocument>();
            List<BsonDocument> product = new List<BsonDocument>();
            List<BsonDocument> intermediate = new List<BsonDocument>();

            //type, name, Parameters["reqflow"], Parameters["maxflow"], Parameters["price"]

            foreach (Material mat in materials)
            {
                BsonDocument item = new BsonDocument();

                item["name"] = mat.Name;
                item["type"] = GetTypeByInt(mat.Type);
                
                double flow_rate_lower_bound = mat.ParameterList["reqflow"].Value != -1 ? mat.ParameterList["reqflow"].Value : Config.Instance.Material.FlowRateLowerBound;
                double flow_rate_upper_bound = mat.ParameterList["maxflow"].Value != -1 ? mat.ParameterList["maxflow"].Value : Config.Instance.Material.FlowRateUpperBound;
                double price = mat.ParameterList["price"].Value != -1 ? mat.ParameterList["price"].Value : Config.Instance.Material.Price;

                item["flow_rate_lower_bound"] = MUs.ConvertToUnifiedUnit(mat.ParameterList["reqflow"].MU, flow_rate_lower_bound);
                item["flow_rate_upper_bound"] = MUs.ConvertToUnifiedUnit(mat.ParameterList["maxflow"].MU, flow_rate_upper_bound);
                item["price"] = MUs.ConvertToUnifiedUnit(mat.ParameterList["price"].MU, price);

                materialArray.Add(item);
            }

            BsonArray operatingArray = new BsonArray();
            BsonArray connectionArray = new BsonArray();

            //name, Parameters["caplower"], Parameters["capupper"], Parameters["investcostfix"]+Parameters["opercostfix"],
            // Parameters["investcostprop"], Parameters["opercostprop"]

            foreach (OperatingUnit op in operatings)
            {
                BsonDocument item = new BsonDocument();

                item["name"] = op.Name;

                double capacity_lower_bound = op.ParameterList["caplower"].Value != -1 ? op.ParameterList["caplower"].Value : Config.Instance.OperatingUnit.CapacityLowerBound;
                double capacity_upper_bound = op.ParameterList["capupper"].Value != -1 ? op.ParameterList["capupper"].Value : Config.Instance.OperatingUnit.CapacityUpperBound;

                double working_hour = MUs.ConvertToUnifiedUnit(op.WorkingHourProp.MU, op.WorkingHourProp.Value);
                double payout_period = MUs.ConvertToUnifiedUnit(op.PayoutPeriodProp.MU, op.PayoutPeriodProp.Value);

                double invest_fix_cost = MUs.ConvertToUnifiedUnit(op.ParameterList["investcostfix"].MU, op.ParameterList["investcostfix"].Value);
                double oper_fix_cost = MUs.ConvertToUnifiedUnit(op.ParameterList["opercostfix"].MU, op.ParameterList["opercostfix"].Value);
                double fix_cost;
                
                try
                {
                    fix_cost = invest_fix_cost / (working_hour * payout_period) + oper_fix_cost;
                }
                catch (Exception e)
                {
                    fix_cost = 0;
                }

                double invest_prop_cost = MUs.ConvertToUnifiedUnit(op.ParameterList["investcostprop"].MU, op.ParameterList["investcostprop"].Value);
                double oper_prop_cost = MUs.ConvertToUnifiedUnit(op.ParameterList["opercostprop"].MU, op.ParameterList["opercostprop"].Value);

                double prop_cost;

                try
                {
                    prop_cost = invest_prop_cost / (working_hour * payout_period) + oper_prop_cost;
                }
                catch (Exception e)
                {
                    prop_cost = 0;
                }

                item["capacity_lower_bound"] = MUs.ConvertToUnifiedUnit(op.ParameterList["caplower"].MU, capacity_lower_bound);
                item["capacity_upper_bound"] = MUs.ConvertToUnifiedUnit(op.ParameterList["capupper"].MU, capacity_upper_bound);

                item["fix_cost"] = fix_cost >= 0 ? fix_cost : Config.Instance.OperatingUnit.OpUnitFixCost;

                item["proportional_cost"] = prop_cost >= 0 ? prop_cost : Config.Instance.OperatingUnit.OpUnitPropCost;

                operatingArray.Add(item);

                BsonDocument conn = new BsonDocument(new BsonElement("name", op.Name));
                conn["inputs"] = new BsonArray();
                conn["outputs"] = new BsonArray();

                connectionArray.Add(conn);
            }

            foreach (Edge conn in connections)
            {
                BsonDocument item = new BsonDocument();

                OperatingUnit op;
                Material mat;
                bool inputIsMaterial = false;

                if (conn.begin.GetType() == typeof(PNSDraw.OperatingUnit))
                {
                    op = (OperatingUnit)conn.begin;
                    mat = (Material)conn.end;
                }
                else
                {
                    op = (OperatingUnit)conn.end;
                    mat = (Material)conn.begin;
                    inputIsMaterial = true;
                }

                item["name"] = op.Name;
                item = GetElemByName(connectionArray, item);

                BsonDocument elem = new BsonDocument();
                elem["rate"] = conn.Rate;
                elem["name"] = mat.Name;

                if (inputIsMaterial)
                {
                    item["inputs"].AsBsonArray.Add(elem);
                }
                else
                {
                    item["outputs"].AsBsonArray.Add(elem);
                }
            }

            BsonArray mutually = new BsonArray();

            foreach (MutualExclusion me in mutualExclusions)
            {
                BsonDocument elem = new BsonDocument();
                elem["name"] = me.Name;
                BsonArray set = new BsonArray();
                foreach (OperatingUnit op in me.OpUnits)
                {
                    set.Add(op.Name);
                }
                elem["operating_units"] = set;

                mutually.Add(elem);
            }


            BsonDocument convertedGraph = new BsonDocument();
            
            convertedGraph["type"] = "PNS_problem_v1";

            DateTime dt1970 = new DateTime(1970, 1, 1);
            DateTime current = DateTime.Now;
            TimeSpan span = (current - dt1970);
            convertedGraph["time"] = (long)span.TotalMilliseconds / 1000;
            convertedGraph["name"] = "Graph_" + convertedGraph["time"];
            convertedGraph["hash_to_pns_draw"] = problem.graph.GetHashCode();

            BsonDocument mu = new BsonDocument();
            mu["mass_unit"] = Config.Instance.Quantity.mass_mu.ToString();//MUs.GetBaseQuantity("mass");
            mu["time_unit"] = Config.Instance.Quantity.time_mu.ToString();//MUs.GetBaseQuantity("time");
            mu["money_unit"] = Config.Instance.Quantity.money_mu.ToString();//MUs.GetBaseQuantity("currency");

            convertedGraph["measurement_units"] = mu;

            BsonDocument defaults = new BsonDocument();

            BsonDocument defmat = new BsonDocument();
            defmat["type"] = GetTypeByInt(Config.Instance.Material.Type);
            defmat["flow_rate_lower_bound"] = Config.Instance.Material.FlowRateLowerBound;
            defmat["flow_rate_upper_bound"] = Config.Instance.Material.FlowRateUpperBound;
            defmat["price"] = Config.Instance.Material.Price;

            BsonDocument defop = new BsonDocument();
            defop["capacity_lower_bound"] = Config.Instance.OperatingUnit.CapacityLowerBound;
            defop["capacity_upper_bound"] = Config.Instance.OperatingUnit.CapacityUpperBound;
            defop["fix_cost"] = Config.Instance.OperatingUnit.OpUnitFixCost;
            defop["proportional_cost"] = Config.Instance.OperatingUnit.OpUnitPropCost;

            defaults["materials"] = defmat;
            defaults["operating_units"] = defop;

            convertedGraph["defaults"] = defaults;

            convertedGraph["materials"] = materialArray;
            convertedGraph["operating_units"] = operatingArray;
            convertedGraph["material_to_operating_unit_flow_rates"] = connectionArray;
            convertedGraph["mutually_exlcusive_sets_of_operating_units"] = mutually;

            Dictionary<string, BsonValue> query = new Dictionary<string, BsonValue>();
            query.Add("hash_to_pns_draw", convertedGraph["hash_to_pns_draw"]);
            List<BsonDocument> exist = mh.Find("convertedProblems", query, 1);

            string convertedGraphID = "";

            if (exist.Count == 0)
            {
                convertedGraphID = mh.Insert("convertedProblems", convertedGraph);
            }
            else
            {
                convertedGraphID = exist[0]["_id"].ToString();
                convertedGraph["name"] = exist[0]["name"];
            }

            query = new Dictionary<string, BsonValue>();
            query.Add("converted", new ObjectId(convertedGraphID));

            exist = mh.Find("graphs", query, 1);

            if (exist.Count > 0)
            {
                return SaveProblem(exist[0]["_id"].ToString(), convertedGraph["name"].ToString());
            }
            
            BsonDocument graph = new BsonDocument();

            graph["source"] = "draw";
            graph["name"] = convertedGraph["name"];
            graph["converted"] = new ObjectId(convertedGraphID);

            string graphID = mh.Insert("graphs", graph);

            return SaveProblem(graphID, convertedGraph["name"].ToString());
        }

        private string SaveProblem(string graphID, string name)
        {
            Dictionary<string, BsonValue> query = new Dictionary<string, BsonValue>();
            query.Add("name", name);
            query.Add("gID", new ObjectId(graphID));
            query.Add("algorithm", problem.algorithm);
            query.Add("thread", problem.threadNum);
            query.Add("limit", problem.limit);

            List<BsonDocument> exist = mh.Find("problems", query, 1);

            if (exist.Count > 0)
            {
                return exist[0]["_id"].ToString();
            }

            BsonDocument problemBson = new BsonDocument();

            problemBson["gID"] = new ObjectId(graphID);
            problemBson["name"] = name;
            problemBson["algorithm"] = problem.algorithm;
            problemBson["limit"] = problem.limit;
            problemBson["thread"] = problem.threadNum;
            problemBson["status"] = "pending";

            DateTime dt1970 = new DateTime(1970, 1, 1);
            DateTime current = DateTime.Now;
            TimeSpan span = (current - dt1970);
            problemBson["timestampReq"] = (long)span.TotalMilliseconds / 1000;

            return mh.Insert("problems", problemBson);
        }

        private string GetTypeByInt(int type)
        {
            if (type.Equals(MaterialTypes.Raw))
            {
                return "raw_material";
            }
            else if (type.Equals(MaterialTypes.Intermediate))
            {
                return "intermediate";
            }
            else if (type.Equals(MaterialTypes.Product))
            {
                return "product";
            }
            else
            {
                return "raw_material";
            }
        }

        private BsonDocument GetElemByName(BsonArray array, BsonDocument query)
        {
            BsonValue[] values = array.ToArray();
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i]["name"] == query["name"])
                {
                    return new BsonDocument(values[i].AsBsonDocument);
                }
            }
            return null;
        }

        private void ParseSolution(string mongoID)
        {
            Dictionary<string,BsonValue> query = new Dictionary<string,BsonValue>();
            query.Add("_id", new ObjectId(mongoID));

            System.Threading.Thread.Sleep(problem.limit/100);

            List<BsonDocument> exist = mh.Find("solutions", query, 1);
            List<Solution> Solutions = problem.graph.Solutions;
            Solutions.Clear();
            BsonArray solBsonArray = new BsonArray();

            if (exist.Count == 0)
            {
                return;
            }
            else
            {
                solBsonArray = exist[0]["solves"].AsBsonArray;
            }

            for(int i=0;i<solBsonArray.Count;i++){                
                Solution solution = new Solution(i);

                if (solBsonArray[i]["max"].AsBoolean)
                {
                    solution.Title = "Maximal structure";
                    ParseMSGSolution(solBsonArray[i], solution);
                }
                else if (problem.algorithm == "SSG")
                {
                    solution.Title = "Solution structure #" + i.ToString();
                    ParseSSGSolution(solBsonArray[i], solution);
                }
                else if(problem.algorithm == "INSIDE_OUT")
                {
                    solution.Title = "Feasible structure #" + i.ToString();
                    ParseInsideOutSolution(solBsonArray[i], solution);
                }
                else
                {
                    solution.Title = "Unknown structure #" + i.ToString();
                }

                Solutions.Add(solution);
            }

            //Console.WriteLine(problem.graph.SolutionCount);
        }

        private void ParseMSGSolution(BsonValue solBson, Solution solution)
        {
            solution.AlgorithmUsed = "MSG";
            solution.OptimalValue = solBson["result"]["value"].AsDouble;

            BsonArray materials = solBson["materials"].AsBsonArray;
            BsonArray opunits = solBson["operating_units"].AsBsonArray;

            foreach (string d in materials)
            {
                solution.AddMaterial(d, 0, 0);
            }

            foreach (string d in opunits)
            {
                solution.AddOperatingUnit(d, 0, 0, new List<MaterialProperty>(), new List<MaterialProperty>());
            }
        }

        private void ParseSSGSolution(BsonValue solBson, Solution solution)
        {
            solution.AlgorithmUsed = "SSG";
            BsonArray materials = solBson["materials"].AsBsonArray;
            BsonArray opunits = solBson["operating_units"].AsBsonArray;

            foreach (string d in materials)
            {
                solution.AddMaterial(d, 0, 0);
            }

            foreach (string d in opunits)
            {
                solution.AddOperatingUnit(d, 0, 0, new List<MaterialProperty>(), new List<MaterialProperty>());
            }
        }

        private void ParseInsideOutSolution(BsonValue solBson, Solution solution)
        {
            solution.AlgorithmUsed = "ABB";
            solution.OptimalValue = Math.Round(MUs.ConvertToSpecialUnit(Config.Instance.Quantity.money_mu.ToString(), solBson["result"]["value"].AsDouble), 2);
            BsonArray materials = solBson["complete"]["materials"].AsBsonArray;
            BsonArray opunits = solBson["complete"]["operatings"].AsBsonArray;

            foreach (BsonDocument d in materials)
            {
                if (d["balanced"].AsBoolean)
                {
                    solution.AddMaterial(d["name"].AsString, 0, 0);
                }
                else
                {
                    double flow = MUs.ConvertToSpecialUnit(Config.Instance.Quantity.mass_mu.ToString(), d["flow"].AsDouble);
                    double cost = MUs.ConvertToSpecialUnit(Config.Instance.Quantity.money_mu.ToString(), d["cost"].AsDouble);

                    solution.AddMaterial(d["name"].AsString, Math.Round(flow, 4), Math.Round(cost, 2));
                }
            }

            foreach (BsonDocument d in opunits)
            {
                BsonArray consumed = d["consumed"].AsBsonArray;
                BsonArray produced = d["produced"].AsBsonArray;
                List<MaterialProperty> input = new List<MaterialProperty>();
                List<MaterialProperty> output = new List<MaterialProperty>();

                foreach (BsonDocument inDoc in consumed)
                {
                    double flow = MUs.ConvertToSpecialUnit(Config.Instance.Quantity.mass_mu.ToString(), inDoc["flow"].AsDouble);
                    MaterialProperty prop = new MaterialProperty(inDoc["name"].AsString, Math.Round(flow, 4), 0);
                    input.Add(prop);
                }

                foreach (BsonDocument outDoc in produced)
                {
                    double flow = MUs.ConvertToSpecialUnit(Config.Instance.Quantity.mass_mu.ToString(), outDoc["flow"].AsDouble);
                    MaterialProperty prop = new MaterialProperty(outDoc["name"].AsString, Math.Round(flow, 4), 0);
                    output.Add(prop);
                }

                double capacity = MUs.ConvertToSpecialUnit(Config.Instance.Quantity.mass_mu.ToString(), d["capacity"].AsDouble);
                double cost = MUs.ConvertToSpecialUnit(Config.Instance.Quantity.money_mu.ToString(), d["cost"].AsDouble);

                solution.AddOperatingUnit(d["name"].AsString, Math.Round(capacity, 4), Math.Round(d["cost"].AsDouble, 2), input, output);
            }
        }
    }
}
