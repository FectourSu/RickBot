using NeuronNetwork_View.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuronNetwork_View.Views.Controls
{
    public class _OpenFileDialog : IOpenFileDialog
    {
        private NeuralMemory neuralMemory;

        private NNForms view;
        public _OpenFileDialog(NeuralMemory nm, NNForms vw)
        {
            neuralMemory = nm;
            view = vw;
        }

        public void AddZipFile(ComboBox box)
        {
            OpenFileDialog open = new OpenFileDialog();

            open.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); // открываем прямо из десктопа
            open.Filter = "All files (*.txt)|*.txt";
            open.FilterIndex = 2;
            open.RestoreDirectory = true;
            open.Multiselect = false;

            view.Invoke(new Action(() => {

                if (open.ShowDialog() != DialogResult.OK)
                    return;

                string[] text = File.ReadAllLines(open.FileName, Encoding.UTF8);

                if (text.Length == 0)
                {
                    MessageBox.Show("Файл пуст");
                    return;
                }

                foreach (var item in text)
                {
                    bool isAny = box.Items.Cast<string>().Any(v => v == item.ToString());

                    if (!isAny)
                    {
                        neuralMemory.AddMemoryFile(item);
                        box.Items.Add(item);
                    }
                }
                MessageBox.Show("Память успешно загружена");
            }));
        }

        public void AddZipFile(System.Windows.Controls.ComboBox box)
        {
            throw new NotImplementedException();
        }

        public async Task AddZipFileAsync(ComboBox box) 
        {
            await Task.Run(() => AddZipFile(box));
        }
       
        public void OpenFile()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = ("Jpeg files (*.jpg)|*.jpg");
            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Bitmap bitmap = null;
                    bitmap = new Bitmap(open.FileName);
                    view.Invoke(new Action(() => { view.pictureBox13.Image = bitmap; }));
                }
                catch
                {
                    MessageBox.Show("Морти, твой файл не открыть!", "Ну что за дела", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
