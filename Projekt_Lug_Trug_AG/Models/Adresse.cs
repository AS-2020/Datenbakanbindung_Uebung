using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_Lug_Trug_AG.Models
{
    class Adresse
    {
        public string Strase { get; set; }
        public string Hausnummer { get; set; }
        public string Ort { get; set; }
        public int PLZ { get; set; }

        public Adresse(string strase, string hausnummer, string ort, int plz)
        {
            Strase = strase;
            Hausnummer = hausnummer;
            Ort = ort;
            PLZ = plz;
        }

        public override string ToString()
        {
            return $"{Strase} {Hausnummer} {Ort} {PLZ}";
        }
    }
}
