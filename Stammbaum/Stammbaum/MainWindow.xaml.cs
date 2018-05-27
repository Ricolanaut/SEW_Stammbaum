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

namespace Stammbaum
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Person> PersonenListe;       

        public MainWindow()
        {
            InitializeComponent();

            PersonenListe = new List<Person>();            

            using (StreamReader srp = new StreamReader(@"L:\Stammbaum\personen.csv"))
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
                        PersonenListe.Add(k);
                    }
                    else if (spalte[1] == "E")
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
                        PersonenListe.Add(e);
                    }
                    lb.ItemsSource = PersonenListe;
                }               
            }
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
                return "Nachname: " + Nachname +
                       ", Vorname: " + Vorname +
                       ", Geburtsdatum: " + Gebdatum +
                       ", Geschlecht: " + Geschlecht +
                       ", Schule: " + Schule;
            }
        }

        class Erwachsener : Person
        {
            public string Beruf { get; set; }

            public override string ToString()
            {
                return "Nachname: " + Nachname +
                       ", Vorname: " + Vorname +
                       ", Geburtsdatum: " + Gebdatum +
                       ", Geschlecht: " + Geschlecht +
                       ", Beruf: " + Beruf;
            }
        }

        class Beziehung
        {
            public int Quelle { get; set; }
            public int Ziel { get; set; }
            public string Rolle { get; set; }
        }
       
        private void lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void lb_f_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void bt_fa_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
