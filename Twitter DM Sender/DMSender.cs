using System;
using System.IO;
using System.Net;
using System.Text;

namespace Twitter_DM_Sender
{
    class DMSender
    {
        public static bool Go(CookieContainer cookie, string token, string mesaj, string target_user, string ourusername, string csrf)
        {
            
            var conversation_id = Helper.GetUserID(cookie, target_user);
            var request = (HttpWebRequest)WebRequest.Create("https://api.twitter.com/1.1/dm/new.json");
            request.Headers.Add("Origin", "https://twitter.com");
            request.Headers.Add("Sec-Fetch-Mode", "cors");
            request.Headers.Add("x-twitter-client-language", "en");
            request.Headers.Add("x-csrf-token", csrf);
            request.Headers.Add("x-twitter-auth-type", "OAuth2Session");
            request.Headers.Add("x-twitter-active-user", "no");
            request.Headers.Add("authorization", "Bearer AAAAAAAAAAAAAAAAAAAAANRILgAAAAAAnNwIzUejRCOuH5E6I8xnZz4puTs%3D1Zv7ttfk8LF81IUq16cHjhLTvJu4FA33AGWWjCpTnA");

            request.UserAgent = Helper.UserAgent;
            request.Method = "POST";
            request.Timeout = 10000;
            request.Host = "twitter.com";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.CookieContainer = cookie;
            request.Referer = "https://twitter.com/";
            Random rast = new Random();
            int rastint7 = rast.Next(1000000, 9999999);
            mesaj = Spinner.Spin(mesaj);
            string postdata = "authenticity_token=" + token + "&&conversation_id=" + conversation_id
                + "&scribeContext%5Bcomponent%5D=dm_existing_conversation_dialog&text="
                + mesaj + "&tweetboxId=swift_tweetbox_"
                                    + "142824" + rastint7.ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(postdata);
            request.ContentLength = bytes.Length;
            Stream stream = request.GetRequestStream();
            stream.Write(bytes, 0, bytes.Length);
            HttpWebResponse res = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(res.GetResponseStream());
            string oku = sr.ReadToEnd();
            stream.Close();
            res.Close();

            if (oku.Contains("--sent"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
