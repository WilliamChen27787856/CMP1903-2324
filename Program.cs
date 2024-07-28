using System;

namespace CMP1903_A2_2324
{
    internal class Program
    {
        private static readonly string[] _twoPlayerMenu = { "One Player", "Two Player" };
        
        private static bool TwoPlayer(InputOutputManager io)
        {
            (bool, int) twoPlayerOption = io.HandleMenu(_twoPlayerMenu);

            if (!twoPlayerOption.Item1)
            {
                twoPlayerOption = io.HandleMenu(_twoPlayerMenu);
            }

            return twoPlayerOption.Item2 != 0;
        }

        public static (bool newHighScore, int player) ProcessStats(int playerOneScore, int playerTwoScore, Statistics.StatisticCodes highScoreCode,
            Statistics.StatisticCodes countCode, Statistics statsManager, bool twoPlayer)
        {
            (string statisticName, int highestScore) = statsManager.GetStatistic(highScoreCode);
            
            statsManager.UpdateStatistic(countCode, 1, true);
            
            if (playerOneScore > highestScore)
            {
                statsManager.UpdateStatistic(highScoreCode, playerOneScore);

                return (true, 1);
            }

            if (twoPlayer && playerTwoScore > highestScore)
            {
                statsManager.UpdateStatistic(highScoreCode, playerTwoScore);

                return (true, 2);
            }
            
            return (false, 0);
        }
        
        public static void Main(string[] args)
        {
            Statistics statsManager = new Statistics();
            SevensOut sevensOut = new SevensOut();
            ThreeOrMore threeOrMore = new ThreeOrMore();
            InputOutputManager io = new InputOutputManager();
        
            string[] menuOptions =
            {
                "Play Three Or More",
                "Play Sevens Out",
                "View Current Statistics",
                "Test the Program",
                "Exit"
            };
            
            while (true)
            {
                (bool, int) optionChosen = io.HandleMenu(menuOptions);

                if (!optionChosen.Item1)
                {
                    optionChosen = io.HandleMenu(menuOptions);
                }

                bool twoPlayer = false;
                int playerOneScore;
                int playerTwoScore;
                int lastScore = 0;

                bool newHighScore;
                int playerHighScore;

                if (optionChosen.Item2 == 0 || optionChosen.Item2 == 1)
                {
                    twoPlayer = TwoPlayer(io);
                }
                
                switch (optionChosen.Item2)
                {
                    case 0:
                        (playerOneScore, playerTwoScore, lastScore) = threeOrMore.playGame(twoPlayer);
                        (newHighScore, playerHighScore) = ProcessStats(playerOneScore, playerTwoScore, Statistics.StatisticCodes.ThreeOrMoreHighScore, Statistics.StatisticCodes.ThreeOrMorePlayCount, statsManager, twoPlayer);
                        
                        if (newHighScore)
                        {
                            io.WriteColourTextLine($"\nPlayer {playerHighScore} has reached a new high score!", ConsoleColor.Yellow);
                        }
                        
                        break;
                    case 1:
                        (playerOneScore, playerTwoScore, lastScore) = sevensOut.playGame(twoPlayer);
                        (newHighScore, playerHighScore) = ProcessStats(playerOneScore, playerTwoScore, Statistics.StatisticCodes.SevensOutHighScore, Statistics.StatisticCodes.SevensOutPlayCount, statsManager, twoPlayer);

                        if (newHighScore)
                        {
                            io.WriteColourTextLine($"\nPlayer {playerHighScore} has reached a new high score!", ConsoleColor.Yellow);
                        }
                        
                        break;
                    case 2:
                        statsManager.OutputStatistics();
                        
                        break;
                    case 3:
                        Testing newTester = new Testing();
                        newTester.Test();
                        break;
                    case 4:
                        System.Environment.Exit(0);
                        break;
                        
                }
            }
        }
    }
    
}