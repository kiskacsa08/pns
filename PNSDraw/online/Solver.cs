using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

using MongoDB.Bson;
using System.ComponentModel;

namespace PNSDraw.online
{
    class Solver
    {
        Problem problem;

        MongoHandler mh;
        SolverSocket socket;

        public Solver(Problem problem)
        {
            this.problem = problem;
            mh = new MongoHandler();
            socket = new SolverSocket();
        }

        public Problem Run(BackgroundWorker worker)
        {
            try
            {
                socket.Connect("193.6.33.141", 51000);
            }
            catch (Exception e)
            {
                throw e;
            }

            worker.ReportProgress(10);

            BsonDocument document = socket.ToBson("{\"HEAD\":{\"status\":\"LOGIN\"},\"BODY\":{\"data\":{\"request\":{\"query\":\"LOGIN\",\"params\":{\"password\":\"Kepler-37b\",\"username\":\"Jaffa\"}}}}}");

            string resp = socket.Send(document.ToString());

            worker.ReportProgress(30);

            if (resp.Contains("LOGGEDIN"))
            {
                resp = Work();
            }

            BsonDocument response = socket.ToBson(resp);

            worker.ReportProgress(70);

            if (response["HEAD"]["status"].Equals("DONE"))
            {
                ParseSolution(response["BODY"]["data"]["response"]["body"]["mongoID"].ToString());
            }

            worker.ReportProgress(100);

            return problem;
        }

        private string Work()
        {
            string problemID = SaveGraph();

            BsonDocument document = socket.ToBson("{\"HEAD\":{\"status\":\"REQUEST\"},\"BODY\":{\"data\":{\"request\":{\"query\":\"RUN\",\"params\":{\"mongoID\":\""+problemID+"\"}}}}}");

            string resp = socket.Send(document.ToJson());

            Console.WriteLine(resp);

            if (resp.Contains("SUCCESS"))
            {
                resp = socket.Done();
                Console.WriteLine(resp);
            }

            socket.Close();

            return resp;
        }

        private string SaveGraph()
        {
            List<Material> materials = problem.graph.Materials;
            List<OperatingUnit> operatings = problem.graph.OperatingUnits;
            List<Edge> connections = problem.graph.Edges;

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
                
                double flow_rate_lower_bound = mat.ParameterList["reqflow"].Value != -1 ? mat.ParameterList["reqflow"].Value : Default.flow_rate_lower_bound;
                double flow_rate_upper_bound = mat.ParameterList["maxflow"].Value != -1 ? mat.ParameterList["maxflow"].Value : Default.flow_rate_upper_bound;
                double price = mat.ParameterList["price"].Value != -1 ? mat.ParameterList["price"].Value : Default.price;

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

                double capacity_lower_bound = op.ParameterList["caplower"].Value != -1 ? op.ParameterList["caplower"].Value : Default.capacity_lower_bound;
                double capacity_upper_bound = op.ParameterList["capupper"].Value != -1 ? op.ParameterList["capupper"].Value : Default.capacity_upper_bound;

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

                item["fix_cost"] = fix_cost >= 0 ? fix_cost : Default.fix_cost;
                
                item["proportional_cost"] = prop_cost >= 0 ? prop_cost : Default.prop_cost;

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

            BsonDocument convertedGraph = new BsonDocument();
            
            convertedGraph["type"] = "PNS_problem_v1";

            DateTime dt1970 = new DateTime(1970, 1, 1);
            DateTime current = DateTime.Now;
            TimeSpan span = (current - dt1970);
            convertedGraph["time"] = (long)span.TotalMilliseconds / 1000;
            convertedGraph["name"] = "Graph_" + convertedGraph["time"];
            convertedGraph["hash_to_pns_draw"] = problem.graph.GetHashCode();

            BsonDocument mu = new BsonDocument();
            mu["mass_unit"] = Default.mass_mu.ToString();//MUs.GetBaseQuantity("mass");
            mu["time_unit"] = Default.time_mu.ToString();//MUs.GetBaseQuantity("time");
            mu["money_unit"] = Default.money_mu.ToString();//MUs.GetBaseQuantity("currency");

            convertedGraph["measurement_units"] = mu;

            BsonDocument defaults = new BsonDocument();

            BsonDocument defmat = new BsonDocument();
            defmat["type"] = GetTypeByInt(Default.type);
            defmat["flow_rate_lower_bound"] = Default.flow_rate_lower_bound;
            defmat["flow_rate_upper_bound"] = Default.flow_rate_upper_bound;
            defmat["price"] = Default.price;

            BsonDocument defop = new BsonDocument();
            defop["capacity_lower_bound"] = Default.capacity_lower_bound;
            defop["capacity_upper_bound"] = Default.capacity_upper_bound;
            defop["fix_cost"] = Default.fix_cost;
            defop["proportional_cost"] = Default.prop_cost;

            defaults["materials"] = defmat;
            defaults["operating_units"] = defop;

            convertedGraph["defaults"] = defaults;

            convertedGraph["materials"] = materialArray;
            convertedGraph["operating_units"] = operatingArray;
            convertedGraph["material_to_operating_unit_flow_rates"] = connectionArray;
            convertedGraph["mutually_exlcusive_sets_of_operating_units"] = new BsonArray();

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
            if (type.Equals(Globals.MaterialTypes.Raw))
            {
                return "raw_material";
            }
            else if (type.Equals(Globals.MaterialTypes.Intermediate))
            {
                return "intermediate";
            }
            else if (type.Equals(Globals.MaterialTypes.Product))
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
            //megoldást bele kell parzolni a problem.graphba
            Dictionary<string,BsonValue> query = new Dictionary<string,BsonValue>();
            query.Add("_id", new ObjectId(mongoID));

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
            solution.OptimalValue = Math.Round(MUs.ConvertToSpecialUnit(Default.money_mu.ToString(), solBson["result"]["value"].AsDouble), 2);
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
                    double flow = MUs.ConvertToSpecialUnit(Default.mass_mu.ToString(), d["flow"].AsDouble);
                    double cost = MUs.ConvertToSpecialUnit(Default.money_mu.ToString(), d["cost"].AsDouble);

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
                    double flow = MUs.ConvertToSpecialUnit(Default.mass_mu.ToString(), inDoc["flow"].AsDouble);
                    MaterialProperty prop = new MaterialProperty(inDoc["name"].AsString, Math.Round(flow, 4), 0);
                    input.Add(prop);
                }

                foreach (BsonDocument outDoc in produced)
                {
                    double flow = MUs.ConvertToSpecialUnit(Default.mass_mu.ToString(), outDoc["flow"].AsDouble);
                    MaterialProperty prop = new MaterialProperty(outDoc["name"].AsString, Math.Round(flow, 4), 0);
                    output.Add(prop);
                }

                double capacity = MUs.ConvertToSpecialUnit(Default.mass_mu.ToString(), d["capacity"].AsDouble);
                double cost = MUs.ConvertToSpecialUnit(Default.money_mu.ToString(), d["cost"].AsDouble);

                solution.AddOperatingUnit(d["name"].AsString, Math.Round(capacity, 4), Math.Round(d["cost"].AsDouble, 2), input, output);
            }
        }
    }
}
