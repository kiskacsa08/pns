using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;

using MongoDB.Bson;

namespace PNSDraw.online
{
    class SolverSocket
    {
        static TcpClient client;
        static NetworkStream stream;

        public SolverSocket()
        {
            
        }

        public void Connect(String server, int port)
        {
            try
            {
                client = new TcpClient(server, port);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
                throw new Exception("ERROR in argument", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                throw new Exception("ERROR in socket", e);
            }
        }

        public void Close()
        {
            stream.Close();
            client.Close();
        }

        public string Send(string msg)
        {
            string message = msg.Length + "$LENGTH$" + msg + "END_OF_DATAEND_OF_DATA";

            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            List<string> response = new List<string>();

            try
            {
                stream = client.GetStream();

                stream.Write(data, 0, data.Length);

                data = new Byte[256];

                String responseData = String.Empty;

                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                return GetJsonString(responseData);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
                return "ERROR: IOException";
            }
        }

        public string Done()
        {
            Byte[] data = new Byte[256];
            Int32 bytes = stream.Read(data, 0, data.Length);
            String responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            return GetJsonString(responseData);
        }

        private string GetJsonString(string str)
        {
            string[] separator = new string[2];
            separator[0] = "$LENGTH$";
            separator[1] = "END_OF_DATA";
            string[] parts = str.Split(separator,StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length >= 2 && Convert.ToInt32(parts[0]) == parts[1].Length)
            {
                return parts[1];
            }

            return "ERROR: Response message is corrupted";
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
    }
}
