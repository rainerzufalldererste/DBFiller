using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFiller
{
    public static class NameGen
    {
        public static Random rand = new Random();

        public static string getName(params string[][] pieces)
        {
            string ret = "";

            for (int i = 0; i < pieces.Length; i++)
            {
                int n = ((int)(ushort)(rand.NextDouble() * int.MaxValue)) % pieces[i].Length;

                ret += pieces[i][n];
            }

            return ret;
        }

        public static string getNameLoopFirst(string[] piecesStart, string[] piecesEnd)
        {
            string ret = "";

            int num = (int)(((rand.NextDouble() + .5d) * 3d) * ((rand.NextDouble() + .25d) * 2d)) + 2;

            for (int i = 0; i < num - 1; i++)
            {
                int n = ((int)(ushort)(rand.NextDouble() * int.MaxValue)) % piecesStart.Length;

                ret += piecesStart[n];
            }

            int n_ = ((int)(ushort)(rand.NextDouble() * int.MaxValue)) % piecesEnd.Length;

            ret += piecesEnd[n_];

            return ret;
        }

        public static string[] vornamenM = { "Ludwig ", "Berthold ", "Alois ", "Winfried ", "Otto ", "Gregor ", "Thomas ", "Johannes ", "Markus ", "Peter ", "Gerd ", "Tobias ", "Hans ", "Walther ", "Günther ", "Manfred ", "Pierre ", "Juan ", "Klaus ", "Kürbis" };
        public static string[] vornamenW = { "Gudrun ", "Anette ", "Luise ", "Patrice ", "Waltraud ", "Anastasia ", "Julietta ", "Petra ", "Anna ", "Lisa ", "Mona ", "Augusta ", "Hanna ", "Sandra ", "Gundula ", "Claudia ", "Henrietta" };

        public static string[] grünzeug = { "Schnittlauch", "Potato", "Spitzkohl", "Kohl", "Möhre", "Kürbis", "Salbei", "Hopfen", "Gerste", "Sanddorn", "Holunder", "Pfifferling", "Birne", "Rotkraut", "Eiche", "Tannenwald", "Soja" };
        public static string[] tiere = { "Eber", "Hirsch", "Lux", "Katze", "Hund", "Esel", "Pferd", "Hornisse", "Salamander", "Kröte", "Frosch", "Ziege", "Gemse", "Nashorn", "Antilope", "Wespe", "Hummel", "Biene", "Stechmücke" };

        public static string[] medizinS = { "li", "mo", "na", "de", "wo", "mu", "ka", "to", "psi", "cho", "mi", "no", "ri", "ta", "vi", "top", "so", "no", "ro", "mor", "as", "pe", "ni", "ci"};
        public static string[] medizinE = { "rin", "sin", "se", "mer", "ter", "ma", "do", "phorm", "phium", "pirin", "lin" };

        internal static string getDiagnosis()
        {
            return getName(new string[] { "", "", "", "", "", "partielle ", "gemeine ", "Verdacht auf ", "chronische ", "tropische ", "subsonare ", "allgemeine ", "westliche ", "südländische ", "eitrige ", "geschwollene ","gebrochene ", "infektiöse ", "ansteckende ", "vorzeitige-", "resistente-" },
                new string[] { "Vogel-", "Schweine-", "Leber-", "Hirn-", "Herz-", "Magen-", "Darm-", "Schädel-", "Bindehaut-", "Nasennebenhöhlen-", "Schienbein-", "Knochen-", "Knochenmark-", "Blut-", "Schädelbasis-", "Winter-", "Schüttel-", "Rückenmark-", "Nieren-", "Rachen-", "Hirnhaut-", "Rinder-", "Polyviren-", "Tumor-", "", "", "", "", "", "" },
                new string[] { "Grippe", "Pocken", "Entzündung", "Erkältung", "Masern", "Zirrose", "Athrose", "Glukose", "Fruktose", "Laktose", "Spirrose", "Seerose", "Krebserkrankung", "Schädigung", "Verbrennung", "Stauchung", "Seuche", "Angina", "Pest", "Erkrankung", "Erfrierungen", "Schwangerschaft", "Geburt", "Röteln", "Transplantation", "Spende", "Lähmung" });
        }

        internal static Medikament getMed()
        {
            return Master.medikamente[(int)(rand.NextDouble() * int.MaxValue) % Master.medikamente.Count];
        }
    }
}
