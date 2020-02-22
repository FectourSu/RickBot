using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuronNetwork_View.Models
{
    #region Загрузка изображения
    public static class DonwloadImg
    {
        public static int[,] ToByte(this Image img) //конвертация картинки в 0 и 1
        {
            var bmp = new Bitmap(img);
            int[,] mass = new int[bmp.Width, bmp.Height];

            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    var IsWhite = bmp.GetPixel(x, y).R >= 230 && bmp.GetPixel(x, y).G >= 230 && bmp.GetPixel(x, y).B >= 230;
                    mass[x, y] = IsWhite ? 0 : 1;
                }
            }
            return mass;
        }
        public static Image ToImage(this int[,] img) // конвертация в чёрнобелую картинку
        {
            var bmp = new Bitmap(img.GetLength(0), img.GetLength(1));

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    bmp.SetPixel(x, y, img[x, y] == 1 ? Color.Black : Color.White);
                }
            }
            return bmp;
        }
        //public static int[,] CutNumber(this int[,] bytes) //Вырезание черной цифры с белого фона
        //{
        //    var r = getRect(bytes);
        //    var mass = new int[r.Width, r.Height];

        //    for (int y = 0; y < mass.GetLength(1); y++)
        //    {
        //        for (int x = 0; x < mass.GetLength(0); x++)
        //        {
        //            mass[x, y] = bytes[x + r.X, y + r.Y];
        //        }
        //    }
        //    return mass;
        //}
        public static Image ScaleImage(this Image source, int width, int height) //Масштабируемость изображения
        {
            Image desk = new Bitmap(width, height);
            using (Graphics gr = Graphics.FromImage(desk)) {
                gr.FillRectangle(Brushes.White, 0, 0, width, height); // Очистка экрана
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                float srcwidth = source.Width;
                float srcheight = source.Height;
                float dscwidth = width;
                float dscheight = height;

                if (srcwidth <= dscwidth && srcheight <= dscheight) // Если исходное изображение меньше целевого
                {
                    int left = (width - source.Width) / 2;
                    int top = (height - source.Height) / 2;
                    gr.DrawImage(source, left, top, source.Width, source.Height);
                }
                else if (srcwidth / srcheight > dscwidth / dscheight) // Более широкие пропорции изображения
                {
                    float cy = srcheight / srcwidth * srcwidth;
                    float top = ((float)dscheight - cy) / 2.0f;
                    if (top < 1.0f) top = 0;
                    gr.DrawImage(source, 0, top, dscwidth, cy);
                }
                else  // Более узкие пропорции изображения
                {
                    float cx = srcwidth / srcheight * dscheight;
                    float left = ((float)dscwidth - cx) / 2.0f;
                    if (left < 1.0f) left = 0;
                    gr.DrawImage(source, left, 0, cx, dscheight);
                }
                return desk;
            }
        }
        public static int[,] ToInput(this Image source, int width, int height) // основная функция выполнения действия над изображением
        {
            return source.ToByte().ToImage().ScaleImage(width, height).ToByte();
        }
    }
    #endregion
}
