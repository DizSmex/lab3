using System;

namespace Lab3
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Задание 1. Заполнение двумерных массивов\n");
            Console.WriteLine("1.1 Ввод с клавиатуры");
            Matrix m1 = new Matrix(2, 3);
            Console.WriteLine("Полученная матрица:\n" + m1);

            Console.WriteLine("1.2 Случайные числа");
            Matrix m2 = new Matrix(4, true);
            Console.WriteLine("Полученная матрица:\n" + m2);

            Console.WriteLine("1.3 Специальный паттерн");
            Matrix m3 = new Matrix(5, 'x');
            Console.WriteLine("Полученная матрица:\n" + m3);

            Console.WriteLine("Задание 2. Работа с двумерными массивами");
            Matrix debts = new Matrix(new double[,] { { 0, 100, 50 }, { 20, 0, 300 }, { 10, 10, 0 } });
            Console.WriteLine("Матрица долгов банков:\n" + debts);
            Console.WriteLine($"Банк с максимальным долгом (индекс): {debts.FindMaxDebtBank()}\n");

            Console.WriteLine("Задание 3. Работа с матрицами");
            Matrix A = new Matrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } });
            Matrix B = new Matrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 } });
            Matrix C = new Matrix(new double[,] { { 1, 0, 1 }, { 0, 1, 0 } });
            Matrix result = 2 * A - B.Transpose() * C;
            Console.WriteLine("Матрица A:\n" + A + "Матрица B:\n" + B + "Матрица C:\n" + C);
            Console.WriteLine("Результат выражения (2*A - B^T * C):\n" + result);

            Console.WriteLine("Задание 4. Бинарные файлы");
            FileProcessor.FillFile4("task4.bin");
            FileProcessor.SolveTask4("task4.bin");
            Console.WriteLine();

            Console.WriteLine("Задание 5. XML Сериализация");
            FileProcessor.FillFile5("task5.xml");
            FileProcessor.SolveTask5("task5.xml");
            Console.WriteLine();

            Console.WriteLine("Задание 6. Текстовые файлы");
            FileProcessor.FillFile6("task6.txt");
            Console.WriteLine($"task6.txt: не содержит нуля: {FileProcessor.SolveTask6("task6.txt")}\n");

            Console.WriteLine("Задание 7. Текстовые файлы");
            FileProcessor.FillFile7("task7.txt");
            FileProcessor.SolveTask7("task7.txt");
            Console.WriteLine();

            Console.WriteLine("Задание 8. Текстовый файл");
            FileProcessor.SolveTask8("task8_in.txt", "task8_out.txt", 'а');
            Console.WriteLine();

            Console.ReadKey();
        }
    }
}
