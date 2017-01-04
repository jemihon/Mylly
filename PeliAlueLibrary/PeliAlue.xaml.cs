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
using System.Media;

namespace PeliAlueLibrary
{
    /// <summary>
    /// Pelialue toteutettuna kontrollina.
    /// </summary>
    public partial class PeliAlue : Grid
    {
        
        private const int MAX_NAPPULOITA_CONST = 18;
        private int MAX_NAPPULOITA_ORIG = MAX_NAPPULOITA_CONST;
        public int MAX_NAPPULOITA = MAX_NAPPULOITA_CONST;

        private int p1_nappuloita = 0;
        private int p2_nappuloita = 0;
        Random rnd = new Random();

        private Color siirrettavanVari; // kun nappula siirretään paikasta toiseen, sen väri tallennetaan tähän.
        private Color siirtoOkVari = Color.FromRgb(180, 0, 0); // (punainen) väri joka kertoo että kyseiseen paikkaan voi siirtää. 
        private Color p1Vari = Color.FromRgb(120, 12, 12);
        private Color p2Vari = Color.FromRgb(0, 120, 12);
        public int nappuloidenMaara = 0; // tätä kasvatetaan kun nappuloita lisätään ja vähennetään kun niitä poistetaan
        private bool joPoistettu = false; // apumuuttuja jolla varmistetaan, että yhdellä myllyllä poistetaan vain yksi nappula
        private bool enableAll = false; // apumuuttuja kaikkien nappuloiden enabloimiseen

        private bool vuoro = true; // true == p1, false == p2


        public static readonly DependencyProperty P1nameProperty = DependencyProperty.Register(
                           "P1name",
                           typeof(string),
                           typeof(PeliAlue),
                           new FrameworkPropertyMetadata("John",
                                FrameworkPropertyMetadataOptions.AffectsRender
                                ));

        public static readonly DependencyProperty P2nameProperty = DependencyProperty.Register(
                           "P2name",
                           typeof(string),
                           typeof(PeliAlue),
                           new FrameworkPropertyMetadata("Jane",
                                FrameworkPropertyMetadataOptions.AffectsRender
                                ));

        public static readonly DependencyProperty SiirtoProperty = DependencyProperty.Register(
                           "Siirto",
                           typeof(bool),
                           typeof(PeliAlue),
                           new FrameworkPropertyMetadata(false,
                                FrameworkPropertyMetadataOptions.AffectsRender
                                ));

        public static readonly DependencyProperty LisaysProperty = DependencyProperty.Register(
                           "Lisays",
                           typeof(bool),
                           typeof(PeliAlue),
                           new FrameworkPropertyMetadata(false,
                                FrameworkPropertyMetadataOptions.AffectsRender
                                ));

        public static readonly DependencyProperty PoistoProperty = DependencyProperty.Register(
                           "Poisto",
                           typeof(bool),
                           typeof(PeliAlue),
                           new FrameworkPropertyMetadata(false,
                                FrameworkPropertyMetadataOptions.AffectsRender
                                ));

        public string P1name
        {
            get { return (string)GetValue(P1nameProperty); }
            set { SetValue(P1nameProperty, value); }
        }

        public string P2name
        {
            get { return (string)GetValue(P2nameProperty); }
            set { SetValue(P2nameProperty, value); }
        }

        public bool Poisto
        {
            get { return (bool)GetValue(PoistoProperty); }
            set { SetValue(PoistoProperty, value); }
        }

        public bool Siirto
        {
            get { return (bool)GetValue(SiirtoProperty); }
            set { SetValue(SiirtoProperty, value); }
        }

        public bool Lisays
        {
            get { return (bool)GetValue(LisaysProperty); }
            set { SetValue(LisaysProperty, value); }
        }
        public PeliAlue()
        {
            Lisays = false;
            Siirto = false;
            Poisto = false;
            vuoro = rnd.NextDouble() >= 0.5; // arvotaan aloittaja satunnaisesti
            InitializeComponent();
            if (vuoro) infolabel.Content = "P1 lisäysvuoro";
            else infolabel.Content = "P2 lisäysvuoro";
        }

        /// <summary>
        /// Vaihtaa kaikkien nappuloiden checked, tai unchecked-värin.
        /// </summary>
        /// <param name="oldColor">minkäväriset vaihdetaan</param>
        /// <param name="newColor">minkävärisiksi vaihdetaan</param>
        /// <param name="checkd">jos false niin tsekkaamattomat vaihdetaan, muuten tsekatut</param>
        void VarinVaihto(Color oldColor, Color newColor, bool checkd)
        {
            foreach (var item in grid.Children)
            {
                if (item is OwnCanvas)
                {
                    foreach (var nappula in ((OwnCanvas)item).Children)
                    {
                        if (nappula is PeliNappula)
                        {
                            if (checkd == true && ((PeliNappula)nappula).Vari == oldColor)
                                ((PeliNappula)nappula).Vari = newColor;
                            if (checkd == false)
                                ((PeliNappula)nappula).UnCheckedVari = newColor;
                        }

                    }
                }
            }
        }

       
        /// <summary>
        /// Enabloi nappulat joiden IsChecked-arvo on checkd ja IsEnabled-arvo on enabld
        /// </summary>
        public void NappulaEnable(bool checkd, bool enabld)
        {
            foreach (var item in grid.Children)
            {
                if (item is OwnCanvas)
                {
                    foreach (var nappula in ((OwnCanvas)item).Children)
                    {

                        if (nappula is PeliNappula)
                        {
                            if (enableAll)
                                if (((PeliNappula)nappula).IsChecked == false)
                                {
                                    ((PeliNappula)nappula).IsEnabled = true;
                                    continue;
                                }


                            if ((Lisays == true && Poisto == false) || enabld == false)
                            {
                                if (((PeliNappula)nappula).IsChecked == checkd)
                                    ((PeliNappula)nappula).IsEnabled = enabld;
                            }
                            else if (vuoro)
                            {
                                if (((PeliNappula)nappula).IsChecked == checkd && ((PeliNappula)nappula).Vari.Equals(p1Vari))
                                    ((PeliNappula)nappula).IsEnabled = enabld;
                            }
                            else if (!vuoro)
                            {
                                if (((PeliNappula)nappula).IsChecked == checkd && ((PeliNappula)nappula).Vari.Equals(p2Vari))
                                    ((PeliNappula)nappula).IsEnabled = enabld;
                            }

                        }


                    }
                }

            }
        }

        /// <summary>
        /// Mahdollistaa nappulan siirtämisen, parametrina tulee antaa siirrettävä nappula
        /// </summary>
        /// <param name="pelinappula"></param>
        private void EnableSiirto(PeliNappula pelinappula)
        {
            int row = 0;
            int column = 0;

            if (vuoro && p1_nappuloita == 3)
            {
                enableAll = true;
                infolabel.Content = "P1 voi siirtää mihin tahansa!";
                NappulaEnable(false, true);
                enableAll = false;
                return;
            }

            if (!vuoro && p2_nappuloita == 3)
            {
                enableAll = true;
                infolabel.Content = "P2 voi siirtää mihin tahansa!";
                NappulaEnable(false, false);
                enableAll = false;
                return;
            }

            foreach (var item in grid.Children)
            {
                if (item is OwnCanvas)
                {
                    foreach (var nappula in ((OwnCanvas)item).Children)
                    {
                        if (nappula is PeliNappula)
                            if (((PeliNappula)nappula).Parent.Equals(pelinappula.Parent))
                            {
                                for (int x = 0; x < grid.ColumnDefinitions.Count; x++)
                                {
                                    for (int y = 0; y < grid.RowDefinitions.Count; y++)
                                    {
                                        if (pelinappula.Parent.Equals(grid.Children.Cast<UIElement>().FirstOrDefault(e => Grid.GetColumn(e) == x && Grid.GetRow(e) == y)))
                                        {
                                            row = y;
                                            column = x;
                                            break;
                                        }

                                    }
                                }
                            }
                        //row = Grid.GetRow((UIElement)((OwnCanvas)item).Parent);
                    }
                }
            }
            //north south west east...

            Siirtaminen('W', row, column);
            Siirtaminen('E', row, column);
            Siirtaminen('S', row, column);
            Siirtaminen('N', row, column);

        }

        private void Siirtaminen(char direction, int row, int column)
        {
            int columnTemp = column;
            int rowTemp = row;
            bool leaveLoop = false;
            bool value;
            int counter;

            switch (direction)
            {
                case 'W':
                    value = column > 0;
                    break;
                case 'E':
                    value = column < grid.ColumnDefinitions.Count - 2;
                    break;
                case 'N':
                    value = row > 0;
                    break;
                case 'S':
                    value = row < grid.RowDefinitions.Count - 1;
                    break;
                default:
                    return;
            }
            bool directions;
            if (directions = (direction == 'N' || direction == 'S'))
                counter = row;
            else counter = column;

            while (value && !leaveLoop)
            {
                if (direction == 'E' || direction == 'S')
                    counter++;
                else counter--;
                OwnCanvas current = (OwnCanvas)grid.Children.Cast<UIElement>().FirstOrDefault(e =>
                    Grid.GetColumn(e) == (directions ? column : counter) && Grid.GetRow(e) == (directions ? counter : row));
                if (current == null || current.Children.Count == 0)
                    return;
                foreach (var item in current.Children)
                {

                    if (item is PeliNappula)
                    {
                        if (((PeliNappula)item).IsChecked == false)
                        {
                            ((PeliNappula)item).IsEnabled = true;
                            ((PeliNappula)item).UnCheckedVari = siirtoOkVari;
                            leaveLoop = true;
                            break;
                        }
                        else return;
                    }
                }
            }
        }

        private bool OnkoMylly(PeliNappula lisattava)
        {
            if (joPoistettu) // jos myllyn takia on jo poistettu nappula, toista ei voi poistaa rakentamatta uutta myllyä
                return false;
            bool lisattavaMukana = false;
            int row = 0;
            int column = 0;
            int nappulaCounter = 0;
            // käydään rivit läpi
            for (column = 0; column < grid.ColumnDefinitions.Count - 2; column++)
            {

                while (row < grid.RowDefinitions.Count)
                {
                    OwnCanvas current = (OwnCanvas)grid.Children.Cast<UIElement>().FirstOrDefault(
                        e => Grid.GetColumn(e) == column && Grid.GetRow(e) == row);
                    if (current == null || current.Children.Count == 0)
                        nappulaCounter = 0;
                    foreach (var item in current.Children)
                    {

                        if (item is PeliNappula)
                        {
                            if (((PeliNappula)item).IsChecked == true)
                            {
                                if ((vuoro == true && ((PeliNappula)item).Vari.Equals(p1Vari)) || (vuoro == false && ((PeliNappula)item).Vari.Equals(p2Vari)) || ((PeliNappula)item).Equals(lisattava))
                                {
                                    if (((PeliNappula)item).Equals(lisattava))
                                        lisattavaMukana = true;
                                    nappulaCounter++;
                                    if (nappulaCounter == 3 && lisattavaMukana)
                                        return true;
                                }

                            }
                            //else return;
                        }
                    }
                    row++;

                }
                lisattavaMukana = false;
                row = 0;
                nappulaCounter = 0;
            }
            column = 0;
            // käydään sarakkeet läpi
            for (row = 0; row < grid.RowDefinitions.Count; row++)
            {

                while (column < grid.ColumnDefinitions.Count - 1)
                {
                    OwnCanvas current = (OwnCanvas)grid.Children.Cast<UIElement>().FirstOrDefault(
                        e => Grid.GetColumn(e) == column && Grid.GetRow(e) == row);
                    if (current == null || current.Children.Count == 0)
                        nappulaCounter = 0;
                    foreach (var item in current.Children)
                    {

                        if (item is PeliNappula)
                        {
                            if (((PeliNappula)item).IsChecked == true)
                            {
                                if ((vuoro == true && ((PeliNappula)item).Vari.Equals(p1Vari)) || (vuoro == false && ((PeliNappula)item).Vari.Equals(p2Vari)) || ((PeliNappula)item).Equals(lisattava))
                                {
                                    if (((PeliNappula)item).Equals(lisattava))
                                        lisattavaMukana = true;
                                    nappulaCounter++;
                                    if (nappulaCounter == 3 && lisattavaMukana)
                                        return true;
                                }

                            }
                            //else return;
                        }
                    }

                    column++;
                }
                lisattavaMukana = false;

                column = 0;
                nappulaCounter = 0;
            }
            return false;

        }

        private void PeliNappula_Checked(object sender, RoutedEventArgs e)
        {
            if (OnkoMylly((PeliNappula)sender)) // poisto-operaatio
            {
                if (Lisays)
                {
                    if (vuoro)
                    {
                        p1_nappuloita++;
                        ((PeliNappula)sender).Vari = p1Vari;
                    }
                    else
                    {
                        p2_nappuloita++;
                        ((PeliNappula)sender).Vari = p2Vari;
                    }

                }
                NappulaEnable(true, false);
                NappulaEnable(false, false);
                if (!vuoro)
                    infolabel.Content = "Pelaaja 2 mylly";
                else infolabel.Content = "Pelaaja 1 mylly";
                vuoro = !vuoro;
                Poisto = true; // tähän poistonappienable = true?
                Siirto = false;

                if (!Lisays) ((PeliNappula)sender).Vari = siirrettavanVari;
                ((PeliNappula)sender).UnCheckedVari = Color.FromRgb(0, 0, 0);

                //NappulaEnable(true, true);
                VarinVaihto(siirtoOkVari, Color.FromRgb(0, 0, 0), false);
                return;
            }
            joPoistettu = false;
            if (Lisays == true && nappuloidenMaara < MAX_NAPPULOITA)
            {
                if (vuoro)
                {
                    infolabel.Content = "Pelaaja 2 lisäysvuoro";
                    ((PeliNappula)sender).Vari = p1Vari;
                    p1_nappuloita++;
                }

                else
                {
                    infolabel.Content = "Pelaaja 1 lisäysvuoro";
                    ((PeliNappula)sender).Vari = p2Vari;
                    p2_nappuloita++;
                }
                vuoro = !vuoro;
                nappuloidenMaara++;

                NappulaEnable(false, false);
                NappulaEnable(true, false);
                Lisays = false;

                if (nappuloidenMaara == MAX_NAPPULOITA) // peli alkaa
                {
                    if (vuoro)
                        infolabel.Content = "Pelaaja 1 siirtovuoro";
                    else infolabel.Content = "Pelaaja 2 siirtovuoro";
                    NappulaEnable(true, true);
                }
                return;
            }

            if (Siirto == true)
            {

                NappulaEnable(true, false);
                NappulaEnable(false, false);

                ((PeliNappula)sender).Vari = siirrettavanVari;
                ((PeliNappula)sender).UnCheckedVari = Color.FromRgb(0, 0, 0);

                Siirto = false;
                vuoro = !vuoro;
                if (vuoro)
                    infolabel.Content = "Pelaaja 1 siirtovuoro";
                else infolabel.Content = "Pelaaja 2 siirtovuoro";
                /*if (OnkoMylly((PeliNappula)sender)) // poisto-operaatio
                {
                    if (!vuoro)
                        infolabel.Content = "Pelaaja 1 mylly";
                    else infolabel.Content = "Pelaaja 2 mylly";
                }*/
                NappulaEnable(true, true);


                VarinVaihto(siirtoOkVari, Color.FromRgb(0, 0, 0), false);

                return;
            }

        }

        private void PeliNappula_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Lisays == true)
            {
                NappulaEnable(false, false);
                Lisays = false;
                if (Poisto)
                {
                    if (vuoro) p1_nappuloita--;
                    else p2_nappuloita--;
                    MAX_NAPPULOITA--;
                    NappulaEnable(true, false);
                    return;
                }
            }
            if (Poisto == true)
            {
                if (vuoro) p1_nappuloita--;
                else p2_nappuloita--;
                if (p1_nappuloita == 2)
                {
                    infolabel.Content = "P2 voitti";
                    NappulaEnable(false, false);
                    NappulaEnable(true, false);
                    SystemSounds.Asterisk.Play();
                    return;
                }

                if (p2_nappuloita == 2)
                {
                    infolabel.Content = "P1 voitti";
                    NappulaEnable(false, false);
                    NappulaEnable(true, false);
                    SystemSounds.Asterisk.Play();
                    return;
                }
                NappulaEnable(true, false);
                NappulaEnable(false, false);
                //vuoro = !vuoro;
                NappulaEnable(true, true);
                Poisto = false;
                joPoistettu = true;
                return;
            }
            Siirto = true;
            NappulaEnable(true, false);
            siirrettavanVari = ((PeliNappula)sender).Vari;

            EnableSiirto((PeliNappula)sender);
        }

        void RedoCmdExecuted(object target, ExecutedRoutedEventArgs e)
        {
            Lisays = false;
            Siirto = false;
            Poisto = false;
            NappulaEnable(true, true);
            vuoro = !vuoro;
            NappulaEnable(true, true);
            foreach (var item in grid.Children)
            {
                if (item is OwnCanvas)
                {
                    foreach (var nappula in ((OwnCanvas)item).Children)
                    {
                        if (nappula is PeliNappula)
                        {
                            ((PeliNappula)nappula).IsEnabled = true;
                            ((PeliNappula)nappula).IsChecked = false;
                        }

                    }
                }
            }
            p1_nappuloita = 0;
            p2_nappuloita = 0;
            nappuloidenMaara = 0;
            MAX_NAPPULOITA = MAX_NAPPULOITA_ORIG;
            NappulaEnable(false, true);
            VarinVaihto(siirtoOkVari, Color.FromRgb(0, 0, 0), false);
            NappulaEnable(false, false);
            infolabel.Content = "Uusi peli on alkanut";
        }

        private void testButton_Click(object sender, RoutedEventArgs e)
        {
            Lisays = true;

            NappulaEnable(false, true);
            Siirto = false;
            Poisto = false;
        }

        public void newButton()
        {
            Lisays = true;

            NappulaEnable(false, true);
            Siirto = false;
            Poisto = false;
        }

        public void redo()
        {
            Lisays = false;
            Siirto = false;
            Poisto = false;
            NappulaEnable(true, true);
            vuoro = !vuoro;
            NappulaEnable(true, true);
            foreach (var item in grid.Children)
            {
                if (item is OwnCanvas)
                {
                    foreach (var nappula in ((OwnCanvas)item).Children)
                    {
                        if (nappula is PeliNappula)
                        {
                            ((PeliNappula)nappula).IsEnabled = true;
                            ((PeliNappula)nappula).IsChecked = false;
                        }

                    }
                }
            }
            p1_nappuloita = 0;
            p2_nappuloita = 0;
            nappuloidenMaara = 0;
            MAX_NAPPULOITA = MAX_NAPPULOITA_ORIG;
            NappulaEnable(false, true);
            VarinVaihto(siirtoOkVari, Color.FromRgb(0, 0, 0), false);
            NappulaEnable(false, false);
            infolabel.Content = "Uusi peli on alkanut";

        }

        public void move()
        {
            Siirto = true;
            NappulaEnable(true, true);
            Lisays = false;
            Poisto = false;
        }

        public void delete()
        {
            NappulaEnable(true, true);
            Poisto = true;
            NappulaEnable(true, true);
            Lisays = false;
            Siirto = false;
        }

        public void setUp(Color p1V, Color p2V, string p1n, string p2n)
        {
            Color p1VariOld = p1Vari;
            Color p2VariOld = p2Vari;
            p1Vari = p1V;
            p2Vari = p2V;
            p1Name.Content = p1n;
            p2Name.Content = p2n;

            VarinVaihto(p1VariOld, p1Vari, true);
            VarinVaihto(p2VariOld, p2Vari, true);
        }
    }


    /// <summary>
    /// Oma canvas-luokka, joka lisää pari propertya avuksi. HalfWidth ja HalfHeight antavat puolitetun todellisen leveyden ja korkeuden ja näihin voi bindata xamlista
    /// </summary>
    public class OwnCanvas : Canvas
    {
        public OwnCanvas()
        {
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            SizeChanged += OwnCanvas_SizeChanged;
        }

        void OwnCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            HalfWidth = ActualWidth / 2;
            HalfHeight = ActualHeight / 2;
        }


        public double HalfWidth
        {
            get { return (double)GetValue(HalfWidthProperty); }
            set { SetValue(HalfWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HalfWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HalfWidthProperty =
            DependencyProperty.Register("HalfWidth", typeof(double), typeof(OwnCanvas), new PropertyMetadata(0.0));


        public double HalfHeight
        {
            get { return (double)GetValue(HalfHeightProperty); }
            set { SetValue(HalfHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HalfHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HalfHeightProperty =
            DependencyProperty.Register("HalfHeight", typeof(double), typeof(OwnCanvas), new PropertyMetadata(0.0));



    }

    public class PeliNappula : CheckBox
    {
        public static readonly DependencyProperty VariProperty =
            DependencyProperty.Register("Vari", typeof(Color), typeof(PeliNappula), new FrameworkPropertyMetadata(Color.FromRgb(121, 120, 12), FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty UnCheckedVariProperty =
            DependencyProperty.Register("UnCheckedVari", typeof(Color), typeof(PeliNappula), new FrameworkPropertyMetadata(Color.FromRgb(1, 1, 1), FrameworkPropertyMetadataOptions.AffectsRender));

        public Color Vari
        {
            get { return (Color)GetValue(VariProperty); }
            set { SetValue(VariProperty, value); }
        }

        public Color UnCheckedVari
        {
            get { return (Color)GetValue(UnCheckedVariProperty); }
            set { SetValue(UnCheckedVariProperty, value); }
        }

        public PeliNappula()
        {
            //Vari = Color.FromRgb(12, 12, 12);
        }
    }
}
