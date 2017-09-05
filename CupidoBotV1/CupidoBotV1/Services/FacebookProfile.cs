using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CupidoBotV1.Data;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace CupidoBotV1.Services
{
    public class FacebookProfile
    {
        public static async Task<FbResponse> findInfoFromFb(string userId)
        {
            FbResponse resp = new FbResponse();
            string accessToken = ConfigurationManager.AppSettings["FacebookPageAccessToken"];
            string fbBaseUrl = ConfigurationManager.AppSettings["FacebookBaseUrl"];
            string url = fbBaseUrl + userId + "?fields=first_name,last_name,profile_pic&access_token=" + accessToken;
            //to change here to httpclient that should be better...
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(receiveStream);
                resp = JsonConvert.DeserializeObject<FbResponse>(sr.ReadToEnd());
            }
            return resp;
        }
    }
}