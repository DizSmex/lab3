using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Lab3
{
    internal class Matrix
    {
        private double[,] _data;

        public Matrix(int n, int m)
        {
            _data = new double[n, m];
            Console.WriteLine($"Введите {n * m} элементов матрицы {n}x{m} (по строкам):");
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    Console.Write($"Элемент [{i}, {j}] = ");
                    _data[i, j] = double.Parse(Console.ReadLine());
                }
        }

        public Matrix(int n, bool isRandom)
        {
            _data = new double[n, n];
            Random rnd = new Random();
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    _data[i, j] = (i + j < n - 1)
                        ? Math.Round(rnd.NextDouble() * 185 - 65, 2)
                        : Math.Round(rnd.NextDouble() * 14.25 - 3.5, 2);
        }

        public Matrix(int n, char pattern)
        {
            _data = new double[n, n];
            for (int i = 0; i < n; i++) _data[i, i] = 11 + i;
            int val = 1;
            for (int d = n - 1; d >= 1; d--)
                for (int i = 0; i <= n - 1 - d; i++)
                {
                    if (d % 2 == 0) _data[i, i + d] = val++;
                    else _data[n - 1 - d - i, n - 1 - i] = val++;
                }
        }

        public Matrix(double[,] data) { _data = data; }

        public int FindMaxDebtBank()
        {
            int maxBank = -1; double maxDebt = double.MinValue;
            for (int i = 0; i < _data.GetLength(0); i++)
            {
                double sum = 0;
                for (int j = 0; j < _data.GetLength(0); j++) sum += _data[i, j];
                if (sum > maxDebt) { maxDebt = sum; maxBank = i; }
            }
            return maxBank;
        }

        public Matrix Transpose()
        {
            int r = _data.GetLength(0), c = _data.GetLength(1);
            Matrix res = new Matrix(new double[c, r]);
            for (int i = 0; i < r; i++)
                for (int j = 0; j < c; j++) res._data[j, i] = _data[i, j];
            return res;
        }

        public static Matrix operator *(double scalar, Matrix m)
        {
            int r = m._data.GetLength(0), c = m._data.GetLength(1);
            Matrix res = new Matrix(new double[r, c]);
            for (int i = 0; i < r; i++)
                for (int j = 0; j < c; j++) res._data[i, j] = m._data[i, j] * scalar;
            return res;
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            int r1 = m1._data.GetLength(0), c1 = m1._data.GetLength(1), c2 = m2._data.GetLength(1);
            Matrix res = new Matrix(new double[r1, c2]);
            for (int i = 0; i < r1; i++)
                for (int j = 0; j < c2; j++)
                    for (int k = 0; k < c1; k++) res._data[i, j] += m1._data[i, k] * m2._data[k, j];
            return res;
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            int r = m1._data.GetLength(0), c = m1._data.GetLength(1);
            Matrix res = new Matrix(new double[r, c]);
            for (int i = 0; i < r; i++)
                for (int j = 0; j < c; j++) res._data[i, j] = m1._data[i, j] - m2._data[i, j];
            return res;
        }

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < _data.GetLength(0); i++)
            {
                for (int j = 0; j < _data.GetLength(1); j++) result += $"{_data[i, j],8:F2} ";
                result += "\n";
            }
            return result;
        }
    }

    [Serializable] public struct BagItem { public string Name { get; set; } public double Weight { get; set; } }
    [Serializable] public struct PassengerBaggage { public string Name { get; set; } public BagItem[] Items { get; set; } }

    internal static class FileProcessor
    {
        public static void FillFile4(string path)
        {
            Random rnd = new Random();
            using BinaryWriter bw = new BinaryWriter(File.Open(path, FileMode.Create));
            for (int i = 0; i < 15; i++) bw.Write(rnd.Next(-5, 6));
        }

        public static void SolveTask4(string path)
        {
            List<int> nums = new List<int>();
            using (BinaryReader br = new BinaryReader(File.Open(path, FileMode.Open)))
                while (br.BaseStream.Position < br.BaseStream.Length) nums.Add(br.ReadInt32());

            int pairs = 0;
            for (int i = 0; i < nums.Count; i++)
                for (int j = i + 1; j < nums.Count; j++)
                    if (nums[i] + nums[j] == 0 && nums[i] != 0) { pairs++; break; }

            Console.WriteLine($"{path}: количество пар противоположных чисел = {pairs}");
        }

        public static void FillFile5(string path)
        {
            PassengerBaggage[] passengers = {
                new PassengerBaggage { Name = "Иванов", Items = new[] { new BagItem { Name = "Чемодан", Weight = 20.5 }, new BagItem { Name = "Сумка", Weight = 5.0 } } },
                new PassengerBaggage { Name = "Петров", Items = new[] { new BagItem { Name = "Коробка", Weight = 30.0 } } },
                new PassengerBaggage { Name = "Сидоров", Items = new[] { new BagItem { Name = "Рюкзак", Weight = 8.0 }, new BagItem { Name = "Сумка", Weight = 7.5 } } }
            };
            using FileStream fs = new FileStream(path, FileMode.Create);
            new XmlSerializer(typeof(PassengerBaggage[])).Serialize(fs, passengers);
        }

        public static void SolveTask5(string path)
        {
            PassengerBaggage[] loaded;
            using (FileStream fs = new FileStream(path, FileMode.Open))
                loaded = (PassengerBaggage[])new XmlSerializer(typeof(PassengerBaggage[])).Deserialize(fs);

            double maxWeight = double.MinValue, minWeight = double.MaxValue;
            foreach (var p in loaded)
            {
                double totalWeight = 0;
                foreach (var item in p.Items) totalWeight += item.Weight;
                if (totalWeight > maxWeight) maxWeight = totalWeight;
                if (totalWeight < minWeight) minWeight = totalWeight;
            }
            Console.WriteLine($"{path}: разница между макс. и мин. багажом = {maxWeight - minWeight} кг");
        }

        public static void FillFile6(string path)
        {
            Random rnd = new Random();
            using StreamWriter sw = new StreamWriter(path);
            for (int i = 0; i < 10; i++) sw.WriteLine(rnd.Next(-2, 3));
        }

        public static bool SolveTask6(string path)
        {
            foreach (string line in File.ReadLines(path))
                if (line.Trim() == "0") return false;
            return true;
        }

        public static void FillFile7(string path)
        {
            Random rnd = new Random();
            using StreamWriter sw = new StreamWriter(path);
            for (int i = 0; i < 5; i++)
                sw.WriteLine($"{rnd.Next(10, 99)} {rnd.Next(10, 99)} {rnd.Next(10, 99)}");
        }

        public static void SolveTask7(string path)
        {
            int min = int.MaxValue;
            foreach (string line in File.ReadLines(path))
                foreach (string p in line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                    if (int.Parse(p) < min) min = int.Parse(p);
            Console.WriteLine($"{path}: минимальный элемент = {min}");
        }

        public static void SolveTask8(string inputPath, string outputPath, char endChar)
        {
            File.WriteAllLines(inputPath, new[] {
                "Первая строка заканчивается на букву а", "Вторая строка тоже на а",
                "Эта строка на букву б", "И эта на б"
            });

            List<string> result = new List<string>();
            foreach (string line in File.ReadLines(inputPath))
                if (line.EndsWith(endChar.ToString())) result.Add(line);

            File.WriteAllLines(outputPath, result);
            Console.WriteLine($"{outputPath}: скопированы строки, оканчивающиеся на '{endChar}'");
        }
    }
}
