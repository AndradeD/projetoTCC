using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EntradaDados.Models
{
    public class Hashtag
    {
        public virtual long Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual int Quantidade { get; set; }
    }
}