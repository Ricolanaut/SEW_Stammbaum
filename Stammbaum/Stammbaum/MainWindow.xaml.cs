using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections;

namespace Stammbaum
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Person> PersonenCollection { get; set; }
        ObservableCollection<Beziehung> BeziehungenCollection { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            PersonenCollection = new ObservableCollection<Person>();
            BeziehungenCollection = new ObservableCollection<Beziehung>();

            using (StreamReader srp = new StreamReader(@"..\..\..\personen.csv"))
            {
                string zeilep;
                while ((zeilep = srp.ReadLine()) != null)
                {                   
                    string[] spalte = zeilep.Split(';');
                    if (spalte[1] == "K")
                    {
                        Kind k = new Kind()
                        {
                            ID = Convert.ToInt32(spalte[0]),
                            KE = spalte[1],
                            Vorname = spalte[2],
                            Nachname = spalte[3],
                            Gebdatum = spalte[4],
                            Geschlecht = spalte[5],
                            Schule = spalte[6]
                        };
                        PersonenCollection.Add(k);
                    }
                    else if(spalte[1] == "E")
                    {
                        Erwachsener e = new Erwachsener()
                        {
                            ID = Convert.ToInt32(spalte[0]),
                            KE = spalte[1],
                            Vorname = spalte[2],
                            Nachname = spalte[3],
                            Gebdatum = spalte[4],
                            Geschlecht = spalte[5],
                            Beruf = spalte[6]
                        };
                        PersonenCollection.Add(e);
                    }
                    lb.ItemsSource = PersonenCollection;
                }
            }

            using (StreamReader srb = new StreamReader(@"..\..\..\beziehungen.csv"))
            {
                string zeileb;
                while ((zeileb = srb.ReadLine()) != null)
                {                      
                    string[] spalte = zeileb.Split(';');
                    Beziehung b = new Beziehung()
                    {
                        Quelle = Convert.ToInt32(spalte[0]),
                        Ziel = Convert.ToInt32(spalte[1]),
                        Rolle = spalte[2]
                    };
                    BeziehungenCollection.Add(b);                        
                }
            }                                  
        }

        List<string> Familiefinden(int id, ObservableCollection<Person> pc, ObservableCollection<Beziehung> bc)
        {            
            Queue q = new Queue();
            id++;
            foreach (var item in bc)
            {
                try
                {
                    if (item.Ziel == id)
                    {
                        q.Enqueue(item.Rolle +
                                ": " + pc[item.Quelle - 1].Vorname
                                + " " + pc[item.Quelle - 1].Nachname
                                + " | * " + pc[item.Quelle - 1].Gebdatum);
                    }
                }
                catch
                {
                    throw new CSVFileException();
                }
            }
            List<string> erg = new List<string>();           
            foreach (string item in q)
            {
                erg.Add(item);
            }       
            return erg;
        }

        List<string> Familiespeichern(ObservableCollection<Person> pc, ObservableCollection<Beziehung> bc)
        {
            Queue q = new Queue();
            foreach (var item in bc)
            {
                foreach (var item2 in pc)
                {
                    try
                    {
                        if (item.Ziel == item2.ID)
                        {
                            q.Enqueue(pc[item.Quelle - 1].Vorname
                                        + " " + pc[item.Quelle - 1].Nachname
                                        + " | * " + pc[item.Quelle - 1].Gebdatum                                       
                                        + " | " + item.Rolle
                                        + " von " + item2.Vorname
                                        + " " + item2.Nachname);
                        }
                    }
                    catch
                    {
                        throw new TXTFileException();
                    }
                }
            }
            List<string> erg = new List<string>();
            foreach (string item in q)
            {
                erg.Add(item);
            }
            return erg;
        }

        abstract class Person
        {
            public int ID { get; set; }
            public string KE { get; set; }
            public string Vorname { get; set; }
            public string Nachname { get; set; }
            public string Gebdatum { get; set; }
            public string Geschlecht { get; set; }
        }

        class Kind : Person
        {
            public string Schule { get; set; }

            public override string ToString()
            {
                return Vorname +
                       " " + Nachname +
                       " | * " + Gebdatum +
                       " | " + Geschlecht +
                       " | Schule: " + Schule;
            }
        }

        class Erwachsener : Person
        {
            public string Beruf { get; set; }

            public override string ToString()
            {
                return Vorname +
                       " " + Nachname +
                       " | * " + Gebdatum +
                       " | " + Geschlecht +
                       " | Beruf: " + Beruf;
            }
        }

        class Beziehung
        {
            public int Quelle { get; set; }
            public int Ziel { get; set; }
            public string Rolle { get; set; }
        }
        
        class CSVFileException : Exception
        {
            public override string Message
            {
                get
                {
                    return "Der Inhalt einer CSV-Datei ist inkorrekt oder die Datei existiert nicht!";
                }
            }           
        }

        class TXTFileException : Exception
        {
            public override string Message
            {
                get
                {
                    return "Die TXT-Datei existiert nicht!";
                }
            }
        }

        private void lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void lb_f_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {           
            
        }

        private void bt_fa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> ausgabe = Familiefinden(lb.SelectedIndex, PersonenCollection, BeziehungenCollection);
                lb_f.ItemsSource = ausgabe;
            }
            catch (CSVFileException csve)
            {
                MessageBox.Show("Fehler: " + csve.Message);
            }
        }

        private void bt_clear_Click(object sender, RoutedEventArgs e)
        {
            lb_f.ItemsSource = null;
        }

        private void bt_fs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> speicher = Familiespeichern(PersonenCollection, BeziehungenCollection);
                StreamWriter swf = new StreamWriter(@"..\..\..\familie.txt");
                speicher.ForEach(swf.WriteLine);
                swf.Close();
                MessageBox.Show("Familie wurde gespeichert!");
            }
            catch (TXTFileException txte)
            {
                MessageBox.Show("Fehler: " + txte.Message);
            }
        }
    }
}
