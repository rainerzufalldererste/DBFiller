using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFiller
{
    internal class Bett
    {
        static int lastID = 0;

        public Patient patient;

        public Bett(Zimmer z)
        {
            zimmer = z;
            _zimmerNr = z._id;
            _bettenNr = lastID++;

            Master.betten.Add(this);
            Program.freieBetten.Add(this);
        }

        internal int _bettenNr;
        internal int _zimmerNr;

        internal Zimmer zimmer;

        public void belegen(Patient p)
        {
            patient = p;

            Program.freieBetten.Remove(this);

            if(zimmer.freieBetten == zimmer.betten.Count)
            {
                zimmer.age = p._alter;
                zimmer.geschlecht = p._geschlecht;

                Program.freieZimmer.Remove(zimmer);
            }

            zimmer.freieBetten--;
        }

        public void freiMachen()
        {
            patient = null;

            zimmer.freieBetten++;

            Program.freieBetten.Add(this);

            if (zimmer.freieBetten == zimmer.betten.Count)
            {
                zimmer.age = -1;
                zimmer.geschlecht = Patient.EGeschlecht.none;

                Program.freieZimmer.Add(this.zimmer);
            }
        }
    }

    internal class Zimmer
    {
        static int lastID = 0;

        internal int _stationNr;
        internal int _id;

        internal int age = -1;
        internal Patient.EGeschlecht geschlecht = Patient.EGeschlecht.none;

        public Zimmer(Abteilung a)
        {
            _id = lastID++;

            abteilung = a;
            _stationNr = a._stationNr;

            Master.zimmer.Add(this);
            Program.freieZimmer.Add(this);
        }

        public List<Bett> betten = new List<Bett>();
        internal Abteilung abteilung;

        internal int freieBetten = 0;
        internal bool zimmerFrei = false;

        public void generateBetten(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Bett b = new Bett(this);
                betten.Add(b);
            }

            freieBetten = count;
            zimmerFrei = true;
        }
    }

    internal class Abteilung
    {
        public List<Arzt> ärzte = null;

        static int lastID = 0;

        public Abteilung(string text)
        {
            _stationNr = lastID++;
            _name = text;

            Master.abteilung.Add(this);
        }

        public int _stationNr;
        public string _name;

        public List<Zimmer> zimmer = new List<Zimmer>();
        public int freieZimmer = 0;

        public void generateZimmer(int count, int betten)
        {
            for (int i = 0; i < count; i++)
            {
                Zimmer z = new Zimmer(this);
                z.generateBetten(betten);
                zimmer.Add(z);
            }

            freieZimmer = count;
        }
    }

    internal class PflegerProZimmer
    {
        public PflegerProZimmer(Pfleger p, Zimmer z)
        {
            pfleger = p;
            zimmer = z;

            _pflegerId = p._id;
            _zimmerNr = z._id;

            Master.pflegerProZimmer.Add(this);
        }

        internal Pfleger pfleger;
        internal Zimmer zimmer;

        internal int _pflegerId, _zimmerNr;
    }

    internal class Angestellter
    {
        protected static int lastID = 0;

        public Angestellter(string name)
        {
            _id = lastID++;
            _name = name;

            Master.angestellte.Add(this);
            Program.inaktiveAngestellte.Add(this);
        }

        public int _id;
        public string _name;

        public bool working = false;

        public Arbeitslog currentArbeitslog;

        public void startArbeit(DateTime startDate)
        {
            if (currentArbeitslog != null)
                throw new Exception("I AM ALREADY WORKING, DUDE!");

            currentArbeitslog = new Arbeitslog(this, startDate, DateTime.Now);
            working = true;

            Program.aktiveAngestellte.Add(this);
            Program.inaktiveAngestellte.Remove(this);
        }

        public void endArbeit(DateTime endDate)
        {
            if (currentArbeitslog == null)
                throw new NullReferenceException();

            currentArbeitslog._endDate = endDate;

            Master.arbeitslogs.Add(currentArbeitslog);

            currentArbeitslog = null;
            working = false;

            Program.aktiveAngestellte.Remove(this);
            Program.inaktiveAngestellte.Add(this);
        }
    }

    internal class Pfleger : Angestellter
    {
        public Pfleger(string name) : base(name)
        {
            Master.pfleger.Add(this);
        }
    }

    internal class Arzt : Angestellter
    {
        public bool istBereitsOberarztIrgendwo = false;
        internal Abteilung station;

        internal int _stationNr;

        public Arzt(string name, Abteilung station) : base(name)
        {
            Master.ärzte.Add(this);
            this.station = station;
            this._stationNr = station._stationNr;
        }
    }

    internal class Arbeitslog
    {
        internal Angestellter angestellter;
        internal int _angestellterId;
        internal DateTime _endDate;
        internal DateTime _startDate;

        /// <summary>
        /// DOES NOT ADD TO MASTER.BLAH!!!
        /// </summary>
        /// <param name="angestellter"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public Arbeitslog(Angestellter angestellter, DateTime startDate, DateTime endDate)
        {
            this.angestellter = angestellter;
            this._startDate = startDate;
            this._endDate = endDate;
            this._angestellterId = angestellter._id;
        }
    }

    internal class Aufenthalt
    {
        static int lastID = 0;

        /// <summary>
        /// DOES NOT ADD TO MASTER.BLAH!!!
        /// </summary>
        /// <param name="patient"></param>
        /// <param name=""></param>
        public Aufenthalt(Patient patient, DateTime startDate, DateTime endDate, params string[] diagnosen)
        {
            _id = lastID++;
            _krankenkassenNr = patient._krankenkassenNr;

            this.patient = patient;

            Master.aufenthalte.Add(this);

            if(diagnosen.Length > 0)
            {
                for (int i = 0; i < diagnosen.Length; i++)
                {
                    addDiagnose(diagnosen[i]);
                }
            }

            // first try: find perfect room

            for (int i = 0; i < Program.freieBetten.Count; i++)
            {
                if(Program.freieBetten[i].zimmer.freieBetten < Program.freieBetten[i].zimmer.betten.Count && Program.freieBetten[i].zimmer.freieBetten > 0)
                {
                    if (Program.freieBetten[i].zimmer.geschlecht == patient._geschlecht)
                    {
                        if (Math.Abs(Program.freieBetten[i].zimmer.age - patient._alter) < 5) // 5 Jahre altersunterschied
                        {
                            this.bett = Program.freieBetten[i];
                            Program.freieBetten[i].belegen(patient);
                            return;
                        }
                    }
                }
            }

            // second try: find a new room

            for (int i = 0; i < Program.freieBetten.Count; i++)
            {
                if (Program.freieBetten[i].zimmer.freieBetten == Program.freieBetten[i].zimmer.betten.Count)
                {
                    this.bett = Program.freieBetten[i];
                    Program.freieBetten[i].belegen(patient);
                    return;
                }
            }


            // third try: find a same gender room

            for (int i = 0; i < Program.freieBetten.Count; i++)
            {
                if (Program.freieBetten[i].zimmer.geschlecht == patient._geschlecht)
                {
                    this.bett = Program.freieBetten[i];
                    Program.freieBetten[i].belegen(patient);
                    return;
                }
            }

            // last try: just find a room

            for (int i = 0; i < Program.freieBetten.Count; i++)
            {
                this.bett = Program.freieBetten[i];
                Program.freieBetten[i].belegen(patient);
                return;
            }

            throw new Exception("bed overflow exception");

        }

        public int _id;
        internal DateTime _endDate;
        internal DateTime _startDate;
        public int _krankenkassenNr;
        public int _bettenNr;

        public Patient patient;
        public Bett bett;
        public List<Diagnose> diagnosen = new List<Diagnose>();
        public List<Medikament> medikamente = new List<Medikament>();

        internal void endAufenthalt(DateTime endDate)
        {
            bett.freiMachen();
            _endDate = endDate;
        }

        internal void addDiagnose(string diagnose)
        {
            Diagnose diag = new Diagnose(diagnose, this);
            diagnosen.Add(diag);
        }

        internal void addMed(Medikament medikament)
        {
            medikamente.Add(medikament);
        }
    }

    internal class Diagnose
    {
        public Aufenthalt aufenthalt;
        public int _aufenthaltsID;
        public string _diagnose;

        public Diagnose(string name, Aufenthalt aufenthalt)
        {
            this.aufenthalt = aufenthalt;
            this._aufenthaltsID = aufenthalt._id;
            this._diagnose = name;

            Master.diagnosen.Add(this);
        }
    }

    internal class Patient
    {
        static int lastID = 10203040;

        public bool currentlyInKrankenhaus = false;
        public List<Aufenthalt> aufenthalte = new List<Aufenthalt>();

        public string _name;
        public int _krankenkassenNr;
        public EGeschlecht _geschlecht;
        public int _alter;

        public Aufenthalt currentAufenthalt;

        public Patient(string name, EGeschlecht geschlecht, int alter)
        {
            this._name = name;
            this._geschlecht = geschlecht;
            this._alter = alter;
            this._krankenkassenNr = lastID++;

            Master.patienten.Add(this);
            Program.inaktivePersonen.Add(this);
        }

        internal enum EGeschlecht
        {
            männlich, weiblich,
            none
        }

        public void addAufenthalt(DateTime startDateTime)
        {
            if (currentAufenthalt != null)
                throw new Exception("Patient is already in Krankenhaus!");

            currentAufenthalt = new Aufenthalt(this, startDateTime, DateTime.Now);

            Program.inaktivePersonen.Remove(this);
            Program.aktivePersonen.Add(this);
        }

        public void endAufenthalt(DateTime endDateTime)
        {
            if (currentAufenthalt == null)
                throw new NullReferenceException();

            currentAufenthalt.endAufenthalt(endDateTime);
            aufenthalte.Add(currentAufenthalt);
            Master.aufenthalte.Add(currentAufenthalt);

            currentAufenthalt.addDiagnose(NameGen.getDiagnosis());
            currentAufenthalt.addMed(NameGen.getMed());

            for (int i = 0; i < 10; i++)
            {
                if (NameGen.rand.NextDouble() < .25)
                {
                    currentAufenthalt.addDiagnose(NameGen.getDiagnosis());
                }
                else if (NameGen.rand.NextDouble() > .75)
                {
                    currentAufenthalt.addMed(NameGen.getMed());
                }
            }

            Program.aktivePersonen.Remove(this);
            Program.inaktivePersonen.Add(this);

            currentAufenthalt = null;
        }
    }

    internal class MedProAufenthalt
    {
        internal Aufenthalt aufenthalt;
        internal Medikament medikament;

        internal int _aufenthaltsID;
        internal int _medID;

        public MedProAufenthalt(Aufenthalt aufenthalt, Medikament medikament)
        {
            this.aufenthalt = aufenthalt;
            this.medikament = medikament;

            this._aufenthaltsID = aufenthalt._id;
            this._medID = medikament._id;

            Master.medsProAufenthalt.Add(this);
        }
    }

    internal class Medikament
    {
        static int lastID = 0;

        public Medikament(string name, params string[] Medikamentenunverträglichkeiten)
        {
            _id = lastID++;
            _name = name;

            Master.medikamente.Add(this);

            if(Medikamentenunverträglichkeiten.Length > 0)
            {
                for (int i = 0; i < Medikamentenunverträglichkeiten.Length; i++)
                {
                    bool found = false;

                    for (int j = 0; j < Master.medikamente.Count; j++)
                    {
                        if(Master.medikamente[j]._name == Medikamentenunverträglichkeiten[i])
                        {
                            addUnverträglichkeit(Master.medikamente[j]);

                            found = true;
                            break;
                        }
                    }
                    
                    if(!found)
                    {
                        Medikament m = new Medikament(Medikamentenunverträglichkeiten[i]);

                        addUnverträglichkeit(m);

                        found = true;
                        break;
                    }
                }
            }
        }

        public int _id;
        public string _name;

        public List<Medikament> unverträglichkeiten = new List<Medikament>();

        internal void addUnverträglichkeit(Medikament medikament)
        {
            unverträglichkeiten.Add(medikament);
            medikament.unverträglichkeiten.Add(this);

            new Unverträglichkeit(this._id, medikament._id);
        }
    }

    internal class Unverträglichkeit
    {
        private int _med1;
        private int _med2;

        public Unverträglichkeit(int _id1, int _id2)
        {
            this._med1 = _id1;
            this._med2 = _id2;

            Master.unverträglichkeiten.Add(this);
        }
    }
}
