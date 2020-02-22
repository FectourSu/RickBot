using NeuronNetwork_View.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronNetwork_View.Interface
{
    interface INeuralLoadReflection
    {
        string CheckLitera(int[,] arr);
        void SaveState();
        string[] GetLiteras();
        void SetTraining(string trainingName, int[,] data);
    }
}
