using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekt_Lug_Trug_AG.Helper;
using Projekt_Lug_Trug_AG.Models;
using Projekt_Lug_Trug_AG.Controls;
using System.Windows;

namespace Projekt_Lug_Trug_AG.ViewModels
{
    class MainVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> Zustaende { get; set; }
        private ObservableCollection<Kunde> kunden;
        public ObservableCollection<Kunde> Kunden {
            get { return kunden; }
            set 
            {
                kunden = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Kunden"));
            } 
        }


        private string _aktion;
        public string Aktion
        {
            get { return _aktion; }
            set
            {
                _aktion = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Aktion"));
            }
        }

        private int _kundenNummer;
        public int KundenNummer
        {
            get { return _kundenNummer; }
            set
            {
                _kundenNummer = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("KundenNummer"));
            }
        }
        private string _nameFirma;
        public string NameFirma
        {
            get { return _nameFirma; }
            set
            {
                _nameFirma = value;
                // View muss über eine Änderung informiert werden!
                // Event NotifyPropertyChanged("Vorname")
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NameFirma"));
                // Typen.Add(Vorname);
            }
        }
        // Adresse vlt einzeln Straße ...

        private string _adresseKunde;
        public string AdresseKunde
        {
            get { return _adresseKunde; }
            set
            {
                _adresseKunde = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AdresseKunde"));
            }
        }
        private string _ansprechpartner;
        public string Ansprechpartner
        {
            get { return _ansprechpartner; }
            set
            {
                _ansprechpartner = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Ansprechpartner"));
            }
        }
        private string _telefonnummer;
        public string Telefonnummer
        {
            get { return _telefonnummer; }
            set
            {
                _telefonnummer = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Telefonnummer"));
            }
        }
        private string _aktiv;
        public string Aktiv
        {
            get { return _aktiv; }
            set
            {
                _aktiv = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Aktiv"));
            }
        }

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }
        public RelayCommand ChangeCommand { get; set; }

        public void Cancel(object o)
        {
            KundenNummer = 0;
            NameFirma = "";
            AdresseKunde = "";
            Ansprechpartner = "";
            Telefonnummer = "";
            Aktiv = "";
        }

        public void Exit(object o)
        {
            Environment.Exit(0);
        }

        public void NewClient(object o)
        {
            Aktion = "Neuer Kunde";
        }

        public MainVM()
        {
            Kundenverwaltung.Instance.Load();
            Zustaende = new ObservableCollection<string>() { "Aktiv", "Inaktiv", "" };
            Kunden = new ObservableCollection<Kunde>(
                Kundenverwaltung.Instance.GetKunden()
                );

            SaveCommand = new RelayCommand((o) =>
            {
                Kunde vorhanden = Kundenverwaltung.Instance.GetKunden().Find(k => k.KundenNummer == KundenNummer);
                if (vorhanden == null)
                {
                    Kunde p = new Kunde()
                    {
                        KundenNummer = KundenNummer,
                        NameFirma = NameFirma,
                        AdresseKunde = AdresseKunde,
                        Ansprechpartner = Ansprechpartner,
                        Telefonnummer = Telefonnummer,
                        Aktiv = Aktiv
                    };
                    Kundenverwaltung.Instance.AddKunde(p);
                    Kunden.Add(p);
                    Kundenverwaltung.Instance.Save();
                }
                else
                {
                    MessageBox.Show("Kundennummer existert bereits");
                }
            });

            CancelCommand = new RelayCommand(Cancel);

            ExitCommand = new RelayCommand(Exit);

            ChangeCommand = new RelayCommand((o) =>
            {
                Kunde vorhanden = Kundenverwaltung.Instance.GetKunden().Find(k => k.KundenNummer == KundenNummer);
                if (vorhanden != null)
                {
                    Kundenverwaltung.Instance.RemoveKunde(vorhanden);

                    Kunde p = new Kunde()
                    {
                        KundenNummer = KundenNummer,
                        NameFirma = NameFirma,
                        AdresseKunde = AdresseKunde,
                        Ansprechpartner = Ansprechpartner,
                        Telefonnummer = Telefonnummer,
                        Aktiv = Aktiv
                    };
                    Kundenverwaltung.Instance.AddKunde(p);
                    Kunden.Add(p);
                    Kundenverwaltung.Instance.Save();
                    Kunden = new ObservableCollection<Kunde>(Kundenverwaltung.Instance.GetKunden());
                }
            });


        }

    }
}