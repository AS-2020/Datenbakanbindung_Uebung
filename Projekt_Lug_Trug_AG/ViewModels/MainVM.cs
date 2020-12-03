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
using System.Windows.Controls;

namespace Projekt_Lug_Trug_AG.ViewModels
{
    class MainVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> Zustaende { get; set; }
        private ObservableCollection<Kunde> kunden;
        public ObservableCollection<Kunde> Kunden
        {
            get { return kunden; }
            set
            {
                kunden = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Kunden"));
            }
        }
        public ICollectionView KundenListe { get; set; }
        public List<Kunde> SelectedKunde { get; set; }

        private string _sortieren;
        public string Sortieren
        {
            get { return _sortieren; }
            set
            {
                _sortieren = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sortieren"));
            }
        }


        private string _kundenSuchen = "";
        public string KundenSuchen
        {
            get { return _kundenSuchen; }
            set
            {
                _kundenSuchen = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("KundenSuchen"));
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
        public RelayCommand SelectCommand { get; set; }
        public RelayCommand SearchCommand { get; set; }
        public RelayCommand SortCommand { get; set; }

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
        public void SortNr(object o)
        {
            Kunden = new ObservableCollection<Kunde>(Kundenverwaltung.Instance.GetKunden().OrderBy(k => k.KundenNummer));
        }
        public void SelectKunde(object o)
        {
            try
            {
                Kunde k = SelectedKunde.LastOrDefault<Kunde>();
                KundenNummer = k.KundenNummer;
                NameFirma = k.NameFirma;
                AdresseKunde = k.AdresseKunde;
                Ansprechpartner = k.Ansprechpartner;
                Telefonnummer = k.Telefonnummer;
                Aktiv = k.Aktiv;

            }
            catch (System.ArgumentNullException)
            {

                MessageBox.Show("Kein Kunde gewählt");
            }
        }

        public MainVM()
        {
            Kundenverwaltung.Instance.Load();
            Zustaende = new ObservableCollection<string>() { "Aktiv", "Inaktiv", "" };
            Kunden = new ObservableCollection<Kunde>(Kundenverwaltung.Instance.GetKunden().OrderBy(k => k.KundenNummer));

            SaveCommand = new RelayCommand((o) =>
            {
                Kunde vorhanden = Kundenverwaltung.Instance.GetKunden().Find(k => k.KundenNummer == KundenNummer);
                if (vorhanden == null && NameFirma != null && KundenNummer > 0)
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
                else if (vorhanden != null)
                {
                    MessageBox.Show("Kundennummer existert bereits");
                }
                else if (NameFirma == null)
                {
                    MessageBox.Show("Es muss ein Firmenname eingegeben werden");
                }
                else if (KundenNummer <= 0)
                {
                    MessageBox.Show("Kundennummer muss größer als 0 sein");
                }
            });

            CancelCommand = new RelayCommand(Cancel);

            ExitCommand = new RelayCommand(Exit);

            SearchCommand = new RelayCommand((o) =>
            {
                    Kunden = new ObservableCollection<Kunde>(Kundenverwaltung.Instance.GetKunden().FindAll(k => k.NameFirma.ToLower().Contains(KundenSuchen.ToLower())));
            });

            SortCommand = new RelayCommand((o) =>
            {
                if (Sortieren == "Zustand sortieren")
                {
                    Kunden = new ObservableCollection<Kunde>(Kundenverwaltung.Instance.GetKunden().OrderBy(k => k.Aktiv));
                    Sortieren = "Nummer sortieren";
                }
                else if (Sortieren == "Nummer sortieren")
                {
                    Kunden = new ObservableCollection<Kunde>(Kundenverwaltung.Instance.GetKunden().OrderBy(k => k.KundenNummer));
                    Sortieren = "Zustand sortieren";
                }
            });

            SelectCommand = new RelayCommand(SelectKunde);

            ChangeCommand = new RelayCommand((o) =>
            {
                Kunde vorhanden = Kundenverwaltung.Instance.GetKunden().Find(k => k.KundenNummer == KundenNummer);

                if (vorhanden == null)
                {
                    MessageBox.Show("Die Kundennummer existiert noch nicht!");
                }
                else
                {
                    if (NameFirma != null && KundenNummer > 0)
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
                        Kunden = new ObservableCollection<Kunde>(Kundenverwaltung.Instance.GetKunden().OrderBy(k => k.KundenNummer));
                    }
                    else if (NameFirma == null)
                    {
                        MessageBox.Show("Es muss ein Firmenname eingegeben werden");
                    }
                    else if (KundenNummer <= 0)
                    {
                        MessageBox.Show("Kundennummer muss größer als 0 sein");
                    }
                }
            });
        }
    }
}