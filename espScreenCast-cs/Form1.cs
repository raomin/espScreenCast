using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

<<<<<<< HEAD
namespace espScreenCast
=======
namespace wsNet
>>>>>>> c83a49eda35bdea1a4968e48cb623e01323bc6b1
{
    

    public partial class Form1 : Form
    {
        Bitmap bmpScreenshot = new Bitmap(8,
                                8,
                                PixelFormat.Format24bppRgb);
        private static Timer aTimer;
        WSBitmap wsBitmap = new WSBitmap();
        List<PictureBox> pictureboxes;
        Graphics gfxScreenshot;
        bool sending = false;
        bool scrolling = false;
        private int brightness;

        public Form1()
        {
            InitializeComponent();
            gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            pictureboxes = Enumerable.Range(1, 64).ToList().Select(h => new PictureBox() { Name = "runebox" + h.ToString() }).ToList();
            var i = 0;
            pictureboxes.ForEach(pic =>
            {
                int y = (i / 8) * 30 + panel1.Location.X;
                int x = (i % 8) * 30 + panel1.Location.Y;
                pic.BackColor = Color.FromArgb(new Random(i).Next(Int32.MaxValue));
                pic.Size = new Size(25, 25);
                pic.Location = new Point(x, y);
                Console.WriteLine("adding one\n");

                this.panel1.Controls.Add(pic);
                i++;
            });

        }
        private void getSnip()
        {
            var pos = System.Windows.Forms.Cursor.Position;
            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(pos.X,
                                        pos.Y,
                                        0,
                                        0,
                                        new Size(8, 8));


            wsBitmap.emulate(pictureboxes, bmpScreenshot);
            if (sending)
                wsBitmap.send(bmpScreenshot, brightness);


        }

        private Bitmap textToBmp(String str)
        {
            Font font = new Font("Tahoma", 8);

            Bitmap bmp = new Bitmap(800,8,PixelFormat.Format24bppRgb);
            RectangleF rectf = new RectangleF(0, 0, bmp.Width, bmp.Height);
            Graphics g = Graphics.FromImage(bmp);
            StringFormat format = new StringFormat()
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center
            };

            g.DrawString(str, font, Brushes.Red, rectf, format);
            // Flush all graphics changes to the bitmap
            g.Flush();
            return bmp;
        }

        int pos = 0;
        private void scrollTick(Bitmap txtBmp,int strLen)
        {

            using (Graphics grD = Graphics.FromImage(bmpScreenshot))
            {
                grD.DrawImage(txtBmp, new Rectangle(0,0,8,8), new Rectangle(pos++,0,8,8), GraphicsUnit.Pixel);
            }
            wsBitmap.emulate(pictureboxes, bmpScreenshot);
            wsBitmap.send(bmpScreenshot, brightness);
            if (pos > strLen) { 
                aTimer.Dispose();
                pos = 0;
                scrolling = false;
            }
        }

        private void scrollText(String text)
        {
            scrolling = true;
            Bitmap bmp = textToBmp(text);
            var size = richTextBox1.GetPreferredSize(Size.Empty);
            if (!(aTimer is null)) { 
                aTimer.Dispose();
                pos = 0;
            }
            aTimer = new Timer();
            aTimer.Interval =  hScrollBar2.Value * 2;

            aTimer.Tick += (sender, e) => scrollTick(bmp,size.Width);
            aTimer.Enabled = true;

        }




        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!scrolling)
                getSnip();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            sending = checkBox1.Checked;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            checkBox1.Enabled = (textBox1.Text.Length > 1);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            wsBitmap.setHost(textBox1.Text);
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            label3.Text = hScrollBar1.Value.ToString();
            brightness = hScrollBar1.Value;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            hScrollBar1_Scroll(null, null);
            hScrollBar2_Scroll(null, null);

            textBox1_TextChanged(null, null);

            textBox1_Leave(null, null);

        }

        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            label6.Text = hScrollBar2.Value.ToString();
            timer1.Interval = hScrollBar2.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            scrollText(richTextBox1.Text);
        }

    }
}
