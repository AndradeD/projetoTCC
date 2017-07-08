using EntradaDados.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Persistencia
{
    public class Repository
    {
        private DataSet ds = new DataSet();
        string postgresConStr = "Server=localhost;user id=postgres;password=daniel123;database=twitterdb;";
        NpgsqlConnection Connection = null;
        DataTable tableElement = null;

        public Repository()
        {
            Connection = new NpgsqlConnection(postgresConStr);

        }

        public List<string> SelectByPolitico(Tweet tweet)
        {
            List<string> listaTweets = new List<string>();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM twitter.tweet where politico = '" + tweet.Politico + "'", Connection);
            Connection.Open();
            NpgsqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Tweet tweetElement = new Tweet();
                tweetElement.Id = long.Parse(dr["id"].ToString());
                tweetElement.Conteudo = dr["conteudo"].ToString();
                tweetElement.Politico = dr["politico"].ToString();
                tweetElement.DataHora = DateTime.ParseExact(dr["datahora"].ToString(), "dd/MM/yyyy HH:mm:ss", null);
                listaTweets.Add(tweetElement.Politico);
            }
            Connection.Close();
            return listaTweets;
        }
        public void ApplyToDb(Tweet tweet)
        {
            NpgsqlCommand command = null;

            if (tweet.Id != 0)
            {
                command = new NpgsqlCommand("UPDATE twitter.tweet SET conteudo=?, politico=?,datahora=? WHERE id =? ", Connection);
            }
            else
            {
                command = new NpgsqlCommand("INSERT INTO twitter.tweet (id,conteudo, politico, datahora) VALUES((select max(id)+1 from twitter.tweet),'" + tweet.Conteudo + "', '" + tweet.Politico + "', :date)", Connection);
            }


            NpgsqlParameter param = new NpgsqlParameter(":date", NpgsqlTypes.NpgsqlDbType.Date);
            param.Value = tweet.DataHora;
            command.Parameters.Add(param);

            Connection.Open();
            command.ExecuteNonQuery();
            Connection.Close();
        }


    }
}
