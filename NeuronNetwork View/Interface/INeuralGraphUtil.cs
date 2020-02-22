using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuronNetwork_View.Interface
{
    interface INeuralGraphUtil
    {
        void Clear(PictureBox pictureBox );
        int[,] GetArrayFromBitmap(Bitmap image);
        int[,] CutImageToArray(Bitmap b, Point max);
        int[,] LeadArray(int[,] source, int[,] res);
        Bitmap GetBitmapFromArr(int[,] array);
        Image DrawCharacter(Image bmp, string l);
    }
}
