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
using System.Net;
using System.Threading.Tasks;
using System.IO;
using MongoDB.Bson;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

using PNSDraw.Configuration;

namespace PNSDraw
{
    class WebserverHttpClient
    {
        string url = "https://pgraph.dcs.uni-pannon.hu:51005";
        X509Certificate cert;

        public WebserverHttpClient()
        {
            cert = X509Certificate.CreateFromCertFile("cert/ca.crt");
            ServicePointManager.ServerCertificateValidationCallback = CertificateValidationCallback;
        }

        public bool Check()
        {
            string uri = url + "/test";
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.ClientCertificates.Add(cert);

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (WebException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
            return true;
        }

        public bool Login(string username, string password)
        {
            if (!Check())
            {
                MessageBox.Show("Server not found or your computer is not connected to the Internet!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            BsonDocument info = GetUser(username, password);
            Console.WriteLine(info.ToString());
            if (info["code"].AsInt32 == 200)
            {
                Config.Instance.Login.Username = info["body"]["username"].AsString;
                Config.Instance.Login.Email = info["body"]["email"].AsString;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Register(string username, string password, string email)
        {
            if (!Check())
            {
                MessageBox.Show("Server not found or your computer is not connected to the Internet!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            BsonDocument info = CreateUser(username, password, email);
            if (info["code"].AsInt32 == 201)
            {
                return true;
            }
            else
            {
                return false;
            }          
        }

        public bool Validate(string username, string code)
        {
            BsonDocument info = ValidateUser(username, code);
            if (info["code"].AsInt32 == 200)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private BsonDocument GetUser(string username, string password)
        {
            string uri = url + "/user?username=" + username + "&password=" + password;

            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.ClientCertificates.Add(cert);

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return ToBson(responseString);
        }

        public BsonDocument CreateUser(string username, string password, string email)
        {
            var request = (HttpWebRequest)WebRequest.Create(url+"/user");

            var postData = "username="+username;
            postData += "&password="+password;
            postData += "&email="+email;
            var data = Encoding.UTF8.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.ClientCertificates.Add(cert);            
            
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return ToBson(responseString);
        }

        private BsonDocument ValidateUser(string username, string code)
        {
            var request = (HttpWebRequest)WebRequest.Create(url + "/user/validation");

            var putData = "username=" + username;
            putData += "&code=" + code;
            var data = Encoding.UTF8.GetBytes(putData);

            request.Method = "PUT";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.ClientCertificates.Add(cert);

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return ToBson(responseString);
        }

        private BsonDocument ToBson(string docStr)
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

        public bool ForgotPassword(string username)
        {
            if (!Check())
            {
                MessageBox.Show("Server not found or your computer is not connected to the Internet!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            var request = (HttpWebRequest)WebRequest.Create(url + "/forgotpwd");

            var postData = "username=" + username;
            var data = Encoding.UTF8.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.ClientCertificates.Add(cert);

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            var responseBSON = ToBson(responseString);

            if (responseBSON["code"].AsInt32 == 205)
            {
                return true;
            }

            return false;
        }

        private static bool CertificateValidationCallback(
         object sender,
         System.Security.Cryptography.X509Certificates.X509Certificate certificate,
         System.Security.Cryptography.X509Certificates.X509Chain chain,
         System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            // If the certificate is a valid, signed certificate, return true.
            if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
            {
                return true;
            }
            // If there are errors in the certificate chain, look at each error to determine the cause.
            if ((sslPolicyErrors & System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors) != 0)
            {
                if (chain != null && chain.ChainStatus != null)
                {
                    foreach (System.Security.Cryptography.X509Certificates.X509ChainStatus status in chain.ChainStatus)
                    {
                        if ((certificate.Subject == certificate.Issuer) &&
                           (status.Status == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.UntrustedRoot))
                        {
                            // Self-signed certificates with an untrusted root are valid.
                            continue;
                        }
                        else
                        {
                            if (status.Status != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
                            {
                                // If there are any other errors in the certificate chain, the certificate is invalid,
                                // so the method returns false.
                                return false;
                            }
                        }
                    }
                }
                // When processing reaches this line, the only errors in the certificate chain are
                // untrusted root errors for self-signed certificates. These certificates are valid
                // for default Exchange Server installations, so return true.
                return true;
            }
            else
            {
                // In all other cases, return false.
                return false;
            }
        }

        public bool ChangePassword(string username, string email, string currentPwd, string newPwd)
        {
            if (!Check())
            {
                MessageBox.Show("Server not found or your computer is not connected to the Internet!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            var request = (HttpWebRequest)WebRequest.Create(url + "/user");
            
            var putData = "username=" + username;
            putData += "&oldPassword=" + currentPwd;
            putData += "&newPassword=" + newPwd;
            putData += "&email=" + email;            

            var data = Encoding.UTF8.GetBytes(putData);

            request.Method = "PUT";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.ClientCertificates.Add(cert);

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            BsonDocument responseBSON = ToBson(responseString);

            if (responseBSON["code"].AsInt32 == 201)
            {
                return true;
            }

            return false;
        }

        public bool Report(string level, string text)
        {
            if (!Check())
            {
                //MessageBox.Show("Server not found or your computer is not connected to the Internet!\n\n" + text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            var request = (HttpWebRequest)WebRequest.Create(url + "/report");

            var putData = "username=" + Config.Instance.Login.Username;
            putData += "&email=" + Config.Instance.Login.Email;
            putData += "&level=" + level;
            putData += "&text=" + text;

            var data = Encoding.UTF8.GetBytes(putData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.ClientCertificates.Add(cert);

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            BsonDocument responseBSON = ToBson(responseString);

            if (responseBSON["code"].AsInt32 == 204)
            {
                return true;
            }

            return false;
        }
    }
}
