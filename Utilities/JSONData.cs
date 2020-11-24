using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orpheus.Utilities {
    class JSONData {

        [JsonProperty("Theme")]
        public string Theme { get; set; }

        [JsonProperty("Songs")]
        public List<SongLocation> SongList { get; set; }
    }
}
