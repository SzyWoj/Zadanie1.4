using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

// Klasa reprezentująca autora
class Autor
{
    private string imie;

    public Autor(string imie)
    {
        this.imie = imie;
    }

    public string GetImie()
    {
        return imie;
    }
}

// Klasa reprezentująca plik tekstowy
class Plik_tekstowy
{
    private string tekst; // Treść pliku
    private string sciezka; // Ścieżka do pliku na dysku
    private string plik; // Nazwa pliku bez rozszerzenia txt
    private Autor autor; // Autor pliku

    public string Tekst
    {
        get { return tekst; }
        set { tekst = value; }
    }

    public Plik_tekstowy(string tekst, string sciezka, string plik)
    {
        this.tekst = tekst;
        this.sciezka = sciezka;
        this.plik = plik;
    }

    public Plik_tekstowy(string tekst, string sciezka, string plik, Autor autor)
    {
        this.tekst = tekst;
        this.sciezka = sciezka;
        this.plik = plik;
        this.autor = autor;
    }

    public void SetAutor(Autor autor)
    {
        this.autor = autor;
    }

    public void ZapisDoPliku()
    {
        File.WriteAllText($"{sciezka}\\{plik}.txt", $"{tekst}{Environment.NewLine}{autor.GetImie()}{Environment.NewLine}");
        Console.WriteLine("Udany zapis do pliku");
    }

    public void ZapisDoIstniejacegoPliku()
    {
        string tekst = OdczytZPliku().Replace(autor.GetImie(), "") + this.tekst + Environment.NewLine + autor.GetImie() + Environment.NewLine;
        File.WriteAllText($"{sciezka}\\{plik}.txt", tekst);
        Console.WriteLine("Udany zapis do pliku");
    }

    public string OdczytZPliku()
    {
        string tekst = File.ReadAllText($"{sciezka}\\{plik}.txt");
        Console.WriteLine("Udany odczyt z pliku");
        return tekst;
    }

    public void WyczyscPlik()
    {
        File.WriteAllText($"{sciezka}\\{plik}.txt", "");
        Console.WriteLine("Plik jest pusty");
    }

    public override string ToString()
    {
        return $"Plik tekstowy {plik}.txt";
    }
}

// Klasa reprezentująca szyfrowanie pliku
class Szyfruj_plik
{
    private string plik;
    private bool stan;

    public Szyfruj_plik(string plik)
    {
        this.plik = plik;
        stan = false;
    }

    public void SzyfrujPlik(int klucze)
    {
        try
        {
            string tekst = File.ReadAllText(plik);
            StringBuilder sb = new StringBuilder();

            foreach (char c in tekst)
            {
                sb.Append((char)(c + klucze));
            }

            File.WriteAllText(plik, sb.ToString());
            stan = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Błąd podczas szyfrowania pliku: " + ex.Message);
        }
    }

    public void DeszyfrujPlik(int klucze)
    {
        if (!stan)
        {
            Console.WriteLine("Plik nie jest zaszyfrowany.");
            return;
        }

        try
        {
            string tekst = File.ReadAllText(plik);
            StringBuilder sb = new StringBuilder();

            foreach (char c in tekst)
            {
                sb.Append((char)(c - klucze));
            }

            File.WriteAllText(plik, sb.ToString());
            stan = false;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Błąd podczas deszyfrowania pliku: " + ex.Message);
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Autor autor_1 = new Autor("Jan Kowalski");
        Autor autor_2 = new Autor("Basia Nowak");
        List<Autor> autorzy = new List<Autor>();
        autorzy.Add(autor_1);
        autorzy.Add(autor_2);
        autorzy.Add(new Autor("Stefan Niawiadomski"));

        foreach (var autor in autorzy)
            Console.WriteLine(autor.GetImie());

        Plik_tekstowy plik_1 = new Plik_tekstowy("ala lubi C#", @"C:\Users\szymo\OneDrive\Desktop", "text", autorzy[0]);

        Console.WriteLine(plik_1);

        plik_1.ZapisDoPliku();

        plik_1.Tekst = " i ja też";
        plik_1.SetAutor(autorzy[1]);
        plik_1.ZapisDoIstniejacegoPliku();

        plik_1.Tekst = "ala";
        plik_1.ZapisDoIstniejacegoPliku();

        Console.WriteLine(plik_1.OdczytZPliku());

        // Utworzenie obiektu do szyfrowania pliku
        Szyfruj_plik szyfruj_plik = new Szyfruj_plik(@"C:\Users\szymo\OneDrive\Desktop\text.txt");

        // Szyfrowanie pliku z kluczem równym 3
        szyfruj_plik.SzyfrujPlik(3);

        // Deszyfrowanie pliku z kluczem równym 3
        szyfruj_plik.DeszyfrujPlik(3);

        Console.ReadKey();
    }
}
