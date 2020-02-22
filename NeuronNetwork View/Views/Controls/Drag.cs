using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace NeuronNetwork_View.Views
{
    class Drag : Component
    {
            private Control HendleControl;

            public Control SelectControl
            {
                get
                {
                    return this.HendleControl;
                }
                set
                {
                    this.HendleControl = value;
                    this.HendleControl.MouseDown += new MouseEventHandler(this.DragForm_MouseDown);
                }
            }

            [DllImport("user32.dll")]
            public static extern int SendMessage(IntPtr a, int msg, int wParam, int IParam);

            [DllImport("user32.dll")]
            public static extern bool ReleaseCapture();

            private void DragForm_MouseDown(object sender, MouseEventArgs e)
            {
                bool flag = e.Button == MouseButtons.Left;

                if (flag)
                {
                    Drag.ReleaseCapture();
                    Drag.SendMessage(this.SelectControl.FindForm().Handle, 161, 2, 0);
                }
            }
        }
    }


