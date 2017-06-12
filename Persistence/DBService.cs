using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class DBService
    {
        private ISession _mSession;
        private ISessionFactory _mSessionFactory;
        private ConfigurationNHibernate cfg = new ConfigurationNHibernate();

        public void SetSession()
        {
            cfg.SetUpConfiguration(_mSessionFactory);
            _mSession = _mSessionFactory.OpenSession();
        }

        public void Update(Object obj)
        {            
            using (var transaction = _mSession.BeginTransaction())
            {
                _mSession.Save(obj);
                transaction.Commit();
            }
        }

        public void Delete(Object obj)
        {
            using (var transaction = _mSession.BeginTransaction())
            {
                _mSession.Delete(obj);
                transaction.Commit();
            }
        }

    }
}
