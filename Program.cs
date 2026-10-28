using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Lab3
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("--- ЗАДАНИЕ 1 ---");
            Console.WriteLine("1.1 Ввод с клавиатуры:");
            Matrix m1 = new Matrix(2, 3);
            Console.WriteLine(m1);

            Console.WriteLine("1.2 Случайные числа (n x n):");
            Matrix m2 = new Matrix(4, true);
            Console.WriteLine(m2);

            Console.WriteLine("1.3 Специальный паттерн (n x n):");
            Matrix m3 = new Matrix(5, 'x');
            Console.WriteLine(m3);

            Console.WriteLine("\n--- ЗАДАНИЕ 2 ---");
            Matrix debts = new Matrix(new double[,] { { 0, 100, 50 }, { 20, 0, 300 }, { 10, 10, 0 } });
            Console.WriteLine("Матрица долгов:");
            Console.WriteLine(debts);
            Console.WriteLine("Банк с максимальным долгом: " + debts.FindMaxDebtBank());

            Console.WriteLine("\n--- ЗАДАНИЕ 3 ---");
            Matrix A = new Matrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } });
            Matrix B = new Matrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 } });
            Matrix C = new Matrix(new double[,] { { 1, 0, 1 }, { 0, 1, 0 } });

            Matrix result = 2 * A - B.Transpose() * C;
            Console.WriteLine("Матрица A:");
            Console.WriteLine(A);
            Console.WriteLine("Матрица B:");
            Console.WriteLine(B);
            Console.WriteLine("Матрица C:");
            Console.WriteLine(C);
            Console.WriteLine("Результат (2*A - B^T * C):");
            Console.WriteLine(result);

            Console.WriteLine("\n--- ЗАДАНИЕ 4 ---");
            FileProcessor.FillFile4("task4.bin");
            FileProcessor.SolveTask4("task4.bin");

            Console.WriteLine("\n--- ЗАДАНИЕ 5 ---");
            FileProcessor.FillFile5("task5.xml");
            FileProcessor.SolveTask5("task5.xml");

            Console.WriteLine("\n--- ЗАДАНИЕ 6 ---");
            FileProcessor.FillFile6("task6.txt");
            bool hasNoZero = FileProcessor.SolveTask6("task6.txt");
            Console.WriteLine($"Файл не содержит нуля: {hasNoZero}");

            Console.WriteLine("\n--- ЗАДАНИЕ 7 ---");
            FileProcessor.FillFile7("task7.txt");
            FileProcessor.SolveTask7("task7.txt");

            Console.WriteLine("\n--- ЗАДАНИЕ 8 ---");
            FileProcessor.SolveTask8("task8_in.txt", "task8_out.txt", 'а');

            Console.ReadKey();
        }
    }

    // КЛАСС ДЛЯ ЗАДАНИЙ 1-3 (строго по заданию)
    internal class Matrix
    {
        private double[,] _data;

        // Задание 1.1: Ввод с клавиатуры
        public Matrix(int n, int m)
        {
            _data = new double[n, m];
            Console.WriteLine($"Введите элементы матрицы {n}x{m} по строкам:");
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    Console.Write($"[{i},{j}] = ");
                    _data[i, j] = double.Parse(Console.ReadLine());
                }
            }
        }

        // Задание 1.2: Случайные числа (размер n x n)
        public Matrix(int n, bool isRandom)
        {
            _data = new double[n, n];
            Random rnd = new Random();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i + j < n - 1)
                        _data[i, j] = Math.Round(rnd.NextDouble() * (120 - (-65)) + (-65), 2);
                    else
                        _data[i, j] = Math.Round(rnd.NextDouble() * (10.75 - (-3.5)) + (-3.5), 2);
                }
            }
        }

        // Задание 1.3: Специальный паттерн
        public Matrix(int n, char pattern)
        {
            _data = new double[n, n];
            for (int i = 0; i < n; i++) _data[i, i] = 11 + i;

            int val = 1;
            for (int d = n - 1; d >= 1; d--)
            {
                if (d % 2 == 0)
                {
                    for (int i = 0; i <= n - 1 - d; i++) _data[i, i + d] = val++;
                }
                else
                {
                    for (int i = n - 1 - d; i >= 0; i--) _data[i, i + d] = val++;
                }
            }
        }

        // Вспомогательный конструктор для создания матриц в Заданиях 2 и 3
        public Matrix(double[,] data) { _data = data; }

        // Задание 2: Банк с максимальным долгом
        public int FindMaxDebtBank()
        {
            int maxBank = -1;
            double maxDebt = double.MinValue;
            int n = _data.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                for (int j = 0; j < n; j++) sum += _data[i, j];
                if (sum > maxDebt)
                {
                    maxDebt = sum;
                    maxBank = i;
                }
            }
            return maxBank;
        }

        // Задание 3: Транспонирование
        public Matrix Transpose()
        {
            int r = _data.GetLength(0);
            int c = _data.GetLength(1);
            Matrix res = new Matrix(new double[c, r]);
            for (int i = 0; i < r; i++)
                for (int j = 0; j < c; j++)
                    res._data[j, i] = _data[i, j];
            return res;
        }

        // Задание 3: Перегрузка оператора умножения на число
        public static Matrix operator *(double scalar, Matrix m)
        {
            int r = m._data.GetLength(0);
            int c = m._data.GetLength(1);
            Matrix res = new Matrix(new double[r, c]);
            for (int i = 0; i < r; i++)
                for (int j = 0; j < c; j++)
                    res._data[i, j] = m._data[i, j] * scalar;
            return res;
        }

        // Задание 3: Перегрузка оператора умножения матриц
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            int r1 = m1._data.GetLength(0);
            int c1 = m1._data.GetLength(1);
            int c2 = m2._data.GetLength(1);

            Matrix res = new Matrix(new double[r1, c2]);
            for (int i = 0; i < r1; i++)
                for (int j = 0; j < c2; j++)
                    for (int k = 0; k < c1; k++)
                        res._data[i, j] += m1._data[i, k] * m2._data[k, j];
            return res;
        }

        // Задание 3: Перегрузка оператора вычитания
        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            int r = m1._data.GetLength(0);
            int c = m1._data.GetLength(1);
            Matrix res = new Matrix(new double[r, c]);
            for (int i = 0; i < r; i++)
                for (int j = 0; j < c; j++)
                    res._data[i, j] = m1._data[i, j] - m2._data[i, j];
            return res;
        }

        // Перегрузка ToString
        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < _data.GetLength(0); i++)
            {
                for (int j = 0; j < _data.GetLength(1); j++)
                    result += $"{_data[i, j],8:F2} ";
                result += "\n";
            }
            return result;
        }
    }

    // СТРУКТУРЫ ДЛЯ ЗАДАНИЯ 5 (XML сериализация)
    [Serializable]
    public struct BagItem
    {
        public string Name { get; set; }
        public double Weight { get; set; }
    }

    [Serializable]
    public struct PassengerBaggage
    {
        public string Name { get; set; }
        public BagItem[] Items { get; set; }
    }

    // КЛАСС ДЛЯ ЗАДАНИЙ 4-8
    internal static class FileProcessor
    {
        // Задание 4: Заполнение
        public static void FillFile4(string path)
        {
            Random rnd = new Random();
            using (BinaryWriter bw = new BinaryWriter(File.Open(path, FileMode.Create)))
            {
                for (int i = 0; i < 15; i++) bw.Write(rnd.Next(-5, 6));
            }
        }

        // Задание 4: Решение (пары противоположных чисел)
        public static void SolveTask4(string path)
        {
            List<int> nums = new List<int>();
            using (BinaryReader br = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                while (br.BaseStream.Position < br.BaseStream.Length) nums.Add(br.ReadInt32());
            }

            int pairs = 0;
            for (int i = 0; i < nums.Count; i++)
            {
                for (int j = i + 1; j < nums.Count; j++)
                {
                    if (nums[i] + nums[j] == 0 && nums[i] != 0)
                    {
                        pairs++;
                        break;
                    }
                }
            }
            Console.WriteLine($"Количество пар противоположных чисел: {pairs}");
        }

        // Задание 5: Заполнение (XML сериализация)
        public static void FillFile5(string path)
        {
            PassengerBaggage[] passengers = new PassengerBaggage[]
            {
                new PassengerBaggage { Name = "Иванов", Items = new BagItem[] { new BagItem { Name = "Чемодан", Weight = 20.5 }, new BagItem { Name = "Сумка", Weight = 5.0 } } },
                new PassengerBaggage { Name = "Петров", Items = new BagItem[] { new BagItem { Name = "Коробка", Weight = 30.0 } } },
                new PassengerBaggage { Name = "Сидоров", Items = new BagItem[] { new BagItem { Name = "Рюкзак", Weight = 8.0 }, new BagItem { Name = "Сумка", Weight = 7.5 } } }
            };

            XmlSerializer serializer = new XmlSerializer(typeof(PassengerBaggage[]));
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                serializer.Serialize(fs, passengers);
            }
        }

        // Задание 5: Решение
        public static void SolveTask5(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PassengerBaggage[]));
            PassengerBaggage[] loaded;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                loaded = (PassengerBaggage[])serializer.Deserialize(fs);
            }

            double maxWeight = double.MinValue;
            double minWeight = double.MaxValue;

            foreach (var p in loaded)
            {
                double totalWeight = 0;
                foreach (var item in p.Items) totalWeight += item.Weight;
                if (totalWeight > maxWeight) maxWeight = totalWeight;
                if (totalWeight < minWeight) minWeight = totalWeight;
            }
            Console.WriteLine($"Разница между макс. и мин. багажом: {maxWeight - minWeight} кг.");
        }

        // Задание 6: Заполнение
        public static void FillFile6(string path)
        {
            Random rnd = new Random();
            using (StreamWriter sw = new StreamWriter(path))
            {
                for (int i = 0; i < 10; i++) sw.WriteLine(rnd.Next(-2, 3)); // случайно попадет 0 или нет
            }
        }

        // Задание 6: Решение
        public static bool SolveTask6(string path)
        {
            foreach (string line in File.ReadLines(path))
            {
                if (line.Trim() == "0") return false;
            }
            return true;
        }

        // Задание 7: Заполнение
        public static void FillFile7(string path)
        {
            Random rnd = new Random();
            using (StreamWriter sw = new StreamWriter(path))
            {
                for (int i = 0; i < 5; i++)
                {
                    sw.WriteLine($"{rnd.Next(10, 99)} {rnd.Next(10, 99)} {rnd.Next(10, 99)}");
                }
            }
        }

        // Задание 7: Решение (Минимальный элемент)
        public static void SolveTask7(string path)
        {
            int min = int.MaxValue;
            foreach (string line in File.ReadLines(path))
            {
                string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string p in parts)
                {
                    int val = int.Parse(p);
                    if (val < min) min = val;
                }
            }
            Console.WriteLine($"Минимальный элемент: {min}");
        }

        // Задание 8: Заполнение и решение
        public static void SolveTask8(string inputPath, string outputPath, char endChar)
        {
            File.WriteAllLines(inputPath, new string[] {
                "Первая строка заканчивается на букву а",
                "Вторая строка тоже на а",
                "Эта строка на букву б",
                "И эта на б"
            });

            List<string> result = new List<string>();
            foreach (string line in File.ReadLines(inputPath))
            {
                if (line.EndsWith(endChar.ToString()))
                {
                    result.Add(line);
                }
            }
            File.WriteAllLines(outputPath, result);
            Console.WriteLine($"Строки, оканчивающиеся на '{endChar}', скопированы в {outputPath}.");
        }
    }
}
