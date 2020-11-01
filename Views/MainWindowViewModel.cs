using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Orpheus.Views
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _title;
        private double _currentTrackLenght;
        private double _currentTrackPosition;
        private string _playPauseImageSource;
        private float _currentVolume;

        private ObservableCollection<Track> _playlist;
        private Track _currentlyPlayingTrack;
        private Track _currentlySelectedTrack;
        private AudioPlayer _audioPlayer;

        private enum PlaybackState
        {
            Playing, Stopped, Paused
        }

        private PlaybackState _playbackState;

        public string Title
        {
            get { return _title; }
            set
            {
                if (value == _title) return;
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public string PlayPauseImageSource
        {
            get { return _playPauseImageSource; }
            set
            {
                if (value == _playPauseImageSource) return;
                _playPauseImageSource = value;
                OnPropertyChanged(nameof(PlayPauseImageSource));
            }
        }

        public float CurrentVolume
        {
            get { return _currentVolume; }
            set
            {

                if (value.Equals(_currentVolume)) return;
                _currentVolume = value;
                OnPropertyChanged(nameof(CurrentVolume));
            }
        }

        public double CurrentTrackLenght
        {
            get { return _currentTrackLenght; }
            set
            {
                if (value.Equals(_currentTrackLenght)) return;
                _currentTrackLenght = value;
                OnPropertyChanged(nameof(CurrentTrackLenght));
            }
        }

        public double CurrentTrackPosition
        {
            get { return _currentTrackPosition; }
            set
            {
                if (value.Equals(_currentTrackPosition)) return;
                _currentTrackPosition = value;
                OnPropertyChanged(nameof(CurrentTrackPosition));
            }
        }

        public Track CurrentlySelectedTrack
        {
            get { return _currentlySelectedTrack; }
            set
            {
                if (Equals(value, _currentlySelectedTrack)) return;
                _currentlySelectedTrack = value;
                OnPropertyChanged(nameof(CurrentlySelectedTrack));
            }
        }

        public Track CurrentlyPlayingTrack
        {
            get { return _currentlyPlayingTrack; }
            set
            {
                if (Equals(value, _currentlyPlayingTrack)) return;
                _currentlyPlayingTrack = value;
                OnPropertyChanged(nameof(CurrentlyPlayingTrack));
            }
        }

        public ObservableCollection<Track> Playlist
        {
            get { return _playlist; }
            set
            {
                if (Equals(value, _playlist)) return;
                _playlist = value;
                OnPropertyChanged(nameof(Playlist));
            }    
        }

        public ICommand ExitApplicationCommand { get; set; }
        public ICommand AddFileToPlaylistCommand { get; set; }
        public ICommand AddFolderToPlaylistCommand { get; set; }
        public ICommand SavePlaylistCommand { get; set; }
        public ICommand LoadPlaylistCommand { get; set; }

        public ICommand RewindToStartCommand { get; set; }
        public ICommand StartPlaybackCommand { get; set; }
        public ICommand StopPlaybackCommand { get; set; }
        public ICommand ForwardToEndCommand { get; set; }
        public ICommand ShuffleCommand { get; set; }

        public ICommand TrackControlMouseDownCommand { get; set; }
        public ICommand TrackControlMouseUpCommand { get; set; }
        public ICommand VolumeControlValueChangedCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            System.Windows.Application.Current.MainWindow.Closing += MainWindow_Closing;

            Title = "Orpheus";

            LoadCommands();

            Playlist = new ObservableCollection<Track>();

            _playbackState = PlaybackState.Stopped;

            PlayPauseImageSource = "Play";
            CurrentVolume = 1;
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (_audioPlayer != null)
            {
                _audioPlayer.Dispose();
            }
        }

        private void LoadCommands()
        {
            // Menu commands
            ExitApplicationCommand = new RelayCommand(ExitApplication, CanExitApplication);
            AddFileToPlaylistCommand = new RelayCommand(AddFileToPlaylist, CanAddFileToPlaylist);
            AddFolderToPlaylistCommand = new RelayCommand(AddFolderToPlaylist, CanAddFolderToPlaylist);
            SavePlaylistCommand = new RelayCommand(SavePlaylist, CanSavePlaylist);
            LoadPlaylistCommand = new RelayCommand(LoadPlaylist, CanLoadPlaylist);

            // Player commands
            RewindToStartCommand = new RelayCommand(RewindToStart, CanRewindToStart);
            StartPlaybackCommand = new RelayCommand(StartPlayback, CanStartPlayback);
            StopPlaybackCommand = new RelayCommand(StopPlayback, CanStopPlayback);
            ForwardToEndCommand = new RelayCommand(ForwardToEnd, CanForwardToEnd);
            ShuffleCommand = new RelayCommand(Shuffle, CanShuffle);

            // Event commands
            TrackControlMouseDownCommand = new RelayCommand(TrackControlMouseDown, CanTrackControlMouseDown);
            TrackControlMouseUpCommand = new RelayCommand(TrackControlMouseUp, CanTrackControlMouseUp);
            VolumeControlValueChangedCommand = new RelayCommand(VolumeControlValueChanged, CanVolumeControlValueChanged);
        }

        // Menu commands
        private void ExitApplication(object p)
        {
            if (_audioPlayer != null)
            {
                _audioPlayer.Dispose();
            }

            System.Windows.Application.Current.Shutdown();
        }
        private bool CanExitApplication(object p)
        {
            return true;
        }

        private void AddFileToPlaylist(object p)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Choose Your Song";
            open.Filter = "Music Files (*.mp3, *.wav, *.ogg)|*.mp3;*.wav;*.ogg";
            if (open.ShowDialog() == DialogResult.OK)
            {
                string FilePath = open.FileName;
                TagLib.File tagFile = TagLib.File.Create(FilePath);
                Playlist.Add(new Track(FilePath, tagFile.Tag.Title));
            }
        }

        private bool CanAddFileToPlaylist(object p)
        {
            if (_playbackState == PlaybackState.Stopped)
            {
                return true;
            }
            return false;
        }

        private void AddFolderToPlaylist(object p)
        {
            var cofd = new CommonOpenFileDialog();
            cofd.IsFolderPicker = true;
            var result = cofd.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                var folderName = cofd.FileName;
                var audioFiles = Directory.EnumerateFiles(folderName, "*.*", SearchOption.AllDirectories)
                                          .Where(f => f.EndsWith(".wav") || f.EndsWith(".mp3") || f.EndsWith(".wma") || f.EndsWith(".ogg") || f.EndsWith(".flac"));
                foreach (var audioFile in audioFiles)
                {
                    // var removePath = audioFile.RemovePath();
                    // var friendlyName = removePath.Remove(removePath.Length - 4);
                    var track = new Track(audioFile, "test");
                    Playlist.Add(track);
                }
                Playlist = new ObservableCollection<Track>(Playlist.OrderBy(z => z.friendlyName).ToList());
            }
        }

        private bool CanAddFolderToPlaylist(object p)
        {
            if (_playbackState == PlaybackState.Stopped)
            {
                return true;
            }
            return false;
        }

        private void SavePlaylist(object p)
        {
            var sfd = new SaveFileDialog();
            sfd.CreatePrompt = false;
            sfd.OverwritePrompt = true;
            sfd.Filter = "PLAYLIST files (*.playlist) | *.playlist";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                // var ps = new PlaylistSaver();
                // ps.Save(Playlist, sfd.FileName); // save the playlist
            }
        }

        private bool CanSavePlaylist(object p)
        {
            return true;
        }

        private void LoadPlaylist(object p)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "PLAYLIST files (*.playlist) | *.playlist";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                // Playlist = new PlaylistLoader().Load(ofd.FileName).ToObservableCollection(); // load the playlist
            }
        }

        private bool CanLoadPlaylist(object p)
        {
            return true;
        }

        // Player commands
        private void RewindToStart(object p)
        {
            _audioPlayer.SetPosition(0); // set position to zero
        }
        private bool CanRewindToStart(object p)
        {
            if (_playbackState == PlaybackState.Playing)
            {
                return true;
            }
            return false;
        }

        private void StartPlayback(object p)
        {
            if (CurrentlySelectedTrack != null)
            {
                if (_playbackState == PlaybackState.Stopped)
                {
                    _audioPlayer = new AudioPlayer(CurrentlySelectedTrack.fileName, CurrentVolume);
                    _audioPlayer.PlaybackStopType = AudioPlayer.PlaybackStopTypes.PlaybackStoppedReachingEndOfFile;
                    _audioPlayer.PlaybackPaused += _audioPlayer_PlaybackPaused;
                    _audioPlayer.PlaybackResumed += _audioPlayer_PlaybackResumed;
                    _audioPlayer.PlaybackStopped += _audioPlayer_PlaybackStopped;
                    CurrentTrackLenght = _audioPlayer.GetLenghtInSeconds();
                    CurrentlyPlayingTrack = CurrentlySelectedTrack;
                }
                if (CurrentlySelectedTrack == CurrentlyPlayingTrack)
                {
                    _audioPlayer.TogglePlayPause(CurrentVolume);
                }
            }
        }
        private bool CanStartPlayback(object p)
        {
            if (CurrentlySelectedTrack != null)
            {
                return true;
            }
            return false;
        }

        private void StopPlayback(object p)
        {
            if (_audioPlayer != null)
            {
                _audioPlayer.PlaybackStopType = AudioPlayer.PlaybackStopTypes.PlaybackStoppedByUser;
                _audioPlayer.Stop();
            }
        }
        private bool CanStopPlayback(object p)
        {
            if (_playbackState == PlaybackState.Playing || _playbackState == PlaybackState.Paused)
            {
                return true;
            }
            return false;
        }

        private void ForwardToEnd(object p)
        {
            if (_audioPlayer != null)
            {
                _audioPlayer.PlaybackStopType = AudioPlayer.PlaybackStopTypes.PlaybackStoppedReachingEndOfFile;
                _audioPlayer.SetPosition(_audioPlayer.GetLenghtInSeconds());
            }
        }
        private bool CanForwardToEnd(object p)
        {
            if (_playbackState == PlaybackState.Playing)
            {
                return true;
            }
            return false;
        }

        private void Shuffle(object p)
        {
            Playlist = Playlist.Shuffle();
        }
        private bool CanShuffle(object p)
        {
            if (_playbackState == PlaybackState.Stopped)
            {
                return true;
            }
            return false;
        }

        // Events
        private void TrackControlMouseDown(object p)
        {
            if (_audioPlayer != null)
            {
                _audioPlayer.Pause();
            }
        }

        private void TrackControlMouseUp(object p)
        {
            if (_audioPlayer != null)
            {
                _audioPlayer.SetPosition(CurrentTrackPosition);
                _audioPlayer.Play(NAudio.Wave.PlaybackState.Paused, CurrentVolume);
            }
        }

        private bool CanTrackControlMouseDown(object p)
        {
            if (_playbackState == PlaybackState.Playing)
            {
                return true;
            }
            return false;
        }

        private bool CanTrackControlMouseUp(object p)
        {
            if (_playbackState == PlaybackState.Paused)
            {
                return true;
            }
            return false;
        }

        private void VolumeControlValueChanged(object p)
        {
            if (_audioPlayer != null)
            {
                _audioPlayer.SetVolume(CurrentVolume); // set value of the slider to current volume
            }
        }

        private bool CanVolumeControlValueChanged(object p)
        {
            return true;
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void _audioPlayer_PlaybackStopped()
        {
            _playbackState = PlaybackState.Stopped;
            PlayPauseImageSource = "Play";
            CommandManager.InvalidateRequerySuggested();
            CurrentTrackPosition = 0;

            if (_audioPlayer.PlaybackStopType == AudioPlayer.PlaybackStopTypes.PlaybackStoppedReachingEndOfFile)
            {
                // CurrentlySelectedTrack = Playlist.NextItem(CurrentlyPlayingTrack);
                StartPlayback(null);
            }
        }

        private void _audioPlayer_PlaybackResumed()
        {
            _playbackState = PlaybackState.Playing;
            PlayPauseImageSource = "Pause";
        }

        private void _audioPlayer_PlaybackPaused()
        {
            _playbackState = PlaybackState.Paused;
            PlayPauseImageSource = "Play";
        }
    }
}
