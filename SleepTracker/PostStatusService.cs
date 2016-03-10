using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace SleepTracker
{
    class PostStatusService1 : AuthPostService
    {
        public void tryRequest(string message)
        {
            if (message == "")
            {
                return;
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("status={0}", Uri.EscapeDataString(message));

            string postBody = sb.ToString();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Headers.Add("Authorization", CreateAuthorizationHeaderParameter(CreateSignature(message)));
            request.ContentType = "application/x-www-form-urlencoded";
            request.ServicePoint.Expect100Continue = false;
            request.Method = "POST";


            try
            {
                using (Stream stream = request.GetRequestStream())
                {
                    Byte[] streamContent = Encoding.UTF8.GetBytes(postBody);
                    stream.Write(streamContent, 0, streamContent.Length);
                }

                WebResponse response = request.GetResponse();

                string contents = "";
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    contents = reader.ReadToEnd();
                }

                Console.WriteLine("Twitter response: " + contents);



            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message);


            }




        }
    }
}
