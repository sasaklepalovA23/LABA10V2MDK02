using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LABA10V2MDK02
{
    class Program
    {
        private const string InputFile = "input.txt";
        private const string OutputFile = "output.txt";
        private const string NumbersFile = "numbers.txt";
        private const string SortedNumbersFile = "sorted_numbers.txt";
        private const string MatrixFile = "matrix.txt";
        private const string EncryptedFile = "encrypted.txt";
        private const string DecryptedFile = "decrypted.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("ВАРИАНТ 2\n");

            Console.WriteLine("Задача 1: В данном текстовом файле удалить все слова, которые содержат хотя бы одну цифру.");
            RemoveWordsWithDigits(InputFile, OutputFile);
            Console.WriteLine($"Исходные числа: {InputFile}\nИтог: {OutputFile}\n");


            Console.WriteLine("Задача 2: Создать и заполнить файл случайными целыми значениями. Выполнить сортировку содержимого файла по возрастанию.\r\n");
            CreateAndSortNumbers(15, NumbersFile, SortedNumbersFile);
            Console.WriteLine($"Исходные числа: {NumbersFile}\nОтсортированные: {SortedNumbersFile}\n");

          
            Console.WriteLine("Задача 3: Текстовый файл содержит квадратную матрицу, которая записана по принципу: одна строка файла – одна строка матрицы. Необходимо построить двухмерный массив и вывести на экран исходную матрицу и результат ее транспонирования.\r\n");
            var matrix = ReadMatrix(MatrixFile);
            if (matrix != null)
            {
                Console.WriteLine("Исходная матрица:");
                PrintMatrix(matrix);

                var transposed = Transpose(matrix);
                Console.WriteLine("\nТранспонированная матрица:");
                PrintMatrix(transposed);
            }
            Console.WriteLine();

         
            Console.WriteLine("Задача 4: Имеется файл с текстом. Осуществить шифрование данного текста в новый файл путем записи текста шифром Цезаря. Осуществить расшифровку текста.\r\n");
            EncryptFile(InputFile, EncryptedFile, 3);
            DecryptFile(EncryptedFile, DecryptedFile, 3);

            Console.WriteLine($"\nШифрование завершено. Файл: {EncryptedFile}");
            Console.WriteLine($"Расшифровка завершена. Файл: {DecryptedFile}");
            Console.ReadKey();
        }

       
        static void RemoveWordsWithDigits(string inputPath, string outputPath)
        {
            try
            {
                string text = File.ReadAllText(inputPath);
                string result = Regex.Replace(text, @"\b\w*\d\w*\b", "");
                File.WriteAllText(outputPath, result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

    

        static void CreateAndSortNumbers(int count, string inputPath, string outputPath)
        {
            try
            {
                var random = new Random();
                List<int> numbers = Enumerable.Range(1, count)
                    .Select(_ => random.Next(1, 100))
                    .ToList();

                File.WriteAllLines(inputPath, numbers.Select(n => n.ToString()));

                var sortedNumbers = File.ReadAllLines(inputPath)
                    .Select(int.Parse)
                    .OrderBy(n => n)
                    .ToList();

                File.WriteAllLines(outputPath, sortedNumbers.Select(n => n.ToString()));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

      

        static int[,] ReadMatrix(string path)
        {
            try
            {
                var lines = File.ReadAllLines(path).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
                int size = lines.Length;
                int[,] matrix = new int[size, size];

                for (int i = 0; i < size; i++)
                {
                    var nums = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                        .Select(int.Parse).ToArray();
                    if (nums.Length != size)
                        throw new Exception("Матрица не является квадратной.");
                    for (int j = 0; j < size; j++)
                        matrix[i, j] = nums[j];
                }
                return matrix;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка чтения матрицы: {ex.Message}");
                return null;
            }
        }

        static int[,] Transpose(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            int[,] result = new int[cols, rows];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    result[j, i] = matrix[i, j];

            return result;
        }

        static void PrintMatrix(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                    Console.Write($"{matrix[i, j],4}"); 
                Console.WriteLine();
            }
        }

    
        public static string CaesarCipher(string text, int shift)
        {
            char[] buffer = text.ToCharArray();
            int n = buffer.Length;

            for (int i = 0; i < n; i++)
            {
                char letter = buffer[i];
                char baseChar;
                int alphabetSize;

                if (letter >= 'A' && letter <= 'Z')
                {
                    baseChar = 'A';
                    alphabetSize = 26;
                }
                else if (letter >= 'a' && letter <= 'z')
                {
                    baseChar = 'a';
                    alphabetSize = 26;
                }
                else if (letter >= 'А' && letter <= 'Я')
                {
                    baseChar = 'А';
                    alphabetSize = 32;
                }
                else if (letter >= 'а' && letter <= 'я')
                {
                    baseChar = 'а';
                    alphabetSize = 32;
                }
                else
                {
                    continue; 
                }

                int offset = ((letter - baseChar) + shift) % alphabetSize;
                if (offset < 0) offset += alphabetSize;

                buffer[i] = (char)(baseChar + offset);
            }
            return new string(buffer);
        }

        public static void EncryptFile(string inputPath, string outputPath, int shift)
        {
            try
            {
                string text = File.ReadAllText(inputPath);
                string encryptedText = CaesarCipher(text, shift);
                File.WriteAllText(outputPath, encryptedText);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при шифровании: {ex.Message}");
            }
        }

        public static void DecryptFile(string inputPath, string outputPath, int shift)
        {
            EncryptFile(inputPath, outputPath, -shift);
        }
    }
}