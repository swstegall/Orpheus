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
        public static RoutedCommand StopPlaybackCmd = new RoutedCommand();
        public static RoutedCommand PlayPausePlaybackCmd = new RoutedCommand();
        public WaveOutEvent waveOut;

        //JSONHandler object that will take care of the JSON interactions  - Isaac
        JSONHandler handler;

        //List of songs from the JSON file  - Isaac
        SongList listOfSongs;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.listOfSongs.List.ForEach(song =>
            {
                Playlist.Items.Add(new Song() { Title = song.Title, Artist = song.Artist, Album = song.Album, Track = song.Track, Name = song.SongName });
            });
        }

        public MainWindow()
        {
            InitializeComponent();
            //Builds a new JSONHandler object that will take care of the JSON interactions  - Isaac
            this.handler = new JSONHandler();
            //Returns the list of songs from the JSON file or an empty SongList object if none existed or it failed  - Isaac
            this.listOfSongs = this.handler.ReadJsonFile();
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

            Playlist.Items.Clear();

            this.listOfSongs.List.ForEach(song =>
            {
                Playlist.Items.Add(new Song() { Title = song.Title, Artist = song.Artist, Album = song.Album, Track = song.Track, Name = song.SongName });
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

            Playlist.Items.Clear();

            this.listOfSongs.List.ForEach(song =>
            {
                Playlist.Items.Add(new Song() { Title = song.Title, Artist = song.Artist, Album = song.Album, Track = song.Track, Name = song.SongName });
            });

            // var musicReader = new MediaFoundationReader(this.listOfSongs.List[this.listOfSongs.List.Count - 1].FilePath);
            // waveOut.Init(musicReader);
            // waveOut.Play();

            //This will write everything in the passed in SongList object to the JSON file - Isaac
            this.handler.WriteToJSONFile(this.listOfSongs);
        }

        void SongRowDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as Song;
            if (item != null)
            {
                var song = this.listOfSongs.List.Find(x => x.SongName == item.Name);
                var musicReader = new MediaFoundationReader(song.FilePath);
                waveOut.Stop();
                waveOut.Init(musicReader);
                waveOut.Play();
            }
        }

        void StopPlaybackCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            waveOut.Stop();
        }

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
                    waveOut.Play();
                }
            }
            else
            {
                waveOut.Play();
            }
        }

        private void CloseCmdExecuted(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
