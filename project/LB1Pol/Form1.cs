using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace LB1Pol
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Bitmap image;
        int n = 1;
        OpenFileDialog open_dialog = new OpenFileDialog();
        public void button1_Click(object sender, EventArgs e)
        {


            //создание диалогового окна для выбора файла
            open_dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*"; //формат загружаемого файла
            if (open_dialog.ShowDialog() == DialogResult.OK) //если в окне была нажата кнопка "ОК"
            {
                try
                {
                    image = new Bitmap(open_dialog.FileName);
                    //вместо pictureBox1 укажите pictureBox, в который нужно загрузить изображение 
                    this.pictureBox1.Size = image.Size;
                    pictureBox1.Image = image;
                    pictureBox1.Invalidate();
                    textBox1.Text = open_dialog.FileName;
                    textBox3.Text = Convert.ToString(new FileInfo(open_dialog.FileName).Length);

                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Невозможно открыть выбранный файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {//1 способ

            Converter converter = new Converter();
            byte[] vs = converter.imageToByteArray(image);
            //textBox4.Text = Convert.ToString(vs.Length);
            byte[] vs1 = Converter.Encode(vs);
            textBox5.Text = Convert.ToString(vs1.Length);
            //pictureBox1.Image = Converter.BytesToBitmap(vs);
            //pictureBox1.Invalidate();


            pictureBox1.Image = image;
            //Converter.LockBits(image);
            //byte[] vs = Converter.Pixels;
            //textBox4.Text = Convert.ToString(vs.Length);
            //byte[] vs1 = Converter.Encode(vs);
            //textBox5.Text = Convert.ToString(vs1.Length);
            //byte[] vs2 = Converter.RLEDecode(vs1);



            //2 способ


            //Bitmap Temp = new Bitmap(image.Width, image.Height); //Чистое изображение.
            //Color[,] ALL = new Color[image.Width - 1, image.Height - 1];
            //byte[,] ALL1 = new byte[image.Width, image.Height];
            ////Массив для цветов пикселей.
            //for (int i = 0; i < (image.Width - 1); i++)
            //{
            //    for (int j = 0; j < (image.Height - 1); j++)
            //    {
            //        ALL[i, j] = image.GetPixel(i, j); //Получаем все цвета в массив.
            //    }
            //}
            //for (int i = 0; i < image.Width - 1; i++)
            //{
            //    for (int j = 0; j < (image.Height - 1); j++)
            //    { }
            //}


            //for (int i = 0; i < (image.Width - 1); i++)
            //{
            //    for (int j = 0; j < (image.Height - 1); j++)
            //    {
            //        Temp.SetPixel(i, j, ALL[i, j]); //Красим все пиксели по порядку в Темп.
            //    }
            //}
            //pictureBox2.Image = Temp; //Выводим.

            //3 способ



            //второй способ


            //Image image1 = Image.FromFile(open_dialog.FileName);
            //System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            //image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            //byte[] b = memoryStream.ToArray();
            //textBox4.Text = Convert.ToString(b.Length);
            //byte[] b1 = Converter.Encode(b);
            //textBox5.Text = Convert.ToString(b1.Length);

            //// третий способ
            //byte[] imgdata = System.IO.File.ReadAllBytes(open_dialog.FileName);
            //byte[] imgdata1 = Converter.Encode(imgdata);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            ConverterLZW converter1 = new ConverterLZW();
            byte[] lzw = converter1.imageToByteArray(image);

            //textBox4.Text = Convert.ToString(lzw.Length);
            int[] lzw1 = converter1.compressLZW(lzw);
            textBox5.Text = Convert.ToString(lzw1.Length);
            string lzw2 = converter1.DecompressLZW(lzw1);

            pictureBox1.Image = image;


        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (Bitmap bmp1 = new Bitmap(pictureBox1.Image))
            {
                Converter lose = new Converter();

                ImageCodecInfo jpgEncoder = lose.GetEncoder(ImageFormat.Jpeg);
                byte[] bytes = lose.imageToByteArray(pictureBox1.Image);
                //textBox4.Text = Convert.ToString(bytes.Length);
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 0L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                bmp1.Save(@"C:\Users\ReaLBERG\Pictures\lose" + n + ".jpeg", jpgEncoder, myEncoderParameters);
                pictureBox1.Load(@"C:\Users\ReaLBERG\Pictures\lose" + n + ".jpeg");
                n++;
                bytes = lose.imageToByteArray(pictureBox1.Image);
                textBox5.Text = Convert.ToString(bytes.Length);
                
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
