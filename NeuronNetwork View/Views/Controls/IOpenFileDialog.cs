using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NeuronNetwork_View.Views.Controls
{
    interface IOpenFileDialog
    {
        void AddZipFile(ComboBox box);
        void OpenFile();
    }
}
