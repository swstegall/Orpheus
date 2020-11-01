using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orpheus
{
    public class Song
    {
        private string fileName { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public int Track { get; set; }
        public string Name { get; set; }
        public string Error { get; set; }

        public Song(string fileName)
        {
            this.fileName = fileName;
        }

        public Song(string fileName, string Title, string Artist, string Album, int Track, string Name, string Error)
        {
            this.fileName = fileName;
            this.Title = Title;
            this.Artist = Artist;
            this.Album = Album;
            this.Track = Track;
            this.Name = Name;
            this.Error = Error;
        }
    }
}