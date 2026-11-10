using System.Windows;
using System.Windows.Controls;

namespace ActiveClub.Firma.Views
{
    public class JedenViewBase : UserControl
    {
        static JedenViewBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(JedenViewBase), new FrameworkPropertyMetadata(typeof(JedenViewBase)));
        }
    }
}