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

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

using PNSDraw.Configuration;

namespace PNSDraw.online
{
    class MongoHandler
    {
        private MongoServer server;
        private MongoDatabase db;

        public MongoHandler()
        {
            Connect(Config.Instance.Mongo.Host, Config.Instance.Mongo.Port, Config.Instance.Mongo.Database);
        }

        public void Connect(string host, int port, string database)
        {
            if (server != null)
            {
                server.Disconnect();
            }

            string connectionString = "mongodb://" + host + ":" + port.ToString() + "/" + database;
            var mongoClient = new MongoClient(connectionString);
            server = mongoClient.GetServer();
            db = server.GetDatabase(database);
        }

        public void Connect(string host, int port, string database, string user, string pass)
        {
            if (server != null)
            {
                server.Disconnect();
            }

            var credential = MongoCredential.CreateMongoCRCredential(database, user, pass);

            var settings = new MongoServerSettings
            {
                Server = new MongoServerAddress(host, port),
                Credentials = new[] { credential }
            };

            server = new MongoServer(settings);

            db = server.GetDatabase(database);
        }

        public void Disconnect()
        {
            server.Disconnect();
        }

        public List<BsonDocument> FindAll(string collection)
        {
            var coll = db.GetCollection<BsonDocument>(collection);
            var collCursor = coll.Find(null);
            List<BsonDocument> list = new List<BsonDocument>();

            foreach (var item in collCursor)
            {
                list.Add(item);
            }

            return list;
        }

        public List<BsonDocument> Find(string collection, Dictionary<string,BsonValue> queries, int limit=100)
        {
            QueryDocument query = new QueryDocument {};

            foreach (KeyValuePair<string, BsonValue> pair in queries)
            {
                query.Add(pair.Key, pair.Value);
            }

            var coll = db.GetCollection<BsonDocument>(collection);
            var collCursor = coll.Find(query).SetLimit(limit);
            List<BsonDocument> list = new List<BsonDocument>();

            foreach (var item in collCursor)
            {
                list.Add(item);
            }

            return list;
        }

        public string Insert(string collection, BsonDocument doc)
        {
            doc.Add("email", Config.Instance.Login.Email);
            var coll = db.GetCollection<BsonDocument>(collection);
            coll.Insert(doc);
            return doc["_id"].ToString();
        }

        public void Update(string collection, Dictionary<string, string> queries, Dictionary<string, string> updates)
        {
            QueryDocument query = new QueryDocument { };

            foreach (KeyValuePair<string, string> pair in queries)
            {
                query.Add(pair.Key, pair.Value);
            }

            UpdateDocument update = new UpdateDocument { };

            foreach (KeyValuePair<string, string> pair in updates)
            {
                update.Add(pair.Key, pair.Value);
            }

            var coll = db.GetCollection<BsonDocument>(collection);

            long count = coll.Find(query).Count();

            for (int i = 0; i < count; i++) 
            {
                coll.Update(query, update);
            }       
        }

        public BsonDocument ToBson(string docStr)
        {
            try
            {
                return MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(docStr);
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.ToString());
                return new BsonDocument();
            }
        }

        public void Test()
        {
            /*Connect();
            Dictionary<string, string> qry = new Dictionary<string,string>();
            qry.Add("a", "c");
            Dictionary<string, string> update = new Dictionary<string, string>();
            update.Add("a", "value");
           // Insert("test", new BsonDocument("a","c"));
            List<BsonDocument> l = FindAll("test");
            for (int i = 0; i < l.Count; i++)
            {
                Console.WriteLine(l[i].ToString());
            }

            Update("test", qry, update);
            l = FindAll("test");
            for (int i = 0; i < l.Count; i++)
            {
                Console.WriteLine(l[i].ToString());
            }
            Disconnect();
           /*
            var credential = MongoCredential.CreateMongoCRCredential("pgraph", "pgraph", "dsgfhd@@@@dksfjgdszhj125458ASDFMJDAFJA");

            var settings = new MongoServerSettings
            {
                Server = new MongoServerAddress("193.6.33.151", 57017),
                Credentials = new[] { credential }
            };
            try
            {
                MongoServer server = new MongoServer(settings);
                MongoDatabase db = server.GetDatabase("pgraph");

                var problems = db.GetCollection<BsonDocument>("problems");
                var problemCursor = problems.Find(null);

                foreach (var problem in problemCursor)
                {
                    Console.WriteLine(problem["name"]);
                }

                // Only disconnect when your app is terminating.

                server.Disconnect();
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine("SGFDGGF");
            }
            */
        }
    }
}
