using NeuronNetwork_View.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using NeuronNetwork_View.Views;
using System.Windows.Forms;

namespace NeuronNetwork_View.Models
{
    public class NeuralMemory : INeuralLoadReflection
    {
        public const int neuralInArrayWidth = 10; // Кол-во по горизонтали

        public const int neuralInArrayHeight = 10; // Кол-во по вертикали

        public const string memory = "memory.txt"; // Имя файла хранения сети

        private List<NeuralNetwork> neuralArray = null; // Массив нейронов

        private NeuralNetwork neural;

        public NeuralMemory()
        {
            neuralArray = InitWeb();
        }

        // Открыть текстовый файл и преобразовать его в массив нейронов
        private static List<NeuralNetwork> InitWeb()
        {
            if (!File.Exists(memory)) // проверка на целостность файла
                return new List<NeuralNetwork>();

            string[] lines = File.ReadAllLines(memory);

            if(lines.Length == 0) //проверка на пустоту листа
                return new List<NeuralNetwork>();

            string jStr = lines[0];

            JavaScriptSerializer json = new JavaScriptSerializer();

            List<Object> objects = json.Deserialize<List<Object>>(jStr); //десериализация

            List<NeuralNetwork> res = new List<NeuralNetwork>();

            foreach (var items in objects)
                res.Add(NeuralCreate((Dictionary<string, Object>) items));

            return res;

        }
        // Преобразование структуры данных в класс нейрона
        private static NeuralNetwork NeuralCreate(Dictionary<string, object> obj)
        {
            NeuralNetwork res = new NeuralNetwork();

            res.name = (string)obj["name"];
            res.countTraining = (int)obj["countTraining"];

            Object[] veightData = (Object[])obj["veight"];

            int arrSize = (int)Math.Sqrt(veightData.Length);
            res.veight = new double[arrSize, arrSize];

            int index = 0;

            for (int i = 0; i < res.veight.GetLength(0); i++)
            {
                for (int j = 0; j < res.veight.GetLength(1); j++)
                {
                    res.veight[i, j] = Double.Parse(veightData[index].ToString());
                    index++;
                }
            }

            return res;
        }

        // Функция отвечающая за распознавание образа
        public string CheckLitera(int[,] arr)
        {
            string res = null;
            double max = 0;

            foreach (var items in neuralArray)
            {
                double inputmass = items.getResult(arr);
                if (inputmass > max) // сравнивает входной массив с каждым нейроном
                {
                    max = inputmass;
                    res = items.getName();
                }
            }
            return res;
        }

        // Получить список всех имён, входящих в память
        public string[] GetLiteras()
        {
            var res = new List<string>();

            for (int i = 0; i < neuralArray.Count; i++)
                res.Add(neuralArray[i].getName());

            res.Sort();
           
            return res.ToArray();
        }

        // Функция сохраняет массив нейронов в файл
        public void SaveState()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();

            string jStr = json.Serialize(neuralArray);

            StreamWriter file = new StreamWriter(memory);

            file.WriteLine(jStr);
            file.Close();
        }

        // Функция заносит в память нейрона trainingName новый образ с именем data
        public void SetTraining(string trainingName, int[,] data)
        {
            AddMemoryFile(trainingName);

            int countTraining = neural.Training(data); // обучим нейрон новому образу
            string m1 = neural.getName();
            string m2 = countTraining.ToString();

            string messageStr = "Имя образа: ";
            string message2 = "Вариантов образа в памяти: ";

            // Визуальное отображение памяти обученного нейрона
            NNMemory resultForm = new NNMemory(neural);

            resultForm.label1.Text = messageStr;
            resultForm.label2.Text = message2;

            resultForm.label3.Text = m1;
            resultForm.label4.Text = m2;

            resultForm.Show();

        }

        public void DeleteMemory(string trainingName)
        {
            NeuralNetwork neural = neuralArray.First(v => v.name == trainingName);
            neuralArray.Remove(neural);
        }

        public void AddMemoryFile(string trainingName)
        {
            neural = neuralArray.Find(v => v.name.Equals(trainingName));

            if (neural == null) // если нейрона с таким именем не существует, создадим новый массив и добавим его
            {
                neural = new NeuralNetwork();
                neural.ClearMemoryNeural(trainingName, neuralInArrayHeight, neuralInArrayWidth);
                neuralArray.Add(neural);
            }
        }
    }
}
