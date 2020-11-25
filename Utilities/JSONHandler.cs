using Newtonsoft.Json;
using System;
using System.IO;

namespace Orpheus.Utilities
{
    class JSONHandler
    {

        public string theme;

        //Will read the music_storage json file and return a SongList object of all the songs in the file - Isaac
        public SongList ReadJsonFile()
        {
            //Object that is read from the JSON file - Isaac
            JSONData DataFromJSON;
            //Object to be returned - Isaac
            SongList MusicFromJSON = new SongList();
            try
            {
                string JSONFromFile;
                //Builds the path based on the location of the application in the current user's directory  - Isaac
                //If the path is incorrect in testing on your machine let me know and I will try someting different - Isaac
                string path = Path.Combine(Environment.CurrentDirectory, @"music_storage.json");
                using (var reader = new StreamReader(path))
                {
                    JSONFromFile = reader.ReadToEnd();
                }
                //Deserializes the text from the file using Newtonsoft's tool - Isaac
                DataFromJSON = JsonConvert.DeserializeObject<JSONData>(JSONFromFile);
                //Separate them from SongList data
                theme = DataFromJSON.Theme;
                MusicFromJSON.List = DataFromJSON.SongList;
            }
            catch (Exception ex)
            {
                //Initializes the object to be returned to a empty variable if the file could not be found - Isaac
                Console.WriteLine(ex);
                MusicFromJSON = new SongList();
            }
            return MusicFromJSON;
        }

        //Calling this will write the passed in SongList to the JSON file - Isaac
        public void WriteToJSONFile(SongList ListToWrite)
        {
            JSONData DataForJSON;
            try
            {
                //Builds the path based on the location of the application in the current user's directory  - Isaac
                //If the path is incorrect in testing on your machine let me know and I will try someting different - Isaac
                string path = Path.Combine(Environment.CurrentDirectory, @"music_storage.json");
                //Constructs the JSON data to be written - Isaac
                DataForJSON = new JSONData();
                DataForJSON.Theme = theme;
                DataForJSON.SongList = ListToWrite.List;
                //Serializes the DataForJSON object to a string with the data in JSON format - Isaac
                string ConvertedString = JsonConvert.SerializeObject(DataForJSON);
                using (var writer = new StreamWriter(path))
                {
                    writer.Write(ConvertedString);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
