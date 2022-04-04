using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.FileIO;

namespace lab_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // File
        string photoPath;
        byte[] binaryPhoto;
        byte[] binaryPhotoCMY;
        Bitmap bmp;

        int MBundoBufferSize = 5;




        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            panel1.Show();
            panel1.BringToFront();

            OpenFileDialog file = new OpenFileDialog();

            file.Filter = "jpg|*.jpg|bmp|*.bmp|png|*.png";
            file.Title = "Select an image";

            if (file.ShowDialog()==DialogResult.OK)
            {
                //pictureBox1.Image = new Bitmap(file.OpenFile());
                Bitmap bmp2 = new Bitmap(file.OpenFile());
                bmp = bmp2;
                pictureBox1.Image = bmp;

                photoPath = file.FileName;
                FileStream fs = new FileStream(photoPath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                
                binaryPhoto = br.ReadBytes((int)fs.Length);

                


                fs.Close();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show("Do you want to close?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question)== DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        //FILTERS

        public static Bitmap ByteToImage(byte[] blob)
        {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();
            return bm;
        }

        private void kanalskeSlikeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.Hide();
            panel2.Show();
            panel2.BringToFront();

            if(binaryPhoto == null)
            {
                loadToolStripMenuItem_Click(sender, e);
            }
            int width = bmp.Width;
            int height = bmp.Height;

            Bitmap rbmp = new Bitmap(bmp);
            Bitmap gbmp = new Bitmap(bmp);
            Bitmap bbmp = new Bitmap(bmp);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color p = bmp.GetPixel(x, y);
                    int a = p.A;
                    int r = p.R;
                    int g = p.G;
                    int b = p.B;

                    rbmp.SetPixel(x, y, Color.FromArgb(a, r, 0, 0));
                    gbmp.SetPixel(x, y, Color.FromArgb(a, 0, g, 0));
                    bbmp.SetPixel(x, y, Color.FromArgb(a, 0, 0, b));
                }
            }

            pictureBox3.Image = rbmp;
            pictureBox4.Image = gbmp;
            pictureBox5.Image = bbmp;
            pictureBox2.Image = bmp;
            //pictureBox2.Image = ByteToImage(binaryPhoto);
            
        }

        //OPTIONS
        private void velicinaUndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string st = "";
            int value = 0;
            bool tryToParse = false;

            while (value <5 || tryToParse != true)
            {
                st = Interaction.InputBox("Enter the size of the Undo buffer in MB (min is 5MB)", "Undo buffer size", "", -1, -1);

                if ( st != "")
                {
                    tryToParse = int.TryParse(st, out value);
                    if (!tryToParse)
                    {
                        MessageBox.Show("You must enter a number.");
                    }
                    else
                    {
                        if(value < 5)
                        {
                            MessageBox.Show("You must enter a value bigger then 5MB.");
                        }
                    }

                }
                else
                {
                    //kliknuto cancle
                    value = MBundoBufferSize;
                }
            }

            MBundoBufferSize = value;
            testBox.Text = value.ToString();

            
        }

        private static Image InvertGDI(Image imgSource)
        {
            Bitmap bmpDest = null;

            using (Bitmap bmpSource = new Bitmap(imgSource))
            {
                bmpDest = new Bitmap(bmpSource.Width, bmpSource.Height);

                for (int x = 0; x < bmpSource.Width; x++)
                {
                    for (int y = 0; y < bmpSource.Height; y++)
                    {

                        Color clrPixel = bmpSource.GetPixel(x, y);

                        clrPixel = Color.FromArgb(255 - clrPixel.R, 255 -
                           clrPixel.G, 255 - clrPixel.B);

                        bmpDest.SetPixel(x, y, clrPixel);
                    }
                }
            }

            return (Image)bmpDest;

        }

        private void invertFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.Hide();
            panel2.Show();
            panel2.BringToFront();

            if (binaryPhoto == null)
            {
                loadToolStripMenuItem_Click(sender, e);
            }

            Bitmap bmp2 = new Bitmap(bmp);

            int width = bmp.Width;
            int height = bmp.Height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color p = bmp.GetPixel(x, y);

                    p = Color.FromArgb(255 - p.R, 255 - p.G, 255 - p.B);

                    bmp2.SetPixel(x, y, p);
                }
            }

            bmp = bmp2;

            kanalskeSlikeToolStripMenuItem_Click(sender, e);
        }
    }
}
