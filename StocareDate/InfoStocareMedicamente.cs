using System.Collections.Generic;
using System.IO;
using LibrarieModele;

namespace StocareDate
{
    public interface IStocareDataMedicamente
    {
        void AddMedicament(Medicament m);
        List<Medicament> GetMedicamente();
        Medicament CautaDupaId(int id);
        Medicament CautaDupaNume(string nume);
        void ModificaMedicament(Medicament mModificat);
        void StergeMedicament(int id);
    }

    public class InfoStocareMedicamente : IStocareDataMedicamente
    {
        string numeFisier;

        public InfoStocareMedicamente(string numeFisier)
        {
            this.numeFisier = numeFisier;
            Stream stream = File.Open(numeFisier, FileMode.OpenOrCreate);
            stream.Close();
        }

        public void AddMedicament(Medicament m)
        {
            using (StreamWriter sw = new StreamWriter(numeFisier, true))
            {
                sw.WriteLine(m.ConversieLaSirPentruFisier());
            }
        }

        public List<Medicament> GetMedicamente()
        {
            List<Medicament> lista = new List<Medicament>();
            using (StreamReader sr = new StreamReader(numeFisier))
            {
                string linie;
                while ((linie = sr.ReadLine()) != null)
                    lista.Add(new Medicament(linie));
            }
            return lista;
        }

        public Medicament CautaDupaId(int id)
        {
            foreach (Medicament m in GetMedicamente())
                if (m.Id == id) return m;
            return null;
        }

        public Medicament CautaDupaNume(string nume)
        {
            foreach (Medicament m in GetMedicamente())
                if (m.Nume.ToLower().Contains(nume.ToLower())) return m;
            return null;
        }

        public void ModificaMedicament(Medicament mModificat)
        {
            List<Medicament> lista = GetMedicamente();
            for (int i = 0; i < lista.Count; i++)
                if (lista[i].Id == mModificat.Id)
                    lista[i] = mModificat;
            using (StreamWriter sw = new StreamWriter(numeFisier, false))
                foreach (Medicament m in lista)
                    sw.WriteLine(m.ConversieLaSirPentruFisier());
        }

        public void StergeMedicament(int id)
        {
            List<Medicament> lista = GetMedicamente();
            lista.RemoveAll(m => m.Id == id);
            using (StreamWriter sw = new StreamWriter(numeFisier, false))
                foreach (Medicament m in lista)
                    sw.WriteLine(m.ConversieLaSirPentruFisier());
        }
    }
}