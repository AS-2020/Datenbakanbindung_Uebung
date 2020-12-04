using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        //private const string FILENAME = "Kunden.xml";
        private const string VERBINDUNG = @"Data Source=localhost\sqlexpress;Initial Catalog=Lug_Trug_AG;User ID=sa;Password=P@ssword";
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

        //public void OldSave()
        //{
        //    XmlSerializer ser = new XmlSerializer(Kunden.GetType());
        //    using (FileStream stream = File.Create(FILENAME))
        //    {
        //        ser.Serialize(stream, Kunden);
        //    }
        //}
        //public void OldLoad()
        //{
        //    try
        //    {
        //        if (File.Exists(FILENAME))
        //        {
        //            XmlSerializer ser = new XmlSerializer(Kunden.GetType());
        //            using (FileStream stream = File.Open(FILENAME, FileMode.Open))
        //            {
        //                Kunden = ser.Deserialize(stream) as List<Kunde>;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //        // Event auslösen ...
        //    }
        //}
        public void Save(Kunde kunde)
        {
            SqlConnection con = new SqlConnection(VERBINDUNG);

            con.Open();
            SqlCommand com = new SqlCommand();

            string sql = $"Insert into Kunden values ({kunde.KundenNummer}, '{kunde.NameFirma}', '{kunde.AdresseKunde}', '{kunde.Ansprechpartner}', '{kunde.Telefonnummer}', '{kunde.Aktiv}')";

            com = new SqlCommand(sql, con);
            SqlDataAdapter adapter = new SqlDataAdapter();

            adapter.InsertCommand = com;
            adapter.InsertCommand.ExecuteNonQuery();

            com.Dispose();
            con.Close();
        }

        public void Load()
        {
            SqlConnection con = new SqlConnection(VERBINDUNG);

            string sql = "Select * from Kunden";

            con.Open();

            SqlCommand com = new SqlCommand(sql, con);
            SqlDataReader reader = com.ExecuteReader();

            while (reader.Read())
            {
                Kunde k = new Kunde();

                k.KundenNummer = (int)reader.GetValue(0);
                k.NameFirma = reader.GetValue(1).ToString();
                k.AdresseKunde = reader.GetValue(2).ToString();
                k.Ansprechpartner = reader.GetValue(3).ToString();
                k.Telefonnummer = reader.GetValue(4).ToString();
                k.Aktiv = reader.GetValue(5).ToString();

                Instance.AddKunde(k);
            }

            reader.Close();
            com.Dispose();
            con.Close();
        }

        public void Change(Kunde k)
        {
            SqlConnection con = new SqlConnection(VERBINDUNG);

            con.Open();

            string sql = $@"Update Kunden set Firmenname = '{k.NameFirma}', Anschrift = '{k.AdresseKunde}', Ansprechpartner = '{k.Ansprechpartner}', Telefonnummer = '{k.Telefonnummer}',
Zustand = '{k.Aktiv}' where Kundennummer = '{k.KundenNummer}'";
            SqlCommand com = new SqlCommand(sql, con);
            SqlDataAdapter adapter = new SqlDataAdapter();

            adapter.UpdateCommand = com;
            adapter.UpdateCommand.ExecuteNonQuery();

            com.Dispose();
            con.Close();
        }
    }
}
