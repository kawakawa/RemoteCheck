using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemCommon.Model;

namespace SystemCheck.Model
{
    public class Analyze
    {
        private static byte[] eof = new byte[] { 45,79,70,95,100,97,116,97};
        
        
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


                var buf = ms.GetBuffer();
                buf = buf.Concat(eof).ToArray();
                
                return buf;
            }

            if (command == "app_end")
            {

            }

            if (command == "ps")
            {
                var  ps =
                    System.Diagnostics.Process.GetProcesses();

                var list = new List<ProcessProperty>();
                foreach (var p in ps)
                {
                    var pp = new ProcessProperty();
                    pp.Name = p.ProcessName;
                    pp.Id = p.Id;
                
                    list.Add(pp);
                }
            
                var sb =new StringBuilder();
                foreach (var p in list.OrderBy(n=>n.Name))
                {
                    sb.AppendLine(p.Name + "\t" + p.Id);
                }
                
                
                var  b = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
                
                var buf = b.Concat(eof).ToArray();
                return buf;
            }


            if (command.StartsWith("kill/"))
            {
                var split = command.Split('/');

                if (int.TryParse(split[1], out var id))
                {
                    var ps = System.Diagnostics.Process.GetProcessById(id);
                    ps.Kill();
                }
                var b= new byte[] {79,75,13,10};
                var buf = b.Concat(eof).ToArray();
                return buf;
                
            }


            var end = new byte[] {101,110,100,13,10};
            return end.Concat(eof).ToArray();
        }
    }
}
