using System;

namespace ServerPhB.Models
{
    public class Photo
    {
        public int PhotoID { get; set; }
        public enum Format
        {
            JPEG,
            PNG,
            GIF,
            BMP,
            TIFF
        }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public uint FileSize { get; set; }
    }
}
