using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemCheck.Model
{
    public class Analyze
    {
        public static byte[] Exec(string command)
        {
            if (command == "cap")
            {
                var rect = Screen.PrimaryScreen.Bounds;

                var bmp = new Bitmap(rect.Width, rect.Height);

                using (var g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(rect.X, rect.Y, 0, 0, rect.Size, CopyPixelOperation.SourceCopy);
                }

                var ms = new MemoryStream();
                bmp.Save(ms, ImageFormat.Png);

                return ms.GetBuffer();
            }

            if (command == "app_end")
            {

            }


            return new byte[] {101,110,100};
        }
    }
}
