using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaics
{
    class FileClass
    {
        public static int i = 0;
        public static List<string> GetAllFileNames(string FolderPath)
        {
            List<string> FilePaths = Directory.GetFiles(FolderPath).ToList();
            return FilePaths;
        }
        public static string CreateThumbnail(int maxWidth, int maxHeight, string path)
        {

            var image = System.Drawing.Image.FromFile(path);
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);
            //var newWidth = (int)(image.Width * ratio);
            //var newHeight = (int)(image.Height * ratio);

            var newWidth = (int)maxWidth;
            var newHeight = (int)maxHeight;

            var newImage = new Bitmap(newWidth, newHeight);
            //var newImage = new Bitmap(15, 30);


            Graphics thumbGraph = Graphics.FromImage(newImage);

            thumbGraph.DrawImage(image, 0, 0, newWidth, newHeight);
            image.Dispose();

            string fileRelativePath = "newsizeimages/" + maxWidth;
            newImage.Save(".\\ShrunkImages\\File" + i + ".png", newImage.RawFormat);
            i++;
            return fileRelativePath;
        }
    }
}
