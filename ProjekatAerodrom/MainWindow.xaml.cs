using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;

namespace ProjekatAerodrom
{
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AppData.UcitajSve();
            DGAerodromi.ItemsSource = AppData.ListaAerodroma;
        }

        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Izmeni_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Obrisi_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExportCSV_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DGAerodromi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGAerodromi.SelectedItem is Aerodrom selektovaniAerodrom)
            {
                
                string nazivAerodroma = selektovaniAerodrom.Naziv;

                var filtriraniLetovi = AppData.ListaLetova
                    .Where(let => let.AerodromPolaska == nazivAerodroma || let.AerodromOdredista == nazivAerodroma)
                    .ToList();

              
                DGSelektovaniLetovi.ItemsSource = filtriraniLetovi;
            }
            else
            {
                DGSelektovaniLetovi.ItemsSource = null;
            }
        }

        private void Dodaj_Aerodrom(object sender, RoutedEventArgs e)
        {
            AerodromProzor prozor = new AerodromProzor();
            prozor.Owner = this;
            prozor.ShowDialog();
        }

        private void Izmeni_Aerodrom(object sender, RoutedEventArgs e)
        {
            if (DGAerodromi.SelectedItem is Aerodrom selektovaniAerodrom)
            {
                AerodromProzor prozor = new AerodromProzor(selektovaniAerodrom);
                prozor.Owner = this;
                prozor.ShowDialog();
            }
            else {
                MessageBox.Show("Selektujte prvo aerodrom koji zelite da menjate");
            }
        }

        private void Obrisi_Aerodrom(object sender, RoutedEventArgs e)
        {
            if (DGAerodromi.SelectedItem is Aerodrom selektovaniAerodrom)
            {
                MessageBoxResult potvrda = MessageBox.Show(
                    $"Da li ste sigurni da želite da obrišete aerodrom \"{selektovaniAerodrom.Naziv}\"?\n\n" +
                    "Pažnja: Svi letovi vezani za ovaj aerodrom će postati neraspoređeni!",
                    "Potvrda brisanja",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (potvrda == MessageBoxResult.Yes)
                {
                    string nazivZaBrisanje = selektovaniAerodrom.Naziv;

                    foreach (var let in AppData.ListaLetova)
                    {
                        if (let.AerodromPolaska == nazivZaBrisanje)
                        {
                            let.AerodromPolaska = "Neraspoređeno";
                        }

                        if (let.AerodromOdredista == nazivZaBrisanje)
                        {
                            let.AerodromOdredista = "Neraspoređeno";
                        }
                    }

                    AppData.ListaAerodroma.Remove(selektovaniAerodrom);
                    AppData.SacuvajSve();
                    DGSelektovaniLetovi.ItemsSource = null;

                    MessageBox.Show("Aerodrom je uspešno obrisan, a njegovi letovi su neraspoređeni!",
                                    "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Molimo vas da prvo izaberete aerodrom iz tabele koji želite da obrišete.",
                                "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}