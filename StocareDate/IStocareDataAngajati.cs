using LibrarieModele;

namespace StocareDate
{
    public interface IStocareDataAngajati
    {
        void AddAngajat(Angajat a);
        List<Angajat> GetAngajati();
        Angajat CautaDupaId(int id);
        Angajat CautaDupaNume(string nume);
        void ModificaAngajat(Angajat aModificat);
        void StergeAngajat(int id);
    }
}
