using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

        //This will open the file selection window to allow a user to select a music file or files - Isaac
        //It will then add the song choice or choices to List and return a bool that states if all the songs were added  - Isaac
        public bool AddSongLocation() {
            //Bool to return  - Isaac
            bool AddedPaths = false;
            //This will allow for the opening of the file selection and referencing the chosen file's information - Isaac
            OpenFileDialog open = new OpenFileDialog();
            //Dispalys at the top of the file selection window - Isaac
            open.Title = "Choose Your Song(s)";
            //The first part is what dispalys in the file selection window and the second part is what actually restricts the file types - Isaac
            open.Filter = "Music Files (*.mp3, *.wav, *.ogg)|*.mp3;*.wav;*.ogg";
            //This will allow multiple files to be selected and returned - Isaac
            open.Multiselect = true;
            //This will open the file selection window and will go in the if statement if a file was selected - Isaac
            if (open.ShowDialog() == DialogResult.OK) {
                //This gets the number of returned files - Isaac
                int ReturnedAmount = open.FileNames.Length;
                //Loop throught the returned results and all each selected song to the list - Isaac
                for (int i = 0; i < ReturnedAmount; i++) {
                    //This contains the entire file path - Isaac
                    string FilePath = open.FileNames[i];
                    //This contains only the file name - Isaac
                    string FileName = open.SafeFileNames[i];
                    AddToList(FilePath, FileName);
                    AddedPaths = true;
                }
            }
            return AddedPaths;
        }

        public bool RemapSongLocation()
        {
            bool AddedPaths = false;

            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Choose Your Song(s)";
            open.Filter = "Music Files (*.mp3, *.wav, *.ogg)|*.mp3;*.wav;*.ogg";
            open.Multiselect = true;
            if (open.ShowDialog() == DialogResult.OK)
            {
                int ReturnedAmount = open.FileNames.Length;
                for (int i = 0; i < ReturnedAmount; i++)
                {
                    string FilePath = open.FileNames[i];
                    string FileName = open.SafeFileNames[i];
                    Remap(FilePath, FileName);
                    AddedPaths = true;
                }
            }
            return AddedPaths;
        }
        private void Remap(string FilePath, string FileName)
        {
            int GreatestId = 0;
            TagLib.File TagFile = TagLib.File.Create(FilePath);
            string Artist = "";
            if (TagFile.Tag.Performers.Length > 0)
            {
                Artist = TagFile.Tag.Performers[0];
            }
            string Album = TagFile.Tag.Album;
            string Title = TagFile.Tag.Title;
            int Track = (int)TagFile.Tag.Track;
            if (List.Count == 1)
            {
                List.Clear();
            }
            else
            {
                var brokenSong = List.Find(song => song.Title == Title);
                List.RemoveAt(List.IndexOf(brokenSong));
            }
            if (List.Count > 0)
            {
                GreatestId = List.Max(x => x.Id);
            }
            List.Add(new SongLocation()
            {
                Id = GreatestId + 1,
                SongName = FileName,
                FilePath = FilePath,
                Artist = Artist,
                Album = Album,
                Title = Title,
                Track = Track,
            });
        }

        public bool AddFolderOfSongs() {
            //Bool to return  - Isaac
            bool AddedPaths = false;
            //This will allow for the opening of the folder selection and referencing the chosen folder's information - Isaac
            FolderBrowserDialog OpenFolder = new FolderBrowserDialog();
            if (OpenFolder.ShowDialog() == DialogResult.OK) {
                //These will allow for only the files of valid music types to be selected - Isaac
                string[] FilePathsMp3 = Directory.GetFiles(OpenFolder.SelectedPath, "*.mp3");
                string[] FilePathsWAV = Directory.GetFiles(OpenFolder.SelectedPath, "*.wav");
                string[] FilePathsOGG = Directory.GetFiles(OpenFolder.SelectedPath, "*.ogg");
                //This will then go through all of the found files and extract their information and then add it to List - Isaac
                foreach (string CurrentFile in FilePathsMp3) {
                    string FileName = CurrentFile;
                    int IndexLastSlash = CurrentFile.LastIndexOf("\\");
                    if (IndexLastSlash > 0 ) {
                        FileName = CurrentFile.Substring(0, IndexLastSlash);
                    }
                    AddToList(CurrentFile, FileName);
                    AddedPaths = true;
                }
                foreach (string CurrentFile in FilePathsWAV) {
                    string FileName = CurrentFile;
                    int IndexLastSlash = CurrentFile.LastIndexOf("\\");
                    if (IndexLastSlash > 0) {
                        FileName = CurrentFile.Substring(0, IndexLastSlash);
                    }
                    AddToList(CurrentFile, FileName);
                    AddedPaths = true;
                }
                foreach (string CurrentFile in FilePathsOGG) {
                    string FileName = CurrentFile;
                    int IndexLastSlash = CurrentFile.LastIndexOf("\\");
                    if (IndexLastSlash > 0) {
                        FileName = CurrentFile.Substring(0, IndexLastSlash);
                    }
                    AddToList(CurrentFile, FileName);
                    AddedPaths = true;
                }
            }
            return AddedPaths;
        }


        //This will add the passed in file information to List - Isaac
        private void AddToList(string FilePath, string FileName) {
            //Initialize the new Id to zero - Isaac
            int GreatestId = 0;
            // Used this package to grab metadata and pass it to the SongList instance - Sam
            TagLib.File TagFile = TagLib.File.Create(FilePath);
            string Artist = "";
            if (TagFile.Tag.Performers.Length > 0) {
                Artist = TagFile.Tag.Performers[0];
            }
            string Album = TagFile.Tag.Album;
            string Title = TagFile.Tag.Title;
            int Track = (int)TagFile.Tag.Track;
            //If there aren't any SongLocation items in the list then it keeps the default Id of zero - Isaac
            if (List.Count > 0) {
                //If there are SongLocation items in the list then it goes through all of the stored Id's to get the highest one - Isaac
                GreatestId = List.Max(x => x.Id);
            }
            List.Add(new SongLocation() {
                //The Id is one greater than the found highest Id to ensure that the new Id is unique - Isaac
                Id = GreatestId + 1,
                SongName = FileName,
                FilePath = FilePath,
                Artist = Artist,
                Album = Album,
                Title = Title,
                Track = Track,
            });
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

        //Calling this will remove all of the songlocations in List that have invalid paths - Isaac
        //Calling to verify paths before hand is not necessary since it will do this to ensure all bad paths are removed - Isaac
        public void PruneInvalidSongLocations() {
            //Ensure all bad paths have been found and get all of them - Isaac
            List<SongLocation> BadPaths = VerifyPaths();
            //Go through each of the returned song locations and remove them from List - Isaac
            foreach (SongLocation path in BadPaths) {
                List.Remove(path);
            }
        }

    }
}
