using System;

namespace ProjekatAerodrom
{
    public class Aerodrom
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Grad { get; set; }    
        public string Drzava { get; set; }
        public string Ikonica { get; set; }

        public Aerodrom()
        {
        }

        public Aerodrom(int id, string naziv, string grad, string drzava, string ikonica)
        {
            Id = id;
            Naziv = naziv;
            Grad = grad;
            Drzava = drzava;
            Ikonica = ikonica;
        }
    }
}