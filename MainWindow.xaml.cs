using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace practic29
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Изображения|*.jpg;*.png;*.bmp";

            if (dialog.ShowDialog() == true)
            {
                BitmapImage bitmap = new BitmapImage(new Uri(dialog.FileName));
                MyNigg.Source = bitmap;
            }

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            WriteableBitmap wb = new WriteableBitmap((BitmapSource)MyNigg.Source);

            int stride = wb.PixelWidth * 4; // Кол-во байтов в строке (4 байта на пиксель — BGRA)

            byte[] pixels = new byte[wb.PixelHeight * stride]; // Буфер для всех пикселей

            wb.CopyPixels(pixels, stride, 0); // Копируем пиксели в массив pixels

            for (int y = 0; y < wb.PixelHeight; y++) // Проход по строкам (y) и столбцам (x)
            {
                for (int x = 0; x < wb.PixelWidth; x++)
                {

                    //// Индекс текущего пикселя в массиве (смещение по y и x)
                    int index = y * stride + x * 4;

                    int n = (pixels[index] + pixels[index + 1] + pixels[index + 2]) / 3;

                    pixels[index] = 0;
                    pixels[index + 1] = 0;
                    pixels[index + 2] = (byte)(n);

                    //// Среднее значение трёх каналов RGB
                    //byte gray = (byte)((pixels[index] + pixels[index + 1] + pixels[index + 2]) / 3);
                    //// Присваиваем серое значение всем трём каналам (B, G, R)
                    //pixels[index] = pixels[index + 1] = pixels[index + 2] = gray;
                    //// Alpha (pixels[index + 3]) не трогаем
                }
            }
            // Записываем изменённые пиксели обратно в изображение
            wb.WritePixels(new Int32Rect(0, 0, wb.PixelWidth, wb.PixelHeight), pixels, stride, 0);
            MyNigg.Source = wb; // Отображаем результат

        }

        private void Green_Click(object sender, RoutedEventArgs e)
        {
            WriteableBitmap wb = new WriteableBitmap((BitmapSource)MyNigg.Source);

            int stride = wb.PixelWidth * 4;

            byte[] pixels = new byte[wb.PixelHeight * stride];

            wb.CopyPixels(pixels, stride, 0);

            for (int y = 0; y < wb.PixelHeight; y++)
            {
                for (int x = 0; x < wb.PixelWidth; x++)
                {
                    int index = y * stride + x * 4;

                    int n = (pixels[index] + pixels[index + 1] + pixels[index + 2]) / 3;

                    pixels[index] = 0;
                    pixels[index + 1] = (byte)(n);
                    pixels[index + 2] = 0;

                }
            }

            wb.WritePixels(new Int32Rect(0, 0, wb.PixelWidth, wb.PixelHeight), pixels, stride, 0);
            MyNigg.Source = wb; // Отображаем результат
        }

        private void Blue_Click(object sender, RoutedEventArgs e)
        {
            WriteableBitmap wb = new WriteableBitmap((BitmapSource)MyNigg.Source);

            int stride = wb.PixelWidth * 4;

            byte[] pixels = new byte[wb.PixelHeight * stride];

            wb.CopyPixels(pixels, stride, 0);

            for (int y = 0; y < wb.PixelHeight; y++)
            {
                for (int x = 0; x < wb.PixelWidth; x++)
                {
                    int index = y * stride + x * 4;

                    int n = (pixels[index] + pixels[index + 1] + pixels[index + 2]) / 3;

                    pixels[index] = (byte)(n);
                    pixels[index + 1] = 0;
                    pixels[index + 2] = 0;

                }
            }
            wb.WritePixels(new Int32Rect(0, 0, wb.PixelWidth, wb.PixelHeight), pixels, stride, 0);
            MyNigg.Source = wb; // Отображаем результат

        }


        private void Negative_Click(object sender, RoutedEventArgs e)
        {
            WriteableBitmap wb = new WriteableBitmap((BitmapSource)MyNigg.Source);

            int stride = wb.PixelWidth * 4;

            byte[] pixels = new byte[wb.PixelHeight * stride];

            wb.CopyPixels(pixels, stride, 0);

            for (int y = 0; y < wb.PixelHeight; y++)
            {
                for (int x = 0; x < wb.PixelWidth; x++)
                {
                    int index = y * stride + x * 4;

                    int n = (pixels[index] + pixels[index + 1] + pixels[index + 2]) / 3;

                    pixels[index] = (byte)(255 - n);
                    pixels[index + 1] = (byte)(255 - n);
                    pixels[index + 2] = (byte)(255 - n);

                }
            }
            // Записываем изменённые пиксели обратно в изображение
            wb.WritePixels(new Int32Rect(0, 0, wb.PixelWidth, wb.PixelHeight), pixels, stride, 0);
            MyNigg.Source = wb; // Отображаем результат
        }

        private void RevealSecret_Click(object sender, RoutedEventArgs e)
        {
            if (MyNigg.Source == null)
            {
                MessageBox.Show("Сначала откройте изображение!");
                return;
            }

            BitmapSource source = (BitmapSource)MyNigg.Source;
            if (source.Format != PixelFormats.Bgra32)
            {
                source = new FormatConvertedBitmap(source, PixelFormats.Bgra32, null, 0);
            }

            WriteableBitmap wb = new WriteableBitmap(source);

            int stride = wb.PixelWidth * 4;
            byte[] pixels = new byte[wb.PixelHeight * stride];

            wb.CopyPixels(pixels, stride, 0);

            for (int y = 0; y < wb.PixelHeight; y++)
            {
                for (int x = 0; x < wb.PixelWidth; x++)
                {
                    int index = y * stride + x * 4;

                    byte b = pixels[index];
                    byte g = pixels[index + 1];
                    byte r = pixels[index + 2];

                    if (r == 255 && g == 0 && b == 0)
                    {
                        pixels[index] = 255;
                        pixels[index + 1] = 255;
                        pixels[index + 2] = 255;
                    }
                    else if (r > 200 && g < 180 && b < 180 && Math.Abs(g - b) < 30)
                    {
                        pixels[index] = 255;
                        pixels[index + 1] = 255;
                        pixels[index + 2] = 255;
                    }
                    else
                    {
                        pixels[index] = b;
                        pixels[index + 1] = g;
                        pixels[index + 2] = r;
                    }

                    pixels[index + 3] = 255;
                }
            }

            wb.WritePixels(new Int32Rect(0, 0, wb.PixelWidth, wb.PixelHeight), pixels, stride, 0);
            MyNigg.Source = wb;
        }



        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PNG|*.png|JPEG|*.jpg";

            if (dialog.ShowDialog() == true)
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)MyNigg.Source));
                using FileStream stream = new FileStream(dialog.FileName, FileMode.Create);
                encoder.Save(stream);
            }

        }

        

    }
}