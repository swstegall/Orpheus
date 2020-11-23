namespace Orpheus.Utilities
{
    public class Song
    {
        public string filePath { get; set; }
        public string title { get; set; }
        public string artist { get; set; }
        public string album { get; set; }
        public int trackNumber { get; set; }
        public string error { get; set; }

        public Song(string filePath, string title, string artist, string album, int trackNumber, string error)
        {
            this.filePath = filePath;
            this.title = title;
            this.artist = artist;
            this.album = album;
            this.trackNumber = trackNumber;
            this.error = error;
        }
    }
}