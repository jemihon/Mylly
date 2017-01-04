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
using SettingsLibrary;
using AboutLibrary;
using System.ComponentModel;
using System.Media;
using PeliAlueLibrary;

namespace mylly
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {

            //vuoro = rnd.NextDouble() >= 0.5;
            InitializeComponent();
            //if (vuoro) infolabel.Content = "P1 lisäysvuoro";
            //else infolabel.Content = "P2 lisäysvuoro";


        }

        

        void NewCmdExecuted(object target, ExecutedRoutedEventArgs e)
        {
            alue.newButton();
        }

        void RedoCmdExecuted(object target, ExecutedRoutedEventArgs e)
        {
            alue.redo();
        }


        void OpenCmdExecuted(object target, ExecutedRoutedEventArgs e)
        {
            alue.move();
        }

        void DeleteCmdExecuted(object target, ExecutedRoutedEventArgs e)
        {
            alue.delete();
        }

        void ReplaceCmdExecuted(object target, ExecutedRoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.Closing += new CancelEventHandler(settings_Closing);
            settings.ShowDialog();
        }

        void DeleteCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (alue.Poisto) e.CanExecute = true;
        }

        void NewCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (alue.nappuloidenMaara < alue.MAX_NAPPULOITA)
                e.CanExecute = true;
        }

        private void settings_Closing(object sender, CancelEventArgs e)
        {
            if (((Settings)sender).canceling) return;

            Color p1V = ((Settings)sender).P1Vari;
            Color p2V = ((Settings)sender).P2Vari;
            string p1n = ((Settings)sender).p1nimi;
            string p2n = ((Settings)sender).p2nimi;

            alue.setUp(p1V, p2V, p1n, p2n);
        }


        private void menuAbout_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }


 




    }

   


}
