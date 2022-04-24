using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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

        //Histograms
        int min = 1;
        int max = 255;

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

        //LAB 2 /////////////////////////////////////////////////////////////////////////////////////////

        
        private void makeHistogramsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(bmp == null)
            {
                loadImage();
            }
            ChartPanel.Show();
            ChartPanel.BringToFront();
            pictureBoxChart.Image = bmp;

            makeHistograms("Red");
            makeHistograms("Green");
            makeHistograms("Blue");

            undoHandeler(true);
        }

        public void makeHistograms(string pickAColor)
        {
            BitmapData data = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            
            unsafe
            {
                byte* ptr = (byte*)data.Scan0;

                int remain = data.Stride - data.Width * 3;
                
                int[] histogram = new int[256];
                for (int i = 0; i < histogram.Length; i++)
                    histogram[i] = 0;
                
                int num= 0;
                switch (pickAColor)
                {
                    case "Blue": num = 0; break;
                    case "Green": num = 1; break;
                    case "Red": num = 2; break;
                }

                for (int i = 0; i < data.Height; i++)
                {
                    for (int j = 0; j < data.Width; j++)
                    {
                        int mean = ptr[num];

                        histogram[mean]++;

                        ptr += 3;
                    }

                    ptr += remain;
                }

                int MostMin=0;
                int MostMax=0;

                for(int i = min; i < 127; i++)
                {
                    if (histogram[i] > MostMin) { MostMin = histogram[i]; }
                }
                for (int i = 128; i < max; i++)
                {
                    if (histogram[i] > MostMax) { MostMax = histogram[i]; }
                }

                for (int i = 0; i < min; i++)
                {
                    histogram[i] = MostMin;
                }

                for (int i = max; i < 256; i++)
                {
                    histogram[i] = MostMax;
                }


                switch (pickAColor)
                {
                    case "Blue": chartBlue.Series["Blue"].Points.DataBindY(histogram);break;
                    case "Green": chartGreen.Series["Green"].Points.DataBindY(histogram); break;
                    case "Red": chartRed.Series["Red"].Points.DataBindY(histogram); break;
                }
                //chartRed.Series["Blue"].Points.DataBindY(histogram);

            }

            bmp.UnlockBits(data);
        }

        private void setMinValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string st = " ";
            int value = 0;
            bool tryToParse = false;
            bool valueRange = false;
            bool cancel = false;

           
                while (valueRange != true && tryToParse != true & cancel != true)
                {
                    st = Interaction.InputBox("Enter the minimum size for the histogram (between 1 and 126)", "Minimum size for the histogram", "", -1, -1);

                    if (st != "")
                    {
                        tryToParse = int.TryParse(st, out value);
                        if (!tryToParse)
                        {
                            MessageBox.Show("You must enter a number.");
                        }
                        else
                        {
                            if (value < 1 || value > 127)
                            {
                                MessageBox.Show("You must enter a value between 1 and 127.");
                            }
                            
                            if(value > 1 && value < 127)
                            {
                                this.min = value;
                                valueRange = true;
                                makeHistogramsToolStripMenuItem_Click(sender, e);
                            }
                            else
                            {
                                tryToParse = false;
                            }
                        }

                }
                    else
                    {
                        cancel = true;
                    }
                }
            //testLabel.Text = this.min.ToString();
            
        }

        private void setMaxValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string st = " ";
            int value = 0;
            bool tryToParse = false;
            bool valueRange = false;
            bool cancel = false;


            while (valueRange != true && tryToParse != true & cancel != true)
            {
                st = Interaction.InputBox("Enter the minimum size for the histogram (between 127 and 255)", "Minimum size for the histogram", "", -1, -1);

                if (st != "")
                {
                    tryToParse = int.TryParse(st, out value);
                    if (!tryToParse)
                    {
                        MessageBox.Show("You must enter a number.");
                    }
                    else
                    {
                        if (value < 127 || value > 255)
                        {
                            MessageBox.Show("You must enter a value between 127 and 255.");
                        }

                        if (value > 127 && value < 255)
                        {
                            this.max = value;
                            valueRange = true;
                            makeHistogramsToolStripMenuItem_Click(sender, e);
                        }
                        else
                        {
                            tryToParse = false;
                        }
                    }

                }
                else
                {
                    cancel = true;
                }
            }
            //testLabel.Text = this.max.ToString();
        }

        private void grayScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if(bmp == null)
            {
                loadImage();
            }

            panelGray.Show();
            panelGray.BringToFront();

            GrayOriginal.Image = bmp;
            Bitmap image = new Bitmap(bmp);

            Bitmap gray1 = new Bitmap(image.Width, image.Height); ;
            Bitmap gray2 = new Bitmap(image.Width, image.Height); ;
            Bitmap gray3 = new Bitmap(image.Width, image.Height); ;

            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    Color pixel = image.GetPixel(j, i);

                    //1
                    int gray_1 = (pixel.R + pixel.G + pixel.B) / 3;
                    Color g1 = Color.FromArgb(pixel.A, gray_1, gray_1, gray_1);
                    gray1.SetPixel(j, i, g1);

                    //2
                    int gray_2 = Math.Max(pixel.R, Math.Max(pixel.G, pixel.B));
                    Color g2 = Color.FromArgb(pixel.A, gray_2, gray_2, gray_2);
                    gray2.SetPixel(j, i, g2);

                    //3
                    int gray_3 = (Math.Max(pixel.R, Math.Max(pixel.G, pixel.B)) + Math.Min(pixel.R, Math.Min(pixel.G, pixel.B))) /2;
                    Color g3 = Color.FromArgb(pixel.A, gray_3, gray_3, gray_3);
                    gray3.SetPixel(j, i, g3);
                }
            }

            Gray1.Image = gray1;
            Gray2.Image = gray2;
            Gray3.Image = gray3;

        }
        
        
        //Dithering ////////////////////////////////////

        private void orderedDitheringToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if(bmp == null) { loadImage(); }
            
            panelDithering.Show();
            panelDithering.BringToFront();

            pictureBoxDitheringOrigilan.Image = bmp;

            Bitmap temp = (Bitmap)bmp.Clone();
            OrderedDitheringApply(temp, 2, 4);
            pictureBoxDitheringNew.Image = temp;
            undoHandeler(true);


        }

        public void OrderedDitheringApply(Bitmap b, int colorsNum, int thresholdSize)
        {

            float[,] dither2x2Matrix =
            new float[,] { { 1, 3 },
                           { 4, 2 } };

            float[,] dither3x3Matrix =
                new float[,] { { 8, 3, 4 },
                               { 6, 1, 2 },
                               { 7, 5, 9 } };

            float[,] dither4x4Matrix =
                new float[,] { { 1, 9, 3, 11 },
                               { 13, 5, 15, 7 },
                               { 4, 12, 2, 10 },
                               {16, 8, 14, 6 } };

            Bitmap bitmap = b;

            float[,] bayerMatrix;

            if (thresholdSize == 2)
            {
                bayerMatrix = new float[2, 2];
                for (int i = 0; i < 2; ++i)
                    for (int j = 0; j < 2; ++j)
                        bayerMatrix[i, j] = dither2x2Matrix[i, j] / 5;
            }
            else if(thresholdSize == 3)
            {
                bayerMatrix = new float[3, 3];
                for (int i = 0; i < 3; ++i)
                    for (int j = 0; j < 3; ++j)
                        bayerMatrix[i, j] = dither3x3Matrix[i, j] / 10;
            } else
            {
                bayerMatrix = new float[4, 4];
                for (int i = 0; i < 4; ++i)
                    for (int j = 0; j < 4; ++j)
                        bayerMatrix[i, j] = dither4x4Matrix[i, j] / 17;
            }

            for (int i = 0; i < bitmap.Width; ++i)
                for (int j = 0; j < bitmap.Height; ++j)
                {

                    Color color = bitmap.GetPixel(i, j);
                    float colorIntensity = color.GetBrightness();

                    float tempValue = (float)(Math.Floor((double)((colorsNum - 1) * colorIntensity)));
                    float re = (colorsNum - 1) * colorIntensity - tempValue;

                    if (re >= bayerMatrix[i % thresholdSize, j % thresholdSize])
                        tempValue++;

                    if (tempValue == 0)
                        bitmap.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                    else
                        bitmap.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                }
        }

        private void floydSteinbergToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bmp == null) { loadImage(); }

            panelDithering.Show();
            panelDithering.BringToFront();

            pictureBoxDitheringOrigilan.Image = bmp;

            Bitmap input = (Bitmap)bmp.Clone();
            Bitmap temp;

            temp = DitherCalculateAndSave(input);
            pictureBoxDitheringNew.Image = temp;
        }

        public Bitmap DitherCalculateAndSave(Bitmap input)
        {
            Bitmap dithered = DoDithering(input);
            double mse = CalculateMSE(input, dithered);
            double psrn = CalculatePSRN(mse);


             return dithered;
            // Console.WriteLine("Method: " + method.GetMethodName() + " PSRN: " + psrn);
            // dithered.Save(filenameWithoutExtension + method.GetFilenameAddition() + ".png");
        }


        byte[] currentBitmapAsByteArray = null;
        

        private Bitmap DoDithering(Bitmap input)
        {
            /*
            // Copy input to different bitmap so it can be modified
            Bitmap currentBitmap = new Bitmap(input);

            Color originalPixel = Color.White; // Default value isn't used
            Color newPixel = Color.White; // Default value isn't used
            short[] quantError = null; // Default values aren't used

            for (int y = 0; y < input.Height; y++)
            {
                for (int x = 0; x < input.Width; x++)
                {
                    originalPixel = currentBitmap.GetPixel(x, y);
                    //newPixel = this.colorFunction(originalPixel);
                    newPixel = TrueColorToWebSafeColor(originalPixel);
                    currentBitmap.SetPixel(x, y, newPixel);

                    //quantError = GetQuantError(originalPixel, newPixel);

                    //PushError(x, y, quantError);
                }
            }

            return currentBitmap;*/

            
            // Lock input bitmap for reading
            Rectangle rect = new Rectangle(0, 0, input.Width, input.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                input.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly,
                input.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int byteCount = Math.Abs(bmpData.Stride) * input.Height;
            this.currentBitmapAsByteArray = new byte[byteCount];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, this.currentBitmapAsByteArray, 0, byteCount);

            // Release the lock
            input.UnlockBits(bmpData);

            Color originalPixel = Color.White; // Default value isn't used
            Color newPixel = Color.White; // Default value isn't used
            short[] quantError = null; // Default values aren't used

            for (int y = 0; y < input.Height; y++)
            {
                for (int x = 0; x < input.Width; x++)
                {
                    originalPixel = GetColorFromByteArray(this.currentBitmapAsByteArray, x, y, input.Width);
                    newPixel = TrueColorToWebSafeColor(originalPixel);

                    SetColorToByteArray(this.currentBitmapAsByteArray, x, y, input.Width, newPixel);

                    //quantError = GetQuantError(originalPixel, newPixel);
                    //PushError(x, y, quantError);
                }
            }

            // Create new bitmap
            Bitmap returnBitmap = new Bitmap(input.Width, input.Height);
            // Lock it
            bmpData =
                returnBitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.WriteOnly,
                input.PixelFormat);
            // Get the address of the first line.
            ptr = bmpData.Scan0;
            // Copy array to it
            System.Runtime.InteropServices.Marshal.Copy(this.currentBitmapAsByteArray, 0, ptr, byteCount);
            // Unlock the bitmap
            returnBitmap.UnlockBits(bmpData);

            return returnBitmap;
        }

        private static Color GetColorFromByteArray(byte[] byteArray, int x, int y, int width)
        {
            int baseAddress = 3 * (y * width + x);
            return Color.FromArgb(byteArray[baseAddress + 2], byteArray[baseAddress + 1], byteArray[baseAddress]);
        }

        private static void SetColorToByteArray(byte[] byteArray, int x, int y, int width, Color color)
        {
            int baseAddress = 3 * (y * width + x);
            byteArray[baseAddress + 2] = color.R;
            byteArray[baseAddress + 1] = color.G;
            byteArray[baseAddress] = color.B;
        }

        protected short[] GetQuantError(Color originalPixel, Color newPixel)
        {
            short[] returnValue = new short[4];

            returnValue[0] = (short)(originalPixel.R - newPixel.R);
            returnValue[1] = (short)(originalPixel.G - newPixel.G);
            returnValue[2] = (short)(originalPixel.B - newPixel.B);
            returnValue[3] = (short)(originalPixel.A - newPixel.A);

            return returnValue;
        }

        private static double CalculateMSE(Bitmap original, Bitmap dithered)
        {
            long mseR = 0;
            long mseG = 0;
            long mseB = 0;

            int difference = 0;

            for (int i = 0; i < original.Width; i++)
            {
                for (int j = 0; j < original.Height; j++)
                {
                    difference = original.GetPixel(i, j).R - dithered.GetPixel(i, j).R;
                    mseR += (difference * difference);

                    difference = original.GetPixel(i, j).G - dithered.GetPixel(i, j).G;
                    mseG += (difference * difference);

                    difference = original.GetPixel(i, j).B - dithered.GetPixel(i, j).B;
                    mseB += (difference * difference);
                }
            }

            return (double)(mseR + mseG + mseB) / (double)((original.Width * original.Height) * 3);
        }

        private static double CalculatePSRN(double mse)
        {
            return 10 * Math.Log(255 * 255 / mse) / Math.Log(10);
        }

        private static Color TrueColorToWebSafeColor(Color inputColor)
        {
            Color returnColor = Color.FromArgb((byte)Math.Round(inputColor.R / 51.0) * 51,
                                                (byte)Math.Round(inputColor.G / 51.0) * 51,
                                                (byte)Math.Round(inputColor.B / 51.0) * 51);
            return returnColor;
        }

        protected bool IsValidCoordinate(int x, int y)
        {
            return (0 <= x && x < this.bmp.Width && 0 <= y && y < this.bmp.Height);
        }

        private void PushError(int x, int y, short[] quantError)
        {
            // Push error
            // 			X		7/16
            // 3/16		5/16	1/16

            int xMinusOne = x - 1;
            int xPlusOne = x + 1;
            int yPlusOne = y + 1;

            // Current row
            if (this.IsValidCoordinate(xPlusOne, y))
            {
                this.ModifyImageWithErrorAndMultiplier(xPlusOne, y, quantError, 7.0f / 16.0f);
            }

            // Next row
            if (this.IsValidCoordinate(xMinusOne, yPlusOne))
            {
                this.ModifyImageWithErrorAndMultiplier(xMinusOne, yPlusOne, quantError, 3.0f / 16.0f);
            }

            if (this.IsValidCoordinate(x, yPlusOne))
            {
                this.ModifyImageWithErrorAndMultiplier(x, yPlusOne, quantError, 5.0f / 16.0f);
            }

            if (this.IsValidCoordinate(xPlusOne, yPlusOne))
            {
                this.ModifyImageWithErrorAndMultiplier(xPlusOne, yPlusOne, quantError, 1.0f / 16.0f);
            }
        }

        public void ModifyImageWithErrorAndMultiplier(int x, int y, short[] quantError, float multiplier)
        {
            Color oldColor = Color.White; // Default color isn't used
            
                oldColor = this.bmp.GetPixel(x, y);
            

            // We limit the color here because we don't want the value go over 255 or under 0
            Color newColor = Color.FromArgb(
                                //GetLimitedValue(oldColor.A, (int)Math.Round(quantError[3] * multiplier)),
                                GetLimitedValue(oldColor.R, (int)Math.Round(quantError[0] * multiplier)),
                                GetLimitedValue(oldColor.G, (int)Math.Round(quantError[1] * multiplier)),
                                GetLimitedValue(oldColor.B, (int)Math.Round(quantError[2] * multiplier)));

            
                this.bmp.SetPixel(x, y, newColor);
            
        }

        private static byte GetLimitedValue(byte original, int error)
        {
            int newValue = original + error;
            return (byte)Clamp(newValue, byte.MinValue, byte.MaxValue);
        }

        private static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        //colorize /////////////////////////////////////////////////////

        private void simpleColorizeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if(bmp == null) { loadImage(); }
            panelColor.Show();
            panelColor.BringToFront();

            pictureColorOriginal.Image = bmp;
            pictureColorNew.Image = bmp;

            undoHandeler(true);
        }


        private void drawHueSaturation(double hue, double saturation)
        {

            pictureColorOriginal.Image = bmp;

            Bitmap image = (Bitmap)bmp.Clone();

            for (int x = 0; x < image.Width - 1; x++)
            {
                for (int y = 0; y < image.Height - 1; y++)
                {
                    Color pixel = image.GetPixel(x, y);

                    Color tmp = HSLtoRGB( pixel.R,  pixel.G,  pixel.B,  saturation,  hue);

                    image.SetPixel(x, y, tmp);
                }
            }

            pictureColorNew.Image = image;
        }

        private void trackBarHue_Scroll(object sender, EventArgs e)
        {
            Bitmap newBitmap = (Bitmap)bmp.Clone();
            drawHueSaturation(trackBarHue.Value, trackBarSaturation.Value*0.1);
            

        }

        private void trackBarSaturation_Scroll(object sender, EventArgs e)
        {
            Bitmap newBitmap = (Bitmap)bmp.Clone();
            drawHueSaturation(trackBarHue.Value, trackBarSaturation.Value*0.1);
        }

        public Color HSLtoRGB(Byte R, Byte G, Byte B, double saturation, double hue)
        {
            float _R = (R / 255f);
            float _G = (G / 255f);
            float _B = (B / 255f);

            float _Min = Math.Min(Math.Min(_R, _G), _B);
            float _Max = Math.Max(Math.Max(_R, _G), _B);
            float _Delta = _Max - _Min;

            float H = 0;
            float S = 0;
            float L = (float)((_Max + _Min) / 2.0f);

            if (_Delta != 0)
            {
                if (L < 0.5f)
                {
                    S = (float)(_Delta / (_Max + _Min));
                }
                else
                {
                    S = (float)(_Delta / (2.0f - _Max - _Min));
                }


                if (_R == _Max)
                {
                    H = (_G - _B) / _Delta;
                }
                else if (_G == _Max)
                {
                    H = 2f + (_B - _R) / _Delta;
                }
                else if (_B == _Max)
                {
                    H = 4f + (_R - _G) / _Delta;
                }
            }

            return ToRGB(S+ saturation, H+ hue, L);


        }

        public Color ToRGB(double Saturation, double Hue, float Luminosity)
        {
            byte r, g, b;
            if (Saturation == 0)
            {
                r = (byte)Math.Round(Luminosity * 255d);
                g = (byte)Math.Round(Luminosity * 255d);
                b = (byte)Math.Round(Luminosity * 255d);
            }
            else
            {
                double t1, t2;
                double th = Hue / 6.0d;

                if (Luminosity < 0.5d)
                {
                    t2 = Luminosity * (1d + Saturation);
                }
                else
                {
                    t2 = (Luminosity + Saturation) - (Luminosity * Saturation);
                }
                t1 = 2d * Luminosity - t2;

                double tr, tg, tb;
                tr = th + (1.0d / 3.0d);
                tg = th;
                tb = th - (1.0d / 3.0d);

                tr = ColorCalc(tr, t1, t2);
                tg = ColorCalc(tg, t1, t2);
                tb = ColorCalc(tb, t1, t2);
                r = (byte)Math.Round(tr * 255d);
                g = (byte)Math.Round(tg * 255d);
                b = (byte)Math.Round(tb * 255d);
            }
            return Color.FromArgb(r, g, b);
        }
        private static double ColorCalc(double c, double t1, double t2)
        {

            if (c < 0) c += 1d;
            if (c > 1) c -= 1d;
            if (6.0d * c < 1.0d) return t1 + (t2 - t1) * 6.0d * c;
            if (2.0d * c < 1.0d) return t2;
            if (3.0d * c < 2.0d) return t1 + (t2 - t1) * (2.0d / 3.0d - c) * 6.0d;
            return t1;
        }

        private void downsamplingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bmp == null) { loadImage(); }

            /*Bitmap tmp = (Bitmap)bmp.Clone();
            tmp = Shannon_Fano_Compress(bmp);

            this.bmp = (Bitmap)tmp.Clone();*/
            kanalskeSlikeToolStripMenuItem_Click(sender, e);

            HandleDownsampling();
        }

        public void HandleDownsampling()
        {

            this.pictureBox2.Image = ResizeImage(this.pictureBox2.Image, pictureBox2.Width, pictureBox2.Height);
            this.pictureBox3.Image = ResizeImage(this.pictureBox3.Image, pictureBox3.Width, pictureBox3.Height);
            this.pictureBox4.Image = ResizeImage(this.pictureBox4.Image, pictureBox4.Width, pictureBox4.Height);
            this.pictureBox5.Image = ResizeImage(this.pictureBox5.Image, pictureBox5.Width, pictureBox5.Height);

            this.pictureBox3.SizeMode = PictureBoxSizeMode.Normal;
            this.pictureBox4.SizeMode = PictureBoxSizeMode.Normal;
            this.pictureBox5.SizeMode = PictureBoxSizeMode.Normal;

        }

        public static Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private void kuvaharaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.bmp == null) { loadImage(); }

            Bitmap tmp = KuwaharaBlur(this.bmp, 10);
            kanalskeSlikeToolStripMenuItem_Click(sender, e);


        }

        public static Bitmap KuwaharaBlur(Bitmap Image, int Size)
        {
            System.Drawing.Bitmap TempBitmap = Image;
            System.Drawing.Bitmap NewBitmap = new System.Drawing.Bitmap(TempBitmap.Width, TempBitmap.Height);
            System.Drawing.Graphics NewGraphics = System.Drawing.Graphics.FromImage(NewBitmap);
            NewGraphics.DrawImage(TempBitmap, new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), System.Drawing.GraphicsUnit.Pixel);
            NewGraphics.Dispose();
            Random TempRandom = new Random();
            int[] ApetureMinX = { -(Size / 2), 0, -(Size / 2), 0 };
            int[] ApetureMaxX = { 0, (Size / 2), 0, (Size / 2) };
            int[] ApetureMinY = { -(Size / 2), -(Size / 2), 0, 0 };
            int[] ApetureMaxY = { 0, 0, (Size / 2), (Size / 2) };
            for (int x = 0; x < NewBitmap.Width; ++x)
            {
                for (int y = 0; y < NewBitmap.Height; ++y)
                {
                    int[] RValues = { 0, 0, 0, 0 };
                    int[] GValues = { 0, 0, 0, 0 };
                    int[] BValues = { 0, 0, 0, 0 };
                    int[] NumPixels = { 0, 0, 0, 0 };
                    int[] MaxRValue = { 0, 0, 0, 0 };
                    int[] MaxGValue = { 0, 0, 0, 0 };
                    int[] MaxBValue = { 0, 0, 0, 0 };
                    int[] MinRValue = { 255, 255, 255, 255 };
                    int[] MinGValue = { 255, 255, 255, 255 };
                    int[] MinBValue = { 255, 255, 255, 255 };
                    for (int i = 0; i < 4; ++i)
                    {
                        for (int x2 = ApetureMinX[i]; x2 < ApetureMaxX[i]; ++x2)
                        {
                            int TempX = x + x2;
                            if (TempX >= 0 && TempX < NewBitmap.Width)
                            {
                                for (int y2 = ApetureMinY[i]; y2 < ApetureMaxY[i]; ++y2)
                                {
                                    int TempY = y + y2;
                                    if (TempY >= 0 && TempY < NewBitmap.Height)
                                    {
                                        Color TempColor = TempBitmap.GetPixel(TempX, TempY);
                                        RValues[i] += TempColor.R;
                                        GValues[i] += TempColor.G;
                                        BValues[i] += TempColor.B;
                                        if (TempColor.R > MaxRValue[i])
                                        {
                                            MaxRValue[i] = TempColor.R;
                                        }
                                        else if (TempColor.R < MinRValue[i])
                                        {
                                            MinRValue[i] = TempColor.R;
                                        }

                                        if (TempColor.G > MaxGValue[i])
                                        {
                                            MaxGValue[i] = TempColor.G;
                                        }
                                        else if (TempColor.G < MinGValue[i])
                                        {
                                            MinGValue[i] = TempColor.G;
                                        }

                                        if (TempColor.B > MaxBValue[i])
                                        {
                                            MaxBValue[i] = TempColor.B;
                                        }
                                        else if (TempColor.B < MinBValue[i])
                                        {
                                            MinBValue[i] = TempColor.B;
                                        }
                                        ++NumPixels[i];
                                    }
                                }
                            }
                        }
                    }
                    int j = 0;
                    int MinDifference = 10000;
                    for (int i = 0; i < 4; ++i)
                    {
                        int CurrentDifference = (MaxRValue[i] - MinRValue[i]) + (MaxGValue[i] - MinGValue[i]) + (MaxBValue[i] - MinBValue[i]);
                        if (CurrentDifference < MinDifference && NumPixels[i] > 0)
                        {
                            j = i;
                            MinDifference = CurrentDifference;
                        }
                    }

                    Color MeanPixel = Color.FromArgb(RValues[j] / NumPixels[j],
                        GValues[j] / NumPixels[j],
                        BValues[j] / NumPixels[j]);
                    NewBitmap.SetPixel(x, y, MeanPixel);
                }
            }
            return NewBitmap;
        }

        /*
        public static byte[] ImageToByte(Image img)
        {
            /*ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));*/
        /* Image tmp = img;
         MemoryStream ms = new MemoryStream();
         tmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
         byte[] bts = ms.ToArray();

         return bts;
     }*/

        /*public static BitmapImage ByteToImage(byte[] bite)
        {
            //MemoryStream ms = new MemoryStream(bite);

            //Image img = Image.FromStream(ms);
            //Bitmap img = new Bitmap(ms);

            using (var ms = new System.IO.MemoryStream(bite))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
            //return img;
        }

        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        //Compress
        public class BitStream
        {
            public byte[] BytePointer;
            public uint BitPosition;
            public uint Index;
        }

        public struct Symbol
        {
            public uint Sym;
            public uint Count;
            public uint Code;
            public uint Bits;
        }

        private static void initBitStream(ref BitStream stream, byte[] buffer)
        {
            stream.BytePointer = buffer;
            stream.BitPosition = 0;
        }

        private static void writeBits(ref BitStream stream, uint x, uint bits)
        {
            byte[] buffer = stream.BytePointer;
            uint bit = stream.BitPosition;
            uint mask = (uint)(1 << (int)(bits - 1));

            for (uint count = 0; count < bits; ++count)
            {
                buffer[stream.Index] = (byte)((buffer[stream.Index] & (0xff ^ (1 << (int)(7 - bit)))) + ((Convert.ToBoolean(x & mask) ? 1 : 0) << (int)(7 - bit)));
                x <<= 1;
                bit = (bit + 1) & 7;

                if (!Convert.ToBoolean(bit))
                {
                    ++stream.Index;
                }
            }

            stream.BytePointer = buffer;
            stream.BitPosition = bit;
        }

        private static void histogram(byte[] input, Symbol[] sym, uint size)
        {
            Symbol temp;
            int i, swaps;
            int index = 0;

            for (i = 0; i < 256; ++i)
            {
                sym[i].Sym = (uint)i;
                sym[i].Count = 0;
                sym[i].Code = 0;
                sym[i].Bits = 0;
            }

            for (i = (int)size; Convert.ToBoolean(i); --i, ++index)
            {
                sym[input[index]].Count++;
            }

            do
            {
                swaps = 0;

                for (i = 0; i < 255; ++i)
                {
                    if (sym[i].Count < sym[i + 1].Count)
                    {
                        temp = sym[i];
                        sym[i] = sym[i + 1];
                        sym[i + 1] = temp;
                        swaps = 1;
                    }
                }
            } while (Convert.ToBoolean(swaps));
        }

        private static void makeTree(Symbol[] sym, ref BitStream stream, uint code, uint bits, uint first, uint last)
        {
            uint i, size, sizeA, sizeB, lastA, firstB;

            if (first == last)
            {
                writeBits(ref stream, 1, 1);
                writeBits(ref stream, sym[first].Sym, 8);
                sym[first].Code = code;
                sym[first].Bits = bits;
                return;
            }
            else
            {
                writeBits(ref stream, 0, 1);
            }

            size = 0;

            for (i = first; i <= last; ++i)
            {
                size += sym[i].Count;
            }

            sizeA = 0;

            for (i = first; sizeA < ((size + 1) >> 1) && i < last; ++i)
            {
                sizeA += sym[i].Count;
            }

            if (sizeA > 0)
            {
                writeBits(ref stream, 1, 1);

                lastA = i - 1;

                makeTree(sym, ref stream, (code << 1) + 0, bits + 1, first, lastA);
            }
            else
            {
                writeBits(ref stream, 0, 1);
            }

            sizeB = size - sizeA;

            if (sizeB > 0)
            {
                writeBits(ref stream, 1, 1);

                firstB = i;

                makeTree(sym, ref stream, (code << 1) + 1, bits + 1, firstB, last);
            }
            else
            {
                writeBits(ref stream, 0, 1);
            }
        }

        public static int Compress(byte[] input, byte[] output, uint inputSize)
        {
            Symbol[] sym = new Symbol[256];
            Symbol temp;
            BitStream stream = new BitStream();
            uint i, totalBytes, swaps, symbol, lastSymbol;

            if (inputSize < 1)
                return 0;

            initBitStream(ref stream, output);
            histogram(input, sym, inputSize);

            for (lastSymbol = 255; sym[lastSymbol].Count == 0; --lastSymbol) ;

            if (lastSymbol == 0)
                ++lastSymbol;

            makeTree(sym, ref stream, 0, 0, 0, lastSymbol);

            do
            {
                swaps = 0;

                for (i = 0; i < 255; ++i)
                {
                    if (sym[i].Sym > sym[i + 1].Sym)
                    {
                        temp = sym[i];
                        sym[i] = sym[i + 1];
                        sym[i + 1] = temp;
                        swaps = 1;
                    }
                }
            } while (Convert.ToBoolean(swaps));

            for (i = 0; i < inputSize; ++i)
            {
                symbol = input[i];
                writeBits(ref stream, sym[symbol].Code, sym[symbol].Bits);
            }

            totalBytes = stream.Index;

            if (stream.BitPosition > 0)
            {
                ++totalBytes;
            }

            return (int)totalBytes;
        }

        //compress use
        public Bitmap Shannon_Fano_Compress(Bitmap bitmap)
        {
            //string str = "This is an example for Shannon–Fano coding";
            byte[] originalData = ImageToByte(bitmap);

            uint originalDataSize = (uint)originalData.Length;

            byte[] compressedData = new byte[originalDataSize * (101 / 100) + 384];

            int compressedDataSize = Compress(originalData, compressedData, originalDataSize);
            BitmapImage tmp = ByteToImage(compressedData);


            return BitmapImage2Bitmap(tmp);
        }


        // decompress

        public void Shannon_Fano_Deompress(byte[] compressedData )
        {

        }*/



    }
}

