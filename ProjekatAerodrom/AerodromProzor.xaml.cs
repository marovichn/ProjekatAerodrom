using System;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;

namespace ProjekatAerodrom
{
    public partial class AerodromProzor : Window
    {
        
        private Aerodrom aerodromZaIzmenu = null;

        public AerodromProzor(Aerodrom aerodrom = null)
        {
            InitializeComponent();

            
            this.aerodromZaIzmenu = aerodrom;

            
            if (aerodromZaIzmenu != null)
            {
                lblNaslov.Text = "IZMENI AERODROM";
                btnPromeniSacuvaj.Content = "Sačuvaj promene";


                txtID.Text = aerodromZaIzmenu.Id.ToString();
                txtID.IsEnabled = false;

                txtNaziv.Text = aerodromZaIzmenu.Naziv;
                txtGrad.Text = aerodromZaIzmenu.Grad;
                txtDrzava.Text = aerodromZaIzmenu.Drzava;
                txtIkonicaPutanja.Text = aerodromZaIzmenu.Ikonica;
            }
        }

      
        private void BtnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNaziv.Text) ||
                string.IsNullOrWhiteSpace(txtGrad.Text) ||
                string.IsNullOrWhiteSpace(txtDrzava.Text))
            {
                MessageBox.Show("Sva tekstualna polja moraju biti popunjena!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string putanjaIkonice = string.IsNullOrWhiteSpace(txtIkonicaPutanja.Text) ? "Slike/default.png" : txtIkonicaPutanja.Text;

            
            if (aerodromZaIzmenu != null)
            {
              
                aerodromZaIzmenu.Naziv = txtNaziv.Text;
                aerodromZaIzmenu.Grad = txtGrad.Text;
                aerodromZaIzmenu.Drzava = txtDrzava.Text;
                aerodromZaIzmenu.Ikonica = putanjaIkonice;

                MessageBox.Show("Promene su uspešno sačuvane!", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                
                if (!int.TryParse(txtID.Text, out int unetiId))
                {
                    MessageBox.Show("ID aerodroma mora biti broj!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (AppData.ListaAerodroma.Any(a => a.Id == unetiId))
                {
                    MessageBox.Show("Aerodrom sa ovim ID-jem već postoji!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Aerodrom novi = new Aerodrom(unetiId, txtNaziv.Text, txtGrad.Text, txtDrzava.Text, putanjaIkonice);
                AppData.ListaAerodroma.Add(novi);
                MessageBox.Show("Aerodrom je uspešno dodat!", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
            }

           
            AppData.SacuvajSve();
            this.Close();
        }

        private void BtnIzaberiIkonicu_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dijalog = new OpenFileDialog();
            dijalog.Filter = "Slike (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
            if (dijalog.ShowDialog() == true)
            {
                string folderZaSlike = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Slike");
                if (!Directory.Exists(folderZaSlike)) Directory.CreateDirectory(folderZaSlike);

                string imeFajla = Path.GetFileName(dijalog.FileName);
                string konacnaPutanja = Path.Combine(folderZaSlike, imeFajla);

                try
                {
                    if (!File.Exists(konacnaPutanja)) File.Copy(dijalog.FileName, konacnaPutanja);
                    txtIkonicaPutanja.Text = "Slike/" + imeFajla;
                }
                catch (Exception ex) { MessageBox.Show("Greška: " + ex.Message); }
            }
        }

        private void BtnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}