using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EntradaDados.Models
{
    public class Tweet
    {
        public long Id { get; set; }
        public string Conteudo { get; set; }
        public string Politico { get; set; }
        public DateTime DataHora { get; set; }
    }
}
