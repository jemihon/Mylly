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
using Xceed.Wpf.Toolkit;

namespace SettingsLibrary
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public bool canceling { get; set; }

        public string p1nimi { get; set; }
        public string p2nimi { get; set; }

        public static readonly DependencyProperty P1VariProperty = DependencyProperty.Register(
                           "P1Vari",
                           typeof(Color),
                           typeof(Settings),
                           new FrameworkPropertyMetadata(Color.FromRgb(120, 12, 12),
                                FrameworkPropertyMetadataOptions.AffectsRender
                                ));

        public static readonly DependencyProperty P2VariProperty = DependencyProperty.Register(
                           "P2Vari",
                           typeof(Color),
                           typeof(Settings),
                           new FrameworkPropertyMetadata(Color.FromRgb(120, 110, 12),
                                FrameworkPropertyMetadataOptions.AffectsRender
                                ));

        public Color P1Vari
        {
            get { return (Color)GetValue(P1VariProperty); }
            set { SetValue(P1VariProperty, value); }
        }

        public Color P2Vari
        {
            get { return (Color)GetValue(P2VariProperty); }
            set { SetValue(P2VariProperty, value); }
        }

        public Settings()
        {            
            InitializeComponent();
            canceling = true;
        }

        private void colorPickerP1_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            P1Vari = (Color)((ColorPicker)sender).SelectedColor;
        }

        private void colorPickerP2_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            P2Vari = (Color)((ColorPicker)sender).SelectedColor;
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            canceling = false;
            this.Close();
        }

        private void p1Nimi_TextChanged(object sender, TextChangedEventArgs e)
        {
            p1nimi = p1Nimi.Text;
        }

        private void p2Nimi_TextChanged(object sender, TextChangedEventArgs e)
        {
            p2nimi = p2Nimi.Text;
        }

    }
}
