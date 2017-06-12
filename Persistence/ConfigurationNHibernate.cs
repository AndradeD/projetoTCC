using EntradaDados.Models;
using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class ConfigurationNHibernate
    {
        //public ISessionFactory _sessionFactory;
        private Configuration _configuration;
       
        public void SetUpConfiguration(ISessionFactory _sessionFactory)
        {
            Configuration _configuration = new Configuration();
            _configuration.Properties.Add("connection.providers","NHibernate.Connection.DriverConnectionProvider");
            _configuration.Properties.Add("connection.driver_class", "NHibernate.Driver.NpgsqlDriver");
          //  _configuration = new Configuration();
            _configuration.Configure();
            _configuration.AddAssembly(typeof(Tweet).Assembly);
            _sessionFactory = _configuration.BuildSessionFactory();
        }
    }
}
