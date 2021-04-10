//using SkiaSharp;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

class Brick
{
    /*public const int RGB = 3;
    public const int HEIGHT = 14;
    public const int WIDTH = 28;

    public int[,,] pixels = new int[HEIGHT, WIDTH, RGB];

    public void Parse(int[] a)
    {
        int index = 0;
        for (int i = 0; i < RGB; i++)
        {
            for (int j = 0; j < HEIGHT; j++)
            {
                for (int k = 0; k < WIDTH; k++)
                {
                    pixels[j, k, i] = a[index];
                    index++;
                }
            }
        }
    }
    public void Convert(int n)
    {
        SKBitmap bit = new SKBitmap(WIDTH, HEIGHT);
        for (int h = 0; h < HEIGHT; h++)
        {
            for (int w = 0; w < WIDTH; w++)
            {
                int r, g, b;
                r = pixels[h, w, 0];
                g = pixels[h, w, 1];
                b = pixels[h, w, 2];

                bit.SetPixel(w, h, new SKColor((byte)r, (byte)g, (byte)b));
            }
        }
        //bit.Save($"C:\\Users\\miste\\Desktop\\ArcadeGame-main\\Assets\\Resources\\brick{n}.png");
        //bit.Encode(SKWStream.,SKEncodedImageFormat.Png, 100)
        var image = SKImage.FromBitmap(bit);
        var data = image.Encode(SKEncodedImageFormat.Png, 100);
        var stream = File.OpenWrite($"C:\\Users\\Raflaan\\Documents\\GitHub\\ArcadeGame\\Assets\\Resources\\brick{n}.png");
        data.SaveTo(stream);
    }*/
}
