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

            Console.WriteLine("Lege Patienten an ({0} Elemente)", Master.patienten.Count);

            foreach (Patient pat in Master.patienten)
            {
                cmd.CommandText = "INSERT INTO \"Patient\" (name, krankenkassenNr, geschlecht, alter) VALUES ('" + pat._name + "', " + pat._krankenkassenNr + ", '" + pat._geschlecht + "', " + pat._alter + ")";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Patienten angelegt.");

            Console.WriteLine("Lege Abteilungen an ({0} Elemente)", Master.abteilungen.Count);

            foreach (Abteilung ab in Master.abteilungen)
            {
                cmd.CommandText = "INSERT INTO \"Abteilung\" (stationsNr, name) VALUES (" + ab._stationNr + ", '" + ab._name + "')";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Abteilungen angelegt.");

            Console.WriteLine("Lege Angestellte an ({0} Elemente)", Master.angestellte.Count);

            foreach (Angestellter an in Master.angestellte)
            {
                cmd.CommandText = "INSERT INTO \"Angestellter\" (id, name) VALUES (" + an._id + ", '" + an._name + "')";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Angestellte angelegt.");

            Console.WriteLine("Lege Ärzte an ({0} Elemente)", Master.ärzte.Count);

            foreach (Arzt ar in Master.ärzte)
            {
                cmd.CommandText = "INSERT INTO \"Arzt\" (id, stationsNr) VALUES (" + ar._id + ", " + ar._stationNr + ")";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Ärzte angelegt.");

            Console.WriteLine("Lege Pfleger an ({0} Elemente)", Master.pfleger.Count);

            foreach (Pfleger pfle in Master.pfleger)
            {
                cmd.CommandText = "INSERT INTO \"Pfleger\" (id) VALUES (" + pfle._id + ")";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Pfleger angelegt.");

            Console.WriteLine("Lege Zimmer an ({0} Elemente)", Master.zimmer.Count);

            foreach (Zimmer zi in Master.zimmer)
            {
                cmd.CommandText = "INSERT INTO \"Zimmer\" (zimmerNr, stationsNr) VALUES (" + zi._id + ", " + zi._stationNr + ")";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Zimmer angelegt.");

            Console.WriteLine("Lege PflegerProZimmer an ({0} Elemente)", Master.pflegerProZimmer.Count);

            foreach (PflegerProZimmer ppz in Master.pflegerProZimmer)
            {
                cmd.CommandText = "INSERT INTO \"PflegerProZimmer\" (pflegerID, zimmerNr) VALUES (" + ppz._pflegerId + ", " + ppz._zimmerNr + ")";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("PflegerProZimmer angelegt.");

            Console.WriteLine("Lege Betten an ({0} Elemente)", Master.betten.Count);

            foreach (Bett bett in Master.betten)
            {
                cmd.CommandText = "INSERT INTO \"Bett\" (bettenNr, zimmerNr) VALUES (" + bett._bettenNr + ", " + bett._zimmerNr + ")";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Betten angelegt.");

            Console.WriteLine("Lege Aufenthalte an ({0} Elemente)", Master.aufenthalte.Count);

            for (int i = 0; i < Master.aufenthalte.Count; i++)
            {
                if (i % ((Master.aufenthalte.Count - 1) / 100) == 0)
                    Console.WriteLine("Aufenthalte werden angelegt... (" + ((float)((float)i / ((float)Master.aufenthalte.Count - 1f)) * 100f).ToString("0") + "%)");

                Aufenthalt auf = Master.aufenthalte[i];

                auf.getID();
                cmd.CommandText = "INSERT INTO \"Aufenthalt\" (id, startDate, endDate, krankenkassenNr, bettenNr, behandelnderArzt) VALUES (" + auf._id + ", to_timestamp('" + auf._startDate.ToString("yyyy-MM-dd HH:mm") + "', 'YYYY-MM-DD HH24:MI'), to_timestamp('" + auf._endDate.ToString("yyyy-MM-dd HH:mm") + "', 'YYYY-MM-DD HH24:MI'), " + auf._krankenkassenNr + ", " + auf._bettenNr + ", " + auf._behandelnderArzt + ")";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Aufenthalte angelegt.");

            Console.WriteLine("Lege Diagnosen an ({0} Elemente)", Master.diagnosen.Count);

            foreach (Diagnose dia in Master.diagnosen)
            {
                cmd.CommandText = "INSERT INTO \"Diagnose\" (aufenthaltsID, diagnose) VALUES (" + dia._aufenthaltsID + ", '" + dia._diagnose + "')";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Diagnosen angelegt.");

            Console.WriteLine("Lege Medikamente an ({0} Elemente)", Master.medikamente.Count);

            foreach (Medikament med in Master.medikamente)
            {
                cmd.CommandText = "INSERT INTO \"Medikament\" (name, id) VALUES ('" + med._name + "', " + med._id + ")";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Medikamente angelegt.");

            Console.WriteLine("Lege MedikamenteProAufenthalt an ({0} Elemente)", Master.medsProAufenthalt.Count);

            foreach (MedProAufenthalt mpa in Master.medsProAufenthalt)
            {
                cmd.CommandText = "INSERT INTO \"MedProAufenthalt\" (medID, id) VALUES (" + mpa._medID + ", " + mpa._aufenthaltsID + ")";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("MedProAufenthalte angelegt.");

            Console.WriteLine("Lege Unverträglichkeiten an ({0} Elemente)", Master.unverträglichkeiten.Count);

            foreach (Unverträglichkeit unver in Master.unverträglichkeiten)
            {
                cmd.CommandText = "INSERT INTO \"Unvertraeglichkeiten\" (med1, med2) VALUES (" + unver._med1 + ", " + unver._med2 + ")";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Unverträglichkeiten angelegt.");

            Console.WriteLine("Lege Arbeitslogs an ({0} Elemente)", Master.arbeitslogs.Count);

            for (int i = 0; i < Master.arbeitslogs.Count; i++)
            {
                if (i % ((Master.arbeitslogs.Count - 1) / 100) == 0)
                    Console.WriteLine("Arbeitslogs werden angelegt... (" + ((float)((float)i / ((float)Master.arbeitslogs.Count - 1f)) * 100f).ToString("0") + "%)");

                Arbeitslog al = Master.arbeitslogs[i];

                cmd.CommandText = "INSERT INTO \"Arbeitslog\" (id, startDate, endDate) VALUES (" + al._angestellterId + ", to_timestamp('" + al._startDate.ToString("yyyy-MM-dd HH:mm") + "', 'YYYY-MM-DD HH24:MI'), to_timestamp('" + al._endDate.ToString("yyyy-MM-dd HH:mm") + "', 'YYYY-MM-DD HH24:MI'))";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Arbeitslogs angelegt.");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nAlle Tabellen wurden vollständig angelegt.");
            Console.ForegroundColor = ConsoleColor.White;
        }    
    }
}
