using Orpheus.Utilities;
using Orpheus.Windows;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;

namespace Orpheus.Views
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private Random _rng;
        private DispatcherTimer _timer;
        private string _title;
        private double _currentTrackLength;
        private double _currentTrackPosition;
        private string _playPauseImageSource;
        private float _currentVolume;
        private bool _shuffling;
        private JSONHandler _jsonHandler;
        private SongList _jsonSongList;
        public string _background;
        public string _foreground;
        public string _warning;

        private ObservableCollection<Song> _playlist;
        private Song _currentlyPlayingTrack;
        private Song _currentlySelectedTrack;
        private AudioPlayer _audioPlayer;
        private ThemeWindow _themeWindow;

        private enum PlaybackState
        {
            Playing, Stopped, Paused
        }

        private PlaybackState _playbackState;

        public string Background
        {
            get { return _background; }
            set
            {
                if (value == _background) return;
                _background = value;
                OnPropertyChanged(nameof(Background));
            }
        }

        public string Foreground
        {
            get { return _foreground; }
            set
            {
                if (value == _foreground) return;
                _foreground = value;
                OnPropertyChanged(nameof(Foreground));
            }
        }

        public string Warning
        {
            get { return _warning; }
            set
            {
                if (value == _warning) return;
                _warning = value;
                OnPropertyChanged(nameof(Warning));
            }
        }

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
        public ICommand PruneInvalidSongsCommand { get; set; }
        public ICommand SelectThemeCommand { get; set; }
        public ICommand RemoveSongCommand { get; set; }
        public ICommand RemapSongCommand { get; set; }

        public ICommand RewindToStartCommand { get; set; }
        public ICommand RewindCommand { get; set; }
        public ICommand StartPlaybackCommand { get; set; }
        public ICommand StopPlaybackCommand { get; set; }
        public ICommand FastForwardCommand { get; set; }
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

            _background = "#999999";
            _foreground = "#000000";
            _warning = "#FF0000";

            Title = "Orpheus";

            _jsonHandler = new JSONHandler();
            _jsonSongList = this._jsonHandler.ReadJsonFile();
            Playlist = new ObservableCollection<Song>();
            this._jsonSongList.VerifyPaths();
            RefreshPlaylist();

            LoadCommands();

            _playbackState = PlaybackState.Stopped;
            _rng = new Random();
            _shuffling = false;

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
            PruneInvalidSongsCommand = new RelayCommand(PruneInvalidSongs, CanPruneInvalidSongs);
            SelectThemeCommand = new RelayCommand(SelectTheme, CanSelectTheme);
            RemoveSongCommand = new RelayCommand(RemoveSong, CanRemoveSong);
            RemapSongCommand = new RelayCommand(RemapSong, CanRemapSong);

            // Player commands
            RewindToStartCommand = new RelayCommand(RewindToStart, CanRewindToStart);
            RewindCommand = new RelayCommand(Rewind, CanRewind);
            StartPlaybackCommand = new RelayCommand(StartPlayback, CanStartPlayback);
            StopPlaybackCommand = new RelayCommand(StopPlayback, CanStopPlayback);
            FastForwardCommand = new RelayCommand(FastForward, CanFastForward);
            ForwardToEndCommand = new RelayCommand(ForwardToEnd, CanForwardToEnd);
            ShuffleCommand = new RelayCommand(Shuffle, CanShuffle);
            RowDoubleClickCommand = new RelayCommand(RowDoubleClick, CanRowDoubleClick);

            // Event commands
            TrackControlMouseDownCommand = new RelayCommand(TrackControlMouseDown, CanTrackControlMouseDown);
            TrackControlMouseUpCommand = new RelayCommand(TrackControlMouseUp, CanTrackControlMouseUp);
            VolumeControlValueChangedCommand = new RelayCommand(VolumeControlValueChanged, CanVolumeControlValueChanged);
        }

        private void RefreshPlaylist()
        {
            Playlist.Clear();
            this._jsonSongList.EradicateDuplicates();
            this._jsonSongList.List.ForEach(song =>
            {
                Playlist.Add(new Song(song.FilePath, song.Title, song.Artist, song.Album, song.Track, song.Error));
            });

            this._jsonHandler.WriteToJSONFile(this._jsonSongList);
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
            RefreshPlaylist();
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
            RefreshPlaylist();
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
            this._jsonSongList.VerifyPaths();
            RefreshPlaylist();
        }

        private bool CanScanLibrary(object p)
        {
            if (_playbackState == PlaybackState.Stopped)
            {
                return true;
            }
            return false;
        }

        private void PruneInvalidSongs(object p)
        {
            this._jsonSongList.PruneInvalidSongLocations();
            RefreshPlaylist();
        }

        private bool CanPruneInvalidSongs(object p)
        {
            if (_playbackState == PlaybackState.Stopped &&
                this._jsonSongList.List.Count > 0 &&
                Playlist.Count > 0)
            {
                return true;
            }
            return false;
        }

        private void ThemeWindowClosed(object sender, EventArgs e)
        {
            var selectedValue = _themeWindow.ThemeSelectedValue;
            switch (selectedValue)
            {
                case "Default (Light)":
                {
                    Foreground = "#000000";
                    Background = "#FFFFFF";
                    Warning = "#FF0000";
                    break;
                }
                case "Luna":
                {
                    Foreground = "#00FFFF";
                    Background = "#0000FF";
                    Warning = "#00FF00";
                    break;
                }
                case "S'mores":
                {
                    Foreground = "#D2B48C";
                    Background = "#A52A2A";
                    Warning = "#FFFFFF";
                    break;
                }
                case "Candyland":
                {
                    Foreground = "#ADD8E6";
                    Background = "#FFC0CB";
                    Warning = "#EE82EE";
                    break;
                }
                case "Halloween":
                {
                    Foreground = "#FFA500";
                    Background = "#000000";
                    Warning = "#FFFF00";
                    break;
                }
                case "Thanksgiving":
                {
                    Foreground = "#FFA500";
                    Background = "#A52A2A";
                    Warning = "#FFFF00";
                    break;
                }
                case "Christmas":
                {
                    Foreground = "#FFFF00";
                    Background = "#00FF00";
                    Warning = "#FF0000";
                    break;
                }
                case "Reveille (Light)":
                {
                    Foreground = "#500000";
                    Background = "#FFFFFF";
                    Warning = "#500000";
                    break;
                }
                case "Reveille (Dark)":
                {
                    Foreground = "#500000";
                    Background = "##999999";
                    Warning = "#500000";
                    break;
                }
                case "Hunter (Light)":
                {
                    Foreground = "#018744";
                    Background = "#FFFFFF";
                    Warning = "#C24444";
                    break;
                }
                case "Hunter (Dark)":
                {
                    Foreground = "#018744";
                    Background = "#999999";
                    Warning = "#C24444";
                    break;
                }
                case "Classic":
                {
                    Foreground = "#234508";
                    Background = "#DDA0DD";
                    Warning = "#C67676";
                    break;
                }
                case "Rose":
                {
                    Foreground = "#234555";
                    Background = "#C21E56";
                    Warning = "#C67777";
                    break;
                }
                case "Classic Gold":
                {
                    Foreground = "#234656";
                    Background = "#8F7034";
                    Warning = "#C67878";
                    break;
                }
                case "Classic Teal":
                {
                    Foreground = "#234444";
                    Background = "#038387";
                    Warning = "#C67171";
                    break;
                }
                case "Classic Plum":
                {
                    Foreground = "#234848";
                    Background = "#854085";
                    Warning = "#C67879";
                    break;
                }
                case "Cool Grey":
                {
                    Foreground = "#233147";
                    Background = "#737373";
                    Warning = "#C67870";
                    break;
                }
                case "Warm Grey":
                {
                    Foreground = "#234348";
                    Background = "#867365";
                    Warning = "#C57278";
                    break;
                }
                case "Default (Dark)":
                default:
                {
                    Foreground = "#FFFFFF";
                    Background = "##999999";
                    Warning = "#FF0000";
                    break;
                }
            }
        }

        private void SelectTheme(object p)
        {
            _themeWindow = new ThemeWindow();
            _themeWindow.Closed += ThemeWindowClosed;
            _themeWindow.Show();
        }

        private bool CanSelectTheme(object p)
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
            RefreshPlaylist();
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

        private void RemapSong(object p)
        {
            this._jsonSongList.RemapSongLocation();

            RefreshPlaylist();
        }

        private bool CanRemapSong(object p)
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

        private void Rewind(object p)
        {
            _audioPlayer.SetPosition(CurrentTrackPosition - 5.0);
        }

        private bool CanRewind(object p)
        {
            if (_playbackState == PlaybackState.Playing)
            {
                return true;
            }
            return false;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (_playbackState == PlaybackState.Playing)
            {
                CurrentTrackPosition = _audioPlayer.GetPositionInSeconds();
            }
        }

        private void StartPlayback(object p)
        {
            if (CurrentlySelectedTrack != null && CurrentlySelectedTrack.error != "File not found")
            {
                if (_playbackState == PlaybackState.Stopped)
                {
                    _audioPlayer = new AudioPlayer(CurrentlySelectedTrack.filePath, CurrentVolume);
                    _audioPlayer.PlaybackStopType = AudioPlayer.PlaybackStopTypes.PlaybackStoppedReachingEndOfFile;
                    _audioPlayer.PlaybackPaused += _audioPlayer_PlaybackPaused;
                    _audioPlayer.PlaybackResumed += _audioPlayer_PlaybackResumed;
                    _audioPlayer.PlaybackStopped += _audioPlayer_PlaybackStopped;
                    CurrentTrackLength = _audioPlayer.GetLengthInSeconds();
                    CurrentTrackPosition = _audioPlayer.GetPositionInSeconds();
                    CurrentlyPlayingTrack = CurrentlySelectedTrack;
                    _timer = new DispatcherTimer();
                    _timer.Interval = TimeSpan.FromSeconds(0);
                    _timer.Tick += timer_Tick;
                    _timer.Start();
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

        private void FastForward(object p)
        {
            _audioPlayer.SetPosition(CurrentTrackPosition + 5.0);
        }

        private bool CanFastForward(object p)
        {
            if (_playbackState == PlaybackState.Playing)
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
                _audioPlayer.SetPosition(_audioPlayer.GetLengthInSeconds() - 0.1);
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
            if (_shuffling)
            {
                _shuffling = false;
            }
            else
            {
                _shuffling = true;
            }
        }

        private bool CanShuffle(object p)
        {
            if (_playbackState == PlaybackState.Stopped)
            {
                return true;
            }
            return false;
        }

        private void RowDoubleClick(object p)
        {
            if (CurrentlySelectedTrack != null && CurrentlySelectedTrack.error != "File not found")
            {
                _audioPlayer = new AudioPlayer(CurrentlySelectedTrack.filePath, CurrentVolume);
                _audioPlayer.PlaybackStopType = AudioPlayer.PlaybackStopTypes.PlaybackStoppedReachingEndOfFile;
                _audioPlayer.PlaybackPaused += _audioPlayer_PlaybackPaused;
                _audioPlayer.PlaybackResumed += _audioPlayer_PlaybackResumed;
                _audioPlayer.PlaybackStopped += _audioPlayer_PlaybackStopped;
                CurrentTrackLength = _audioPlayer.GetLengthInSeconds();
                CurrentTrackPosition = _audioPlayer.GetPositionInSeconds();
                CurrentlyPlayingTrack = CurrentlySelectedTrack;
                _timer = new DispatcherTimer();
                _timer.Interval = TimeSpan.FromSeconds(0);
                _timer.Tick += timer_Tick;
                _timer.Start();
                if (CurrentlySelectedTrack == CurrentlyPlayingTrack)
                {
                    _audioPlayer.TogglePlayPause(CurrentVolume);
                }
            }
        }

        private bool CanRowDoubleClick(object p)
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
                _audioPlayer.SetVolume(CurrentVolume);
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
                if (!_shuffling)
                {
                    CurrentlySelectedTrack = Playlist.NextItem(CurrentlyPlayingTrack);
                }
                else
                {
                    var exclusionIndex = Playlist.IndexOf(CurrentlyPlayingTrack);
                    int guess = _rng.Next(0, Playlist.Count);
                    while (guess == exclusionIndex)
                        guess = _rng.Next(0, Playlist.Count);
                    CurrentlySelectedTrack = Playlist.ElementAt(guess);
                }
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
