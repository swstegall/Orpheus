using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace Orpheus.Views
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _title;
        private double _currentTrackLength;
        private double _currentTrackPosition;
        private string _playPauseImageSource;
        private float _currentVolume;
        private JSONHandler _jsonHandler;
        private SongList _jsonSongList;

        private ObservableCollection<Song> _playlist;
        private Song _currentlyPlayingTrack;
        private Song _currentlySelectedTrack;
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

        public double CurrentTrackLength
        {
            get { return _currentTrackLength; }
            set
            {
                if (value.Equals(_currentTrackLength)) return;
                _currentTrackLength = value;
                OnPropertyChanged(nameof(CurrentTrackLength));
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

        public Song CurrentlySelectedTrack
        {
            get { return _currentlySelectedTrack; }
            set
            {
                if (Equals(value, _currentlySelectedTrack)) return;
                _currentlySelectedTrack = value;
                OnPropertyChanged(nameof(CurrentlySelectedTrack));
            }
        }

        public Song CurrentlyPlayingTrack
        {
            get { return _currentlyPlayingTrack; }
            set
            {
                if (Equals(value, _currentlyPlayingTrack)) return;
                _currentlyPlayingTrack = value;
                OnPropertyChanged(nameof(CurrentlyPlayingTrack));
            }
        }

        public ObservableCollection<Song> Playlist
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
        public ICommand AddFileCommand { get; set; }
        public ICommand AddFolderCommand { get; set; }
        public ICommand ScanLibraryCommand { get; set; }
        public ICommand RemoveSongCommand { get; set; }

        public ICommand RewindToStartCommand { get; set; }
        public ICommand StartPlaybackCommand { get; set; }
        public ICommand StopPlaybackCommand { get; set; }
        public ICommand ForwardToEndCommand { get; set; }
        public ICommand ShuffleCommand { get; set; }

        public ICommand RowDoubleClickCommand { get; set; }

        public ICommand TrackControlMouseDownCommand { get; set; }
        public ICommand TrackControlMouseUpCommand { get; set; }
        public ICommand VolumeControlValueChangedCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            System.Windows.Application.Current.MainWindow.Closing += MainWindow_Closing;

            Title = "Orpheus";

            _jsonHandler = new JSONHandler();
            _jsonSongList = this._jsonHandler.ReadJsonFile();
            Playlist = new ObservableCollection<Song>();
            _jsonSongList.List.ForEach(song =>
            {
                Playlist.Add(new Song(song.FilePath, song.Title, song.Artist, song.Album, song.Track, song.Error));
            });

            LoadCommands();

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
            AddFileCommand = new RelayCommand(AddFile, CanAddFile);
            AddFolderCommand = new RelayCommand(AddFolder, CanAddFolder);
            ScanLibraryCommand = new RelayCommand(ScanLibrary, CanScanLibrary);
            RemoveSongCommand = new RelayCommand(RemoveSong, CanRemoveSong);

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

        private void AddFile(object p)
        {
            this._jsonSongList.AddSongLocation();

            Playlist.Clear();

            this._jsonSongList.List.ForEach(song =>
            {
                Playlist.Add(new Song(song.FilePath, song.Title, song.Artist, song.Album, song.Track, song.Error));
            });

            this._jsonHandler.WriteToJSONFile(this._jsonSongList);
        }

        private bool CanAddFile(object p)
        {
            if (_playbackState == PlaybackState.Stopped)
            {
                return true;
            }
            return false;
        }

        private void AddFolder(object p)
        {
            this._jsonSongList.AddFolderOfSongs();

            Playlist.Clear();

            this._jsonSongList.List.ForEach(song =>
            {
                Playlist.Add(new Song(song.FilePath, song.Title, song.Artist, song.Album, song.Track, song.Error));
            });

            this._jsonHandler.WriteToJSONFile(this._jsonSongList);
        }

        private bool CanAddFolder(object p)
        {
            if (_playbackState == PlaybackState.Stopped)
            {
                return true;
            }
            return false;
        }

        private void ScanLibrary(object p)
        {
            List<SongLocation> badPaths = this._jsonSongList.VerifyPaths();

            Playlist.Clear();

            this._jsonSongList.List.ForEach(song =>
            {
                Playlist.Add(new Song(song.FilePath, song.Title, song.Artist, song.Album, song.Track, song.Error));
            });
            this._jsonHandler.WriteToJSONFile(this._jsonSongList);
        }

        private bool CanScanLibrary(object p)
        {
            if (_playbackState == PlaybackState.Stopped)
            {
                return true;
            }
            return false;
        }

        private void RemoveSong(object p)
        {
            this._jsonSongList.RemoveSongLocation(_jsonSongList.List.Where(song => song.Title == CurrentlySelectedTrack.title).First().Id);

            Playlist.Clear();

            this._jsonSongList.List.ForEach(song =>
            {
                Playlist.Add(new Song(song.FilePath, song.Title, song.Artist, song.Album, song.Track, song.Error));
            });

            this._jsonHandler.WriteToJSONFile(this._jsonSongList);
        }
        private bool CanRemoveSong(object p)
        {
            if (_playbackState == PlaybackState.Stopped &&
                this._jsonSongList.List.Count > 0 &&
                Playlist.Count > 0)
            {
                return true;
            }
            return false;
        }

        // Player commands
        private void RewindToStart(object p)
        {
            _audioPlayer.SetPosition(0);
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
                    _audioPlayer = new AudioPlayer(CurrentlySelectedTrack.filePath, CurrentVolume);
                    _audioPlayer.PlaybackStopType = AudioPlayer.PlaybackStopTypes.PlaybackStoppedReachingEndOfFile;
                    _audioPlayer.PlaybackPaused += _audioPlayer_PlaybackPaused;
                    _audioPlayer.PlaybackResumed += _audioPlayer_PlaybackResumed;
                    _audioPlayer.PlaybackStopped += _audioPlayer_PlaybackStopped;
                    CurrentTrackLength = _audioPlayer.GetLengthInSeconds();
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
                _audioPlayer.SetPosition(_audioPlayer.GetLengthInSeconds());
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
                CurrentlySelectedTrack = Playlist.NextItem(CurrentlyPlayingTrack);
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
