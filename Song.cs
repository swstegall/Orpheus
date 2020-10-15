using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orpheus {
    class SongLocation {
        //All variables in this class need a JsonProperty attribute - Isaac
        //This is because the variables here are present and ordered exactly like the objects in music_storeage.json which is necessary for deserialization - Isaac

        //Id is the unique Id for this SongLocation object - Isaac
        [JsonProperty("Id")]
        public int Id { get; set; }

        //Song Name is the title of the song - Isaac
        //This can technically be whatever string as it is the way the user identifies the song, not the program - Isaac
        //Example: "01 Fake Your Death.mp3" - Isaac
        [JsonProperty("Song Name")]
        public string SongName { get; set; }

        //FilePath is the path to the song on the user's machine. - Isaac
        //It is necessary to have the full path of the song including the actual song file name - Isaac
        //Example: "D:\\Music\\My Chemical Romance\\12 Fake Your Death.mp3" - Isaac
        [JsonProperty("File Path")]
        public string FilePath { get; set; }

        [JsonProperty("Broken Path")]
        public bool BrokenPath { get; set; }

        // Added trackers for metadata so this info is available on application load to the frontend - Sam
        [JsonProperty("Artist")]
        public string Artist { get; set; }

        // Added trackers for metadata so this info is available on application load to the frontend - Sam
        [JsonProperty("Album")]
        public string Album { get; set; }

        // Added trackers for metadata so this info is available on application load to the frontend - Sam
        [JsonProperty("Title")]
        public string Title { get; set; }

        // Added trackers for metadata so this info is available on application load to the frontend - Sam
        [JsonProperty("Track")]
        public int Track { get; set; }
    }
}
