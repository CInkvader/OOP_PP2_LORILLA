using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_PP2_LORILLA
{
    class WordReader
    {
        private Dictionary<string, int> _breakdown = new Dictionary<string, int>();
        private Dictionary<string, int> _sortedBD = new Dictionary<string, int>();
        private List<string> _lines = new List<string>();
        private string[] _words = new string[] { };
        private int _wordCount = 0;

        private string _fileName;
        private string _outputFile;

        private bool _isFileValid;
        public WordReader(string sourceFile)
        {
            setNewFile(sourceFile);
            beginWordCounter();
        }
        ~WordReader() { }
        public void setNewFile(string sourceFile)
        {
            Console.Write("Finding source file and creating output file name...");
            if (!checkFileValidity(sourceFile)) { return; };
            _isFileValid = true;
            _fileName = sourceFile;
            _outputFile = _fileName.Split('.')[0] + "_WordCount.txt";
            Console.WriteLine("Done!");
        }
        public void beginWordCounter()
        {
            if (!readSourceFile())
            {
                return;
            }
            countWords();
            sortWords();
            writeToOutputFile();
        }
        private bool checkFileValidity(string fileName)
        {
            try
            {
                StreamReader sr = new StreamReader(fileName);
                return true;
            }
            catch
            {
                Console.WriteLine($"\"{fileName}\" could not be found!");
                _isFileValid = false;
                return false;
            }
        }
        private bool readSourceFile()
        {
            if (!_isFileValid)
            {
                Console.WriteLine("Cannot begin writing - No valid file set!");
                return false;
            }
            Console.Write("Begin Reading...");

            string line = "";
            using (StreamReader sr = new StreamReader(_fileName))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Length > 0)
                        _lines.Add(line);
                }
            }
            Console.WriteLine("Done!");
            return true;
        }
        private void countWords()
        {
            char[] letters = { };
            Console.Write("Begin Counting...");

            foreach (string l in _lines)
            {
                _words = l.Split(' ');
                for (int x = 0; x < _words.Length; x++)
                //foreach(string word in words)
                {
                    if (_words[x].Length > 0)
                    {
                        // special character filter
                        specialCharacterFilter(_words[x], out letters);

                        if (_breakdown.ContainsKey(_words[x].ToLower()))
                            _breakdown[_words[x].ToLower()] += 1;
                        else
                            _breakdown[_words[x].ToLower()] = 1;

                        _wordCount++;
                    }
                }
            }
            Console.WriteLine("Done!");
        }
        private void specialCharacterFilter(string word, out char[] letters)
        {
            char[] blankCharArray = { };
            if (word.Length > 1)
            {
                letters = word.ToCharArray();
                if ((int)letters[letters.Length - 1] == 33 // !
                    || (int)letters[letters.Length - 1] == 44 // ,
                    || (int)letters[letters.Length - 1] == 46 // .
                    || (int)letters[letters.Length - 1] == 63 // ?
                    )
                {
                    word = "";
                    for (int y = 0; y < letters.Length - 1; y++)
                    {
                        word += letters[y];
                    }
                }
            }
            letters = blankCharArray;
        }
        private void sortWords()
        {
            int leastCount = 0;
            string sortKey = "";
            Console.Write("Begin Sorting...");

            while (_breakdown.Count > 0)
            {
                leastCount = 0;

                foreach (KeyValuePair<string, int> kvp in _breakdown)
                {
                    if (leastCount < kvp.Value)
                    {
                        leastCount = kvp.Value;
                        sortKey = kvp.Key;
                    }
                }

                _sortedBD[sortKey] = leastCount;
                _breakdown.Remove(sortKey);
            }
            Console.WriteLine("Done!");
        }
        private void writeToOutputFile()
        {
            Console.Write("Begin Writing...");

            using (StreamWriter sw = new StreamWriter(_outputFile))
            {
                sw.WriteLine("Word count of {0}.", _fileName);
                sw.WriteLine("Total Wordcount is {0}.", _wordCount);
                foreach (KeyValuePair<string, int> kvp in _sortedBD)
                {
                    sw.WriteLine("{0}-{1}", kvp.Key, kvp.Value);
                }
            }
            Console.WriteLine("Done!");
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            _ = new WordReader("Odin.txt"); // Since creating a named instance is not exactly needed for now

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}