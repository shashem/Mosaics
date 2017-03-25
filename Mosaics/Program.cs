using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace Mosaics
{
    class Program
    {

        public static string MainPicFolder = ".\\pics"; //Folder where mosaic pictures are stored
        public static string ShrunkenImages = ".\\ShrunkImages"; //
        public static string InputFilePath = ".\\Input.png"; //
        public static int sizeSmallY = 0;
        public static int sizeSmallX = 0;
        static void Main(string[] args)
        {
            Bitmap InputFile = new Bitmap(InputFilePath);

            ShrinkAll();
            List<string> AllShrunkFiles = FileClass.GetAllFileNames(ShrunkenImages);
            List<ClassClass.ShrunkImages> ImageList = new List<ClassClass.ShrunkImages>();
            List<ClassClass.BlockAddress> Blocks = new List<ClassClass.BlockAddress>();
            ImageAverages(AllShrunkFiles, ImageList);

            sizeSmallX = ImageList[0].image.Width;
            sizeSmallY = ImageList[0].image.Height;

            Bitmap OutPutFile = new Bitmap(InputFile.Width + sizeSmallX, InputFile.Height + sizeSmallY);

            for(int x = 0; x < InputFile.Width; x = x + sizeSmallX)
            {
                for(int y = 0; y < InputFile.Height; y = y + sizeSmallY)
                {
                    ClassClass.BlockAddress Temp = new ClassClass.BlockAddress();
                    Temp.startX = x;
                    Temp.endX = x + sizeSmallX;
                    Temp.startY = y;
                    Temp.endY = y + sizeSmallY;
                    Blocks.Add(Temp);
                }
            }

            foreach(ClassClass.BlockAddress BA in Blocks)
            {
                Console.WriteLine("xStart: " + BA.startX + " xEnd: " + BA.endX + " yStart: " + BA.startY + " yEnd: " + BA.endY);
            }
            Console.WriteLine(string.Format("Size: {0} {1} BLOCKS {2}", InputFile.Width, InputFile.Height, Blocks.Count));
            //Console.ReadKey();

            foreach (ClassClass.BlockAddress BA in Blocks)
            {
                if (BA.endX < InputFile.Width && BA.endY < InputFile.Height)
                {
                    float BlockScore = 0;
                    float r = 0, g = 0, b = 0, a = 0;

                    for (int x = BA.startX; x < BA.endX; x++)
                    {
                        for (int y = BA.startY; y < BA.endY; y++)
                        {
                            Color pixel = InputFile.GetPixel(x, y);
                            r = r + pixel.R;
                            g = g + pixel.G;
                            b = b + pixel.B;
                            a = a + pixel.A;
                        }
                    }
                    //BlockScore = r + g + b;
                    BlockScore = GetScore(r,g, b);
                    ClassClass.ShrunkImages Chosen = ChooseImage(ImageList, BlockScore);

                    int xj = 0;
                    int yj = 0;
                    for (int x = BA.startX; x <= BA.endX; x++)
                    {
                        if(xj == sizeSmallX)
                        {
                            xj = 0;
                        }
                        for (int y = BA.startY; y <= BA.endY - 1; y++)
                        {
                            if(yj == sizeSmallY)
                            {
                                yj = 0;
                            }
                            Color pixel = Chosen.image.GetPixel(xj, yj);
                           // Color pixel = Chosen.image.GetPixel(1, 99);
                           // Console.WriteLine(Chosen.image.Width + " " + Chosen.image.Height);
                            OutPutFile.SetPixel(x, y, pixel);
                            yj++;
                        }
                        xj++;
                        
                    }
                }
            }
            //Console.ReadKey();

                int DeltaX = InputFile.Width / sizeSmallX;
            int DeltaY = InputFile.Height / sizeSmallY;


            /*for(int x = 0; x < InputFile.Width; x = x + sizeSmallX)
            {
                for(int y = 0; y < InputFile.Height; y = y + sizeSmallY)
                {
                    float BlockScore = 0;
                    float r = 0;
                    float g = 0;
                    float b = 0;
                    BlockData(InputFile, sizeSmallX, sizeSmallY, x, y, ref r, ref g, ref b);
                    BlockScore = (r + g + b) / 3;
                    ClassClass.ShrunkImages ChosenImage = new ClassClass.ShrunkImages();
                    ChosenImage = ChooseImage(ImageList, BlockScore, ChosenImage);
                    int Thumbx = 0;
                    int Thumby = 0;
                    while(Thumbx < sizeSmallX)
                    {
                         while(Thumby < sizeSmallY)
                        {
                            Color pixel = ChosenImage.image.GetPixel(Thumbx, Thumby);
                            OutPutFile.SetPixel(x + Thumbx, y + Thumby, pixel);
                            //OutPutFile.Save(".\\output.png");
                            Thumby++;
                        }
                        Thumbx++;
                    }

                    
                    
                }
            }*/

            OutPutFile.Save(".\\output.png");
        }
        private static float GetScore(float r, float g, float b)
        {
             //float Score = (float)Math.Sqrt( r * r + g * g + b * b);
            //float j = sizeSmallX * sizeSmallY;
            float Score = (r + b + g) / 3;
            return Score;
        }
        private static ClassClass.ShrunkImages ChooseImage(List<ClassClass.ShrunkImages> ImageList, float BlockScore)
        {
            ClassClass.ShrunkImages ChosenImage = new ClassClass.ShrunkImages();
            float Diff = 0;
            foreach (ClassClass.ShrunkImages SI in ImageList)
            {
                float tempDiff = Math.Abs(SI.score - BlockScore); 

                if(Diff == 0)
                {
                    Diff = Math.Abs(SI.score - BlockScore);
                }
                if (tempDiff <= Diff)
                {
                    ChosenImage = SI;
                }
            }

            return ChosenImage;
        }

        private static void BlockData(Bitmap InputFile, int sizeSmallX, int sizeSmallY, int x, int y, ref float r, ref float g, ref float b, ref float a)
        {
            for (int BlockX = x; BlockX < x + sizeSmallX; BlockX++)
            {
                for (int BlockY = y; BlockY < y + sizeSmallY; BlockY++)
                {
                    try
                    {
                        Color pixel = InputFile.GetPixel(BlockX, BlockY);
                        r = r + pixel.R;
                        g = g + pixel.G;
                        b = b + pixel.B;
                        
                    }
                    catch { break; }
                }
            }
        }

        private static void ImageAverages(List<string> AllShrunkFiles, List<ClassClass.ShrunkImages> ImageList)
        {
            foreach (string fil in AllShrunkFiles)
            {
                ClassClass.ShrunkImages Temp = new ClassClass.ShrunkImages();
                Temp.image = new Bitmap(fil);
                Console.WriteLine(Temp.image.Height + " " + Temp.image.Width);
                Temp.FilePath = fil;
                float r = 0;
                float g = 0;
                float b = 0;
                float a = 0;
                for (int x = 0; x < Temp.image.Width; x++)
                {
                    for (int y = 0; y < Temp.image.Height; y++)
                    {
                        Color pixel = Temp.image.GetPixel(x, y);
                        r = r + pixel.R;
                        g = g + pixel.G;
                        b = b + pixel.B;
                        a = a + pixel.A;
                    }
                }
                //Temp.score = (r + g + b) / 3;
                Temp.score = GetScore(r,g,b);

                ImageList.Add(Temp);
                //Console.ReadKey();
            }
        }

        private static void ShrinkAll()
        {
            List<string> Files = FileClass.GetAllFileNames(MainPicFolder);
            foreach (string File in Files)
            {
                FileClass.CreateThumbnail(15, 30, File);
            }
        }
    }
}
