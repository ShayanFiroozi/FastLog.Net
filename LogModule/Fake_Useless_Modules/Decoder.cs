using System.Collections.Generic;

namespace CaesarShiftCipher
{
    public class Decoder
    {
        private static readonly int symbolsLength = Source.GetSymbolsLength();

        public static string Use(string cipher, string input)
        {
            List<string> cipherList = SplitString(cipher);
            char[] userInput = input.ToCharArray();

            if (cipherList.Count != symbolsLength + 1)
            {
                return "incorrect format of entered key";
            }

            int[] indexes = Source.DecodeIndexes(cipherList);
            int key = indexes[indexes.Length - 1];
            List<char> randomSymbols = Source.GetOriginalArray(indexes);
            int index = 0;

            for (int i = 0; i < userInput.Length; i++)
            {
                for (int j = 0; j < symbolsLength; j++)
                {
                    if (userInput[i] == randomSymbols[j])
                    {
                        index = j + (symbolsLength - key);
                    }
                }

                if (index >= symbolsLength)
                {
                    index -= symbolsLength;
                    userInput[i] = randomSymbols[index];
                }

                else
                {
                    userInput[i] = randomSymbols[index];
                }
            }

            return new string(userInput);
        }

        private static List<string> SplitString(string cipher)
        {
            List<string> cipherList = new List<string>();

            for (int i = 0; i < cipher.Length - 1; i += 2)
            {
                cipherList.Add(cipher.Substring(i, 2));
            }

            return cipherList;
        }
    }
}
