using System.Collections.Generic;
using System.IO;
using LibrarieModele;

namespace StocareDate
{
    public class StocaDateAngajati : IStocareDataAngajati
    {
        string numeFisier;

        public StocaDateAngajati(string numeFisier)
        {
            this.numeFisier = numeFisier;
            Stream stream = File.Open(numeFisier, FileMode.OpenOrCreate);
            stream.Close();
        }

        public void AddAngajat(Angajat a)
        {
            using (StreamWriter sw = new StreamWriter(numeFisier, true))
                sw.WriteLine(a.ConversieLaSirPentruFisier());
        }

        public List<Angajat> GetAngajati()
        {
            List<Angajat> lista = new List<Angajat>();
            using (StreamReader sr = new StreamReader(numeFisier))
            {
                string linie;
                while ((linie = sr.ReadLine()) != null)
                    lista.Add(new Angajat(linie));
            }
            return lista;
        }

        public Angajat CautaDupaId(int id)
        {
            foreach (Angajat a in GetAngajati())
                if (a.Id == id) return a;
            return null;
        }

        public Angajat CautaDupaNume(string nume)
        {
            foreach (Angajat a in GetAngajati())
                if (a.Nume.ToLower().Contains(nume.ToLower())) return a;
            return null;
        }

        public void ModificaAngajat(Angajat aModificat)
        {
            List<Angajat> lista = GetAngajati();
            for (int i = 0; i < lista.Count; i++)
                if (lista[i].Id == aModificat.Id)
                    lista[i] = aModificat;
            using (StreamWriter sw = new StreamWriter(numeFisier, false))
                foreach (Angajat a in lista)
                    sw.WriteLine(a.ConversieLaSirPentruFisier());
        }

        public void StergeAngajat(int id)
        {
            List<Angajat> lista = GetAngajati();
            lista.RemoveAll(a => a.Id == id);
            using (StreamWriter sw = new StreamWriter(numeFisier, false))
                foreach (Angajat a in lista)
                    sw.WriteLine(a.ConversieLaSirPentruFisier());
        }
    }
}