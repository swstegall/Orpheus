using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orpheus.Utilities {
    class JSONData {

        //All variables in this class need a JsonProperty attribute - Isaac
        //This is because the variables here are present and ordered exactly like the objects in music_storeage.json which is necessary for deserialization - Isaac
        //This is the root level of music_storage.json - Isaac        

        //This is a string that contains the saved theme name - Isaac
        [JsonProperty("Theme")]
        public string Theme { get; set; }

        //This is a list of song objects - Isaac
        [JsonProperty("Songs")]
        public List<SongLocation> SongList { get; set; }
    }
}
