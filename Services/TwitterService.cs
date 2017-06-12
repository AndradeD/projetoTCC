using EntradaDados.Models;
using Model;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace TwitterServiceApplication.Services
{
    public class TwitterService
    {
        public string OAuthConsumerSecret { get; set; }
        public string OAuthConsumerKey { get; set; }
        Repositorio repository = new Repositorio();

        public async Task<IEnumerable<string>> GetTwitts(string userName = "", int count = 0, string urlApiSearch = "", string accessToken = null)
        {
            if (accessToken == null)
            {
                accessToken = await GetAccessToken();
            }

            var requestUserTimeline = new HttpRequestMessage(HttpMethod.Get, urlApiSearch);        
            //var requestUserTimeline = new HttpRequestMessage(HttpMethod.Get, string.Format("https://api.twitter.com/1.1/statuses/user_timeline.json?count={0}&screen_name={1}&trim_user=1&exclude_replies=1", count, userName));
            requestUserTimeline.Headers.Add("Authorization", "Bearer " + accessToken);
            var httpClient = new HttpClient();
            HttpResponseMessage responseUserTimeLine = await httpClient.SendAsync(requestUserTimeline);
            var serializer = new JavaScriptSerializer();
            dynamic json = serializer.Deserialize<object>(await responseUserTimeLine.Content.ReadAsStringAsync());
            var enumerableTwitts = (json as IEnumerable<dynamic>);

            if (enumerableTwitts == null)
            {
                return null;
            }
            return enumerableTwitts.Select(t => (string)(t["text"].ToString()));
        }

        public async Task<string> GetAccessToken()
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.twitter.com/oauth2/token ");
            var customerInfo = Convert.ToBase64String(new UTF8Encoding().GetBytes(OAuthConsumerKey + ":" + OAuthConsumerSecret));
            request.Headers.Add("Authorization", "Basic " + customerInfo);
            request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

            HttpResponseMessage response = await httpClient.SendAsync(request);

            string json = await response.Content.ReadAsStringAsync();
            var serializer = new JavaScriptSerializer();
            dynamic item = serializer.Deserialize<object>(json);
            return item["access_token"];
        }

        public void SaveTweets() {

            Dictionary<ePolitico, string> tweetsPoliticos = new Dictionary<ePolitico, string>();
            IList<string> tweetsUrls = new List<string>();
            IList<string> tweets = new List<string>();
            TwitterService twitterService = new TwitterService();
            twitterService.OAuthConsumerKey = "BqyBLTFbRwjNzd155WjD6Ksk8";
            twitterService.OAuthConsumerSecret = "uwMli96OzrKwB3hiA5956QDhdwafmdIueWNbwVqMnEx7XNtwi2";

            tweetsPoliticos.Add(ePolitico.ePauloMaluf, "https://api.twitter.com/1.1/search/tweets.json?q=from%3Apaulosalimmaluf&result_type=popular");
            tweetsPoliticos.Add(ePolitico.eJairBolsonaro,"https://api.twitter.com/1.1/search/tweets.json?q=from%3Ajairbolsonaro&result_type=popular");
            tweetsPoliticos.Add(ePolitico.eFlavioBolsonaro,"https://api.twitter.com/1.1/search/tweets.json?q=from%3Aflaviobolsonaro&result_type=popular");
            tweetsPoliticos.Add(ePolitico.eJorgePicciani,"https://api.twitter.com/1.1/search/tweets.json?q=from%3Ajorgepicciani&result_type=popular");
            tweetsPoliticos.Add(ePolitico.eMarceloFreixo,"https://api.twitter.com/1.1/search/tweets.json?q=from%3Amarcelofreixo&result_type=popular");
            tweetsPoliticos.Add(ePolitico.eJoseSerra,"https://api.twitter.com/1.1/search/tweets.json?q=from%3Ajoseserra_&result_type=popular");
            tweetsPoliticos.Add(ePolitico.eEduardoPaes,"https://api.twitter.com/1.1/search/tweets.json?q=from%3Aeduardopaes_&result_type=popular");
            tweetsPoliticos.Add(ePolitico.eLula,"https://api.twitter.com/1.1/search/tweets.json?q=from%3ALulapeloBrasil&result_type=popular");
            tweetsPoliticos.Add(ePolitico.eDoria,"https://api.twitter.com/1.1/search/tweets.json?q=from%3Ajdoriajr&result_type=popular");
            tweetsPoliticos.Add(ePolitico.ePezao,"https://api.twitter.com/1.1/search/tweets.json?q=from%3Alfpezao&result_type=popular");

            for (int i = 0; i < tweetsPoliticos.Count; i++)
            {
                IEnumerable<string> twitts = GetTwitts(tweetsPoliticos[(ePolitico)i]).Result;
                                    
                foreach (var t in twitts)
                {
                    Console.WriteLine(t + "\n");
                    Tweet tweet = new Tweet();
                    tweet.Conteudo = t;                                        
                    tweet.Politico = tweetsPoliticos[(ePolitico)i].ToString();
                    tweet.DataHora = DateTime.Now;
                    repository.InsertOrUpdate(tweet);
                }                                            
            }    
                        
                    

        }
    }
}
