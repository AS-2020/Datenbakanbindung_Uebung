using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_Lug_Trug_AG.Models
{
    public class Kunde
    {
        public int KundenNummer { get; set; }
        public string NameFirma { get; set; }
        //public Adresse AdresseKunde { get; set; }
        public string AdresseKunde { get; set; }
        public string Ansprechpartner { get; set; }
        public string Telefonnummer { get; set; }
        public string Aktiv { get; set; }

        public override string ToString()
        {
            return $"{KundenNummer} {NameFirma} {AdresseKunde} {Ansprechpartner} {Telefonnummer} {Aktiv}";
        }


    }
}
