using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjekatAerodrom
{
    /// <summary>
    /// Interaction logic for LetProzor.xaml
    /// </summary>
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
                string.IsNullOrWhiteSpace(txtVremeDolaska.Text)) //ili nije nista selektovano, aktivan, otkazan..
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


            if (letZaIzmenu != null)
            {

                letZaIzmenu.Polazak = txtPolazak.Text;
                letZaIzmenu.Odrediste = txtOdrediste.Text;
                letZaIzmenu.Kompanija = txtKompanija.Text;
                letZaIzmenu.VremePolaska = txtVremePolaska.Text;
                letZaIzmenu.VremeDolaska = txtVremeDolaska.Text;

                MessageBox.Show("Promene su uspešno sačuvane!", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {

                //FALI MI LOGIKA ZA STATUSS
                Let novi = new Let(txtBrojLeta.Text, txtPolazak.Text, txtOdrediste.Text, txtKompanija.Text, txtVremePolaska.Text, txtVremeDolaska.Text, "SVI");
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
            int vremeP = Int32.Parse(deloviP[0]) * 1000 + Int32.Parse(deloviP[1]);

            string[] deloviD = dolazno.Split(':');
            int vremeD = Int32.Parse(deloviD[0]) * 1000 + Int32.Parse(deloviD[1]);

            if (vremeP > vremeD)
            {
                MessageBox.Show("Nije ispravno uneto vreme!");
                vreme = false;
            }
            else
                vreme = true;

            return vreme;
        }
    }
}
