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

        //This will open the file selection window to allow a user to select a music file - Isaac
        //It will then add the song choice to List and return a bool that states if a song was added  - Isaac
        public bool AddSongLocation() {
            //Bool to return  - Isaac
            bool AddedPath = false;
            //This will allow for the opening of the file selection and referencing the chosen file's information - Isaac
            OpenFileDialog open = new OpenFileDialog();
            //Dispalys at the top of the file selection window - Isaac
            open.Title = "Choose Your Song";
            //The first part is what dispalys in the file selection window and the second part is what actually restricts the file types - Isaac
            open.Filter = "Music Files (*.mp3, *.wav, *.ogg)|*.mp3;*.wav;*.ogg";
            //This will open the file selection window and will go in the if statement if a file was selected - Isaac
            if (open.ShowDialog() == DialogResult.OK) {
                //This contains the entire file path - Isaac
                string FilePath = open.FileName;
                //This contains only the file name - Isaac
                string FileName = open.SafeFileName;
                //Initialize the new Id to zero - Isaac
                int GreatestId = 0;

                // Used this package to grab metadata and pass it to the SongList instance - Sam
                TagLib.File tagFile = TagLib.File.Create(FilePath);
                string artist = "";
                if (tagFile.Tag.Performers.Length > 0)
                {
                    artist = tagFile.Tag.Performers[0];
                }
                string album = tagFile.Tag.Album;
                string title = tagFile.Tag.Title;
                int track = (int)tagFile.Tag.Track;

                //If there aren't any SongLocation items in the list then it keeps the default Id of zero - Isaac
                if (List.Count > 0)
                {
                    //If there are SongLocation items in the list then it goes through all of the stored Id's to get the highest one - Isaac
                    GreatestId = List.Max(x => x.Id);
                }
                List.Add(new SongLocation() {
                    //The Id is one greater than the found highest Id to ensure that the new Id is unique - Isaac
                    Id = GreatestId + 1,
                    SongName = FileName,
                    FilePath = FilePath,
                    Artist = artist,
                    Album = album,
                    Title = title,
                    Track = track,
                });
                AddedPath = true;
            }
            return AddedPath;
        }

        //This will remove a SongLocation object item from List based on the Id passed in as a parameter - Isaac
        public void RemoveSongLocation(int GivenId) {
            //Finds the first instance of the SongLocation object to be removed in List - Isaac
            SongLocation ItemToRemove = (SongLocation)List.Where(x => x.Id == GivenId).FirstOrDefault();
            //Removes the found object from List - Isaac
            List.Remove(ItemToRemove);
        }

        //This will run through every file path in List to validate if it is a valid path or not - Isaac
        //Will return a List of SongLocation objects of all the SongLocation objects with non-valid paths - Isaac
        // Added the erro so when the scan happens it say file not found and it makes the text red- Armir
        public List<SongLocation> VerifyPaths() {
            List<SongLocation> BadPaths = (List<SongLocation>)List.Where(x => File.Exists(x.FilePath) == false).ToList();
            List.ForEach(x => { if (File.Exists(x.FilePath) == false) { x.BrokenPath = true; x.Error = "File not found"; } });
            return BadPaths;
        }

    }
}
