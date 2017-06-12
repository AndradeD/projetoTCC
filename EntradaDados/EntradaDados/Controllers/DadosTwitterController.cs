using EntradaDados.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EntradaDados.Controllers
{
    public class DadosTwitterController : Controller
    {
        // GET: DadosTwitter
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllUsuarios()
        {
            //FAZENDO DO MODO BURRO
            IList<Pessoa> Pessoas = new List<Pessoa>();
            Pessoa Bolsonaro = new Pessoa();
            Bolsonaro.Id = 0;
            Bolsonaro.Nome = "Jair Bolsonaro";
            Bolsonaro.Ocupacao = "Deputado";
            Bolsonaro.Partido = "PSC";
            Bolsonaro.Hashtags = GetHashtagByUsuaro(Bolsonaro);
            Bolsonaro.PessoasRelacionadas.Add("Eneas Carneiro");
            Bolsonaro.PessoasRelacionadas.Add("Marco Feliciano");

            Pessoa Lula = new Pessoa();
            Lula.Id = 1;
            Lula.Nome = "Luiz Inacio Lula da Silva";
            Lula.Ocupacao = "Ex-Presidente";
            Lula.Partido = "PT";
            Lula.Hashtags = GetHashtagByUsuaro(Lula);
            Lula.PessoasRelacionadas.Add("Dilma Roussef");
            Lula.PessoasRelacionadas.Add("Jean Willys");

            Pessoas.Add(Bolsonaro);
            Pessoas.Add(Lula);

            return Json(Pessoas, JsonRequestBehavior.AllowGet);
        }

        public IList<Hashtag> GetHashtagByUsuaro(Pessoa pessoa)
        {
            //FAZENDO DO MODO BURRO
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