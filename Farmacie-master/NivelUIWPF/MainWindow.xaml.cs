using LibrarieModele;
using StocareDate;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NivelUIWPF
{
    public class MedicamentViewModel : INotifyPropertyChanged
    {
        private readonly IStocareDataMedicamente _stocare;
        private Medicament _medicamentCurent;

        public MedicamentViewModel(IStocareDataMedicamente stocare)
        {
            _stocare = stocare;
        }

        public Medicament MedicamentCurent
        {
            get => _medicamentCurent;
            set { _medicamentCurent = value; OnPropertyChanged(); }
        }

        public int GenereazaIdNou()
        {
            var lista = _stocare.GetMedicamente();
            return lista.Count > 0 ? lista.Max(m => m.Id) + 1 : 1;
        }

        public void Adauga(Medicament m) => _stocare.AddMedicament(m);
        public void Modifica(Medicament m) => _stocare.ModificaMedicament(m);
        public void Sterge(int id) => _stocare.StergeMedicament(id);
        public List<Medicament> GetToate() => _stocare.GetMedicamente();
        public List<Medicament> Cauta(string nume) =>
            _stocare.GetMedicamente()
                    .Where(m => m.Nume.ToLower().Contains(nume.ToLower()))
                    .ToList();

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    public class AngajatViewModel : INotifyPropertyChanged
    {
        private readonly IStocareDataAngajati _stocare;
        private Angajat _angajatCurent;

        public AngajatViewModel(IStocareDataAngajati stocare)
        {
            _stocare = stocare;
        }

        public Angajat AngajatCurent
        {
            get => _angajatCurent;
            set { _angajatCurent = value; OnPropertyChanged(); }
        }

        public int GenereazaIdNou()
        {
            var lista = _stocare.GetAngajati();
            return lista.Count > 0 ? lista.Max(a => a.Id) + 1 : 101;
        }

        public void Adauga(Angajat a) => _stocare.AddAngajat(a);
        public void Modifica(Angajat a) => _stocare.ModificaAngajat(a);
        public void Sterge(int id) => _stocare.StergeAngajat(id);
        public List<Angajat> GetToti() => _stocare.GetAngajati();
        public List<Angajat> Cauta(string nume) =>
            _stocare.GetAngajati()
                    .Where(a => a.Nume.ToLower().Contains(nume.ToLower()))
                    .ToList();

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const int PRET_MINIM = 0;
        private const int STOC_MINIM = 0;
        private const int SALARIU_MINIM = 1000;
        private const int SALARIU_MAXIM = 50000;

        private readonly MedicamentViewModel vmMed;
        private readonly AngajatViewModel vmAng;

        public Medicament MedicamentCurent
        {
            get => vmMed.MedicamentCurent;
            set { vmMed.MedicamentCurent = value; OnPropertyChanged(); }
        }

        public Angajat AngajatCurent
        {
            get => vmAng.AngajatCurent;
            set { vmAng.AngajatCurent = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private FacilitatiMedicament facilitatiSelectate = FacilitatiMedicament.None;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            vmMed = new MedicamentViewModel(new InfoStocareMedicamente("medicamente.txt"));
            vmAng = new AngajatViewModel(new StocaDateAngajati("angajati.txt"));
            AfiseazaMedicamente();
        }

        private void AscundePaneluri()
        {
            panelMedAdauga.Visibility = Visibility.Collapsed;
            panelMedCauta.Visibility = Visibility.Collapsed;
            panelAngAdauga.Visibility = Visibility.Collapsed;
            panelAngCauta.Visibility = Visibility.Collapsed;
        }

        private void btnMeniuMedAdauga_Click(object sender, RoutedEventArgs e)
        { AscundePaneluri(); panelMedAdauga.Visibility = Visibility.Visible; AfiseazaMedicamente(); }

        private void btnMeniuMedCauta_Click(object sender, RoutedEventArgs e)
        { AscundePaneluri(); panelMedCauta.Visibility = Visibility.Visible; }

        private void btnMeniuAngAdauga_Click(object sender, RoutedEventArgs e)
        { AscundePaneluri(); panelAngAdauga.Visibility = Visibility.Visible; AfiseazaAngajati(); }

        private void btnMeniuAngCauta_Click(object sender, RoutedEventArgs e)
        { AscundePaneluri(); panelAngCauta.Visibility = Visibility.Visible; }

        private void AfiseazaMedicamente()
        {
            var lista = vmMed.GetToate();
            lblNrMedicamente.Content = $"Numar medicamente: {lista.Count}";
            dgMedicamente.ItemsSource = null;
            dgMedicamente.ItemsSource = lista;
        }

        private void btnMedSalveaza_Click(object sender, RoutedEventArgs e)
        {
            if (!ValideazaMedicament(out string nume, out string prod, out double pret, out int stoc)) return;
            int idNou = vmMed.GenereazaIdNou();
            vmMed.Adauga(new Medicament(idNou, nume, prod, pret, stoc, GetTipSelectat(), facilitatiSelectate));
            AfiseazaMedicamente(); ResetMed();
        }

        private void dgMedicamente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MedicamentCurent = dgMedicamente.SelectedItem as Medicament;
            if (MedicamentCurent == null) return;
            facilitatiSelectate = MedicamentCurent.Facilitati;
            cbNecesitaReteta.IsChecked = facilitatiSelectate.HasFlag(FacilitatiMedicament.NecesitaReteta);
            cbRefrigerare.IsChecked = facilitatiSelectate.HasFlag(FacilitatiMedicament.RefrigerareNecesara);
            cbDoarAdulti.IsChecked = facilitatiSelectate.HasFlag(FacilitatiMedicament.DoarAdulti);
            cbCompensat.IsChecked = facilitatiSelectate.HasFlag(FacilitatiMedicament.Compensat);
            btnMedActualizeaza.IsEnabled = true;
            btnMedSterge.IsEnabled = true;
            tbWarnMed.Visibility = Visibility.Visible;
        }

        private void btnMedActualizeaza_Click(object sender, RoutedEventArgs e)
        {
            if (MedicamentCurent == null) return;
            if (!ValideazaMedicament(out string nume, out string prod, out double pret, out int stoc)) return;
            MedicamentCurent.Facilitati = facilitatiSelectate;
            vmMed.Modifica(MedicamentCurent);
            AfiseazaMedicamente(); ResetMed();
        }

        private void btnMedSterge_Click(object sender, RoutedEventArgs e)
        {
            if (MedicamentCurent == null) return;
            if (MessageBox.Show($"Stergi medicamentul '{MedicamentCurent.Nume}'?", "Confirmare",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            { vmMed.Sterge(MedicamentCurent.Id); AfiseazaMedicamente(); ResetMed(); }
        }

        private void btnCautaMed_Click(object sender, RoutedEventArgs e)
        {
            var lista = vmMed.Cauta(txtCautareMed.Text.Trim());
            lblNrMedGasiti.Content = $"Medicamente gasite: {lista.Count}";
            dgMedGasiti.ItemsSource = lista;
        }

        private void btnMedReseteaza_Click(object sender, RoutedEventArgs e) => ResetMed();

        private void ResetMed()
        {
            dgMedicamente.SelectedItem = null; MedicamentCurent = null;
            txtMedNume.Clear(); txtMedProd.Clear(); txtMedPret.Clear(); txtMedStoc.Clear();
            rbAntibiotic.IsChecked = true;
            cbNecesitaReteta.IsChecked = cbRefrigerare.IsChecked = cbDoarAdulti.IsChecked = cbCompensat.IsChecked = false;
            facilitatiSelectate = FacilitatiMedicament.None;
            btnMedActualizeaza.IsEnabled = btnMedSterge.IsEnabled = false;
            tbWarnMed.Visibility = Visibility.Collapsed;
            ResetEroriMed();
        }

        private bool ValideazaMedicament(out string nume, out string prod, out double pret, out int stoc)
        {
            ResetEroriMed(); nume = ""; prod = ""; pret = 0; stoc = 0; bool ok = true;
            nume = txtMedNume.Text.Trim();
            if (string.IsNullOrEmpty(nume)) { AfiseazaEroare(txtMedNume, tbErrMedNume, "Numele este obligatoriu!"); ok = false; }
            prod = txtMedProd.Text.Trim();
            if (string.IsNullOrEmpty(prod)) { AfiseazaEroare(txtMedProd, tbErrMedProd, "Producatorul este obligatoriu!"); ok = false; }
            if (!double.TryParse(txtMedPret.Text.Trim(), out pret) || pret < PRET_MINIM) { AfiseazaEroare(txtMedPret, tbErrMedPret, $"Pretul invalid!"); ok = false; }
            if (!int.TryParse(txtMedStoc.Text.Trim(), out stoc) || stoc < STOC_MINIM) { AfiseazaEroare(txtMedStoc, tbErrMedStoc, $"Stocul invalid!"); ok = false; }
            return ok;
        }

        private void ResetEroriMed()
        {
            AscundeEroare(txtMedNume, tbErrMedNume); AscundeEroare(txtMedProd, tbErrMedProd);
            AscundeEroare(txtMedPret, tbErrMedPret); AscundeEroare(txtMedStoc, tbErrMedStoc);
        }

        private TipMedicament GetTipSelectat()
        {
            if (rbAnalgezic.IsChecked == true) return TipMedicament.Analgezic;
            if (rbAntiviral.IsChecked == true) return TipMedicament.Antiviral;
            if (rbAntifungic.IsChecked == true) return TipMedicament.Antifungic;
            if (rbVitamine.IsChecked == true) return TipMedicament.Vitamine;
            return TipMedicament.Antibiotic;
        }

        private void Facilitati_Changed(object sender, RoutedEventArgs e)
        {
            facilitatiSelectate = FacilitatiMedicament.None;
            if (cbNecesitaReteta.IsChecked == true) facilitatiSelectate |= FacilitatiMedicament.NecesitaReteta;
            if (cbRefrigerare.IsChecked == true) facilitatiSelectate |= FacilitatiMedicament.RefrigerareNecesara;
            if (cbDoarAdulti.IsChecked == true) facilitatiSelectate |= FacilitatiMedicament.DoarAdulti;
            if (cbCompensat.IsChecked == true) facilitatiSelectate |= FacilitatiMedicament.Compensat;
        }

        private void AfiseazaAngajati()
        {
            var lista = vmAng.GetToti();
            lblNrAngajati.Content = $"Numar angajati: {lista.Count}";
            dgAngajati.ItemsSource = null;
            dgAngajati.ItemsSource = lista;
        }

        private void btnAngSalveaza_Click(object sender, RoutedEventArgs e)
        {
            if (!ValideazaAngajat(out string nume, out double salariu)) return;
            int idNou = vmAng.GenereazaIdNou();
            vmAng.Adauga(new Angajat(idNou, nume, GetFunctieSelectata(), salariu));
            AfiseazaAngajati(); ResetAng();
        }

        private void dgAngajati_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AngajatCurent = dgAngajati.SelectedItem as Angajat;
            if (AngajatCurent == null) return;
            btnAngActualizeaza.IsEnabled = true;
            btnAngSterge.IsEnabled = true;
            tbWarnAng.Visibility = Visibility.Visible;
        }

        private void btnAngActualizeaza_Click(object sender, RoutedEventArgs e)
        {
            if (AngajatCurent == null) return;
            if (!ValideazaAngajat(out string nume, out double salariu)) return;
            vmAng.Modifica(AngajatCurent);
            AfiseazaAngajati(); ResetAng();
        }

        private void btnAngSterge_Click(object sender, RoutedEventArgs e)
        {
            if (AngajatCurent == null) return;
            if (MessageBox.Show($"Stergi angajatul '{AngajatCurent.Nume}'?", "Confirmare",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            { vmAng.Sterge(AngajatCurent.Id); AfiseazaAngajati(); ResetAng(); }
        }

        private void btnCautaAng_Click(object sender, RoutedEventArgs e)
        {
            var lista = vmAng.Cauta(txtCautareAng.Text.Trim());
            lblNrAngGasiti.Content = $"Angajati gasiti: {lista.Count}";
            dgAngGasiti.ItemsSource = lista;
        }

        private void btnAngReseteaza_Click(object sender, RoutedEventArgs e) => ResetAng();

        private void ResetAng()
        {
            dgAngajati.SelectedItem = null; AngajatCurent = null;
            txtAngNume.Clear(); txtAngSalariu.Clear();
            rbFarmacist.IsChecked = true;
            btnAngActualizeaza.IsEnabled = btnAngSterge.IsEnabled = false;
            tbWarnAng.Visibility = Visibility.Collapsed;
            ResetEroriAng();
        }

        private bool ValideazaAngajat(out string nume, out double salariu)
        {
            ResetEroriAng(); nume = ""; salariu = 0; bool ok = true;
            nume = txtAngNume.Text.Trim();
            if (string.IsNullOrEmpty(nume)) { AfiseazaEroare(txtAngNume, tbErrAngNume, "Numele este obligatoriu!"); ok = false; }
            if (!double.TryParse(txtAngSalariu.Text.Trim(), out salariu) || salariu < SALARIU_MINIM || salariu > SALARIU_MAXIM)
            { AfiseazaEroare(txtAngSalariu, tbErrAngSalariu, $"Salariu invalid ({SALARIU_MINIM}-{SALARIU_MAXIM} lei)!"); ok = false; }
            return ok;
        }

        private void ResetEroriAng()
        {
            AscundeEroare(txtAngNume, tbErrAngNume);
            AscundeEroare(txtAngSalariu, tbErrAngSalariu);
        }

        private FunctieAngajat GetFunctieSelectata()
        {
            if (rbAsistent.IsChecked == true) return FunctieAngajat.Asistent;
            if (rbManager.IsChecked == true) return FunctieAngajat.Manager;
            if (rbCasier.IsChecked == true) return FunctieAngajat.Casier;
            return FunctieAngajat.Farmacist;
        }

        private void AfiseazaEroare(TextBox tb, TextBlock tbErr, string mesaj)
        {
            tb.BorderBrush = Brushes.Red;
            tb.Background = new SolidColorBrush(Color.FromRgb(255, 230, 230));
            tbErr.Text = mesaj; tbErr.Visibility = Visibility.Visible;
            tb.Focus();
        }

        private void AscundeEroare(TextBox tb, TextBlock tbErr)
        {
            tb.ClearValue(BorderBrushProperty); tb.ClearValue(BackgroundProperty);
            tbErr.Text = string.Empty; tbErr.Visibility = Visibility.Collapsed;
        }
    }
}