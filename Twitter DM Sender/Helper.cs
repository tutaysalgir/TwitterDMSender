using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Twitter_DM_Sender
{
    class Helper
    {
        public static string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.110 Safari/537.36";

        public static string LoginPostData(string token, string password, string username)
        {
            var postdata = "authenticity_token=" + token +
                    "&redirect_after_login=" +
                    "&remember_me=" + "1" +
                    "&scribe_log=" + "" +
                    "&session[password]=" + password +
                    "&session[username_or_email]=" + username +
                    "&ui_metrics=off";
            return postdata;
        }

        public static string GetUserID(CookieContainer cookie, string username)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://twitter.com/" + username);
            request.UserAgent = Helper.UserAgent;
            request.Method = "GET";
            request.CookieContainer = cookie;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("Origin", "https://twitter.com");
            var response = (HttpWebResponse)request.GetResponse();
            var streamReader = new StreamReader(response.GetResponseStream());
            var read = streamReader.ReadToEnd();
            var userid = new Regex("ProfileNav\" role=\"navigation\" data-user-id=\"(.*?)\"").Match(read).Groups[1].ToString();
            string kendiUserID = new Regex("current-user-id\" value=\"(.*?)\"").Match(read).Groups[1].ToString();
            streamReader.Close();
            response.Close();
            return userid + "-" + kendiUserID;
        }
    }
}
