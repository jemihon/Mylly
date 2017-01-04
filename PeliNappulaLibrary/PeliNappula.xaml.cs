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

namespace PeliNappulaLibrary
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class PeliNappula : CheckBox
    {
        public PeliNappula()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty VariProperty =
            DependencyProperty.Register("Vari", typeof(Color), typeof(PeliNappula), new FrameworkPropertyMetadata(Color.FromRgb(120, 12, 12), FrameworkPropertyMetadataOptions.AffectsRender));

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
    }
}
