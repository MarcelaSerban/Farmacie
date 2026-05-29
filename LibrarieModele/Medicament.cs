using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LibrarieModele
{
    public enum TipMedicament
    {
        Antibiotic = 1,
        Analgezic = 2,
        Antiviral = 3,
        Antifungic = 4,
        Vitamine = 5
    }

    [Flags]
    public enum FacilitatiMedicament
    {
        None = 0,
        NecesitaReteta = 1,
        RefrigerareNecesara = 2,
        DoarAdulti = 4,
        Compensat = 8
    }

 
    public class Medicament : INotifyPropertyChanged, IDataErrorInfo
    {
       
        private int _id;
        private string _nume;
        private string _producator;
        private double _pret;
        private int _stoc;
        private TipMedicament _tip;
        private FacilitatiMedicament _facilitati;

      
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

        public string Producator
        {
            get => _producator;
            set { _producator = value; OnPropertyChanged(); }
        }

        public double Pret
        {
            get => _pret;
            set { _pret = value; OnPropertyChanged(); }
        }

        public int Stoc
        {
            get => _stoc;
            set { _stoc = value; OnPropertyChanged(); }
        }

        public TipMedicament Tip
        {
            get => _tip;
            set { _tip = value; OnPropertyChanged(); }
        }

        public FacilitatiMedicament Facilitati
        {
            get => _facilitati;
            set { _facilitati = value; OnPropertyChanged(); }
        }

        public Medicament() { }

        public Medicament(int id, string nume, string producator, double pret, int stoc,
                          TipMedicament tip, FacilitatiMedicament facilitati)
        {
            Id = id;
            Nume = nume;
            Producator = producator;
            Pret = pret;
            Stoc = stoc;
            Tip = tip;
            Facilitati = facilitati;
        }

        public Medicament(string linieFisier)
        {
            string[] p = linieFisier.Split(',');
            Id = int.Parse(p[0]);
            Nume = p[1];
            Producator = p[2];
            Pret = double.Parse(p[3]);
            Stoc = int.Parse(p[4]);
            Tip = (TipMedicament)int.Parse(p[5]);
            Facilitati = (FacilitatiMedicament)int.Parse(p[6]);
        }

        public string ConversieLaSirPentruFisier()
        {
            return $"{Id},{Nume},{Producator},{Pret},{Stoc},{(int)Tip},{(int)Facilitati}";
        }

        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(Nume):
                        if (string.IsNullOrWhiteSpace(Nume))
                            return "Numele este obligatoriu!";
                        if (Nume.Length < 2)
                            return "Numele trebuie sa aiba minim 2 caractere!";
                        break;

                    case nameof(Producator):
                        if (string.IsNullOrWhiteSpace(Producator))
                            return "Producatorul este obligatoriu!";
                        break;

                    case nameof(Pret):
                        if (Pret <= 0)
                            return "Pretul trebuie sa fie mai mare decat 0!";
                        break;

                    case nameof(Stoc):
                        if (Stoc < 0)
                            return "Stocul nu poate fi negativ!";
                        break;
                }
                return string.Empty;
            }
        }
 
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
