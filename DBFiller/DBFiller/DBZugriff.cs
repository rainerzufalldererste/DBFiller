using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Npgsql;

namespace DBFiller
{
   public static class DBZugriff
    { 

     public static NpgsqlConnection DBVerbidung()
        {
           return new NpgsqlConnection("HOST=141.7.66.161;Port=5433;Username=db1;Password=secret;Database=DB1_CRANKIHOUSE_asstilee");
        }


   
    
        public static void LoadData(NpgsqlConnection connection)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();

            cmd.Connection = connection;
            cmd.Connection.Open();

            foreach(Medikament med in Master.medikamente)
            {
                cmd.CommandText = "INSERT INTO \"Medikament\" (name, id) VALUES ('" + med._name + "', " + med._id + ")";
                cmd.ExecuteNonQuery();
            }
        }    
    }}
