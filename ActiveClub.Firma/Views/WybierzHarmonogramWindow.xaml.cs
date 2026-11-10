using ActiveClub.Firma.ViewModels;
using System.Windows;

namespace ActiveClub.Firma.Views
{
    public partial class WybierzHarmonogramWindow : Window
    {
        public WybierzHarmonogramWindow(WybierzHarmonogramViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void Wybierz_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}