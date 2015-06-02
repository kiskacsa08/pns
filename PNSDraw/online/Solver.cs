using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

using MongoDB.Bson;

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

        public Problem Run()
        {
            try
            {
                socket.Connect("193.6.33.141", 51000);
            }
            catch (Exception e)
            {
                throw e;
            }

            BsonDocument document = socket.ToBson("{\"HEAD\":{\"status\":\"LOGIN\"},\"BODY\":{\"data\":{\"request\":{\"query\":\"LOGIN\",\"params\":{\"password\":\"Kepler-37b\",\"username\":\"Jaffa\"}}}}}");

            string resp = socket.Send(document.ToString());

            if (resp.Contains("LOGGEDIN"))
            {
                resp = Work();
            }

            BsonDocument response = socket.ToBson(resp);

            if (response["HEAD"]["status"].Equals("DONE"))
            {
                ParseSolution(response["BODY"]["data"]["response"]["body"]["mongoID"].ToString());
            }

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
                item["flow_rate_lower_bound"] = mat.ParameterList["reqflow"].Value != -1 ? mat.ParameterList["reqflow"].Value : Default.flow_rate_lower_bound;
                item["flow_rate_upper_bound"] = mat.ParameterList["maxflow"].Value != -1 ? mat.ParameterList["maxflow"].Value : Default.flow_rate_upper_bound;
                item["price"] = mat.ParameterList["price"].Value != -1 ? mat.ParameterList["price"].Value : Default.price;

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
                item["capacity_lower_bound"] = op.ParameterList["caplower"].Value != -1 ? op.ParameterList["caplower"].Value : Default.capacity_lower_bound;
                item["capacity_upper_bound"] = op.ParameterList["capupper"].Value != -1 ? op.ParameterList["capupper"].Value : Default.capacity_upper_bound;
                double fix_cost = op.ParameterList["investcostfix"].Value + op.ParameterList["opercostfix"].Value;
                item["fix_cost"] = fix_cost != -1 ? fix_cost : Default.fix_cost;
                double prop_cost = op.ParameterList["investcostprop"].Value + op.ParameterList["opercostprop"].Value;
                item["proportional_cost"] = prop_cost != -1 ? prop_cost : Default.prop_cost;

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
            mu["mass_unit"] = MUs.GetBaseQuantity("mass");
            mu["time_unit"] = MUs.GetBaseQuantity("time");
            mu["money_unit"] = MUs.GetBaseQuantity("currency");

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
            List<Entities.Solution> Solutions = problem.graph.Solutions;
            Solutions.Clear();
            BsonArray sol = new BsonArray();

            if (exist.Count == 0)
            {
                return;
            }
            else
            {
                sol = exist[0]["solves"].AsBsonArray;
            }

            for(int i=0;i<sol.Count;i++){
                string title = "";
                if (sol[i]["max"].AsBoolean)
                {
                    title = "Maximal structure";
                }
                else if (problem.algorithm == "SSG")
                {
                    title = "Solution structure #" + i.ToString();
                }
                else if(problem.algorithm == "INSIDE_OUT")
                {
                    title = "Feasible structure #" + i.ToString();
                }
                else
                {
                    title = "Unknown structure #" + i.ToString();
                }

                Entities.Solution solution = new Entities.Solution(i, title);

                Console.WriteLine(i + ". solution: " + sol[i].ToString());

                Solutions.Add(solution);
            }

            //Console.WriteLine(problem.graph.SolutionCount);
        }
    }
}
