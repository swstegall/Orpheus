using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Orpheus {
    class JSONHandler {

        //Will read the music_storage json file and return a SongList object of all the songs in the file - Isaac
        public SongList ReadJsonFile() {
            //Object to be returned - Isaac
            SongList MusicFromJSON;
            try {
                string JSONFromFile;
                //Builds the path based on the location of the application in the current user's directory  - Isaac
                //If the path is incorrect in testing on your machine let me know and I will try someting different - Isaac
                string path = Path.Combine(Environment.CurrentDirectory, @"music_storage.json");
                using (var reader = new StreamReader(path)) {
                    JSONFromFile = reader.ReadToEnd();
                }
                //Deserializes the text from the file using Newtonsoft's tool - Isaac
                MusicFromJSON = JsonConvert.DeserializeObject<SongList>(JSONFromFile);
            }
            catch (Exception ex){
                //Initializes the object to be returned to a empty variable if the file could not be found - Isaac
                MusicFromJSON = new SongList();
            }
            return MusicFromJSON;
        }

        public void WriteToJSONFile(SongList ListToWrite) {
            try {
                //Builds the path based on the location of the application in the current user's directory  - Isaac
                //If the path is incorrect in testing on your machine let me know and I will try someting different - Isaac
                string path = Path.Combine(Environment.CurrentDirectory, @"music_storage.json");
                string ConvertedString = JsonConvert.SerializeObject(ListToWrite);
                using (var writer = new StreamWriter(path)) {
                    writer.Write(ConvertedString);
                }
                //Deserializes the text from the file using Newtonsoft's tool - Isaac
            }
            catch (Exception ex) {
                //Initializes the object to be returned to a empty variable if the file could not be found - Isaac
            }
        }

    }

}
