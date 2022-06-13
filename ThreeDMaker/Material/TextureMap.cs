using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MeshManager
{
    public class TextureMap
    {
       // public Bitmap Bitmap;
        public string path;
        public float[] o;
        public float[] s;
        public float[] t;
        public TextureMap(string path)
        {
            this.path = path;
             o = new float[3] { 0, 0, 0 };
             s = new float[3] {1,1,1 };
             t = new float[3] {0,0,0};
        }
        public string GetText()
        {
            string text = "";
            if(s[0]!=1 || s[0] != 1|| s[0] != 1)
            {
                text += " -s " + s[0] + " " + s[1] + " " + s[2]; 
            }
            if (o[0] != 0 || o[0] != 0 || o[0] != 0)
            {
                text += " -o " + o[0] + " " + o[1] + " " + o[2];
            }
            if (t[0] != 0 || t[0] != 0 || t[0] != 0)
            {
                text += " -t " + t[0] + " " + t[1] + " " + t[2];
            }
            text += " " + path + ".jpg";
            return text;
        }
        public void ExportTexture(string texturepath)
        {

            int size = 256;
            //Brush brush = Brushes.Gray;
            //if(Bitmap == null)
            //{
            //    using (Bitmap bitmap = new Bitmap(size, size))
            //    {
            //        Graphics graphics = Graphics.FromImage(bitmap);
            //        graphics.Clear(Color.White);
            //        //int nGrid = 8;
            //        //int gridSize = size / nGrid;
            //        //for (int i = 0; i < nGrid; i++)
            //        //{
            //        //    for (int j = 0; j < nGrid; j++)
            //        //    {
            //        //        if ((i + j) % 2 == 0)
            //        //            graphics.FillRectangle(brush, i * gridSize, j * gridSize, gridSize, gridSize);
            //        //    }
            //        //}
            //        Directory.CreateDirectory(texturepath);
            //        Directory.CreateDirectory(texturepath + path);
            //        bitmap.Save(texturepath + path + ".jpg", ImageFormat.Jpeg);

            //    }
            //}
            //else
            //{
            //    Directory.CreateDirectory(texturepath);
            //    Bitmap.Save(texturepath + path + ".jpg", ImageFormat.Jpeg);
            //}
           
        }
    }
}
