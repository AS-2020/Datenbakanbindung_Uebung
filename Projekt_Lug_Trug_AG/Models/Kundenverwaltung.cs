using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Projekt_Lug_Trug_AG.Models
{
    class Kundenverwaltung
    {
        private List<Kunde> Kunden;
        private Kundenverwaltung()
        {
            Kunden = new List<Kunde>();
        }
        private static Kundenverwaltung _instance;
        public static Kundenverwaltung Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Kundenverwaltung();
                }
                return _instance;
            }
        }
        private const string FILENAME = "Kunden.xml";
        public void AddKunde(Kunde k)
        {
            Kunden.Add(k);
        }

        //public IEnumerable<Kunde> GetKunden()
        //{
        //    return Kunden;
        //}
        public List<Kunde> GetKunden()
        {
            return Kunden;
        }
        public void RemoveKunde(Kunde k)
        {
            Kunden.Remove(k);
        }

        public void Save()
        {
            XmlSerializer ser = new XmlSerializer(Kunden.GetType());
            using (FileStream stream = File.Create(FILENAME))
            {
                ser.Serialize(stream, Kunden);
            }
        }
        public void Load()
        {
            try
            {
                if (File.Exists(FILENAME))
                {
                    XmlSerializer ser = new XmlSerializer(Kunden.GetType());
                    using (FileStream stream = File.Open(FILENAME, FileMode.Open))
                    {
                        Kunden = ser.Deserialize(stream) as List<Kunde>;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
                // Event auslösen ...
            }

        }
    }
}
