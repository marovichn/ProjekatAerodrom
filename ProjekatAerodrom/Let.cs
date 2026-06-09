using System;
using System.ComponentModel;

namespace ProjekatAerodrom
{


    public class Let : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public bool IsPlaced { get; set; } = false;
        private string brojLeta;
        private string polazak;
        private string odrediste;
        private string kompanija;
        private string vremePolaska;
        private string vremeDolaska;
        private string status;
        private string ikonica;

        public Let()
        {

        }

        public Let(string brojLeta, string polazak, string odrediste, string kompanija, string vremePolaska, string vremeDolaska, string status)
        {
            this.brojLeta = brojLeta;
            this.polazak = polazak;
            this.odrediste = odrediste;
            this.kompanija = kompanija;
            this.vremePolaska = vremePolaska;
            this.vremeDolaska = vremeDolaska;
            this.status = status;
        }

        public string BrojLeta
        {
            get { return brojLeta; }
            set
            {
                if (value != brojLeta)
                {
                    brojLeta = value;
                    OnPropertyChanged("BrojLeta");
                }
            }
        }

        public string Polazak
        {
            get { return polazak; }
            set
            {
                if (value != polazak)
                {
                    polazak = value;
                    OnPropertyChanged("Polazak");
                }
            }
        }

        public string Odrediste
        {
            get { return odrediste; }
            set
            {
                if (value != odrediste)
                {
                    odrediste = value;
                    OnPropertyChanged("Odrediste");
                }
            }
        }

        public string Kompanija
        {
            get { return kompanija; }
            set
            {
                if (value != kompanija)
                {
                    kompanija = value;
                    OnPropertyChanged("Kompanija");
                }
            }
        }

        public string VremePolaska
        {
            get { return vremePolaska; }
            set
            {
                if (value != vremePolaska)
                {
                    vremePolaska = value;
                    OnPropertyChanged("VremePolaska");
                }
            }
        }

        public string VremeDolaska
        {
            get { return vremeDolaska; }
            set
            {
                if (value != vremeDolaska)
                {
                    vremeDolaska = value;
                    OnPropertyChanged("VremeDolaska");
                }
            }
        }

        public string Status
        {
            get { return status; }
            set
            {
                if (value != status)
                {
                    status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        public string Ikonica { get => ikonica; set => ikonica = value; }
    }
}