using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace espScreenCast
{
    class WSBitmap
    {
        public Bitmap bitmap;
        private UdpClient udpClient;
        private int port = 2812;

        public void setHost(string host)
        {
            this.udpClient = new UdpClient(host, port);
        }

        public void update()
        {
            bitmap.GetHbitmap();
        }

        public static byte[] ImageToByte(Bitmap bitmap)
        {
            var bitdata = bitmap.LockBits(new Rectangle(0, 0, 8, 8), System.Drawing.Imaging.ImageLockMode.ReadWrite, bitmap.PixelFormat);
            // Get the address of the first line.
            IntPtr ptr = bitdata.Scan0;
            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bitdata.Stride) * bitmap.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
            bitmap.UnlockBits(bitdata);
            return rgbValues;
        }

        public void emulate(List<PictureBox> pictureboxes, Bitmap bitmap)
        {
            var i = 0;
            byte[] bytes = ImageToByte(bitmap);

            pictureboxes.ForEach(pic => {
                pic.BackColor = Color.FromArgb(bytes[i * 3+2], bytes[i * 3 +1 ], bytes[i * 3]);
                i++;
            });
        }

        public void send(Bitmap bitmap, int brightness = 128)
        {
            byte[] bytes = ImageToByte(bitmap);
            List<byte> ts = new List<byte>();
            ts.AddRange(bytes);
            ts.Add((byte)brightness);

            udpClient.Send(ts.ToArray(), ts.Count);
        }
    }
}
