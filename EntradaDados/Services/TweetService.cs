using EntradaDados.Models;
using Model;
using Persistence;
using Persistence.Persistencia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using TweetSharp;

namespace TwitterServiceApplication.Services
{
    public class TweetService
    {
        public string OAuthConsumerSecret = "uwMli96OzrKwB3hiA5956QDhdwafmdIueWNbwVqMnEx7XNtwi2";
        public string OAuthConsumerKey = "BqyBLTFbRwjNzd155WjD6Ksk8";
        public string AccessToken = "96777401-6xjcu43NpnHGNdhd6tIJlxkpNqbWVz2ZfJUzkF9C5";
        public string AccessTokenSecret = "8TdU0WfYhxFz3sS1GvX1mmuyjFtMFnCEH4p96fqk1OV6Y";

        Repository repository = new Repository();

        public List<string> GetTwitts(string screenName = "")
        {
            List<string> listaTweets = new List<string>();
            var service = new TwitterService(OAuthConsumerKey, OAuthConsumerSecret);
            service.AuthenticateWith(AccessToken, AccessTokenSecret);
            IEnumerable<TwitterStatus> tweetStatus = service.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions { ScreenName = screenName, Count = 10 });

            foreach (TwitterStatus t in tweetStatus) {
                listaTweets.Add(t.Text);
            }

            //HttpWebRequest http = WebRequest.Create(urlApiSearch) as HttpWebRequest;

            ////var requestUserTimeline = new HttpRequestMessage(HttpMethod.Get, string.Format("https://api.twitter.com/1.1/statuses/user_timeline.json?count={0}&screen_name={1}&trim_user=1&exclude_replies=1", count, userName));
            //http.Headers.Add("Authorization", "Bearer " + AccessToken);

            //HttpWebResponse response = (HttpWebResponse)http.GetResponse();
            ////HttpResponseMessage responseUserTimeLine = httpClient.get(requestUserTimeline);
            //var serializer = new JavaScriptSerializer();
            //dynamic json = serializer.Deserialize<object>(response.ToString());
            //var enumerableTwitts = (json as IEnumerable<dynamic>);

            //if (enumerableTwitts == null)
            //{
            //    return null;
            //}
            return listaTweets;
        }

        //public async Task<string>/*string */GetAccessToken()
        //{
        //   // var httpClient = new HttpClient();
        //    var request = new HttpRequestMessage(HttpMethod.Post, "https://api.twitter.com/oauth2/token ");
        //    var customerInfo = Convert.ToBase64String(new UTF8Encoding().GetBytes(OAuthConsumerKey + ":" + OAuthConsumerSecret));
        //    request.Headers.Add("Authorization", "Basic " + customerInfo);
        //    request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

        //    // WebRequest request = WebRequest.Create("https://api.twitter.com/oauth2/token");
        //    //request.Headers.Add("Authorization", "Basic " + customerInfo);
        //    // request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

        //    //WebResponse response = request.GetResponse();            

        //    //Stream dataStream = response.GetResponseStream();

        //    //StreamReader reader = new StreamReader(dataStream);

        //    //string rt = reader.ReadToEnd();

        //    string response = request.Content.ToString();

        //    //      string json = response.ToString();
        //    string json = response; 
        //    var serializer = new JavaScriptSerializer();
        //    dynamic item = serializer.Deserialize<object>(json);
        //    return item["access_token"];
        //}


        public void SaveTweets()
        {

            Dictionary<ePolitico, string> tweetsPoliticos = new Dictionary<ePolitico, string>();
            IList<string> tweetsUrls = new List<string>();
            IList<string> tweets = new List<string>();            
           
            tweetsPoliticos.Add(ePolitico.ePauloMaluf, "paulosalimmaluf");
            tweetsPoliticos.Add(ePolitico.eJairBolsonaro, "jairbolsonaro");
            tweetsPoliticos.Add(ePolitico.eFlavioBolsonaro, "flaviobolsonaro");
            tweetsPoliticos.Add(ePolitico.eJorgePicciani, "jorgepicciani");
            tweetsPoliticos.Add(ePolitico.eMarceloFreixo, "marcelofreixo");
            tweetsPoliticos.Add(ePolitico.eJoseSerra, "joseserra_");
            tweetsPoliticos.Add(ePolitico.eEduardoPaes, "eduardopaes_");
            tweetsPoliticos.Add(ePolitico.eLula, "LulapeloBrasil");
            tweetsPoliticos.Add(ePolitico.eDoria, "jdoriajr");
            tweetsPoliticos.Add(ePolitico.ePezao, "lfpezao");


            foreach (var key in tweetsPoliticos)
            {
                ePolitico politico = key.Key;
                
                List<string> twitts = GetTwitts(tweetsPoliticos[politico]);

                foreach (var t in twitts)
                {                    
                    Console.WriteLine(t + "\n");
                    Tweet tweet = new Tweet();
                    tweet.Conteudo = t.Replace("'","");
                    tweet.Politico = tweetsPoliticos[politico].ToString();
                    tweet.DataHora = DateTime.Now;

                    List<string> listaTweets = repository.SelectByPolitico(tweet);                    
                    if (!listaTweets.Contains(tweet.Politico))
                    {
                        repository.ApplyToDb(tweet);
                    }
                    else
                    {
                        break;
                    }                   
                }
            }

        }
    }
}