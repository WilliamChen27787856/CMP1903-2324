using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CMP1903_A2_2324
{
    public class Statistics
    {
        private const string FilePath = @"../../gameStats.txt";
        private readonly InputOutputManager _io = new InputOutputManager();
        private readonly Dictionary<StatisticCodes, string> _translationList = new Dictionary<StatisticCodes, string>
        {
            {StatisticCodes.SevensOutHighScore, "Sevens Out High Score"},
            {StatisticCodes.ThreeOrMoreHighScore, "Three Or More High Score"},
            {StatisticCodes.SevensOutPlayCount, "Sevens Out Play Count"},
            {StatisticCodes.ThreeOrMorePlayCount, "Three Or More Play Count"}
        };
        

        public enum StatisticCodes
        {
            SevensOutHighScore,
            ThreeOrMoreHighScore,
            SevensOutPlayCount,
            ThreeOrMorePlayCount,
        }
        
        private Dictionary<StatisticCodes, int> ReadAndParseStatisticsFile()
        {
            Dictionary<StatisticCodes, int> returnStatistics = new Dictionary<StatisticCodes, int>();   
            IEnumerable<string> fileContents = File.ReadAllLines(FilePath);

            foreach (string line in fileContents)
            {
                string[] newStat = line.Split(':');
                StatisticCodes codeToInput = _translationList.FirstOrDefault(entry => entry.Value == newStat[0]).Key;
                returnStatistics.Add(codeToInput, int.Parse(newStat[1]));
            }
            
            return returnStatistics;
        }

        private void WriteToStatisticsFile(Dictionary<StatisticCodes, int> fileContents)
        {
            string textToWrite = "";
            
            foreach (KeyValuePair<StatisticCodes, int> entry in fileContents)
            {
                textToWrite += $"{_translationList[entry.Key]}:{entry.Value}\n";
            }
            
            File.WriteAllText(FilePath, textToWrite);
        }
        
        public (string name, int score) GetStatistic(StatisticCodes statistic)
        {
            if (!File.Exists(FilePath))
            {
                _io.WriteColourTextLine("\nStatistics File cannot be found in directory!\n", ConsoleColor.Red);
                return ("", 0);
            }

            Dictionary<StatisticCodes, int> currentStatistics = ReadAndParseStatisticsFile();

            if (!currentStatistics.ContainsKey(statistic))
            {
                _io.WriteColourTextLine("\nStatistic not currently present in the save file!", ConsoleColor.Red);
                return ("", 0);
            }

            return (_translationList[statistic],currentStatistics[statistic]);
        }

        private bool UpdateStatistic(StatisticCodes statistic, int score,
            Dictionary<StatisticCodes, int> currentStatistics)
        {
            if (currentStatistics.ContainsKey(statistic))
            {
                currentStatistics[statistic] = score;
                WriteToStatisticsFile(currentStatistics);
                return true;
            }

            _io.WriteColourTextLine("\nStatistic not currently present in the save file!", ConsoleColor.Red);
            return false;
        }
        
        public bool UpdateStatistic(StatisticCodes statistic, int score)
        {
            Dictionary<StatisticCodes, int> currentStatistics = ReadAndParseStatisticsFile();

            return UpdateStatistic(statistic, score, currentStatistics);
        }
        
        public bool UpdateStatistic(StatisticCodes statistic, int score, bool addToCurrentScore)
        {
            Dictionary<StatisticCodes, int> currentStatistics = ReadAndParseStatisticsFile();

            if (addToCurrentScore)
            {
                if (currentStatistics.ContainsKey(statistic))
                {
                    currentStatistics[statistic] += score;
                }
                else
                {
                    _io.WriteColourTextLine("\nStatistic not currently present in the save file!", ConsoleColor.Red);
                    return false;
                }
            }

            return UpdateStatistic(statistic, score, currentStatistics);
        }

        public void OutputStatistics()
        {
            Dictionary<StatisticCodes, int> currentStatistics = ReadAndParseStatisticsFile();
            
            Console.WriteLine("");
            
            foreach (KeyValuePair<StatisticCodes, int> entry in currentStatistics)
            {
                Console.WriteLine($"{_translationList[entry.Key]}: {entry.Value}");
            }
        }
    }
}