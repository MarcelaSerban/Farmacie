using System;
using LibrarieModele;
using StocareDate;

namespace Farmacie
{
    class Program
    {
        static void Main()
        {
            IStocareDataAngajati stocareAngajati = new StocaDateAngajati("angajati.txt");
            IStocareDataMedicamente stocareMedicamente = new InfoStocareMedicamente("medicamente.txt");

            bool ramanemInProgram = true;

            while (ramanemInProgram)
            {
                Console.WriteLine("\n--- MENIU GESTIUNE FARMACIE ---");
                Console.WriteLine();
                Console.WriteLine("-- MEDICAMENTE --");
                Console.WriteLine("1. Adauga Medicament");
                Console.WriteLine("2. Afiseaza Inventar");
                Console.WriteLine("3. Cauta Medicament dupa Nume");
                Console.WriteLine("4. Cauta Medicament dupa ID");
                Console.WriteLine("5. Modifica Medicament");
                Console.WriteLine("6. Sterge Medicament");
                Console.WriteLine();
                Console.WriteLine("----- ANGAJATI -----");
                Console.WriteLine("7.  Adauga Angajat");
                Console.WriteLine("8.  Afiseaza Angajati");
                Console.WriteLine("9.  Cauta Angajat dupa Nume");
                Console.WriteLine("10. Cauta Angajat dupa ID");
                Console.WriteLine("11. Modifica Angajat");
                Console.WriteLine("12. Sterge Angajat");
                Console.WriteLine("13. Iesire");
                Console.Write("\nAlege o optiune: ");

                string optiune = Console.ReadLine()!;

                switch (optiune)
                {
                    case "1":
                        Console.WriteLine("\n--- ADAUGARE MEDICAMENT ---");
                        Console.Write("ID: ");
                        int id = int.Parse(Console.ReadLine()!);
                        Console.Write("Nume: ");
                        string nume = Console.ReadLine()!;
                        Console.Write("Producator: ");
                        string prod = Console.ReadLine()!;
                        Console.Write("Pret: ");
                        double pret = double.Parse(Console.ReadLine()!);
                        Console.Write("Stoc: ");
                        int stoc = int.Parse(Console.ReadLine()!);
                        Console.WriteLine("Tip: 1-Antibiotic, 2-Analgezic, 3-Antiviral, 4-Antifungic, 5-Vitamine");
                        Console.Write("Alege tip: ");
                        int tip = int.Parse(Console.ReadLine()!);
                        Console.WriteLine("Facilitati: 0-None, 1-NecesitaReteta, 2-Refrigerare, 4-DoarAdulti, 8-Compensat");
                        Console.WriteLine("(Se pot combina, ex: 1+8=9 inseamna NecesitaReteta+Compensat)");
                        Console.Write("Alege facilitati: ");
                        int facilitati = int.Parse(Console.ReadLine()!);
                        stocareMedicamente.AddMedicament(new Medicament(id, nume, prod, pret, stoc,
                            (TipMedicament)tip, (FacilitatiMedicament)facilitati));
                        Console.WriteLine("Medicament adaugat si salvat in fisier!");
                        break;

                    case "2":
                        var medicamente = stocareMedicamente.GetMedicamente();
                        if (medicamente.Count == 0)
                        {
                            Console.WriteLine("\nNu exista medicamente in baza de date.");
                            break;
                        }
                        Console.WriteLine("\n| ID | NUME            | PRODUCATOR      | PRET      | STOC  | TIP          | FACILITATI");
                        Console.WriteLine("-------------------------------------------------------------------------------------------");
                        foreach (var m in medicamente)
                            Console.WriteLine($"| {m.Id,-2} | {m.Nume,-15} | {m.Producator,-15} | {m.Pret,8} lei | {m.Stoc,5} | {m.Tip,-12} | {m.Facilitati}");
                        break;

                    case "3":
                        Console.Write("\nIntrodu numele medicamentului: ");
                        string numeMed = Console.ReadLine()!;
                        var medicamentGasit = stocareMedicamente.CautaDupaNume(numeMed);
                        if (medicamentGasit != null)
                            Console.WriteLine($"\nGasit => ID: {medicamentGasit.Id}, Nume: {medicamentGasit.Nume}, " +
                                $"Producator: {medicamentGasit.Producator}, Pret: {medicamentGasit.Pret} lei, " +
                                $"Stoc: {medicamentGasit.Stoc}, Tip: {medicamentGasit.Tip}, Facilitati: {medicamentGasit.Facilitati}");
                        else
                            Console.WriteLine("\nMedicamentul nu a fost gasit.");
                        break;

                    case "4":
                        Console.Write("\nIntrodu ID-ul medicamentului: ");
                        int idCautareMed = int.Parse(Console.ReadLine()!);
                        var medById = stocareMedicamente.CautaDupaId(idCautareMed);
                        if (medById != null)
                            Console.WriteLine($"\nGasit => Nume: {medById.Nume}, Pret: {medById.Pret} lei, Stoc: {medById.Stoc}");
                        else
                            Console.WriteLine("\nMedicamentul nu a fost gasit.");
                        break;

                    case "5":
                        Console.Write("\nIntrodu ID-ul medicamentului de modificat: ");
                        int idModMed = int.Parse(Console.ReadLine()!);
                        var medDeModificat = stocareMedicamente.CautaDupaId(idModMed);
                        if (medDeModificat == null)
                        {
                            Console.WriteLine("Medicamentul nu a fost gasit.");
                            break;
                        }
                        Console.WriteLine($"Medicament curent: {medDeModificat.Nume}, Pret: {medDeModificat.Pret}, Stoc: {medDeModificat.Stoc}");
                        Console.Write("Pret nou: ");
                        medDeModificat.Pret = double.Parse(Console.ReadLine()!);
                        Console.Write("Stoc nou: ");
                        medDeModificat.Stoc = int.Parse(Console.ReadLine()!);
                        stocareMedicamente.ModificaMedicament(medDeModificat);
                        Console.WriteLine("Medicament modificat si salvat!");
                        break;

                    case "6":
                        Console.Write("\nIntrodu ID-ul medicamentului de sters: ");
                        int idStergeMed = int.Parse(Console.ReadLine()!);
                        stocareMedicamente.StergeMedicament(idStergeMed);
                        Console.WriteLine("Medicament sters din fisier!");
                        break;

                    case "7":
                        Console.WriteLine("\n--- ADAUGARE ANGAJAT ---");
                        Console.Write("ID: ");
                        int idA = int.Parse(Console.ReadLine()!);
                        Console.Write("Nume: ");
                        string numeA = Console.ReadLine()!;
                        Console.WriteLine("Functie: 1-Farmacist, 2-Asistent, 3-Manager, 4-Casier");
                        Console.Write("Alege functie: ");
                        int functie = int.Parse(Console.ReadLine()!);
                        Console.Write("Salariu: ");
                        double salariu = double.Parse(Console.ReadLine()!);
                        stocareAngajati.AddAngajat(new Angajat(idA, numeA, (FunctieAngajat)functie, salariu));
                        Console.WriteLine("Angajat adaugat si salvat in fisier!");
                        break;

                    case "8":
                        var angajati = stocareAngajati.GetAngajati();
                        if (angajati.Count == 0)
                        {
                            Console.WriteLine("\nNu exista angajati in baza de date.");
                            break;
                        }
                        Console.WriteLine("\n| ID | NUME            | FUNCTIE         | SALARIU      |");
                        Console.WriteLine("------------------------------------------------------------");
                        foreach (var a in angajati)
                            Console.WriteLine($"| {a.Id,-2} | {a.Nume,-15} | {a.Functie,-15} | {a.Salariu,10} lei |");
                        break;

                    case "9":
                        Console.Write("\nIntrodu numele angajatului: ");
                        string numeAngajat = Console.ReadLine()!;
                        var angajatGasit = stocareAngajati.CautaDupaNume(numeAngajat);
                        if (angajatGasit != null)
                            Console.WriteLine($"\nGasit => ID: {angajatGasit.Id}, Nume: {angajatGasit.Nume}, " +
                                $"Functie: {angajatGasit.Functie}, Salariu: {angajatGasit.Salariu} lei");
                        else
                            Console.WriteLine("\nAngajatul nu a fost gasit.");
                        break;

                    case "10":
                        Console.Write("\nIntrodu ID-ul angajatului: ");
                        int idCautareAng = int.Parse(Console.ReadLine()!);
                        var angById = stocareAngajati.CautaDupaId(idCautareAng);
                        if (angById != null)
                            Console.WriteLine($"\nGasit => Nume: {angById.Nume}, Functie: {angById.Functie}, Salariu: {angById.Salariu} lei");
                        else
                            Console.WriteLine("\nAngajatul nu a fost gasit.");
                        break;

                    case "11":
                        Console.Write("\nIntrodu ID-ul angajatului de modificat: ");
                        int idModAng = int.Parse(Console.ReadLine()!);
                        var angDeModificat = stocareAngajati.CautaDupaId(idModAng);
                        if (angDeModificat == null)
                        {
                            Console.WriteLine("Angajatul nu a fost gasit.");
                            break;
                        }
                        Console.WriteLine($"Angajat curent: {angDeModificat.Nume}, Functie: {angDeModificat.Functie}, Salariu: {angDeModificat.Salariu}");
                        Console.WriteLine("Functie noua: 1-Farmacist, 2-Asistent, 3-Manager, 4-Casier");
                        Console.Write("Alege functie noua: ");
                        angDeModificat.Functie = (FunctieAngajat)int.Parse(Console.ReadLine()!);
                        Console.Write("Salariu nou: ");
                        angDeModificat.Salariu = double.Parse(Console.ReadLine()!);
                        stocareAngajati.ModificaAngajat(angDeModificat);
                        Console.WriteLine("Angajat modificat si salvat!");
                        break;

                    case "12":
                        Console.Write("\nIntrodu ID-ul angajatului de sters: ");
                        int idStergeAng = int.Parse(Console.ReadLine()!);
                        stocareAngajati.StergeAngajat(idStergeAng);
                        Console.WriteLine("Angajat sters din fisier!");
                        break;

                    case "13":
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