using Microsoft.Win32;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

class Song
{
    public string Title { get; set; }
    public string Artist { get; set; }
    public string Album { get; set; }
    public int Track { get; set; }
    public string Name { get; set; }
    public string Error { get; set; }
}

namespace Orpheus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // These are class members used for routing commands, as well as the core NAudio object. - Sam
        public static RoutedCommand ScanCmd = new RoutedCommand();
        public static RoutedCommand OpenFileCmd = new RoutedCommand();
        public static RoutedCommand OpenFolderCmd = new RoutedCommand();
        public static RoutedCommand StopPlaybackCmd = new RoutedCommand();
        public static RoutedCommand PlayPausePlaybackCmd = new RoutedCommand();
        public WaveOutEvent waveOut;

        // We track whether NAudio is properly initialized to prevent crashing on Play/Stop/Pause errors - Sam
        public bool initialized;

        //JSONHandler object that will take care of the JSON interactions  - Isaac
        JSONHandler handler;

        //List of songs from the JSON file  - Isaac
        SongList listOfSongs;

        // This will load songs from the JSON file when the application is cold-loaded - Sam
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.listOfSongs.List.ForEach(song =>
            {
                Playlist.Items.Add(new Song() { Title = song.Title, Artist = song.Artist, Album = song.Album, Track = song.Track, Name = song.SongName, Error = song.Error });
            });
        }

        // Main window initializes with existing songs shown by the music_storage.json file. - Sam
        public MainWindow()
        {
            InitializeComponent();
            //Builds a new JSONHandler object that will take care of the JSON interactions  - Isaac
            this.handler = new JSONHandler();
            //Returns the list of songs from the JSON file or an empty SongList object if none existed or it failed  - Isaac
            this.listOfSongs = this.handler.ReadJsonFile();

            // Here, NAudio is initialized. - Sam
            this.waveOut = new WaveOutEvent();
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

            // Playlist is refreshed when scan is executed - Sam
            Playlist.Items.Clear();

            this.listOfSongs.List.ForEach(song =>
            {
                Playlist.Items.Add(new Song() { Title = song.Title, Artist = song.Artist, Album = song.Album, Track = song.Track, Name = song.SongName , Error = song.Error});
            });
            //This will write everything in the passed in SongList object to the JSON file - Isaac
            this.handler.WriteToJSONFile(this.listOfSongs);
        }

        // Still doesn't completely work in clearing out and then adding files to the playlist.
        // Changes currently are only reflected on relaunch of the application.
        // May not be too big of an issue if we go the route of just marking an invalid song with red text. - Sam
        private void OpenFileCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.waveOut.Stop();
            //Calling this will open the file selection window to choose a song from the users machine - Isaac
            //Will add the new song to the SongList object - Isaac
            this.listOfSongs.AddSongLocation();

            // Playlist is refreshed when a new song is added - Sam
            Playlist.Items.Clear();

            this.listOfSongs.List.ForEach(song =>
            {
                Playlist.Items.Add(new Song() { Title = song.Title, Artist = song.Artist, Album = song.Album, Track = song.Track, Name = song.SongName, Error = song.Error});
            });

            //This will write everything in the passed in SongList object to the JSON file - Isaac
            this.handler.WriteToJSONFile(this.listOfSongs);
        }

        // Callback to play a song on double click - Sam
        void SongRowDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as Song;
            if (item != null)
            {
                var song = this.listOfSongs.List.Find(x => x.SongName == item.Name);
                var musicReader = new MediaFoundationReader(song.FilePath);
                this.initialized = true;
                waveOut.Stop();
                waveOut.Init(musicReader);
                waveOut.Play();
            }
        }

        // Stops a song's playback completely and changes tracked NAudio initialized state - Sam
        void StopPlaybackCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            waveOut.Stop();
            waveOut.Dispose();
            this.initialized = false;
        }

        // Play/Pause callback that uses NAudio initialized tracking to prevent a crash in a Stop -> Play event - Sam
        void PlayPausePlaybackCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (waveOut != null)
            {
                if (waveOut.PlaybackState == PlaybackState.Playing)
                {
                    waveOut.Pause();
                }
                else
                {
                    if (this.initialized)
                    {
                        waveOut.Play();
                    }
                }
            }
        }
        // Closes the application outright when the close or Ctrl+W is pressed - Sam
        private void CloseCmdExecuted(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
