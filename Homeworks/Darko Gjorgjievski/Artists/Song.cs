using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artists
{
    class Song
    {
        public int SongID { get; set; }
        public string Name { get; set; }
        public TimeSpan Length { get; set; }
        public int AlbumID { get; set; }
    }
}
