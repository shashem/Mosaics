using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace Mosaics
{
    class ClassClass
    {
        public class ShrunkImages
        {
            public string FilePath { get; set; }
            public Bitmap image { get; set; }
            public float score { get; set; }
        }
        public class BlockAddress
        {
            public int startX { get; set; }
            public int endX { get; set; }
            public int startY { get; set; }
            public int endY { get; set; }
        }
    }
}