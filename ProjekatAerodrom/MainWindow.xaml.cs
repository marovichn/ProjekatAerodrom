using System.Collections.ObjectModel;
using System.Linq;
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

namespace ProjekatAerodrom
{
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AppData.UcitajSve();
            DGAerodromi.ItemsSource = AppData.ListaAerodroma;
            DGletovi.ItemsSource = AppData.ListaLetova;
        }

        //ANIKA TAB 1
        //KOD MENE ISTO TREBA I IKONICA DA SE OBEZBEDI DA SE MENJA, TO SAM TEK SAD VIDELA
        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            LetProzor prozor = new LetProzor();
            prozor.Owner = this;
            prozor.ShowDialog();
        }

        private void Izmeni_Click(object sender, RoutedEventArgs e)
        {
            if (DGletovi.SelectedItem is Let selektovaniLetovi)
            {
                LetProzor prozor = new LetProzor(selektovaniLetovi);
                prozor.Owner = this;
                prozor.ShowDialog();
            }
            else
            {
                MessageBox.Show("Selektujte prvo let koji zelite da menjate");
            }
        }

        private void Obrisi_Click(object sender, RoutedEventArgs e)
        {
            if (DGletovi.SelectedItem is Let selektovaniLet)
            {
                MessageBoxResult potvrda = MessageBox.Show(
                    $"Da li ste sigurni da želite da obrišete let \"{selektovaniLet.BrojLeta}\"?\n\n",
                    "Potvrda brisanja",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (potvrda == MessageBoxResult.Yes)
                {
                    string nazivZaBrisanje = selektovaniLet.BrojLeta;

                    foreach (var let in AppData.ListaLetova)
                    {
                        if (let.Polazak == nazivZaBrisanje)
                        {
                            let.Polazak = "Neraspoređeno";
                        }

                        if (let.Odrediste == nazivZaBrisanje)
                        {
                            let.Odrediste = "Neraspoređeno";
                        }
                    }

                    AppData.ListaLetova.Remove(selektovaniLet);
                    AppData.SacuvajSve();
                    DGSelektovaniLetovi.ItemsSource = null;

                    MessageBox.Show("Let je uspešno obrisan", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Molimo vas da prvo izaberete let iz tabele koji želite da obrišete.",
                                "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ExportCSV_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ObservableCollection<Let> pretrazenaDG = new ObservableCollection<Let>();

            if (txtSearch.Text.Equals(""))
            {
                pretrazenaDG = AppData.ListaLetova;
            }
            else
            {
                foreach (Let l in AppData.ListaLetova)
                {
                    if (l.BrojLeta.ToLower().StartsWith(txtSearch.Text.ToLower()) ||
                        l.Kompanija.ToLower().StartsWith(txtSearch.Text.ToLower()))
                    {
                        pretrazenaDG.Add(l);
                    }
                }
            }

            DGletovi.ItemsSource = pretrazenaDG;
        }

        //NIKOLA TAB 2

        //Read prikazi selektovane aerodrome 
        private void DGAerodromi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGAerodromi.SelectedItem is Aerodrom selektovaniAerodrom)
            {
                
                string nazivAerodroma = selektovaniAerodrom.Naziv;

                var filtriraniLetovi = AppData.ListaLetova
                    .Where(let => let.Polazak == nazivAerodroma || let.Odrediste == nazivAerodroma)
                    .ToList();

              
                DGSelektovaniLetovi.ItemsSource = filtriraniLetovi;
            }
            else
            {
                DGSelektovaniLetovi.ItemsSource = null;
            }
        }

        //Dodaj aerodrom (CRUD)
        private void Dodaj_Aerodrom(object sender, RoutedEventArgs e)
        {
            AerodromProzor prozor = new AerodromProzor();
            prozor.Owner = this;
            prozor.ShowDialog();
        }

        //Izmeni aerodrom (CRUD)
        private void Izmeni_Aerodrom(object sender, RoutedEventArgs e)
        {
            if (DGAerodromi.SelectedItem is Aerodrom selektovaniAerodrom)
            {
                AerodromProzor prozor = new AerodromProzor(selektovaniAerodrom);
                prozor.Owner = this;
                prozor.ShowDialog();
                DGAerodromi.Items.Refresh();
            }
            else {
                MessageBox.Show("Selektujte prvo aerodrom koji zelite da menjate");
            }
        }

        //Obrisi aerodrom (CRUD)
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
                        if (let.Polazak == nazivZaBrisanje)
                        {
                            let.Polazak = "Neraspoređeno";
                        }

                        if (let.Odrediste == nazivZaBrisanje)
                        {
                            let.Odrediste = "Neraspoređeno";
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

        //Drag & Drop

        private void DGAerodromi_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && DGAerodromi.SelectedItem is Aerodrom selektovaniAerodrom)
            {
                if (selektovaniAerodrom.IsPlaced) return;

                DragDrop.DoDragDrop(DGAerodromi, selektovaniAerodrom, DragDropEffects.Move);
            }
        }

        private void DGSelektovaniLetovi_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && DGSelektovaniLetovi.SelectedItem is Let selektovaniLet)
            {
                if (selektovaniLet.IsPlaced) return;

                DragDrop.DoDragDrop(DGSelektovaniLetovi, selektovaniLet, DragDropEffects.Move);
            }
        }

        private void MapaCanvas_Drop(object sender, DragEventArgs e)
        {
            Point pozicija = e.GetPosition(MapaCanvas);
            string relativnaPutanjaIkonice = "";
            string podrazumevanaIkonica = "";

            if (e.Data.GetDataPresent(typeof(Aerodrom)))
            {
                Aerodrom aerodrom = (Aerodrom)e.Data.GetData(typeof(Aerodrom));
                aerodrom.IsPlaced = true;
                relativnaPutanjaIkonice = aerodrom.Ikonica;
                podrazumevanaIkonica = "Slike/aerodrom-default-icon.png";

                DGAerodromi.CommitEdit(DataGridEditingUnit.Cell, true);
                DGAerodromi.CommitEdit(DataGridEditingUnit.Row, true);
                DGAerodromi.Items.Refresh();
            }
            else if (e.Data.GetDataPresent(typeof(Let)))
            {
                Let let = (Let)e.Data.GetData(typeof(Let));
                let.IsPlaced = true;
                relativnaPutanjaIkonice = let.Ikonica;
                podrazumevanaIkonica = "Slike/default-let-icon.png";

                DGSelektovaniLetovi.CommitEdit(DataGridEditingUnit.Cell, true);
                DGSelektovaniLetovi.CommitEdit(DataGridEditingUnit.Row, true);
                DGSelektovaniLetovi.Items.Refresh();
            }

            if (!string.IsNullOrEmpty(relativnaPutanjaIkonice))
            {
                //pravimo ikonicu za mapu 
                Image novaIkonica = new Image
                {
                    Width = 32,
                    Height = 32
                };

                //trazimo putanju ikonice u radnom direktorijumu, a ako ne nadjemo, trazimo je u resursima aplikacije
                try
                {
                    
                    string punaPutanjaNaDisku = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativnaPutanjaIkonice);

                    if (System.IO.File.Exists(punaPutanjaNaDisku))
                    {
                        
                        novaIkonica.Source = new BitmapImage(new Uri(punaPutanjaNaDisku, UriKind.Absolute));
                    }
                    else
                    {
                        
                        novaIkonica.Source = new BitmapImage(new Uri($"pack://application:,,,/{relativnaPutanjaIkonice}", UriKind.Absolute));
                    }
                }
                // Ako ni na jednom mestu ne nadjemo ikonice, postavljamo default ikonicu iz resursa
                catch
                {
                    
                    novaIkonica.Source = new BitmapImage(new Uri($"pack://application:,,,/{podrazumevanaIkonica}", UriKind.Absolute));
                }

                #region KONTEKST MENI
                novaIkonica.Tag = e.Data.GetData(typeof(Aerodrom)) ?? e.Data.GetData(typeof(Let));

                ContextMenu meni = new ContextMenu();

                MenuItem stavkaUkloni = new MenuItem { Header = "Ukloni sa mape" };
                stavkaUkloni.Click += UkloniSaMape_Click; 

                MenuItem stavkaObrisi = new MenuItem { Header = "Obriši" };
                stavkaObrisi.Click += ObrisiEntitet_Click; 

                meni.Items.Add(stavkaUkloni);
                meni.Items.Add(stavkaObrisi);

                novaIkonica.ContextMenu = meni;
                #endregion

                //postavljanje na poziciju :)
                Canvas.SetLeft(novaIkonica, pozicija.X - 16);
                Canvas.SetTop(novaIkonica, pozicija.Y - 16);

                MapaCanvas.Children.Add(novaIkonica);
                AppData.SacuvajSve();
            }
        }

        private void UkloniSaMape_Click(object sender, RoutedEventArgs e)
        {
            
            MenuItem stavka = (MenuItem)sender;
           
            ContextMenu meni = (ContextMenu)stavka.Parent;
            Image kliknutaIkonica = (Image)meni.PlacementTarget;

            
            if (kliknutaIkonica.Tag is Aerodrom aerodrom)
            {
                aerodrom.IsPlaced = false;
                DGAerodromi.Items.Refresh(); 
            }
            else if (kliknutaIkonica.Tag is Let let)
            {
                let.IsPlaced = false;
                DGSelektovaniLetovi.Items.Refresh(); 
            }

            
            MapaCanvas.Children.Remove(kliknutaIkonica);

           
            AppData.SacuvajSve();
        }


        private void ObrisiEntitet_Click(object sender, RoutedEventArgs e)
        {
            MenuItem stavka = (MenuItem)sender;
            ContextMenu meni = (ContextMenu)stavka.Parent;
            Image kliknutaIkonica = (Image)meni.PlacementTarget;

            MessageBoxResult rezultat = MessageBox.Show("Da li ste sigurni da želite trajno da obrišete ovaj entitet?",
                "Potvrda brisanja", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (rezultat != MessageBoxResult.Yes) return;

            if (kliknutaIkonica.Tag is Aerodrom aerodrom)
            {
                string nazivZaBrisanje = aerodrom.Naziv;

                foreach (var let in AppData.ListaLetova)
                {
                    if (let.Polazak == nazivZaBrisanje)
                    {
                        let.Polazak = "Neraspoređeno";
                    }

                    if (let.Odrediste == nazivZaBrisanje)
                    {
                        let.Odrediste = "Neraspoređeno";
                    }
                }

                AppData.ListaAerodroma.Remove(aerodrom);
                DGAerodromi.Items.Refresh();
                DGSelektovaniLetovi.ItemsSource = null;
            }
            else if (kliknutaIkonica.Tag is Let let)
            {
                AppData.ListaLetova.Remove(let);

                if (DGAerodromi.SelectedItem is Aerodrom selektovaniAerodrom)
                {
                    string nazivAerodroma = selektovaniAerodrom.Naziv;
                    DGSelektovaniLetovi.ItemsSource = AppData.ListaLetova
                        .Where(l => l.Polazak == nazivAerodroma || l.Odrediste == nazivAerodroma)
                        .ToList();
                }
            }

            MapaCanvas.Children.Remove(kliknutaIkonica);
            AppData.SacuvajSve();

            MessageBox.Show("Entitet je uspešno obrisan!", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}