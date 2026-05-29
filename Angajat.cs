namespace LibrarieModele
{
    public enum FunctieAngajat
    {
        Farmacist = 1,
        Asistent = 2,
        Manager = 3,
        Casier = 4
    }

    public class Angajat
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public FunctieAngajat Functie { get; set; }
        public double Salariu { get; set; }

        public Angajat(int id, string nume, FunctieAngajat functie, double salariu)
        {
            Id = id;
            Nume = nume;
            Functie = functie;
            Salariu = salariu;
        }
        public Angajat(string linieFisier)
        {
            string[] p = linieFisier.Split(',');
            Id = int.Parse(p[0]);
            Nume = p[1];
            Functie = (FunctieAngajat)int.Parse(p[2]);
            Salariu = double.Parse(p[3]);
        }
        public string ConversieLaSirPentruFisier()
        {
            return $"{Id},{Nume},{(int)Functie},{Salariu}";
        }
    }
}