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
        internal static NpgsqlConnection connection;
        const int teiler = 1000;

        public static void DBVerbidung()
        {
            connection = new NpgsqlConnection(Master.connection);
            connection.Open();

            DBSpammer.spamDB(10);
        }

        public static void LoadData()
        {
            Console.WriteLine();
            string s = "";

            //NpgsqlCommand cmd = new NpgsqlCommand();

            //cmd.Connection = connection;

            Console.WriteLine("Lege Patienten an ({0} Elemente)", Master.patienten.Count);

            foreach (Patient pat in Master.patienten)
            {
                s += "INSERT INTO Patient (name, krankenkassenNr, geschlecht, alter) VALUES ('" + pat._name + "', " + pat._krankenkassenNr + ", '" + pat._geschlecht + "', " + pat._alter + "); ";
            }

            DBSpammer.setToQueue(s);
            s = "";

            Console.WriteLine("Patienten angelegt.");

            Console.WriteLine("Lege Abteilungen an ({0} Elemente)", Master.abteilungen.Count);

            foreach (Abteilung ab in Master.abteilungen)
            {
                s += "INSERT INTO Abteilung (stationsNr, name) VALUES (" + ab._stationNr + ", '" + ab._name + "'); ";
            }

            DBSpammer.setToQueue(s);
            s = "";

            Console.WriteLine("Abteilungen angelegt.");

            Console.WriteLine("Lege Angestellte an ({0} Elemente)", Master.angestellte.Count);

            foreach (Angestellter an in Master.angestellte)
            {
                s += "INSERT INTO Angestellter (id, name) VALUES (" + an._id + ", '" + an._name + "'); ";
            }

            DBSpammer.setToQueue(s);
            s = "";

            Console.WriteLine("Angestellte angelegt.");

            Console.WriteLine("Lege Ärzte an ({0} Elemente)", Master.ärzte.Count);

            foreach (Arzt ar in Master.ärzte)
            {
                s += "INSERT INTO Arzt (id, stationsNr) VALUES (" + ar._id + ", " + ar._stationNr + "); ";
            }

            DBSpammer.setToQueue(s);
            s = "";

            Console.WriteLine("Ärzte angelegt.");

            Console.WriteLine("Lege Pfleger an ({0} Elemente)", Master.pfleger.Count);

            foreach (Pfleger pfle in Master.pfleger)
            {
                s += "INSERT INTO Pfleger (id) VALUES (" + pfle._id + "); ";
            }

            DBSpammer.setToQueue(s);
            s = "";

            Console.WriteLine("Pfleger angelegt.");

            Console.WriteLine("Lege Zimmer an ({0} Elemente)", Master.zimmer.Count);

            foreach (Zimmer zi in Master.zimmer)
            {
                s += "INSERT INTO Zimmer (zimmerNr, stationsNr) VALUES (" + zi._id + ", " + zi._stationNr + "); ";
            }

            DBSpammer.setToQueue(s);
            s = "";

            Console.WriteLine("Zimmer angelegt.");

            Console.WriteLine("Lege PflegerProZimmer an ({0} Elemente)", Master.pflegerProZimmer.Count);

            foreach (PflegerProZimmer ppz in Master.pflegerProZimmer)
            {
                s += "INSERT INTO PflegerProZimmer (pflegerID, zimmerNr) VALUES (" + ppz._pflegerId + ", " + ppz._zimmerNr + "); ";
            }

            DBSpammer.setToQueue(s);
            s = "";

            Console.WriteLine("PflegerProZimmer angelegt.");

            Console.WriteLine("Lege Betten an ({0} Elemente)", Master.betten.Count);

            foreach (Bett bett in Master.betten)
            {
                s += "INSERT INTO Bett (bettenNr, zimmerNr) VALUES (" + bett._bettenNr + ", " + bett._zimmerNr + "); ";
            }

            DBSpammer.setToQueue(s);
            s = "";

            Console.WriteLine("Betten angelegt.");

            Console.WriteLine("Lege Aufenthalte an ({0} Elemente)", Master.aufenthalte.Count);

            for (int i = 0; i < Master.aufenthalte.Count; i++)
            {
                if (i % ((Master.aufenthalte.Count - 1) / teiler) == 0)
                {
                    Console.WriteLine("Aufenthalte werden angelegt... (" + ((float)((float)i / ((float)Master.aufenthalte.Count - 1f)) * 100f).ToString("0.0") + "%)");

                    DBSpammer.setToQueue(s);
                    s = "";
                }

                Aufenthalt auf = Master.aufenthalte[i];
                
                s += "INSERT INTO Aufenthalt (id, startDate, endDate, krankenkassenNr, bettenNr, behandelnderArzt) VALUES (" + auf._id + ", to_timestamp('" + auf._startDate.ToString("yyyy-MM-dd HH:mm") + "', 'YYYY-MM-DD HH24:MI'), to_timestamp('" + auf._endDate.ToString("yyyy-MM-dd HH:mm") + "', 'YYYY-MM-DD HH24:MI'), " + auf._krankenkassenNr + ", " + auf._bettenNr + ", " + auf._behandelnderArzt + "); ";
            }

            if (s != "")
            {
                DBSpammer.setToQueue(s);
                s = "";
            }

            Console.WriteLine("Aufenthalte angelegt.");

            Console.WriteLine("Lege Diagnosen an ({0} Elemente)", Master.diagnosen.Count);

            for (int i = 0; i < Master.diagnosen.Count; i++)
            {
                if (i % ((Master.diagnosen.Count - 1) / teiler) == 0)
                {
                    Console.WriteLine("Diagnosen werden angelegt... (" + ((float)((float)i / ((float)Master.diagnosen.Count - 1f)) * 100f).ToString("0.0") + "%)");

                    DBSpammer.setToQueue(s);
                    s = "";
                }

                Diagnose dia = Master.diagnosen[i];
                dia._aufenthaltsID = dia.aufenthalt._id;

                s += "INSERT INTO Diagnose (aufenthaltsID, diagnose) VALUES (" + dia._aufenthaltsID + ", '" + dia._diagnose + "'); ";
            }

            if (s != "")
            {
                DBSpammer.setToQueue(s);
                s = "";
            }

            Console.WriteLine("Diagnosen angelegt.");

            Console.WriteLine("Lege Medikamente an ({0} Elemente)", Master.medikamente.Count);

            foreach (Medikament med in Master.medikamente)
            {
                s += "INSERT INTO Medikament (name, id) VALUES ('" + med._name + "', " + med._id + "); ";
            }

            DBSpammer.setToQueue(s);
            s = "";

            Console.WriteLine("Medikamente angelegt.");

            Console.WriteLine("Lege MedikamenteProAufenthalt an ({0} Elemente)", Master.medsProAufenthalt.Count);

            for (int i = 0; i < Master.medsProAufenthalt.Count; i++)
            {
                if (i % ((Master.medsProAufenthalt.Count - 1) / teiler) == 0)
                {
                    Console.WriteLine("MedikamenteProAufenthalt werden angelegt... (" + ((float)((float)i / ((float)Master.medsProAufenthalt.Count - 1f)) * 100f).ToString("0.0") + "%)");

                    DBSpammer.setToQueue(s);
                    s = "";
                }

                MedProAufenthalt mpa = Master.medsProAufenthalt[i];

                s += "INSERT INTO MedProAufenthalt (medID, id) VALUES (" + mpa._medID + ", " + mpa._aufenthaltsID + "); ";
            }

            if (s != "")
            {
                DBSpammer.setToQueue(s);
                s = "";
            }

            Console.WriteLine("MedProAufenthalte angelegt.");

            Console.WriteLine("Lege Unverträglichkeiten an ({0} Elemente)", Master.unverträglichkeiten.Count);

            foreach (Unverträglichkeit unver in Master.unverträglichkeiten)
            {
                s += "INSERT INTO Unvertraeglichkeiten (med1, med2) VALUES (" + unver._med1 + ", " + unver._med2 + "); ";
            }

            DBSpammer.setToQueue(s);
            s = "";

            Console.WriteLine("Unverträglichkeiten angelegt.");

            Console.WriteLine("Lege Arbeitslogs an ({0} Elemente)", Master.arbeitslogs.Count);

            for (int i = 0; i < Master.arbeitslogs.Count; i++)
            {
                if (i % ((Master.arbeitslogs.Count - 1) / teiler) == 0)
                {
                    Console.WriteLine("Arbeitslogs werden angelegt... (" + ((float)((float)i / ((float)Master.arbeitslogs.Count - 1f)) * 100f).ToString("0.0") + "%)");

                    DBSpammer.setToQueue(s);
                    s = "";
                }

                Arbeitslog al = Master.arbeitslogs[i];

                s += "INSERT INTO Arbeitslog (id, startDate, endDate) VALUES (" + al._angestellterId + ", to_timestamp('" + al._startDate.ToString("yyyy-MM-dd HH:mm") + "', 'YYYY-MM-DD HH24:MI'), to_timestamp('" + al._endDate.ToString("yyyy-MM-dd HH:mm") + "', 'YYYY-MM-DD HH24:MI'));";
            }

            if (s != "")
            {
                DBSpammer.setToQueue(s);
                s = "";
            }

            Console.WriteLine("Arbeitslogs angelegt.");


            DBSpammer.setNoEntriesLeft();

            DBSpammer.waitForFinished();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nAlle Tabellen wurden vollständig angelegt.");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
