using ActiveClub.Firma.ViewModels;
using System.Windows;

namespace ActiveClub.Firma.Views
{
    public partial class WybierzUzytkownikaWindow : Window
    {
        public WybierzUzytkownikaWindow(WybierzUzytkownikaViewModel viewModel)
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