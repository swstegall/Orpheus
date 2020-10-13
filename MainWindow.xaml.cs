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

        //Build a new JSONHandler object that will take care of the JSON interactions  - Isaac
        JSONHandler handler;

        //Returns the list of songs from the JSON file or an empty SongList object if none existed or it failed  - Isaac
        SongList listOfSongs;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.listOfSongs.List.ForEach(song =>
            {
                Playlist.Items.Add(new Song() { Name = song.SongName, Artist = "Undefined", Album = "Undefined", Track = 0, Length = "99:99" });
            });
        }

        public MainWindow()
        {
            InitializeComponent();

            this.handler = new JSONHandler();

            this.listOfSongs = this.handler.ReadJsonFile();
            Console.WriteLine(this.listOfSongs.List);
        }

        private void ScanCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            //Calling .VerifyPaths() will go through every path stored in the SongList object and return a list of SongLocations - Isaac
            //The returned list contains all of the SongLocation objects with bad paths - Isaac
            List<SongLocation> badPaths = this.listOfSongs.VerifyPaths();

            //Remove all the bad paths from the JSON file.
            badPaths.ForEach(song =>
            {
                this.listOfSongs.RemoveSongLocation(song.Id);
            });

            Playlist.Items.Clear();

            this.listOfSongs.List.ForEach(song =>
            {
                Playlist.Items.Add(new Song() { Name = song.SongName, Artist = "Undefined", Album = "Undefined", Track = 0, Length = "99:99" });
            });

            //This will write everything in the passed in SongList object to the JSON file - Isaac
            this.handler.WriteToJSONFile(this.listOfSongs);
        }

        // Still doesn't completely work in clearing out and then adding files to the playlist.
        // Changes currently are only reflected on relaunch of the application.
        // May not be too big of an issue if we go the route of just marking an invalid song with red text. - Sam
        private void OpenFileCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            //Calling this will open the file selection window to choose a song from the users machine - Isaac
            //Will add the new song to the SongList object - Isaac
            this.listOfSongs.AddSongLocation();

            Playlist.Items.Clear();

            this.listOfSongs.List.ForEach(song =>
            {
                Playlist.Items.Add(new Song() { Name = song.SongName, Artist = "Undefined", Album = "Undefined", Track = 0, Length = "99:99" });
            });

            //This will write everything in the passed in SongList object to the JSON file - Isaac
            this.handler.WriteToJSONFile(this.listOfSongs);
        }

        private void CloseCmdExecuted(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
