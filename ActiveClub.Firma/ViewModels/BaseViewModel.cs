using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

using ActiveClub.Firma.Helper;

namespace ActiveClub.Firma.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged // obsluguje powiadomienia o zmianach wlasciwosci
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #region DisplayName
        public virtual string DisplayName { get; protected set; }
        #endregion
        #region WindowPropertys

        public void ShowMessageBox(string message)
        {
            MessageBox.Show(message, "", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public ICommand Close
        {
            get { return new BaseCommand(CloseApplication); }
        }

        public ICommand Maximice
        {
            get { return new BaseCommand(MaximiceApplication); }
        }

        public ICommand Minimice
        {
            get { return new BaseCommand(MinimiceApplication); }
        }

        public ICommand DragMove
        {
            get { return new BaseCommand(DragMoveCommand); }
        }

        public ICommand Restart
        {
            get { return new BaseCommand(RestartCommand); }
        }

        private static void RestartCommand()
        {
            Application.Current.Shutdown();
        }

        private static void DragMoveCommand()
        {
            Application.Current.MainWindow.DragMove();
        }

        private static void CloseApplication()
        {
            Application.Current.Shutdown();
        }

        private static void MaximiceApplication()
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            else
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }

        private static void MinimiceApplication()
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Minimized)
            {
                Application.Current.MainWindow.Opacity = 1;
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                Application.Current.MainWindow.Opacity = 0;
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
            }
        }

        #endregion
    }
}

