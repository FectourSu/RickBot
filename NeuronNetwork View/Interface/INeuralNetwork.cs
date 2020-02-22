using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronNetwork_View.Interface
{
    interface INeuralNetwork
    {
        string getName();
        void ClearMemoryNeural(string name, int x, int y);
        double getResult(int[,] data);
        int Training(int[,] data);
    }
}
