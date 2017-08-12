using EntradaDados.Models;
using LinqToTwitter;
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

        public List<Tweet> GetTwitts(string screenName = "")
        {
            //List<Tweet> listaTweets = new List<Tweet>();
            //var service = new TwitterService(OAuthConsumerKey, OAuthConsumerSecret);
            //service.AuthenticateWith(AccessToken, AccessTokenSecret);
            //IEnumerable<TwitterStatus> tweetStatus = service.ListTweetsOnUserTimeline(
            //    new ListTweetsOnUserTimelineOptions
            //    {
            //        ScreenName = screenName,
            //        Count = 10
            //    });

            //foreach (TwitterStatus t in tweetStatus)
            //{
            //    Tweet tweet = new Tweet();
            //    tweet.Conteudo = t.Text.Replace("'", "");
            //    tweet.Politico = t.Author.ScreenName;
            //    tweet.DataHora = t.CreatedDate;
            //    listaTweets.Add(tweet);
            //}

            //return listaTweets;

            var auth = new SingleUserAuthorizer
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = OAuthConsumerKey,
                    ConsumerSecret = OAuthConsumerSecret,
                    AccessToken = AccessToken,
                    AccessTokenSecret = AccessTokenSecret
                }
            };

            var context = new TwitterContext(auth);
            var tweets =
                from tweetConfig in context.Status
                where
                    tweetConfig.Type == StatusType.User &&
                    tweetConfig.ScreenName == screenName
                select tweetConfig;


            return tweets
                .Take(20)
                .Select(t =>
                    new Tweet
                    {
                        Politico = t.ScreenName,
                        Conteudo = t.Text.Replace("'", ""),
                        DataHora = t.CreatedAt
                    })
                .ToList();
        }


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
            tweetsPoliticos.Add(ePolitico.eTiririca, "tiriricanaweb");
            tweetsPoliticos.Add(ePolitico.eRomario, "RomarioOnze");
            tweetsPoliticos.Add(ePolitico.eDilma, "dilmabr");
            tweetsPoliticos.Add(ePolitico.eMarinaSilva, "silva_marina");
            tweetsPoliticos.Add(ePolitico.eTemer, "MichelTemer");
            tweetsPoliticos.Add(ePolitico.eGarotinho, "blogdogarotinho");
            tweetsPoliticos.Add(ePolitico.eIndioDaCosta, "indio");
            tweetsPoliticos.Add(ePolitico.eCrivella, "MCrivella");
            tweetsPoliticos.Add(ePolitico.eJeanWyllys, "jeanwyllys_real");



            foreach (var key in tweetsPoliticos)
            {
                ePolitico politico = key.Key;
                
                List<Tweet> twitts = GetTwitts(tweetsPoliticos[politico]);

                foreach (Tweet t in twitts)
                {                                        
                    List<string> listaTweets = repository.SelectByPolitico(t);                    
                    if (listaTweets.Contains(t.Politico) && listaTweets.Count == twitts.Count)
                    {
                        break;                        
                    }
                    else
                    {
                        repository.ApplyToDb(t);
                    }                   
                }
            }

        }
    }
}