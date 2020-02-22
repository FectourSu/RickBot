using NeuronNetwork_View.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuronNetwork_View.Models
{
    public class  NeuralGraphUtil : INeuralGraphUtil
    {
        // Очистить pictureBox
        public void Clear(PictureBox pictureBox)
        {
            pictureBox.Image = (Image)new Bitmap(pictureBox.Width, pictureBox.Height);
        }

        // Обрезать рисунок по краям и преобразовать его в массив
        public int[,] CutImageToArray(Bitmap b, Point max)
        {
            int x1 = 0;
            int y1 = 0;
            int x2 = max.X;
            int y2 = max.Y;

            for (int y = 0; y < b.Height && y1 == 0; y++)
                for (int x = 0; x < b.Width && y1 == 0; x++)
                    if (b.GetPixel(x, y).ToArgb() != 0)
                        y1 = y;

            for (int y = b.Height - 1; y >= 0 && y2 == max.Y; y--)
                for (int x = 0; x < b.Width && y2 == max.Y; x++)
                    if (b.GetPixel(x, y).ToArgb() != 0)
                        y2 = y;

            for (int x = 0; x < b.Width && x1 == 0; x++)
                for (int y = 0; y < b.Height && x1 == 0; y++)
                    if (b.GetPixel(x, y).ToArgb() != 0)
                        x1 = x;

            for (int x = b.Width - 1; x >= 0 && x2 == max.X; x--)
                for (int y = 0; y < b.Height && x2 == max.X; y++)
                    if (b.GetPixel(x, y).ToArgb() != 0)
                        x2 = x;

            if (x1 == 0 && y1 == 0 && x2 == max.X && y2 == max.Y)
                return null;

            int size = x2 - x1 > y2 - y1 ? x2 - x1 + 1 : y2 - y1 + 1;
            int dx = y2 - y1 > x2 - x1 ? ((y2 - y1) - (x2 - x1)) / 2 : 0;
            int dy = y2 - y1 < x2 - x1 ? ((x2 - x1) - (y2 - y1)) / 2 : 0;

            int[,] res = new int[size, size];

            for (int x = 0; x < res.GetLength(0); x++)
            {
                for (int y = 0; y < res.GetLength(1); y++)
                {
                    int pX = x + x1 - dx;
                    int pY = y + y1 - dy;

                    if (pX < 0 || pX >= max.X || pY < 0 || pY >= max.Y)
                        res[x, y] = 0;
                    else
                        res[x, y] = b.GetPixel(x + x1 - dx, y + y1 - dy).ToArgb() == 0 ? 0 : 1;  
                }
            }
            return res;
        }

        // Печать строкового символа на pictureBox
        public Image DrawCharacter(Image bmp, string l)
        {
            Font font = new Font("Arial", 60f);
            using (Graphics gr = Graphics.FromImage(bmp))
            {
                SizeF size = gr.MeasureString(l, font);
                gr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                gr.DrawString(l, font, new SolidBrush(Color.FromArgb(45, 153, 225)), Point.Empty);
            }
            return bmp;
        }

        // Преобразование рисунка в массив, черный - 1, белый - 0
        public int[,] GetArrayFromBitmap(Bitmap image)
        {
            int[,] res = new int[image.Width, image.Height];
            for (int i = 0; i < res.GetLength(0); i++)
            {
                for (int j = 0; j < res.GetLength(1); j++)
                {
                    int color = (image.GetPixel(i, j).R + image.GetPixel(i, j).G + image.GetPixel(i, j).B) / 3;
                    res[i, j] = color > 0 ? 1 : 0;
                }
            }
            return res;
        }

        // Преобразование массива в рисунок
        public Bitmap GetBitmapFromArr(int[,] array)
        {
            Bitmap bitmap = new Bitmap(array.GetLength(0), array.GetLength(1));
            for (int x = 0; x < array.GetLength(0); x++)
            {
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    if (array[x, y] == 0)
                        bitmap.SetPixel(x, y, Color.White);
                    else
                        bitmap.SetPixel(x, y, Color.Black);
                }
            }
            return bitmap;
        }

        // Приведение произвольного массива данных к массиву стандартных размеров
        public int[,] LeadArray(int[,] source, int[,] res)
        {
            for (int i = 0; i < res.GetLength(0); i++)
                for (int j = 0; j < res.GetLength(1); j++)
                    res[i, j] = 0;

            double pointX = (double)res.GetLength(0) / (double)source.GetLength(0);
            double pointY = (double)res.GetLength(1) / (double)source.GetLength(1);

            for (int i = 0; i < source.GetLength(0); i++)
            {
                for (int j = 0; j < source.GetLength(1); j++)
                {
                    int posX = (int)(i * pointX);
                    int posY = (int)(j * pointY);

                    if(res[posX, posY] == 0)
                        res[posX, posY] = source[i, j];
                }
            }
            return res;
        }
    }
}
