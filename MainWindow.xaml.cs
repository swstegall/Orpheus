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

class Song
{
    public string Name { get; set; }
    
    public string Artist { get; set; }
    public string Album { get; set; }
    public int Track { get; set; }
    public string Length { get; set; }
}

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Playlist.Items.Add(new Song() { Name = "Only the Young", Artist = "Journey", Album = "Greatest Hits", Track = 1, Length = "4:17" });
            Playlist.Items.Add(new Song() { Name = "Don't Stop Believin", Artist = "Journey", Album = "Greatest Hits", Track = 2, Length = "4:10" });
            Playlist.Items.Add(new Song() { Name = "Wheel in the Sky", Artist = "Journey", Album = "Greatest Hits", Track = 3, Length = "4:12" });
            Playlist.Items.Add(new Song() { Name = "Faithfully", Artist = "Journey", Album = "Greatest Hits", Track = 4, Length = "4:27" });
            Playlist.Items.Add(new Song() { Name = "I'll Be Alright Without You", Artist = "Journey", Album = "Greatest Hits", Track = 5, Length = "4:50" });
            Playlist.Items.Add(new Song() { Name = "Any Way You Want It", Artist = "Journey", Album = "Greatest Hits", Track = 6, Length = "3:21" });
            Playlist.Items.Add(new Song() { Name = "Ask the Lonely", Artist = "Journey", Album = "Greatest Hits", Track = 7, Length = "3:54" });
            Playlist.Items.Add(new Song() { Name = "Who's Crying Now?", Artist = "Journey", Album = "Greatest Hits", Track = 8, Length = "5:00" });
            Playlist.Items.Add(new Song() { Name = "Separate Ways (Worlds Apart)", Artist = "Journey", Album = "Greatest Hits", Track = 9, Length = "5:24" });
            Playlist.Items.Add(new Song() { Name = "Lights", Artist = "Journey", Album = "Greatest Hits", Track = 10, Length = "3:10" });
            Playlist.Items.Add(new Song() { Name = "Lovin', Touchin' Squeezin'", Artist = "Journey", Album = "Greatest Hits", Track = 11, Length = "3:54" });
            Playlist.Items.Add(new Song() { Name = "Open Arms", Artist = "Journey", Album = "Greatest Hits", Track = 12, Length = "3:23" });
            Playlist.Items.Add(new Song() { Name = "Girl Can't Help It", Artist = "Journey", Album = "Greatest Hits", Track = 13, Length = "3:50" });
            Playlist.Items.Add(new Song() { Name = "Send Her My Love", Artist = "Journey", Album = "Greatest Hits", Track = 14, Length = "3:54" });
            Playlist.Items.Add(new Song() { Name = "Be Good to Yourself", Artist = "Journey", Album = "Greatest Hits", Track = 15, Length = "3:51" });
            Playlist.Items.Add(new Song() { Name = "When You Love a Woman", Artist = "Journey", Album = "Greatest Hits", Track = 16, Length = "4:07" });
            Playlist.Items.Add(new Song() { Name = "Only the Young", Artist = "Journey", Album = "Greatest Hits", Track = 1, Length = "4:17" });
            Playlist.Items.Add(new Song() { Name = "Don't Stop Believin", Artist = "Journey", Album = "Greatest Hits", Track = 2, Length = "4:10" });
            Playlist.Items.Add(new Song() { Name = "Wheel in the Sky", Artist = "Journey", Album = "Greatest Hits", Track = 3, Length = "4:12" });
            Playlist.Items.Add(new Song() { Name = "Faithfully", Artist = "Journey", Album = "Greatest Hits", Track = 4, Length = "4:27" });
            Playlist.Items.Add(new Song() { Name = "I'll Be Alright Without You", Artist = "Journey", Album = "Greatest Hits", Track = 5, Length = "4:50" });
            Playlist.Items.Add(new Song() { Name = "Any Way You Want It", Artist = "Journey", Album = "Greatest Hits", Track = 6, Length = "3:21" });
            Playlist.Items.Add(new Song() { Name = "Ask the Lonely", Artist = "Journey", Album = "Greatest Hits", Track = 7, Length = "3:54" });
            Playlist.Items.Add(new Song() { Name = "Who's Crying Now?", Artist = "Journey", Album = "Greatest Hits", Track = 8, Length = "5:00" });
            Playlist.Items.Add(new Song() { Name = "Separate Ways (Worlds Apart)", Artist = "Journey", Album = "Greatest Hits", Track = 9, Length = "5:24" });
            Playlist.Items.Add(new Song() { Name = "Lights", Artist = "Journey", Album = "Greatest Hits", Track = 10, Length = "3:10" });
            Playlist.Items.Add(new Song() { Name = "Lovin', Touchin' Squeezin'", Artist = "Journey", Album = "Greatest Hits", Track = 11, Length = "3:54" });
            Playlist.Items.Add(new Song() { Name = "Open Arms", Artist = "Journey", Album = "Greatest Hits", Track = 12, Length = "3:23" });
            Playlist.Items.Add(new Song() { Name = "Girl Can't Help It", Artist = "Journey", Album = "Greatest Hits", Track = 13, Length = "3:50" });
            Playlist.Items.Add(new Song() { Name = "Send Her My Love", Artist = "Journey", Album = "Greatest Hits", Track = 14, Length = "3:54" });
            Playlist.Items.Add(new Song() { Name = "Be Good to Yourself", Artist = "Journey", Album = "Greatest Hits", Track = 15, Length = "3:51" });
            Playlist.Items.Add(new Song() { Name = "When You Love a Woman", Artist = "Journey", Album = "Greatest Hits", Track = 16, Length = "4:07" });
        }

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
