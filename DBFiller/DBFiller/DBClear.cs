using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace DBFiller
{
    public static partial class DBZugriff
    {
        public static void dropTable()
        {
            new NpgsqlCommand("drop table \"Patient\" cascade; drop table \"Abteilung\" cascade; drop table \"Angestellter\" cascade; drop table \"Arzt\" cascade; drop table \"Pfleger\" cascade; drop table \"Zimmer\" cascade; drop table \"PflegerProZimmer\" cascade; drop table \"Bett\" cascade; drop table \"Aufenthalt\" cascade; drop table \"Diagnose\" cascade; drop table \"Medikament\" cascade; drop table \"MedProAufenthalt\" cascade; drop table \"Unvertraeglichkeiten\" cascade; drop table \"Arbeitslog\" cascade; ", connection).ExecuteNonQuery();
            Console.WriteLine("Tables dropped.");
        }

        public static void createTable()
        {
            new NpgsqlCommand("CREATE TABLE public.\"Patient\"( \"name\" text NOT NULL, krankenkassenNr integer NOT NULL PRIMARY KEY, \"geschlecht\" text, alter integer); CREATE TABLE public.\"Abteilung\"( stationsNr integer NOT NULL, \"name\" text NOT NULL, CONSTRAINT stationsNr PRIMARY KEY(stationsNr));CREATE TABLE public.\"Angestellter\"( id integer NOT NULL, \"name\" text NOT NULL, CONSTRAINT id PRIMARY KEY(id)); CREATE TABLE public.\"Arzt\"( id integer NOT NULL PRIMARY KEY, stationsNr integer, CONSTRAINT id FOREIGN KEY(id) REFERENCES \"Angestellter\" (id), CONSTRAINT stationsNr FOREIGN KEY(stationsNr) REFERENCES \"Abteilung\" (stationsNr));CREATE TABLE public.\"Pfleger\"( id integer NOT NULL PRIMARY KEY, CONSTRAINT id FOREIGN KEY(id) REFERENCES \"Angestellter\" (id));CREATE TABLE public.\"Zimmer\"( zimmerNr integer NOT NULL, stationsNr integer NOT NULL, CONSTRAINT zimmerNr PRIMARY KEY(zimmerNr), CONSTRAINT stationsNr FOREIGN KEY(stationsNr) REFERENCES \"Abteilung\" (stationsNr));CREATE TABLE public.\"PflegerProZimmer\"( pflegerID integer NOT NULL, zimmerNr integer NOT NULL, CONSTRAINT pflegerID FOREIGN KEY(pflegerID) REFERENCES \"Pfleger\" (id), CONSTRAINT zimmerNr FOREIGN KEY(zimmerNr) REFERENCES \"Zimmer\" (zimmerNr));CREATE TABLE public.\"Bett\"( bettenNr integer NOT NULL, zimmerNr integer NOT NULL, CONSTRAINT bettenNr PRIMARY KEY(bettenNr), CONSTRAINT zimmerNr FOREIGN KEY(zimmerNr) REFERENCES \"Zimmer\" (zimmerNr)); CREATE TABLE public.\"Aufenthalt\"( id integer NOT NULL PRIMARY KEY, startDate timestamp, endDate timestamp, krankenkassenNr integer NOT NULL, bettenNr integer, behandelnderArzt integer NOT NULL, CONSTRAINT krankenkassenNr FOREIGN KEY(krankenkassenNr) REFERENCES \"Patient\" (krankenkassenNr), CONSTRAINT bettenNr FOREIGN KEY(bettenNr) REFERENCES \"Bett\" (bettenNr), CONSTRAINT behandelnderArzt FOREIGN KEY(behandelnderArzt) REFERENCES \"Arzt\" (id)); CREATE TABLE public.\"Diagnose\"( aufenthaltsID integer NOT NULL, \"diagnose\" text NOT NULL, CONSTRAINT aufenthaltsID FOREIGN KEY(aufenthaltsID) REFERENCES \"Aufenthalt\" (id)); CREATE TABLE public.\"Medikament\"( \"name\" text NOT NULL, id integer NOT NULL PRIMARY KEY); CREATE TABLE public.\"MedProAufenthalt\"( medID integer NOT NULL, id integer NOT NULL, CONSTRAINT id FOREIGN KEY(id) REFERENCES \"Aufenthalt\" (id), CONSTRAINT medID FOREIGN KEY(medID) REFERENCES \"Medikament\" (id)); CREATE TABLE public.\"Unvertraeglichkeiten\"( med1 integer NOT NULL, med2 integer NOT NULL, CONSTRAINT med1 FOREIGN KEY(med1) REFERENCES \"Medikament\" (id), CONSTRAINT med2 FOREIGN KEY(med2) REFERENCES \"Medikament\" (id));CREATE TABLE public.\"Arbeitslog\"(id integer NOT NULL,startDate timestamp,endDate timestamp,CONSTRAINT id FOREIGN KEY(id) REFERENCES \"Angestellter\" (id));", connection).ExecuteNonQuery();
            Console.WriteLine("Tables created.");
        }
    }
}
