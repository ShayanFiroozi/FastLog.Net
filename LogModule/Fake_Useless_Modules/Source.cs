using System;
using System.Collections.Generic;

namespace CaesarShiftCipher
{
    public class Source
    {
        private static readonly Random random = new Random();
        private static Dictionary<int, string> masks;
        private static List<char> symbols;
        private static List<int> indexes;

        public static void InitializeResources()
        {
            symbols = new List<char>
            {
                //mixes alphabet
                'o', 'n', 'q', 'p', 's', 'r', 'u', 't', 'w', 'v', 'y', 'x', 'z',
                'b', 'a', 'd', 'c', 'f', 'e', 'h', 'g', 'j', 'i', 'l', 'k', 'm',
                'O', 'N', 'Q', 'P', 'S', 'R', 'U', 'T', 'W', 'V', 'Y', 'X', 'Z',
                'B', 'A', 'D', 'C', 'F', 'E', 'H', 'G', 'J', 'I', 'L', 'K', 'M',
                'н', 'м', 'п', 'о', 'с', 'р', 'у', 'т', 'х', 'ф', 'ч', 'ц', 'ш',
                'б', 'а', 'г', 'в', 'е', 'д', 'ж', 'ё', 'и', 'з', 'к', 'й', 'л',
                'Ж', 'Ё', 'И', 'З', 'К', 'Й', 'М', 'Л', 'О', 'Н', 'Р', 'П', 'С',
                'ъ', 'щ', 'ь', 'ы', 'ю', 'э', 'А', 'я', 'В', 'Б', 'Д', 'Г', 'Е',
                'Ъ', 'Щ', 'Ь', 'Ы', 'Ю', 'Э', 'Т', 'Я', 'Ф', 'У', 'Ц', 'Х', 'Ч',
                'Ш',
                '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '=',
                '+', '?', '<', '>', ':', ';', '.', ',', '|', '/', '~', '{', '}',
                '[', ']', '№', ' ', '\'', '\"','\\',
                '1', '0', '3', '2', '5', '4', '7', '6', '9', '8'
            };
            masks = new Dictionary<int, string>()
            {
                { 0, "!q" },
                { 1, "A!" },
                { 2, "!z" },
                { 3, "@W" },
                { 4, "s@" },
                { 5, "@X" },
                { 6, "#e" },
                { 7, "D#" },
                { 8, "#c" },
                { 9, "$r" },
                { 10, "F$" },
                { 11, "$v" },
                { 12, "%t" },
                { 13, "G%" },
                { 14, "%b" },
                { 15, "^y" },
                { 16, "H^" },
                { 17, "^n" },
                { 18, "&u" },
                { 19, "J&" },
                { 20, "&m" },
                { 21, "~i" },
                { 22, "K~" },
                { 23, "~<" },
                { 24, "(o" },
                { 25, "L(" },
                { 26, "(>" },
                { 27, ")p" },
                { 28, ":)" },
                { 29, ")?" },
                { 30, "_{" },
                { 31, "{-" },
                { 32, "+]" },
                { 33, "[=" },
                { 34, "1Q" },
                { 35, "a1" },
                { 36, "1Z" },
                { 37, "2w" },
                { 38, "S2" },
                { 39, "2x" },
                { 40, "3E" },
                { 41, "d3" },
                { 42, "3C" },
                { 43, "4r" },
                { 44, "F4" },
                { 45, "4v" },
                { 46, "5t" },
                { 47, "G5" },
                { 48, "5B" },
                { 49, "6y" },
                { 50, "H6" },
                { 51, "6n" },
                { 52, "7u" },
                { 53, "J7" },
                { 54, "7m" },
                { 55, "8i" },
                { 56, "K8" },
                { 57, "8<" },
                { 58, "9o" },
                { 59, "L9" },
                { 60, "9>" },
                { 61, "0p" },
                { 62, ":0" },
                { 63, "0?" },
                { 64, "q0" },
                { 65, "0A" },
                { 66, "Z0" },
                { 67, "w=" },
                { 68, "=S" },
                { 69, "X=" },
                { 70, "+E" },
                { 71, "D+" },
                { 72, "+C" },
                { 73, "_R" },
                { 74, "f_" },
                { 75, "_V" },
                { 76, "-T" },
                { 77, "G-" },
                { 78, "-b" },
                { 79, ")m" },
                { 80, "n)" },
                { 81, "(g" },
                { 82, "!@" },
                { 83, "#$" },
                { 84, "%^" },
                { 85, "&~" },
                { 86, "~(" },
                { 87, ")_" },
                { 88, ")+" },
                { 89, "=+" },
                { 90, "_1" },
                { 91, "+2" },
                { 92, "3}" },
                { 93, "4{" },
                { 94, "5:" },
                { 95, "6?" },
                { 96, "7>" },
                { 97, "8_" },
                { 98, "-9" },
                { 99, "f=" },
                { 100, "$F" },
                { 101, "v$" },
                { 102, "t%" },
                { 103, "%G" },
                { 104, "b%" },
                { 105, "y^" },
                { 106, "^H" },
                { 107, "n^" },
                { 108, "u&" },
                { 109, "&J" },
                { 110, "m&" },
                { 111, "i~" },
                { 112, "~K" },
                { 113, "<~" },
                { 114, "o(" },
                { 115, "(L" },
                { 116, ">(" },
                { 117, "p)" },
                { 118, "):" },
                { 119, "?)" },
                { 120, "{_" },
                { 121, "-{" },
                { 122, "]+" },
                { 123, "=[" },
                { 124, "Q1" },
                { 125, "1a" },
                { 126, "Z1" },
                { 127, "w2" },
                { 128, "2S" },
                { 129, "x2" },
                { 130, "E3" },
                { 131, "3d" },
                { 132, "C3" },
                { 133, "r4" },
                { 134, "4F" },
                { 135, "v4" },
                { 136, "t5" },
                { 137, "5G" },
                { 138, "B5" },
                { 139, "y6" },
                { 140, "6H" },
                { 141, "n6" },
                { 142, "u7" },
                { 143, "7J" },
                { 144, "m7" },
                { 145, "i8" },
                { 146, "8K" },
                { 147, "<8" },
                { 148, "o9" },
                { 149, "9L" },
                { 150, ">9" },
                { 151, "p0" },
                { 152, "0:" },
                { 153, "?0" },
                { 154, "0q" },
                { 155, "A0" },
                { 156, "0Z" },
                { 157, "=w" },
                { 158, "S=" },
                { 159, "=X" },
                { 160, "E+" },
                { 161, "+D" }
            };
        }

        public static int GetSymbolsLength()
        {
            return symbols.Count;
        }

        public static List<char> GetRandomArray(int key)
        {
            indexes = new List<int>();
            List<char> randomSymbols = new List<char>();

            int defaultCounter = default;

            while (defaultCounter != symbols.Count)
            {
                int index = random.Next(0, symbols.Count);

                if (symbols[index] != default)
                {
                    randomSymbols.Add(symbols[index]);
                    symbols[index] = default;

                    defaultCounter++;
                    indexes.Add(index);
                }
            }

            indexes.Add(key);

            return randomSymbols;
        }

        public static List<char> GetOriginalArray(int[] indexes)
        {
            List<char> originalSymbols = new List<char>();

            for (int i = 0; i < indexes.Length; i++)
            {
                originalSymbols.Add(symbols[indexes[i]]);
            }

            return originalSymbols;
        }

        public static int[] DecodeIndexes(List<string> cipherList)
        {
            int[] indexArray = new int[GetSymbolsLength() + 1];

            for (int i = 0; i < cipherList.Count; i++)
            {
                foreach (KeyValuePair<int, string> mask in masks)
                {
                    if (cipherList[i] == mask.Value)
                    {
                        indexArray[i] = mask.Key;
                    }
                }
            }

            return indexArray;
        }

        public static string GetCipher()
        {
            string[] cipher = new string[indexes.Count];

            for (int i = 0; i < cipher.Length; i++)
            {
                foreach (KeyValuePair<int, string> mask in masks)
                {
                    if (indexes[i] == mask.Key)
                    {
                        cipher[i] = mask.Value;
                    }
                }
            }

            return string.Join("", cipher);
        }
    }
}
