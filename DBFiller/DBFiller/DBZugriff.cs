using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Npgsql;

namespace DBFiller
{
    public static partial class DBZugriff
    { 
        static NpgsqlConnection connection;

        public static void DBVerbidung()
        {
            connection = new NpgsqlConnection("HOST=141.7.66.161;Port=5433;Username=db1;Password=secret;Database=DB1_CRANKIHOUSE_asstilee");
            connection.Open();
        }


   
    
        public static void LoadData()
        {
            Console.WriteLine();

            NpgsqlCommand cmd = new NpgsqlCommand();

            cmd.Connection = connection;

            foreach(Patient pat in Master.patienten)
            {
                cmd.CommandText = "INSERT INTO \"Patient\" (name, krankenkassenNr, geschlecht, alter) VALUES ('" + pat._name + "', " + pat._krankenkassenNr + ", '" + pat._geschlecht + "', " + pat._alter + ")";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Patienten angelegt.");

            foreach(Abteilung ab in Master.abteilung)
            {
                cmd.CommandText = "INSERT INTO \"Abteilung\" (stationsNr, name) VALUES (" + ab._stationNr + ", '" + ab._name + "')";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Abteilungen angelegt.");

            foreach (Angestellter an in Master.angestellte)
            {
                cmd.CommandText = "INSERT INTO \"Angestellter\" (id, name) VALUES (" + an._id + ", '" + an._name + "')";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Angestellte angelegt.");

            foreach (Arzt ar in Master.ärzte)
            {
                cmd.CommandText = "INSERT INTO \"Arzt\" (id, stationsNr) VALUES (" + ar._id + ", " + ar._stationNr + ")";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Ärzte angelegt.");

            foreach (Pfleger pfle in Master.pfleger)
            {
                cmd.CommandText = "INSERT INTO \"Pfleger\" (id) VALUES (" + pfle._id + ")";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Pfleger angelegt.");

            foreach (Zimmer zi in Master.zimmer)
            {
                cmd.CommandText = "INSERT INTO \"Zimmer\" (zimmerNr, stationsNr) VALUES (" + zi._id + ", " + zi._stationNr + ")";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Zimmer angelegt.");

            foreach (PflegerProZimmer ppz in Master.pflegerProZimmer)
            {
                cmd.CommandText = "INSERT INTO \"PflegerProZimmer\" (pflegerID, zimmerNr) VALUES (" + ppz._pflegerId + ", " + ppz._zimmerNr + ")";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("PflegerProZimmer angelegt.");

            foreach (Bett bett in Master.betten)
            {
                cmd.CommandText = "INSERT INTO \"Bett\" (bettenNr, zimmerNr) VALUES (" + bett._bettenNr + ", " + bett._zimmerNr + ")";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Betten angelegt.");

            foreach (Aufenthalt auf in Master.aufenthalte)
            {
                auf.getID();
                cmd.CommandText = "INSERT INTO \"Aufenthalt\" (id, startDate, endDate, krankenkassenNr, bettenNr, behandelnderArzt) VALUES (" + auf._id + ", to_date('" + auf._startDate.ToString("yyyy-MM-dd HH:mm") + "', 'YYYY-MM-DD HH24:MI'), to_date('" + auf._endDate.ToString("yyyy-MM-dd HH:mm") + "', 'YYYY-MM-DD HH24:MI'), " + auf._krankenkassenNr + ", " + auf._bettenNr + ", " + auf._behandelnderArzt + ")";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Aufenthalte angelegt.");

            foreach (Diagnose dia in Master.diagnosen)
            {
                cmd.CommandText = "INSERT INTO \"Diagnose\" (aufenthaltsID, diagnose) VALUES (" + dia._aufenthaltsID + ", '" + dia._diagnose + "')";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Diagnosen angelegt.");

            foreach (Medikament med in Master.medikamente)
            {
                cmd.CommandText = "INSERT INTO \"Medikament\" (name, id) VALUES ('" + med._name + "', " + med._id + ")";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Medikamene angelegt.");

            foreach (MedProAufenthalt mpa in Master.medsProAufenthalt)
            {
                cmd.CommandText = "INSERT INTO \"MedProAufenthalt\" (medID, id) VALUES (" + mpa._medID + ", " + mpa._aufenthaltsID + ")";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("MedProAufenthalte angelegt.");

            foreach (Unverträglichkeit unver in Master.unverträglichkeiten)
            {
                cmd.CommandText = "INSERT INTO \"Unvertraeglichkeiten\" (med1, med2) VALUES (" + unver._med1 + ", " + unver._med2 + ")";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Unverträglichkeiten angelegt.");

            foreach (Arbeitslog al in Master.arbeitslogs)
            {
                cmd.CommandText = "INSERT INTO \"Arbeitslog\" (id, startDate, endDate) VALUES (" + al._angestellterId + ", to_date('" + al._startDate.ToString("yyyy-MM-dd HH:mm") + "', 'YYYY-MM-DD HH24:MI'), to_date('" + al._endDate.ToString("yyyy-MM-dd HH:mm") + "', 'YYYY-MM-DD HH24:MI'))";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Arbeitslogs angelegt.");

            Console.WriteLine("\nAlle Tabellen wurden vollständig angelegt.");
        }    
    }
}
