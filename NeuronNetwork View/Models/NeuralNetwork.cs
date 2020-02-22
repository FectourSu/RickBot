using NeuronNetwork_View.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronNetwork_View.Models
{
    /// <summary>
    /// Класс нейрона, который обучается, а так же хранит и сравнивает значение в памяти
    /// </summary>
    public class NeuralNetwork : INeuralNetwork
    {
        // Имя образа ( которое хранит нейрон )
        public string name { get; set; }

        // Массив весов нейрона ( т.е. его память )
        public double[,] veight { get; set; }

        // Счётчик тренинга нейрона ( кол-во вариантов образа в памяти )
        public int countTraining { get; set; }

        // Конструктор
        public NeuralNetwork() { }

        // Очистить память нейрона и присвоить ему новое имя
        public void ClearMemoryNeural(string name, int x, int y)
        {
            this.name = name; // присвоить имя
            veight = new double[x, y];
            for (int i = 0; i < veight.GetLength(0); i++)
            {
                for (int j = 0; j < veight.GetLength(1); j++)
                {
                    veight[i, j] = 0; //очистить память
                }
            }
            countTraining = 0;
        }

        // Получение имени образа
        public string getName()
        {
            return name;
        }

        // Функция возвращает сумму величин отклонения входного массива от эталонного
        // Чем результат ближе к 1, тем больше входной массив похож на образ из памяти нейрона
        public double getResult(int[,] data)
        {
            if (veight.GetLength(0) != data.GetLength(0) || veight.GetLength(1) != data.GetLength(1))
                return -1;
            double res = 0;
            for (int i = 0; i < veight.GetLength(0); i++)
                for (int j = 0; j < veight.GetLength(1); j++)
                    res += 1 - Math.Abs(veight[i, j] - data[i, j]); // отклонение каждого элемента массива от усреднённого значения из памяти

            return res / (veight.GetLength(0) * veight.GetLength(1)); // среднее арифметическое по отклонению(при большом кол-ве образов лучшая производительность)
        }

        // Добавление в массив входного образа
        public int Training(int[,] data)
        {
            if (data == null || veight.GetLength(0) != data.GetLength(0) || veight.GetLength(1) != data.GetLength(1))
                return countTraining;

            countTraining++;

            for (int i = 0; i < veight.GetLength(0); i++)
            {
                for (int j = 0; j < veight.GetLength(1); j++)
                {
                    double cast = data[i, j] == 0 ? 0 : 1; // приводим значения входного элемента к дискретному

                    veight[i, j] += 2 * (cast - 0.5f) / countTraining; // каждый элемент в памяти пересчитывается с учётом значения из data

                    if (veight[i, j] > 1) // значение не может быть больше 1
                        veight[i, j] = 1;
                    if (veight[i, j] < 0) // или меньше 0
                        veight[i, j] = 0;
                }
            }
            return countTraining; // возвращение количества обучений
        }
    }
}
