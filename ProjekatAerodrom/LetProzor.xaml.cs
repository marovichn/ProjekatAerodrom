using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace ProjekatAerodrom
{
   
    public partial class LetProzor : Window
    {

        private Let letZaIzmenu = null;
        public LetProzor(Let let = null)
        {
            InitializeComponent();

            this.letZaIzmenu = let;


            if (letZaIzmenu != null)
            {
                letProzor123.Title = "IZMENI LET";


                txtBrojLeta.Text = letZaIzmenu.BrojLeta.ToString();
                txtBrojLeta.IsEnabled = false;

                txtPolazak.Text = letZaIzmenu.Polazak;
                txtOdrediste.Text = letZaIzmenu.Odrediste;
                txtKompanija.Text = letZaIzmenu.Kompanija;
                txtVremePolaska.Text = letZaIzmenu.VremePolaska;
                txtVremeDolaska.Text = letZaIzmenu.VremeDolaska;

            }
        }

        public LetProzor(DataGrid dGSelektovaniLetovi)
        {
            DGSelektovaniLetovi = dGSelektovaniLetovi;
        }

        public DataGrid DGSelektovaniLetovi { get; }

        private void btnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPolazak.Text) ||
                string.IsNullOrWhiteSpace(txtOdrediste.Text) ||
                string.IsNullOrWhiteSpace(txtKompanija.Text) ||
                string.IsNullOrWhiteSpace(txtVremePolaska.Text) ||
                string.IsNullOrWhiteSpace(txtVremeDolaska.Text)) //ili nije nista selektovano, aktivan, otkazan.. to fali...
            {
                MessageBox.Show("Sva tekstualna polja moraju biti popunjena!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (daLiJeIspravnoVreme(txtVremePolaska.Text, txtVremeDolaska.Text) == false)
            {
                MessageBox.Show("Nije ispravno uneto vreme!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (AppData.ListaLetova.Any(a => a.BrojLeta == txtBrojLeta.Text))
            {
                MessageBox.Show("Let sa ovim brojem već postoji!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string putanjaIkonice = string.IsNullOrWhiteSpace(txtIkonica.Text) ? "Slike/default.png" : txtIkonica.Text;

            if (letZaIzmenu != null)
            {

                letZaIzmenu.Polazak = txtPolazak.Text;
                letZaIzmenu.Odrediste = txtOdrediste.Text;
                letZaIzmenu.Kompanija = txtKompanija.Text;
                letZaIzmenu.VremePolaska = txtVremePolaska.Text;
                letZaIzmenu.VremeDolaska = txtVremeDolaska.Text;
                letZaIzmenu.Ikonica = putanjaIkonice;

                MessageBox.Show("Promene su uspešno sačuvane!", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {

                //FALI MI LOGIKA ZA STATUSS
                Let novi = new Let(txtBrojLeta.Text, txtPolazak.Text, txtOdrediste.Text, txtKompanija.Text, txtVremePolaska.Text, txtVremeDolaska.Text, "SVI", txtIkonica.Text);
                AppData.ListaLetova.Add(novi);
                MessageBox.Show("Let je uspešno dodat!", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
            }


            AppData.SacuvajSve();
            this.Close();
        }

        public bool daLiJeIspravnoVreme(string polazno, string dolazno)
        {
            bool vreme = false;

            /*
             * treba podeliti oba string i napraviti brojeve
             * npr ako je vreme 13:35, pamticu ga kao 1335 i 1455
             * pa ako je polazno vreme vece od dolaznog to ne moze
            */
            string[] deloviP = polazno.Split(':');

            bool samoBrojeviP1 = deloviP[0].All(char.IsDigit);
            bool samoBrojeviP2 = deloviP[1].All(char.IsDigit);

            string[] deloviD = dolazno.Split(':');

            bool samoBrojeviD1 = deloviD[0].All(char.IsDigit);
            bool samoBrojeviD2 = deloviD[1].All(char.IsDigit);

            if (samoBrojeviP1 && samoBrojeviP2 && samoBrojeviD1 && samoBrojeviD2)
            {
                int vremeP = Int32.Parse(deloviP[0]) * 1000 + Int32.Parse(deloviP[1]);
                int vremeD = Int32.Parse(deloviD[0]) * 1000 + Int32.Parse(deloviD[1]);

                if (vremeP > vremeD)
                {
                    //MessageBox.Show("Nije ispravno uneto vreme!");
                    vreme = false;
                }
                else
                    vreme = true;
            }
            else
            { MessageBox.Show("Nije ispravno uneto vreme! Primer ispravnog unosa je 13:25"); }

            return vreme;
        }

        private void btnIzaberi_Click(object sender, RoutedEventArgs e)
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
                    txtIkonica.Text = "Slike/" + imeFajla;
                }
                catch (Exception ex) { MessageBox.Show("Greška: " + ex.Message); }
            }
        }
    }
}
