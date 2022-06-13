using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace MeshManager
{
    public class Material
    {
        public string Name { get; set; }
        public TextureMap map_Kd;
        public TextureMap map_Ka;
        public TextureMap map_d;
        public void WriteToMtl(StreamWriter writer, string texturepath = "")
        {
            writer.WriteLine("newmtl " + Name);
            writer.WriteLine("Ns 10.000");
            writer.WriteLine("Ni 1.500");
            writer.WriteLine("d 1.000");
            writer.WriteLine("Tr 0.000");
            writer.WriteLine("Tf 1.000 1.000 1.000");
            writer.WriteLine("illum 2");
            writer.WriteLine("Ka 0.588 0.588 0.588");
            writer.WriteLine("Kd 0.588 0.588 0.588");
            writer.WriteLine("Ks 0.000 0.000 0.000");
            writer.WriteLine("Ke 0.000 0.000 0.000");

            //if (map_Ka!= null)
            //{
            //    writer.WriteLine("map_Ka" + map_Ka.GetText());
            //    map_Ka.ExportTexture(texturepath);
            //}
            //if (map_Kd != null)
            //{
            //    writer.WriteLine("map_Kd" + map_Kd.GetText());
            //    map_Kd.ExportTexture(texturepath);
            //}
            //if (map_d != null)
            //{
            //    writer.WriteLine("map_d" + map_d.GetText());
            //    map_d.ExportTexture(texturepath);
            //}

        }
    }
}
