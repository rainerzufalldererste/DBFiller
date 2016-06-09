using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFiller
{
    internal static class Master
    {
        internal static List<Bett> betten = new List<Bett>();
        internal static List<Zimmer> zimmer = new List<Zimmer>();
        internal static List<Abteilung> abteilung = new List<Abteilung>();
        internal static List<Pfleger> pfleger = new List<Pfleger>();
        internal static List<PflegerProZimmer> pflegerProZimmer = new List<PflegerProZimmer>();
        internal static List<Arzt> ärzte = new List<Arzt>();
        internal static List<Arbeitslog> arbeitslogs = new List<Arbeitslog>();
        internal static List<Aufenthalt> aufenthalte = new List<Aufenthalt>();
        internal static List<Diagnose> diagnosen = new List<Diagnose>();
        internal static List<Patient> patienten = new List<Patient>();
        internal static List<MedProAufenthalt> medsProAufenthalt = new List<MedProAufenthalt>();
        internal static List<Medikament> medikamente = new List<Medikament>();
        internal static List<Unverträglichkeit> unverträglichkeiten = new List<Unverträglichkeit>();
        internal static List<Angestellter> angestellte = new List<Angestellter>();
    }
}
