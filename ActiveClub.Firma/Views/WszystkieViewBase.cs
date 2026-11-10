using System.Windows;
using System.Windows.Controls;

namespace ActiveClub.Firma.Views
{
    public class WszystkieViewBase : UserControl
    {
        static WszystkieViewBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(WszystkieViewBase),
                new FrameworkPropertyMetadata(typeof(WszystkieViewBase)));
        }
    }
}
