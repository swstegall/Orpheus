using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace Orpheus {
    class SongList {
        //All variables in this class need a JsonProperty attribute - Isaac
        //This is because the variables here are present and ordered exactly like the objects in music_storeage.json which is necessary for deserialization - Isaac

        //This is a list of song objects - Isaac
        //This is the root level of music_storage.json - Isaac
        [JsonProperty("Songs")]
        public List<SongLocation> List {get; set;}

        public bool AddSongLocation() {
            bool AddedPath = false;
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Choose Your Song";
            open.Filter = "Music Files (*.mp3, *.wav, *.ogg)|*.mp3;*.wav;*.ogg";
            string FilePath ="";
            string FileName = "";
            if (open.ShowDialog() == DialogResult.OK) {
                FilePath = open.FileName;
                FileName = open.SafeFileName;
                int GreatestId = List.Max(x => x.Id);
                List.Add(new SongLocation() {
                    Id = GreatestId + 1,
                    SongName = FileName,
                    FilePath = FilePath
                });
                AddedPath = true;
            }
            return AddedPath;
        }

        public void RemoveSongLocation(int GivenId) {
            SongLocation ItemToRemove = (SongLocation)List.Where(x => x.Id == GivenId).FirstOrDefault();
            List.Remove(ItemToRemove);
        }

        public List<SongLocation> VerifyPaths() {
            OpenFileDialog open = new OpenFileDialog();
            List<SongLocation> BadPaths = (List<SongLocation>)List.Where(x => File.Exists(x.FilePath) == false).ToList();
            return BadPaths;
        }

    }
}
