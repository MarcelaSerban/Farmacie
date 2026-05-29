using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LibrarieModele
{
    public enum FunctieAngajat
    {
        Farmacist = 1,
        Asistent = 2,
        Manager = 3,
        Casier = 4
    }

 
    public class Angajat : INotifyPropertyChanged
    {
     
        private int _id;
        private string _nume;
        private FunctieAngajat _functie;
        private double _salariu;

       
        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public string Nume
        {
            get => _nume;
            set { _nume = value; OnPropertyChanged(); }
        }

        public FunctieAngajat Functie
        {
            get => _functie;
            set { _functie = value; OnPropertyChanged(); }
        }

        public double Salariu
        {
            get => _salariu;
            set { _salariu = value; OnPropertyChanged(); }
        }

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

       
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
