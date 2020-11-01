using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orpheus
{
    public class Track
    {
        public string fileName;
        public string friendlyName;
        public string filepath;

        public Track(string fileName, string friendlyName)
        {
            this.fileName = fileName;
            this.friendlyName = friendlyName;
        }
    }
}
