using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EntradaDados.Models
{
    public class Pessoa
    {
        public Pessoa()
        {
            Hashtags = new List<Hashtag>();
            PessoasRelacionadas = new List<Pessoa>();
        }

        public virtual long Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Ocupacao { get; set; }
        public virtual string Partido { get; set; }
        public virtual IList<Hashtag> Hashtags { get; set; }
        public virtual IList<Pessoa> PessoasRelacionadas { get; set; }
    }
}