using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Twitter_DM_Sender
{
    class Login
    {
        public static string Go(CookieContainer cookie, string password, string username)
        {

            //get token
            var request = (HttpWebRequest)WebRequest.Create("https://twitter.com/");
            //request.Headers.Add("Origin", "https://twitter.com");
            request.UserAgent = Helper.UserAgent;
            request.Method = "GET";
            request.CookieContainer = cookie;
            request.ContentType = "application/x-www-form-urlencoded";

            var response = (HttpWebResponse)request.GetResponse();
            var streamReader = new StreamReader(response.GetResponseStream());
            var read = streamReader.ReadToEnd();
            var token = new Regex("value=\"(.*?)\" name=\"authenticity_token").Match(read).Groups[1].ToString();
            streamReader.Close();
            response.Close();

            //now login
            var postdata = Helper.LoginPostData(token, password, username);

            request = (HttpWebRequest)WebRequest.Create("https://twitter.com/sessions");
            request.Headers.Add("Origin", "https://twitter.com");
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.UserAgent = Helper.UserAgent;
            request.Method = "POST";
            request.CookieContainer = cookie;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Referer = "https://twitter.com/";

            byte[] bytes = Encoding.UTF8.GetBytes(postdata);
            request.ContentLength = bytes.Length;
            var stream = request.GetRequestStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();

            response = (HttpWebResponse)request.GetResponse();
            streamReader.Close();
            response.Close();
            string csrf = new Regex("ct0=(.*?);").Match(request.Headers.ToString()).Groups[1].ToString();
            return token;
        }
    }
}
