using System;

namespace ProjekatAerodrom
{ 
    public class Let
    {
        
        public string BrojLeta { get; set; }       
        public string AerodromPolaska { get; set; } 
        public string AerodromOdredista { get; set; } 
        public string Aviokompanija { get; set; }    
        public string VremePolaska { get; set; }     
        public string VremeDolaska { get; set; }     
        public string Status { get; set; }          
        public string Ikonica { get; set; }          

        
        public Let()
        {
        }

        public Let(string brojLeta, string aerodromPolaska, string aerodromOdredista,
                   string aviokompanija, string vremePolaska, string vremeDolaska,
                   string status, string ikonica)
        {
            BrojLeta = brojLeta;
            AerodromPolaska = aerodromPolaska;
            AerodromOdredista = aerodromOdredista;
            Aviokompanija = aviokompanija;
            VremePolaska = vremePolaska;
            VremeDolaska = vremeDolaska;
            Status = status;
            Ikonica = ikonica;
        }
    }
}