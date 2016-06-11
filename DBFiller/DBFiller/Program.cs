using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFiller
{
    internal class Program
    {
        internal static List<Patient> inaktivePersonen = new List<Patient>();
        internal static List<Patient> aktivePersonen = new List<Patient>();
        internal static List<Angestellter> inaktiveAngestellte = new List<Angestellter>();
        internal static List<Angestellter> aktiveAngestellte = new List<Angestellter>();
        internal static List<Zimmer> freieZimmer = new List<Zimmer>();
        internal static List<Bett> freieBetten = new List<Bett>();

        static void Main(string[] args)
        {
            DateTime startDateTime = DateTime.Parse("01-01-2010");
            DateTime endDateTime = DateTime.Parse("06-06-2016");

            generateMeds();
            generateBettZimmerAbteilung();
            generatePersons();
            generatePfleger();
            generatePflegerProZimmer();
            generateÄrzte();

            Console.WriteLine();

            while(startDateTime < endDateTime)
            {
                if (startDateTime.Day % 7 == 1 && startDateTime.Hour == 0)
                    Console.WriteLine("Generating History... " + startDateTime.ToShortDateString() + " (" + (Master.arbeitslogs.Count + Master.aufenthalte.Count) + " Einträge | " + (Master.betten.Count - freieBetten.Count) + " belegte Betten | " + aktiveAngestellte.Count + " aktive Angestellte)");

                if (startDateTime.Hour % 4 < 2)
                {
                    for (int i = 0; i < aktiveAngestellte.Count; i++)
                    {
                        if(NameGen.rand.NextDouble() < .02f)
                        {
                            aktiveAngestellte[i].endArbeit(startDateTime);
                        }
                    }

                    for (int i = 0; i < inaktivePersonen.Count; i++)
                    {
                        if (NameGen.rand.NextDouble() < .01f)
                        {
                            inaktivePersonen[i].addAufenthalt(startDateTime);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < inaktiveAngestellte.Count; i++)
                    {
                        if (NameGen.rand.NextDouble() < .02f)
                        {
                            inaktiveAngestellte[i].startArbeit(startDateTime);
                        }
                    }

                    for (int i = 0; i < aktivePersonen.Count; i++)
                    {
                        if (NameGen.rand.NextDouble() < .02f)
                        {
                            aktivePersonen[i].endAufenthalt(startDateTime);
                        }
                    }
                }

                startDateTime = startDateTime.AddHours(2);
            }

            Console.WriteLine();
            System.Threading.Thread.Sleep(1000);

            DBZugriff.DBVerbidung();
            DBZugriff.dropTable();
            DBZugriff.createTable();
            DBZugriff.LoadData();

            Console.ReadKey();
        }

        private static void generateMeds()
        {
            for (int i = 0; i < 100; i++)
            {
                Medikament m = new Medikament(NameGen.getNameLoopFirst(NameGen.medizinS, NameGen.medizinE));

                while (i > 10 && NameGen.rand.NextDouble() < .2)
                {
                    m.addUnverträglichkeit(Master.medikamente[(int)((Master.medikamente.Count - .5d) * NameGen.rand.NextDouble())]);
                }
            }

            Console.WriteLine("{0} Medikamente und {1} Unverträglichkeiten generiert.", Master.medikamente.Count, Master.unverträglichkeiten.Count);
        }

        private static void generateBettZimmerAbteilung()
        {
            int diag = 0;

            for (int i = 0; i < 15; i++)
            {
                int zimmer = (int)((NameGen.rand.NextDouble() + .25) * 20 + 8);
                int betten = (int)(NameGen.rand.NextDouble() * 3 + 2);

                Abteilung a = new Abteilung("Abteilung " + (i + 1));
                a.generateZimmerAndDiagnosen(zimmer, betten);
                diag += a.Krankheitsnamen.Count;
            }

            Console.WriteLine("{0} Abteilungen, {1} Zimmer, {2} Betten und {3} Diagnosen generiert.", Master.abteilung.Count, Master.zimmer.Count, Master.betten.Count, diag);
        }

        private static void generatePersons()
        {
            int m = 0, w = 0;

            for (int i = 0; i < 2000; i++)
            {
                if (NameGen.rand.NextDouble() > .5d)
                {
                    new Patient(NameGen.getName(NameGen.vornamenM, NameGen.grünzeug), Patient.EGeschlecht.männlich, (int)(NameGen.rand.NextDouble() * 80 + 5));
                    m++;
                }
                else
                {
                    new Patient(NameGen.getName(NameGen.vornamenW, NameGen.grünzeug), Patient.EGeschlecht.weiblich, (int)(NameGen.rand.NextDouble() * 80 + 5));
                    w++;
                }
            }

            Console.WriteLine("{0} Patienten generiert. ({1} Männer und {2} Frauen)", Master.patienten.Count, m, w);
        }

        private static void generatePfleger()
        {
            for (int i = 0; i < 250; i++)
            {
                new Pfleger(NameGen.getName(NameGen.vornamenW, NameGen.tiere));
            }
            
            Console.WriteLine("{0} Krankenschwesterinnen generiert.", Master.pfleger.Count);
        }

        private static void generatePflegerProZimmer() // bad!
        {
            for (int i = 0; i < Master.zimmer.Count * 8; i++)
            {
                int pfleger = (int)(NameGen.rand.NextDouble() * int.MaxValue) % Master.pfleger.Count;

                new PflegerProZimmer(Master.pfleger[pfleger], Master.zimmer[i % Master.zimmer.Count]);
            }

            Console.WriteLine("{0} Krankenbrüder-zu-Zimmer-Assoziationen generiert.", Master.pflegerProZimmer.Count);
        }

        private static void generateÄrzte()
        {
            for (int i = 0; i < 75; i++)
            {
                int station = (int)(NameGen.rand.NextDouble() * int.MaxValue) % Master.abteilung.Count;

                new Arzt(NameGen.getName(NameGen.vornamenM, NameGen.tiere), Master.abteilung[station]);
            }

            Console.WriteLine("{0} Ärzte generiert.", Master.ärzte.Count);
        }
    }
}
