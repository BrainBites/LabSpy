using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace DllIm
{
    class Client_Tools
    {
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey); 

        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Int32 vKey);

        public void Capture_Screen(string picname)
        {
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            MessageBox.Show(bounds.ToString());
            Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
            bitmap.Save("E:/Testing/" + picname + ".jpg", ImageFormat.Jpeg);
        }

        public bool isKeyPressed()
        {
            bool status = false;
            string temp = "";
            foreach (System.Int32 i in Enum.GetValues(typeof(Keys)))
            {
                int x = GetAsyncKeyState(i);
                if ((x == 1) || (x == Int16.MinValue))
                    status = true;                  
            }
            return status;

        }
       
        

    }
}
