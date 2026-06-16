using System;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ProjekatAerodrom
{
    public class PutanjaKonverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string putanja && !string.IsNullOrWhiteSpace(putanja))
            {
                // Normalizujemo kose crte da WPF ne pravi problem
                putanja = putanja.Replace("\\", "/");

                // --- 1. SITUACIJA: Slika je naknadno dodata i postoji na disku ---
                string punaPutanjaNaDisku = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, putanja);
                if (File.Exists(punaPutanjaNaDisku))
                {
                    return new BitmapImage(new Uri(punaPutanjaNaDisku, UriKind.Absolute));
                }

                // --- 2. SITUACIJA: Slika je ugrađeni "Resource" unutar .exe fajla ---
                try
                {
                    // Slanje "pack://" prefiksa govori WPF-u da traži sliku unutar svojih resursa
                    string cistaPutanja = putanja.TrimStart('/');
                    Uri resursUri = new Uri($"pack://application:,,,/{cistaPutanja}", UriKind.Absolute);

                    return new BitmapImage(resursUri);
                }
                catch
                {
                    // Ako slika ne postoji ni na disku ni u resursima, vraćamo null da program ne pukne
                    return null;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}