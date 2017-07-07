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
        string postgresConStr = "Server=localhost;user id=postgres;password=daniel123;database=twitterDB;";
        NpgsqlConnection Connection = null;
        DataTable tableElement = null;

        public Repository()
        {
            Connection = new NpgsqlConnection(postgresConStr);
            tableElement = new DataTable();
            tableElement.Columns.Add(new DataColumn("Id", typeof(long)));
            tableElement.Columns.Add(new DataColumn("Conteudo", typeof(string)));
            tableElement.Columns.Add(new DataColumn("Politico", typeof(string)));
            tableElement.Columns.Add(new DataColumn("DataHora", typeof(DateTime)));

        }

        public void SelectObject(Tweet tweet)
        {                        
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM twitter.tweet", Connection);
            Connection.Open();
            NpgsqlDataReader dr = cmd.ExecuteReader();
                       
            while (dr.Read())
            {
                DataRow row = tableElement.NewRow();
                row["Id"] = dr["id"];
                row["Conteudo"] = dr["conteudo"];
                row["Politico"] = dr["politico"];
                row["DataHora"] = dr["datahora"];
                tableElement.Rows.Add(row);
            }
            Connection.Close();
            tableElement.AcceptChanges();
            PrintTable();                 
        }
        public void PrintTable()
        {
            foreach (DataColumn dc in tableElement.Columns)
            {
                Console.Write(dc.ColumnName + "   ");
            }
            Console.WriteLine();
            Console.WriteLine();
            foreach (DataRow dr in tableElement.Rows)
            {
                for (int i = 0; i < dr.ItemArray.Length; i++)
                {

                    Console.Write(dr[i] + "\t");


                }
                Console.WriteLine();
                Console.WriteLine();
            }

        }
        public void ApplyToDb(Tweet tweet)
        {

            NpgsqlCommand update = new NpgsqlCommand("UPDATE twitter.tweet SET conteudo=?, politico=?,datahora=? WHERE id =? ", Connection);

            update.Parameters.Add(new NpgsqlParameter("@conteudo", OleDbType.VarChar));
            update.Parameters.Add(new NpgsqlParameter("@politico", OleDbType.VarChar));
            update.Parameters.Add(new NpgsqlParameter("@datahora", OleDbType.VarChar));
            update.Parameters.Add(new NpgsqlParameter("@id", OleDbType.Integer));




            NpgsqlCommand add = new NpgsqlCommand("INSERT INTO twitter.tweet (conteudo, politico, datahora) VALUES("+ tweet.Conteudo + ", "+ tweet.Politico + ", "+ tweet.DataHora + ")", Connection);
            add.Parameters.Add(new NpgsqlParameter("@conteudo", OleDbType.VarChar));
            add.Parameters.Add(new NpgsqlParameter("@politico", OleDbType.VarChar));
            add.Parameters.Add(new NpgsqlParameter("@datahora", OleDbType.VarChar));


            foreach (DataRow dro in tableElement.GetChanges().Rows)
            {

                if (dro.RowState == DataRowState.Added)
                {
                    add.Parameters[0].Value = dro[1];
                    add.Parameters[1].Value = dro[2];
                    add.Parameters[2].Value = dro[3];
                    Connection.Open();
                    add.ExecuteNonQuery();
                    Connection.Close();
                }
                if (dro.RowState == DataRowState.Modified)
                {
                    update.Parameters[0].Value = dro[1];
                    update.Parameters[1].Value = dro[2];
                    update.Parameters[2].Value = dro[3];
                    update.Parameters[3].Value = dro[0];
                    Connection.Open();
                    update.ExecuteNonQuery();
                    Connection.Close();
                }

            }
        }


    }
}
