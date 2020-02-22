using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NeuronNetwork_View.Models;
using NeuronNetwork_View.Views;
using NeuronNetwork_View.Views.Controls;
using NeuronNetwork_View.Properties;

namespace NeuronNetwork_View
{
    public class Presenter
    {
        private _OpenFileDialog obj;

        // объект класса работы с изображением
        private NeuralGraphUtil _util = new NeuralGraphUtil();

        // основная форма
        public readonly NNForms view;

        // для рисования
        private Point _startP;

        // память нейрона
        private NeuralMemory neuralmemory;

        private int[,] arr;

        private bool enabledlearn;

        public Presenter(NNForms forms)
        {
            view = forms;

            enabledlearn = false;

            // Текст из комбобокса
            string textCombo = view.comboBox1.Text;

            #region События

            //select fix
            view.comboBox1.SelectedIndexChanged += (s, e) =>
            {
                view.pictureBox1.Focus();
            };

            //при загрузке
            view.Load += (s, e) =>
            {
                view.comboBox1.DropDownHeight = 200;

                _util.Clear(view.pictureBox5);

                neuralmemory = new NeuralMemory();

                obj = new _OpenFileDialog(neuralmemory, view);

                string[] items = neuralmemory.GetLiteras();
                if (items.Length > 0)
                {
                    view.comboBox1.Items.AddRange(items);
                    view.comboBox1.SelectedIndex = 0;
                }

                view.comboBox1.SelectedIndex = -1;

                //подключение шрифтов

                view.comboBox1.Font = new Font("Segoe UI", 14, FontStyle.Bold);
                InitCustomLabelFont();
                view.label6.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            };

            //сохранение
            view.FormClosing += (s, e) =>
            {
                neuralmemory.SaveState();
            };

            //отключить курсор
            view.pictureBox5.Cursor.Dispose();

            //выйти
            view.button1.Click += (s, e) =>
            {
                Application.Exit();
            };

            //свернуть окно
            view.button2.Click += (s, e) =>
            {
                view.WindowState = FormWindowState.Minimized;
            };

            //изменить цвет
            view.button1.MouseEnter += (s, e) => {
                view.button1.BackColor = Color.FromArgb(255, 45, 45);
            };
            view.button1.MouseLeave += (s, e) => {
                view.button1.BackColor = Color.FromArgb(45, 153, 225);
            };

            //очистить текстбокс
            view.textBox1.Click += (s, e) => {
                view.textBox1.Clear();
                view.button4.Enabled = true;
            };

            //бургер меню
            view.button4.MouseEnter += (s, e) => {
                view.button4.Text = "Запомнить значение";
            };
            view.button4.MouseLeave += (s, e) => {
                view.button4.Text = " ";
            };
            view.button5.MouseEnter += (s, e) => {
                view.button5.Text = "Разрешить обучение";
            };
            view.button5.MouseLeave += (s, e) => {
                view.button5.Text = " ";
            };
            view.button3.MouseEnter += (s, e) => {
                view.button3.Text = "Загрузить память";
            };
            view.button3.MouseLeave += (s, e) => {
                view.button3.Text = " ";
            };
            view.button6.MouseEnter += (s, e) => {
                view.button6.Text = "Нарисовать символ из списка";
            };
            view.button6.MouseLeave += (s, e) => {
                view.button6.Text = " ";
            };
            view.pictureBox8.Click += (s, e) =>
            {
                view.panel2.Visible = true;
                view.pictureBox9.Visible = true;
            };
            view.pictureBox9.Click += (s, e) =>
            {
                view.panel2.Visible = false;
                view.pictureBox9.Visible = false;
            };
            //

            //не правильно
            view.pictureBox6.Click += (s, e) => {
                view.textBox1.Text = "Введи";
                view.pictureBox15.Visible = true;
                view.label3.Visible = true;
                view.pictureBox7.Visible = true;
                view.textBox1.Visible = true;
                view.pictureBox7.Visible = true;
                view.textBox1.Visible = true;
                view.pictureBox2.Visible = false;
                view.pictureBox3.Visible = true;
                view.pictureBox4.Enabled = false;
                view.label5.Visible = false;
                view.label6.Visible = false;
                view.label8.Visible = true;
            };

            //правильно
            view.pictureBox4.Click += (s, e) => {
                view.pictureBox6.Enabled = false;
                view.label7.Visible = true;
                view.pictureBox15.Visible = false;
                view.label3.Visible = false;
                view.pictureBox7.Visible = false;
                view.textBox1.Visible = false;
                view.pictureBox6.Visible = false;
                view.pictureBox4.Visible = false;
                view.label4.Visible = false;
                view.label5.Visible = false;
                view.label6.Visible = false;
                view.pictureBox5.Enabled = false;
                view.button5.Enabled = false;

                if (enabledlearn == true)
                {

                    int[,] clipArr = _util.CutImageToArray((Bitmap)view.pictureBox5.Image, new Point(view.pictureBox5.Width, view.pictureBox5.Height));

                    if (clipArr == null)
                        return;

                    arr = _util.LeadArray(clipArr, new int[NeuralMemory.neuralInArrayWidth, NeuralMemory.neuralInArrayHeight]);

                    string str = neuralmemory.CheckLitera(arr);

                    if (str == null)
                        str = "null";

                    neuralmemory.SetTraining(str, arr);
                }

                _util.Clear(view.pictureBox5);

            };

            view.pictureBox10.Click += (s, e) => {
                view.pictureBox5.Enabled = true; 
            };

            view.button4.Click += (s, e) =>
            {
                 BrainRickBrain();
            };
            view.textBox1.KeyDown += (s, e) => {
                
                if (e.KeyCode == Keys.Enter)
                {
                    BrainRickBrain();
                    view.button4.Enabled = false;
                }
            };

            //Добавить или не добавить в память
            void BrainRickBrain()
            {
                // Текст из текстбокса
                string text = view.textBox1.Text;

                if (text == null || view.textBox1.TextLength == 0 || text == string.Empty || text == " " || text == "  " || text == "Введи")
                {
                    MessageBox.Show("Плохой с тебя учитель! Введи значение еще раз", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    view.pictureBox1.Focus();
                    return;
                }
                else
                {
                    view.label8.Visible = false;
                    view.label3.Visible = false;

                    //добавить образ в память
                    foreach (var item in view.comboBox1.Items)
                    {
                        if (text == item.ToString() || text == textCombo)
                        {
                            neuralmemory.SetTraining(text, arr);
                            view.pictureBox15.Visible = false;
                            view.label3.Visible = false;
                            view.pictureBox7.Visible = false;
                            view.textBox1.Visible = false;
                            view.pictureBox3.Visible = false;
                            view.pictureBox2.Visible = true;
                            view.pictureBox4.Enabled = false;
                            view.pictureBox6.Enabled = false;
                            view.label8.Visible = false;
                            view.label9.Text = "Хорошо Морти, я запомню";
                            view.label9.Visible = true;
                            view.pictureBox6.Visible = false;
                            view.pictureBox4.Visible = false;
                            _util.Clear(view.pictureBox5);
                            view.pictureBox5.Enabled = false;
                            view.label5.Visible = false;
                            view.label6.Visible = false;
                            return;
                        }
                    }
                    neuralmemory.SetTraining(text, arr);
                    view.comboBox1.Items.Add(text);
                    view.comboBox1.SelectedIndex = view.comboBox1.Items.Count - 1;

                    view.pictureBox15.Visible = false;
                    view.label3.Visible = false;
                    view.pictureBox7.Visible = false;
                    view.textBox1.Visible = false;
                    view.pictureBox3.Visible = false;
                    view.pictureBox2.Visible = true;
                    view.pictureBox4.Enabled = false;
                    view.pictureBox6.Enabled = false;
                    view.label8.Visible = false;
                    view.label9.Text = "Хорошо Морти, я запомню";
                    view.label9.Visible = true;
                    view.pictureBox6.Visible = false;
                    view.pictureBox4.Visible = false;
                    _util.Clear(view.pictureBox5);
                }
            }

            //рисовать
            view.pictureBox5.MouseMove += (s, e) =>
            {
                view.pictureBox5.Cursor = new Cursor(AppDomain.CurrentDomain.BaseDirectory + "oth123.cur");

                if (e.Button == MouseButtons.Left)
                {
                    Point endP = new Point(e.X, e.Y);
                    Bitmap image = (Bitmap)view.pictureBox5.Image;
                    using (Graphics g = Graphics.FromImage(image))
                    {
                        g.DrawLine(new Pen(Color.FromArgb(45, 153, 225), 10), _startP, endP);
                    }
                    view.pictureBox5.Image = image;
                    _startP = endP;
                }
            };

            view.pictureBox5.MouseDown += (s, e) =>
            {
                _startP = new Point(e.X, e.Y);
                view.label9.Visible = false;
                view.label4.Visible = true;
                view.label8.Visible = false;
                view.label5.Visible = false;
                view.label6.Visible = false;
                view.pictureBox4.Enabled = false; 
                view.pictureBox6.Enabled = false;
                view.button5.Enabled = true;
            };

            //распознать рисунок
            view.pictureBox11.Click += (s, e) =>
            {
                if (view.pictureBox5.Enabled != false)
                {
                    Learn();
                    view.pictureBox6.Visible = true;
                    view.pictureBox4.Visible = true;
                    view.label4.Visible = false;
                    view.label5.Visible = true;
                    view.label6.Visible = true;
                    view.label7.Visible = false;
                    view.label8.Visible = false;
                    view.pictureBox2.Visible = true;
                    view.pictureBox3.Visible = false;
                    view.pictureBox6.Enabled = true;
                    view.pictureBox4.Enabled = true;
                    view.label9.Visible = false;
                }
                else
                    MessageBox.Show("Сначала нарисуй рисунок, Морти!", " ", MessageBoxButtons.OK, MessageBoxIcon.Question);
            };

            //очистить холст
            view.pictureBox12.Click += (s, e) =>
            {
                _util.Clear(view.pictureBox5);
                view.pictureBox5.Enabled = false;
                view.label5.Visible = false;
                view.label6.Visible = false;
                view.label4.Visible = true;
                view.pictureBox4.Enabled = false;
                view.pictureBox6.Enabled = false;
                view.label8.Visible = false;
            };

            // Добавить в память txt файл
            view.button3.Click += async (s, e) =>
            {
                try
                {
                    await obj.AddZipFileAsync(view.comboBox1);
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так, попробуй загрузить еще раз!");
                }
            };

            //Нарисовать символ из памяти
            view.button6.Click += (s, e) => 
            {
                if (view.comboBox1.Text == string.Empty)
                {
                    MessageBox.Show("Не могу отрисовать то, чего нет");
                }
                else
                {
                    _util.Clear(view.pictureBox5);
                    view.pictureBox5.Image = _util.DrawCharacter(view.pictureBox5.Image, (string)view.comboBox1.SelectedItem);
                }
            };

            // запомнить текст комбобокса
            view.comboBox1.TextChanged += (s, e) => {
                textCombo = view.comboBox1.Text;
            };

            // Удалить из памяти
            view.button7.Click += (s, e) =>
            {
                try
                {
                    if (textCombo == string.Empty)
                    {
                        MessageBox.Show("Выбери элемент памяти!");
                    }
                    else
                    {
                        neuralmemory.DeleteMemory(textCombo);
                        view.comboBox1.Items.Remove(textCombo);
                    }
                }
                catch
                {
                    MessageBox.Show("Все значения из памяти были удалены");
                }
            };

            view.button5.Click += (s, e) => 
            {
                enabledlearn = true;
                MessageBox.Show("Режим обучения включён");
            };

            void Learn()
            {
                int[,] clipArr = _util.CutImageToArray((Bitmap)view.pictureBox5.Image, new Point(view.pictureBox5.Width, view.pictureBox5.Height));

                if (clipArr == null)
                    return;

                arr = _util.LeadArray(clipArr, new int[NeuralMemory.neuralInArrayWidth, NeuralMemory.neuralInArrayHeight]);

                string s = neuralmemory.CheckLitera(arr);

                if (s == null)
                    s = "null";

                view.label6.Text = s;

                view.button5.Enabled = false;
            }

            void InitCustomLabelFont()
            {
                //Create your private font collection object.
                PrivateFontCollection pfc = new PrivateFontCollection();

                //Select your font from the resources.
                //My font here is "Digireu.ttf"
                int fontLength = Resources._8bit.Length;

                // create a buffer to read in to
                byte[] fontdata = Resources._8bit;

                // create an unsafe memory block for the font data
                IntPtr data = Marshal.AllocCoTaskMem(fontLength);

                // copy the bytes to the unsafe memory block
                Marshal.Copy(fontdata, 0, data, fontLength);

                // pass the font to the font collection
                pfc.AddMemoryFont(data, fontLength);


                foreach (Control f in view.Controls)
                {

                    if (f is Label && f is Button)
                        f.Font = new Font(pfc.Families[0], 8, FontStyle.Regular);
                }
            }
            #endregion
        }
    }
}
