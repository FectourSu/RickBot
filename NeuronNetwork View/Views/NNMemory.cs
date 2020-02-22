using NeuronNetwork_View.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuronNetwork_View.Views
{
    public partial class NNMemory : Form
    {
        private NeuralNetwork _neural;
        public NNMemory(NeuralNetwork neural)
        {
            InitializeComponent();
            this._neural = neural;

            button1.MouseEnter += (s, e) =>
            {
                button1.BackColor = Color.FromArgb(255, 45, 45);
                button1.ForeColor = Color.White;
            };
            button1.MouseLeave += (s, e) =>
            {
                button1.ForeColor = Color.Black;
                button1.BackColor = Color.White;
            };

            button1.Click += (s, e) =>
            {
                Close();
            };

            Load += (s,e) => 
            { 
                dataGridView1.ColumnCount = _neural.veight.GetLength(0);
                dataGridView1.RowCount = _neural.veight.GetLength(1);
                dataGridView1.DefaultCellStyle.ForeColor = Color.Green;

                for (int i = 0; i < _neural.veight.GetLength(0); i++)
                {
                    DataGridViewColumn column = dataGridView1.Columns[i];
                    column.Width = 32;

                    for (int j = 0; j < _neural.veight.GetLength(1); j++)
                    {
                        int color = (int)((1 - _neural.veight[i, j]) * 255);

                        dataGridView1.Rows[j].Cells[i].Style.BackColor = Color.FromArgb(color, color, color);

                        dataGridView1.Rows[j].Cells[i].Value = _neural.veight[i, j];
                    }
                }
                label3.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            };

            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
