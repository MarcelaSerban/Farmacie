using System;
using System.Collections.Generic;
using LibrarieModele;
using System.Linq;

namespace Farmacie
{
    public class GestiuneFarmacie
    {
        private List<Medicament> lista = new List<Medicament>();

        public void Adauga(Medicament m) => lista.Add(m);

        public void StergeDacaEStocZero(int id)
        {
            lista.RemoveAll(m => m.Id == id && m.Stoc == 0);
        }

        public void AfiseazaTabel()
        {
            Console.WriteLine("\n| ID | NUME            | PRODUCATOR      | PRET      | STOC  | TIP         | FACILITATI");
            Console.WriteLine("----------------------------------------------------------------------------------------");
            foreach (var m in lista)
            {
                Console.WriteLine($"| {m.Id,-2} | {m.Nume,-15} | {m.Producator,-15} | {m.Pret,8} lei | {m.Stoc,5} | {m.Tip,-12}| {m.Facilitati}");
            }
        }

        public void CautaDupaNume(string nume)
        {
            var rezultat = lista.Where(m => m.Nume.ToLower().Contains(nume.ToLower()))
                                .FirstOrDefault();
            if (rezultat != null)
                Console.WriteLine($"\nGasit: {rezultat.Nume}, Producator: {rezultat.Producator}, Pret: {rezultat.Pret}, Stoc: {rezultat.Stoc}, Tip: {rezultat.Tip}, Facilitati: {rezultat.Facilitati}");
            else
                Console.WriteLine("\nMedicamentul nu a fost gasit.");
        }
    }  

    public class GestiuneAngajati
    {
        private List<Angajat> lista = new List<Angajat>();

        public void Adauga(Angajat a) => lista.Add(a);

        public void Sterge(int id)
        {
            lista.RemoveAll(a => a.Id == id);
        }

        public void AfiseazaTabel()
        {
            Console.WriteLine("\n| ID | NUME            | FUNCTIE         | SALARIU      |");
            Console.WriteLine("------------------------------------------------------------");
            foreach (var a in lista)
            {
                Console.WriteLine($"| {a.Id,-2} | {a.Nume,-15} | {a.Functie,-15} | {a.Salariu,10} lei |");
            }
        }

        public void CautaDupaNume(string nume)
        {
            var rezultat = lista.Where(a => a.Nume.ToLower().Contains(nume.ToLower()))
                                .FirstOrDefault();
            if (rezultat != null)
                Console.WriteLine($"\nGasit: {rezultat.Nume}, Functie: {rezultat.Functie}, Salariu: {rezultat.Salariu} lei");
            else
                Console.WriteLine("\nAngajatul nu a fost gasit.");
        }
    }  

    class Program
    {
        static void Main()
        {
            GestiuneFarmacie farmaciaMea = new GestiuneFarmacie();
            GestiuneAngajati angajatiMei = new GestiuneAngajati();
            bool ramanemInProgram = true;

            while (ramanemInProgram)
            {
                Console.WriteLine("\n--- MENIU GESTIUNE FARMACIE ---");
                Console.WriteLine();
                Console.WriteLine("-- MEDICAMENTE --");
                Console.WriteLine("1. Adauga Medicament");
                Console.WriteLine("2. Afiseaza Inventar");
                Console.WriteLine("3. Cauta Medicament");
                Console.WriteLine("4. Sterge Medicament (stoc 0)");
                Console.WriteLine();
                Console.WriteLine("----- ANGAJATI -----");
                Console.WriteLine("5. Adauga Angajat");
                Console.WriteLine("6. Afiseaza Angajati");
                Console.WriteLine("7. Cauta Angajat");
                Console.WriteLine("8. Sterge Angajat");
                Console.WriteLine("9. Iesire");
                Console.Write("Alege o optiune: ");

                string optiune = Console.ReadLine();

                switch (optiune)
                {
                    case "1":
                        Console.WriteLine("\n--- ADAUGARE MEDICAMENT ---");
                        Console.Write("ID: "); int id = int.Parse(Console.ReadLine());
                        Console.Write("Nume: "); string nume = Console.ReadLine();
                        Console.Write("Producator: "); string prod = Console.ReadLine();
                        Console.Write("Pret: "); double pret = double.Parse(Console.ReadLine());
                        Console.Write("Stoc: "); int stoc = int.Parse(Console.ReadLine());
                        Console.WriteLine("Tip medicament: 1-Antibiotic, 2-Analgezic, 3-Antiviral, 4-Antifungic, 5-Vitamine");
                        Console.Write("Alege tip: "); int tip = int.Parse(Console.ReadLine());
                        Console.WriteLine("Facilitati: 1-NecesitaReteta, 2-RefrigerareNecesara, 4-DoarAdulti, 8-Compensat");
                        Console.Write("Alege facilitati: "); int facilitati = int.Parse(Console.ReadLine());
                        farmaciaMea.Adauga(new Medicament(id, nume, prod, pret, stoc,
                            (TipMedicament)tip, (FacilitatiMedicament)facilitati));
                        Console.WriteLine("Medicament adaugat cu succes!");
                        break;

                    case "2":
                        farmaciaMea.AfiseazaTabel();
                        break;

                    case "3":
                        Console.Write("\nIntrodu numele pentru cautare: ");
                        string deCautat = Console.ReadLine();
                        farmaciaMea.CautaDupaNume(deCautat);
                        break;

                    case "4":
                        Console.Write("\nIntrodu ID-ul pentru stergere: ");
                        int idStergere = int.Parse(Console.ReadLine());
                        farmaciaMea.StergeDacaEStocZero(idStergere);
                        Console.WriteLine("Daca medicamentul avea stoc 0, a fost sters.");
                        break;

                    case "5":
                        Console.WriteLine("\n--- ADAUGARE ANGAJAT ---");
                        Console.Write("ID: "); int idA = int.Parse(Console.ReadLine());
                        Console.Write("Nume: "); string numeA = Console.ReadLine();
                        Console.WriteLine("Functie: 1-Farmacist, 2-Asistent, 3-Manager, 4-Casier");
                        Console.Write("Alege functie: "); int functie = int.Parse(Console.ReadLine());
                        Console.Write("Salariu: "); double salariu = double.Parse(Console.ReadLine());
                        angajatiMei.Adauga(new Angajat(idA, numeA, (FunctieAngajat)functie, salariu));
                        Console.WriteLine("Angajat adaugat cu succes!");
                        break;

                    case "6":
                        angajatiMei.AfiseazaTabel();
                        break;

                    case "7":
                        Console.Write("\nIntrodu numele pentru cautare: ");
                        string numeAngajat = Console.ReadLine();
                        angajatiMei.CautaDupaNume(numeAngajat);
                        break;

                    case "8":
                        Console.Write("\nIntrodu ID-ul pentru stergere: ");
                        int idStergeA = int.Parse(Console.ReadLine());
                        angajatiMei.Sterge(idStergeA);
                        Console.WriteLine("Angajat sters cu succes!");
                        break;

                    case "9":
                        ramanemInProgram = false;
                        break;

                    default:
                        Console.WriteLine("Optiune invalida!");
                        break;
                }
            }
            Console.WriteLine("\nProgramul s-a inchis. O zi buna!");
        }
    }
}