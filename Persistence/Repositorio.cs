using EntradaDados.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class Repositorio : DBService
    {
        
        public void InsertOrUpdate(Tweet tweet)
        {
            SetSession();
            Update(tweet);
        }

        public void DeleteObj(Tweet tweet)
        {
            SetSession();
            Delete(tweet);
        }

    }
}
