using Microsoft.Win32;
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

namespace Orpheus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RoutedCommand ScanCmd = new RoutedCommand();
        public static RoutedCommand OpenFileCmd = new RoutedCommand();
        public static RoutedCommand OpenFolderCmd = new RoutedCommand();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ScanCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("TODO: Scan library for changes.", "Orpheus", MessageBoxButton.OK);
        }

        private void OpenFileCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("TODO: Add file to library.", "Orpheus", MessageBoxButton.OK);
        }

        private void OpenFolderCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("TODO: Add folder to library.", "Orpheus", MessageBoxButton.OK);
        }

        private void CloseCmdExecuted(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
