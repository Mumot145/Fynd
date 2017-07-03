using Newtonsoft.Json;
using SaveOn.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SaveOn.Http
{
    public class SocialMedia
    {
        public async Task<FacebookUser> GetFacebookProfileAsync(string accessToken)
        {
            FacebookUser facebookUser = new FacebookUser();
            var requestUrl = "https://graph.facebook.com/v2.8/me/"
                             + "?fields=name,picture,cover,age_range,devices,email,gender,is_verified"
                             + "&access_token=" + accessToken;
            var httpClient = new HttpClient();
            var userJson = await httpClient.GetStringAsync(requestUrl);
            facebookUser = JsonConvert.DeserializeObject<FacebookUser>(userJson);
            return facebookUser;
        }
    }
}
