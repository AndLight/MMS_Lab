using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
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
        Bitmap bmp;
        long imgSize = 1048576; //1MB
        long MBundoBufferSize = 41943040; // 5MB
        int undoSize = 5;
        float gama = 1;

        List<ImageStateWrapper> imageStack = new List<ImageStateWrapper>();
        List<ImageStateWrapper> redoImageStack = new List<ImageStateWrapper>();

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if(bmp != null)
            {
                DialogResult dialogResult = MessageBox.Show("Progress will be lost. Do you want to save?", "Warning", MessageBoxButtons.YesNoCancel);

                if (dialogResult == DialogResult.Yes)
                {
                    //Add save image option !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    saveToolStripMenuItem_Click(sender, e);
                    loadImage();
                }
                else if (dialogResult == DialogResult.No)
                {
                    imageStack.Clear();
                    redoImageStack.Clear();
                    loadImage();
                }
            }
            else
            {
                loadImage();
            }
            
            

        }

        private void loadImage()
        {
            panel1.Show();
            panel1.BringToFront();

            String filePath;

            OpenFileDialog file = new OpenFileDialog();

            file.Filter = "bmp|*.bmp|jpg|*.jpg|png|*.png";
            file.Title = "Select an image";

            if (file.ShowDialog() == DialogResult.OK)
            {
                filePath = file.FileName;
                imgSize = new System.IO.FileInfo(filePath).Length;
                if(imgSize > MBundoBufferSize)
                {
                    MBundoBufferSize = imgSize * 5;
                }
                //testBox.Text = imgSize.ToString();

                Bitmap bmp2 = new Bitmap(file.OpenFile());
                this.bmp = (Bitmap)bmp2.Clone();
                pictureBox1.Image = bmp;
            }


            undoHandeler(true); // always loads in view1
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

        //FILTERS ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void kanalskeSlikeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (bmp == null)
            {
                loadToolStripMenuItem_Click(sender, e);
            }

            panel1.Hide();
            panel2.Show();
            panel2.BringToFront();

            Bitmap image = new Bitmap(bmp);
            Bitmap cyanImage = new Bitmap(image.Width, image.Height);
            Bitmap magentaImage = new Bitmap(image.Width, image.Height);
            Bitmap yellowImage = new Bitmap(image.Width, image.Height);

            for(int i=0; i<image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    Color pixel = image.GetPixel(j, i);

                    double r = pixel.R / 255.0;
                    double g = pixel.G / 255.0;
                    double b = pixel.B / 255.0;

                    double kRatio = 1.0 - Math.Max(r, Math.Max(g, b));
                    double cRatio = (1.0 - r - kRatio) / (1.0 - kRatio + 0.000000001);
                    double mRatio = (1.0 - g - kRatio) / (1.0 - kRatio + 0.000000001);
                    double yRatio = (1.0 - b - kRatio) / (1.0 - kRatio + 0.000000001);

                    //int k = Convert.ToInt32(kRatio * 255);
                    int c = Convert.ToInt32(cRatio * 255);
                    int m = Convert.ToInt32(mRatio * 255);
                    int y = Convert.ToInt32(yRatio * 255);

                    //Color kPix = Color.FromArgb(k, Color.Black);
                    Color cPix = Color.FromArgb(c, Color.Cyan);
                    Color mPix = Color.FromArgb(m, Color.Magenta);
                    Color yPix = Color.FromArgb(y, Color.Yellow);

                    cyanImage.SetPixel(j, i, cPix);
                    magentaImage.SetPixel(j, i, mPix);
                    yellowImage.SetPixel(j, i, yPix);
                    
                }
            }

            pictureBox2.Image = bmp;
            pictureBox3.Image = cyanImage;
            pictureBox4.Image = magentaImage;
            pictureBox5.Image = yellowImage;

            undoHandeler(false);

        }

        private void invertFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            removeGamaControls();

            if (bmp == null)
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

            this.bmp = (Bitmap)bmp2.Clone();

            if (Controls.GetChildIndex(panel2) == 0)
            {
                undoHandeler(false);
                kanalskeSlikeToolStripMenuItem_Click(sender, e);
            }
            else if (Controls.GetChildIndex(panel1) == 0)
            {
                undoHandeler(true);
                pictureBox1.Image = bmp;
            }
        }

        private void gamaFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bmp == null)
            {
                loadToolStripMenuItem_Click(sender, e);
            }

            GamaLabel.Visible = Visible;
            GamaTrackBar.Visible = Visible;
            GamaValueLabel.Visible = Visible;

            //testBox.Text = gama.ToString();

        }
        private void embossLaplacianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            removeGamaControls();

            if (bmp == null)
            {
                loadToolStripMenuItem_Click(sender, e);
            }
            bmp = Emboss(bmp);
            bmp = Laplacian(bmp);

            if (Controls.GetChildIndex(panel2) == 0)
            {
                undoHandeler(false);
                kanalskeSlikeToolStripMenuItem_Click(sender, e);
            }
            else if (Controls.GetChildIndex(panel1) == 0)
            {
                undoHandeler(true);
                pictureBox1.Image = bmp;
            }
        }

        private Bitmap Emboss(Bitmap bitmap)
        {

            Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            Color pixel1, pixel2;

            for (int x = 0; x < (bitmap.Width - 1); x++)
            {
                for (int y = 0; y < (bitmap.Height - 1); y++)
                {
                    int r = 0, g = 0, b = 0;

                    pixel1 = bitmap.GetPixel(x, y);
                    pixel2 = bitmap.GetPixel(x + 1, y + 1);

                    r = Math.Min(Math.Abs(pixel1.R - pixel2.R + 128), 255);
                    g = Math.Min(Math.Abs(pixel1.G - pixel2.G + 128), 255);
                    b = Math.Min(Math.Abs(pixel1.B - pixel2.B + 128), 255);

                    newBitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            return newBitmap;
        }

        private Bitmap Laplacian(Bitmap bitmap)
        {

            Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            Bitmap oldBitmap = bitmap;
            Color pixel;

            int[] Laplacian = { -1, -1, -1, -1, 9, -1, -1, -1, -1 };

            for (int x = 1; x < bitmap.Width - 1; x++)
                for (int y = 1; y < bitmap.Height - 1; y++)
                {
                    int r = 0, g = 0, b = 0;
                    int Index = 0;
                    for (int col = -1; col <= 1; col++)
                        for (int row = -1; row <= 1; row++)
                        {
                            pixel = oldBitmap.GetPixel(x + row, y + col); r += pixel.R * Laplacian[Index];
                            g += pixel.G * Laplacian[Index];
                            b += pixel.B * Laplacian[Index];
                            Index++;
                        }
                    //Handle color value overflow
                    r = r > 255 ? 255 : r;
                    r = r < 0 ? 0 : r;
                    g = g > 255 ? 255 : g;
                    g = g < 0 ? 0 : g;
                    b = b > 255 ? 255 : b;
                    b = b < 0 ? 0 : b;
                    newBitmap.SetPixel(x - 1, y - 1, Color.FromArgb(r, g, b));

                }

            return newBitmap;


        }

        private void GamaTrackBar_Scroll_1(object sender, EventArgs e)
        {

            GamaValueLabel.Text = GamaTrackBar.Value.ToString();
            /*gama = 1+(0.4f * GamaTrackBar.Value);

            //testBox.Text = gama.ToString();
            
            Bitmap newBitmap = new Bitmap(bmp.Width, bmp.Height);
            Graphics g = Graphics.FromImage(newBitmap);
            ImageAttributes ia = new ImageAttributes();

            ia.SetGamma(gama);
            g.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, ia);
            g.Dispose();
            ia.Dispose();

            bmp = newBitmap;

            if (Controls.GetChildIndex(panel2) == 0)
            {
                kanalskeSlikeToolStripMenuItem_Click(sender, e);
            }
            else if (Controls.GetChildIndex(panel1) == 0)
            {
                pictureBox1.Image = bmp;
            }
            */
            Bitmap newBitmap = (Bitmap)bmp.Clone();
            Color c;
            byte[] redGamma = CreateGammaArray(GamaTrackBar.Value);
            byte[] greenGamma = CreateGammaArray(GamaTrackBar.Value);
            byte[] blueGamma = CreateGammaArray(GamaTrackBar.Value);
            for (int i = 0; i < newBitmap.Width; i++)
            {
                for (int j = 0; j < newBitmap.Height; j++)
                {
                    c = newBitmap.GetPixel(i, j);
                    newBitmap.SetPixel(i, j, Color.FromArgb(redGamma[c.R], greenGamma[c.G], blueGamma[c.B]));
                }
            }

            this.bmp = (Bitmap)newBitmap.Clone();

            if (Controls.GetChildIndex(panel2) == 0)
            {
                undoHandeler(false);
                kanalskeSlikeToolStripMenuItem_Click(sender, e);
            }
            else if (Controls.GetChildIndex(panel1) == 0)
            {
                undoHandeler(true);
                pictureBox1.Image = bmp;
            }

        }

        private void removeGamaControls()
        {
            GamaLabel.Visible = false;
            GamaTrackBar.Visible = false;
            GamaValueLabel.Visible = false;
        }

        private byte[] CreateGammaArray(double color)
        {
            byte[] gammaArray = new byte[256];
            for (int i = 0; i < 256; ++i)
            {
                gammaArray[i] = (byte)Math.Min(255, (int)((255.0 * Math.Pow(i / 255.0, 1.0 / color)) + 0.5));
            }
            return gammaArray;
        }

        private void edgeDetectDifferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.bmp = edgeDetectDifference(bmp, 50);

            //kanalskeSlikeToolStripMenuItem_Click(sender, e);
            if (Controls.GetChildIndex(panel2) == 0)
            {
                undoHandeler(false);
                kanalskeSlikeToolStripMenuItem_Click(sender, e);
            }
            else if (Controls.GetChildIndex(panel1) == 0)
            {
                undoHandeler(true);
                pictureBox1.Image = bmp;
            }
        }

        public Bitmap edgeDetectDifference(Bitmap b, byte nThreshold)
        {
            // This one works by working out the greatest difference between a nPixel and it's eight neighbours.
            // The threshold allows softer edges to be forced down to black, use 0 to negate it's effect.
            Bitmap b2 = (Bitmap)b.Clone();

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmData2 = b2.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr Scan02 = bmData2.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* p2 = (byte*)(void*)Scan02;

                //int nOffset = stride - b.Width * 3;
                int nOffset = 0;
                int nWidth = b.Width * 3;

                int nPixel = 0, nPixelMax = 0;

                p += stride;
                p2 += stride;

                for (int y = 1; y < b.Height - 1; ++y)
                {
                    p += 3;
                    p2 += 3;

                    for (int x = 3; x < nWidth - 3; ++x)
                    {
                        nPixelMax = Math.Abs((p2 - stride + 3)[0] - (p2 + stride - 3)[0]);

                        nPixel = Math.Abs((p2 + stride + 3)[0] - (p2 - stride - 3)[0]);

                        if (nPixel < nPixelMax)  nPixel=0;

                        nPixel = Math.Abs((p2 - stride)[0] - (p2 + stride)[0]);

                        if (nPixel < nPixelMax) nPixel = 0;

                        nPixel = Math.Abs((p2 + 3)[0] - (p2 - 3)[0]);

                        if (nPixel < nPixelMax) nPixel = 0;

                        if (nPixelMax > nThreshold && nPixelMax > p[0])
                            p[0] = (byte)Math.Max(p[0], nPixelMax);

                        ++p;
                        ++p2;
                    }

                    p += nOffset + 3;
                    p2 += nOffset + 3;
                }
            }

            b.UnlockBits(bmData);
            b2.UnlockBits(bmData2);

            return b;


        }

        private void displacementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplyRandomJitter(bmp, 5);

            //kanalskeSlikeToolStripMenuItem_Click(sender, e);
            if (Controls.GetChildIndex(panel2) == 0)
            {
                undoHandeler(false);
                kanalskeSlikeToolStripMenuItem_Click(sender, e);
            }
            else if (Controls.GetChildIndex(panel1) == 0)
            {
                undoHandeler(true);
                pictureBox1.Image = bmp;
            }
        }

        public static void ApplyRandomJitter(Bitmap bmp, short degree)
        {
            Bitmap TempBmp = (Bitmap)bmp.Clone();

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData TempBmpData = TempBmp.LockBits(new Rectangle(0, 0, TempBmp.Width, TempBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0.ToPointer();
                byte* TempPtr = (byte*)TempBmpData.Scan0.ToPointer();

                int stopAddress = (int)ptr + bmpData.Stride * bmpData.Height;

                int BmpWidth = bmp.Width;
                int BmpHeight = bmp.Height;
                int BmpStride = bmpData.Stride;
                int i = 0, X = 0, Y = 0;
                int Val = 0, XVal = 0, YVal = 0;
                short Half = (short)(degree / 2);
                Random rand = new Random();

                while ((int)ptr != stopAddress)
                {
                    X = i % BmpWidth;
                    Y = i / BmpWidth;

                    XVal = X + (rand.Next(degree) - Half);
                    YVal = Y + (rand.Next(degree) - Half);

                    if (XVal > 0 && XVal < BmpWidth && YVal > 0 && YVal < BmpHeight)
                    {
                        Val = (YVal * BmpStride) + (XVal * 3);

                        ptr[0] = TempPtr[Val];
                        ptr[1] = TempPtr[Val + 1];
                        ptr[2] = TempPtr[Val + 2];
                    }

                    ptr += 3;
                    i++;
                }
            }

            bmp.UnlockBits(bmpData);
            TempBmp.UnlockBits(TempBmpData);
        }
        //OPTIONS ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void velicinaUndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string st = "";
            long value = 1;
            bool tryToParse = false;

            DialogResult dialogResult = MessageBox.Show("Progress will be lost. Do you want to Change?", "Warning", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                while (value < 5 && tryToParse != true)
                {
                    st = Interaction.InputBox("Enter the size of the Undo buffer in MB (min is 5MB)", "Undo buffer size", "", -1, -1);

                    if (st != "")
                    {
                        tryToParse = long.TryParse(st, out value);
                        if (!tryToParse)
                        {
                            MessageBox.Show("You must enter a number.");
                        }
                        else
                        {
                            if (value < 5)
                            {
                                MessageBox.Show("You must enter a value bigger then 5MB.");
                            }
                        }

                    }
                    else
                    {
                        //kliknuto cancle
                        value = MBundoBufferSize / 8388608;
                        tryToParse = true;
                    }
                }
            }
            

            

            MBundoBufferSize = value* 8388608;
            long a = MBundoBufferSize / imgSize;
            undoSize = (int)a;           

        }

       private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (this.imageStack.Count != 0)
            {
                this.redoImageStack.Insert(0, this.imageStack.Last());

                if (this.imageStack.Count != 0 && this.imageStack.Last().firstView == true)
                {
                    panel2.Hide();
                    panel1.Show();
                    panel1.BringToFront();

                    pictureBox1.Image = this.imageStack.Last().Image;
                    imageStack.RemoveAt(imageStack.Count - 1);
                    this.bmp = (Bitmap)this.pictureBox1.Image;
                    

                }
                else if (this.imageStack.Count != 0 && this.imageStack.Last().firstView == false)
                {
                    panel1.Hide();
                    panel2.Show();
                    panel2.BringToFront();

                    this.bmp = (Bitmap)this.imageStack.Last().Image;
                    imageStack.RemoveAt(imageStack.Count - 1);
                    kanalskeSlikeToolStripMenuItem_Click(sender, e);
                }
                else if (this.imageStack.Count == 0)
                {
                    MessageBox.Show("No more undo", "Warning", MessageBoxButtons.OK);
                }
            }
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (this.redoImageStack.Count != 0 && this.redoImageStack.Last().firstView == true)
            {
                panel2.Hide();
                panel1.Show();
                panel1.BringToFront();

                pictureBox1.Image = this.imageStack.Last().Image;
                this.bmp = (Bitmap)this.pictureBox1.Image;

                this.imageStack.Insert(0, this.redoImageStack.Last());
                this.redoImageStack.RemoveAt(redoImageStack.Count - 1);

            }
            else if (this.redoImageStack.Count != 0 && this.redoImageStack.Last().firstView == false)
            {
                panel1.Hide();
                panel2.Show();
                panel2.BringToFront();

                this.bmp = (Bitmap)this.imageStack.Last().Image;
                kanalskeSlikeToolStripMenuItem_Click(sender, e);

                this.imageStack.Insert(0, this.redoImageStack.Last());
                this.redoImageStack.RemoveAt(redoImageStack.Count - 1);
            }
            else if (this.redoImageStack.Count == 0)
            {
                MessageBox.Show("No more redo", "Warning", MessageBoxButtons.OK);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(pictureBox2.Image == null)
            {
                if(pictureBox1.Image == null)
                {
                    loadImage();
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("You want to save the original photo before using a filter?", "?", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        using (SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = @"bmp|*.bmp|jpg|*.jpg|png|*.png" })
                        {
                            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                pictureBox1.Image.Save(saveFileDialog.FileName);
                            }
                        }
                        
                    }
                }
            }
            else
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = @"bmp|*.bmp|jpg|*.jpg|png|*.png" })
                {
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        pictureBox2.Image.Save(saveFileDialog.FileName);
                    }
                }
                
            }
        }

        
        private void undoHandeler(bool view1)
        {
            this.imageStack.Insert(0, new ImageStateWrapper(this.bmp, view1));

            if (this.undoSize < this.imageStack.Count)
            {
                imageStack.RemoveAt(imageStack.Count-1);
            }
        }

        public class ImageStateWrapper
        {
            public System.Drawing.Image Image { get; set; }
            public bool firstView { get; set; }

            public ImageStateWrapper(System.Drawing.Image image, bool firstView)
            {
                this.firstView = firstView;
                this.Image = image;
            }
        }

    }
}

