using System;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuronNetwork_View.Models;

namespace UnitTest1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            NeuralGraphUtil settingimage = new NeuralGraphUtil();
            settingimage.GetArrayFromBitmap(PictureBox)
        }
    }
}
