using System;
using System.Collections.ObjectModel;
using System.Globalization; // OBAVEZNO DODATI ZA CULTUREINFO
using System.IO;

namespace ProjekatAerodrom
{
    public static class AppData
    {
        public static ObservableCollection<Aerodrom> ListaAerodroma { get; set; } = new ObservableCollection<Aerodrom>();
        public static ObservableCollection<Let> ListaLetova { get; set; } = new ObservableCollection<Let>();

        private static string putanjaAerodromi = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Podaci", "aerodromi.csv");
        private static string putanjaLetovi = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Podaci", "letovi.csv");

        // Zapisivanje iz kolekcije u bazu podataka (csv fajlove)
        public static void SacuvajSve()
        {
            try
            {
                // Aerodromi
                using (StreamWriter sw = new StreamWriter(putanjaAerodromi))
                {
                    foreach (Aerodrom a in ListaAerodroma)
                    {
                        string linija = $"{a.Id},{a.Naziv},{a.Grad},{a.Drzava},{a.Ikonica}";
                        sw.WriteLine(linija);
                    }
                }

                // Letovi
                using (StreamWriter sw = new StreamWriter(putanjaLetovi))
                {
                    foreach (Let l in ListaLetova)
                    {
                        // Umesto ToShortDateString(), eksplicitno forsiramo format sa tačkom na kraju
                        string datumPolaskaStr = l.DatumPolaska.ToString("dd.MM.yyyy.", CultureInfo.InvariantCulture);
                        string datumDolaskaStr = l.DatumDolaska.ToString("dd.MM.yyyy.", CultureInfo.InvariantCulture);

                        string linija = $"{l.BrojLeta},{l.Polazak},{l.Odrediste},{l.Kompanija},{l.VremePolaska},{datumPolaskaStr},{l.VremeDolaska},{datumDolaskaStr},{l.Status},{l.Ikonica}";
                        sw.WriteLine(linija);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Greška prilikom čuvanja podataka: " + ex.Message);
            }
        }

        // Čitamo iz baze podataka (csv fajlova) u kolekciju
        public static void UcitajSve()
        {
            try
            {
                ListaAerodroma.Clear();
                ListaLetova.Clear();

                // Aerodromi
                if (File.Exists(putanjaAerodromi))
                {
                    string[] linije = File.ReadAllLines(putanjaAerodromi);
                    foreach (string linija in linije)
                    {
                        if (string.IsNullOrWhiteSpace(linija)) continue;

                        string[] delovi = linija.Split(',');

                        Aerodrom a = new Aerodrom();
                        a.Id = int.Parse(delovi[0]);
                        a.Naziv = delovi[1];
                        a.Grad = delovi[2];
                        a.Drzava = delovi[3];
                        a.Ikonica = delovi[4];

                        ListaAerodroma.Add(a);
                    }
                }

                // Letovi
                if (File.Exists(putanjaLetovi))
                {
                    string[] linije = File.ReadAllLines(putanjaLetovi);

                    // Definišemo moguće formate 
                    string[] formatiDatuma = { "dd.MM.yyyy.", "dd.MM.yyyy" };

                    foreach (string linija in linije)
                    {
                        if (string.IsNullOrWhiteSpace(linija)) continue;

                        string[] delovi = linija.Split(',');

                        Let l = new Let();
                        l.BrojLeta = delovi[0];
                        l.Polazak = delovi[1];
                        l.Odrediste = delovi[2];
                        l.Kompanija = delovi[3];
                        l.VremePolaska = delovi[4];

                        // Bezbedno parsiranje tačnog formata datuma
                        l.DatumPolaska = DateTime.ParseExact(delovi[5].Trim(), formatiDatuma, CultureInfo.InvariantCulture, DateTimeStyles.None);

                        l.VremeDolaska = delovi[6];

                        // Bezbedno parsiranje tačnog formata datuma
                        l.DatumDolaska = DateTime.ParseExact(delovi[7].Trim(), formatiDatuma, CultureInfo.InvariantCulture, DateTimeStyles.None);

                        l.Status = delovi[8];
                        l.Ikonica = delovi[9];

                        ListaLetova.Add(l);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Greška prilikom učitavanja podataka: " + ex.Message);
            }
        }
    }
}