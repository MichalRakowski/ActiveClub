using ActiveClub.Firma.ViewModels;
using System.Windows;

namespace ActiveClub.Firma.Views
{
    public partial class WybierzKlubWindow : Window
    {
        public WybierzKlubWindow(WybierzKlubViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;

            viewModel.Accepted += (_, _) =>
            {
                DialogResult = true;
                Close();
            };
        }
    }
}