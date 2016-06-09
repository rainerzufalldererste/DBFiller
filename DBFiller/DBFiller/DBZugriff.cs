using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace DBFiller
{
    class DBZugriff
    {
        public void LoadData(NpgsqlConnection connection)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();

            cmd.Connection = connection;

            foreach(Medikament med in Master.medikamente)
            {
                cmd.CommandText = "INSERT INTO Medikament (name, id) VALUES (" + med._name + ", " + med._id + ")";
                cmd.ExecuteNonQuery();
            }
        }    
    }
}
