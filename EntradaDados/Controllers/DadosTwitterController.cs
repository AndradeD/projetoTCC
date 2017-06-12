using EntradaDados.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwitterServiceApplication.Services;

namespace EntradaDados.Controllers
{
    public class DadosTwitterController : Controller
    {
        TweetService twitterService = new TweetService();

        // GET: DadosTwitter
        public ActionResult Index()
        {
            twitterService.SaveTweets();
            return View();
        }

        public ActionResult GetAllUsuarios()
        {            
            IList<Pessoa> Pessoas = new List<Pessoa>();
            IList<Pessoa> PessoasRelacionadas = new List<Pessoa>();

            Pessoa Bolsonaro = new Pessoa();
            Bolsonaro.Id = 0;
            Bolsonaro.Nome = "Jair Bolsonaro";
            Bolsonaro.Ocupacao = "Deputado";
            Bolsonaro.Partido = "PSC";
            Bolsonaro.Hashtags = GetHashtagByUsuaro(Bolsonaro);

            Pessoa Eneas = new Pessoa();
            Eneas.Id = 2;
            Eneas.Nome = "Eneas Carneiro";
            Eneas.Ocupacao = "Deputado";
            Eneas.Partido = "Partido";
            Eneas.Hashtags = GetHashtagByUsuaro(Bolsonaro);

            Pessoa Marco = new Pessoa();
            Marco.Id = 3;
            Marco.Nome = "Marco Feliciano";
            Marco.Ocupacao = "Deputado";
            Marco.Partido = "Partido";
            Marco.Hashtags = GetHashtagByUsuaro(Bolsonaro);

            Bolsonaro.PessoasRelacionadas.Add(Eneas);
            Bolsonaro.PessoasRelacionadas.Add(Marco);

            Pessoa Lula = new Pessoa();
            Lula.Id = 1;
            Lula.Nome = "Luiz Inacio Lula da Silva";
            Lula.Ocupacao = "Ex-Presidente";
            Lula.Partido = "PT";
            Lula.Hashtags = GetHashtagByUsuaro(Lula);


            Pessoa Dilma = new Pessoa();
            Marco.Id = 4;
            Marco.Nome = "Dilma Roussef";
            Marco.Ocupacao = "Ex-Presidente";
            Marco.Partido = "PT";
            Marco.Hashtags = GetHashtagByUsuaro(Lula);


            Pessoa Jean = new Pessoa();
            Marco.Id = 5;
            Marco.Nome = "Jean Willys";
            Marco.Ocupacao = "Deputado";
            Marco.Partido = "Partido";
            Marco.Hashtags = GetHashtagByUsuaro(Lula);

            Lula.PessoasRelacionadas.Add(Dilma);
            Lula.PessoasRelacionadas.Add(Jean);

            Pessoas.Add(Bolsonaro);
            Pessoas.Add(Lula);

            return Json(new { Pessoas = Pessoas }, JsonRequestBehavior.AllowGet);
        }

        public IList<Hashtag> GetHashtagByUsuaro(Pessoa pessoa)
        {            
            IList<Hashtag> Hashtags = new List<Hashtag>();
            if (pessoa.Id == 0)
            {
                Hashtag hashtag = new Hashtag();
                hashtag.Id = 0;
                hashtag.Nome = "#Bolsomito2018";
                hashtag.Quantidade = 3;
                Hashtags.Add(hashtag);
            }else
            if (pessoa.Id == 1)
            {
                Hashtag hashtag = new Hashtag();
                hashtag.Id = 1;
                hashtag.Nome = "#Lula2018";
                hashtag.Quantidade = 5;
                Hashtags.Add(hashtag);
            }
            return Hashtags;
        }        

    }
}