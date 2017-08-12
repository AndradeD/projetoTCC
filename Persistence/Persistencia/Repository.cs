using EntradaDados.Models;
using MySql.Data.MySqlClient;
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
        private DataSet ds = new DataSet();;
        string mySQL = "Server=localhost;Database=twitter;Uid=root;Pwd=daniel123;";
        
        MySqlConnection mySQLConnection = null;      
        DataTable tableElement = null;

        public Repository()
        {
            //Connection = new NpgsqlConnection(mySQL);
            mySQLConnection = new MySqlConnection(mySQL);

        }

        public List<string> SelectByPolitico(Tweet tweet)
        {
            List<string> listaTweets = new List<string>();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM twitter.tweet where politico = '" + tweet.Politico + "' and datahora = '"+tweet.DataHora.ToString("yyyy-MM-dd HH:mm:ss") + "' and conteudo = '"+tweet.Conteudo+"'", mySQLConnection);
            mySQLConnection.Open();
            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Tweet tweetElement = new Tweet();
                tweetElement.Id = long.Parse(dr["id"].ToString());
                tweetElement.Conteudo = dr["conteudo"].ToString();
                tweetElement.Politico = dr["politico"].ToString();
                tweetElement.DataHora = DateTime.ParseExact(dr["datahora"].ToString(), "dd/MM/yyyy HH:mm:ss", null);
                listaTweets.Add(tweetElement.Politico);
            }
            mySQLConnection.Close();
            return listaTweets;
        }
        public void ApplyToDb(Tweet tweet)
        {
            MySqlCommand command = null;

            if (tweet.Id != 0)
            {
                command = new MySqlCommand("UPDATE twitter.tweet SET conteudo=?, politico=?,datahora=? WHERE id =? ", mySQLConnection);
            }
            else
            {
                string conteudo = RemoveSurrogatePairs(tweet.Conteudo);
                command = new MySqlCommand("INSERT INTO twitter.tweet (id,conteudo, politico, datahora) SELECT 1 + coalesce((SELECT max(id) FROM twitter.tweet), 0),'" + conteudo + "', '" + tweet.Politico + "', '"+tweet.DataHora.ToString("yyyy-MM-dd HH:mm:ss") +"';", mySQLConnection);
            }


            //MySqlParameter param = new MySqlParameter(":date", MySqlDbType.Timestamp);
            //param.Value = tweet.DataHora;
            //command.Parameters.Add(param);
            //command.CommandText = command.CommandText.Replace(":date", "'" + tweet.DataHora + "'");

            mySQLConnection.Open();
            command.ExecuteNonQuery();
            mySQLConnection.Close();
        }

        public string RemoveSurrogatePairs(string str, string replacementCharacter = "?")
        {
            if (str == null)
            {
                return null;
            }

            StringBuilder sb = null;

            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[i];

                if (char.IsSurrogate(ch))
                {
                    if (sb == null)
                    {
                        sb = new StringBuilder(str, 0, i, str.Length);
                    }

                    sb.Append(replacementCharacter);

                    // If there is a high+low surrogate, skip the low surrogate
                    if (i + 1 < str.Length && char.IsHighSurrogate(ch) && char.IsLowSurrogate(str[i + 1]))
                    {
                        i++;
                    }
                }
                else if (sb != null)
                {
                    sb.Append(ch);
                }
            }

            return sb == null ? str : sb.ToString();
        }


    }
}
