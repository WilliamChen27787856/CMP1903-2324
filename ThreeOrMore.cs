using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CMP1903_A2_2324
{
    public class ThreeOrMore : Game
    {
        private Die _dieOne = new Die();
        private Die _dieTwo = new Die();
        private Die _dieThree = new Die();
        private Die _dieFour = new Die();
        private Die _dieFive = new Die();

        public int playerOneScore;
        public int playerTwoScore;

        private List<int> RollFiveDie()
        {
            List<int> returnList = new List<int>();
            returnList.Add(_dieOne.Roll());
            returnList.Add(_dieTwo.Roll());
            returnList.Add(_dieThree.Roll());
            returnList.Add(_dieFour.Roll());
            returnList.Add(_dieFive.Roll());

            return returnList;
        }

        private List<int> RollThreeDie(List<int> currentDieList, int repeatedValue)
        {
            IEnumerable<int> rerollList = Enumerable.Range(0, currentDieList.Count)
                .Where(index => currentDieList[index] != repeatedValue)
                .ToList();

            foreach (int index in rerollList)
            {
                int replacementRoll = 0;

                switch (index + 1)
                {
                    case 1:
                        replacementRoll = _dieOne.Roll();
                        break;
                    case 2:
                        replacementRoll = _dieTwo.Roll();
                        break;
                    case 3:
                        replacementRoll = _dieThree.Roll();
                        break;
                    case 4:
                        replacementRoll = _dieFour.Roll();
                        break;
                    case 5:
                        replacementRoll = _dieFive.Roll();
                        break;
                }
                
                currentDieList[index] = replacementRoll;
            }

            return currentDieList;
        }

        private (int action, int repeatRoll) CourseOfAction(List<int> dice)
        {
            Dictionary<int, int> numberCount = new Dictionary<int, int>();
            
            foreach (int number in dice)
            {
                if (!numberCount.ContainsKey(number))
                {
                    numberCount.Add(number, 0);
                }
                
                numberCount[number] += 1;
            }

            int highestKeyValue = numberCount
                .Aggregate((entryOne, entryTwo) => entryOne.Value > entryTwo.Value ? entryOne : entryTwo).Key;

            switch (numberCount[highestKeyValue])
            {
                case 2:
                    return (1, highestKeyValue);
                case 3:
                    return (2, highestKeyValue);
                case 4:
                    return(3, highestKeyValue);
                case 5:
                    return (4, highestKeyValue);
                default:
                    return (0, 0);
            }
        }
        
        public override (int playerOneScore, int playerTwoScore, int lastScore) playGame(bool twoPlayer)
        {
            int playerOneScore = 0;
            int playerOneRounds = 0;

            int playerTwoScore = 0;
            int playerTwoRounds = 0;

            int lastScore = 0;
            
            bool player = true;

            List<int> lastDieRoll = new List<int>();
            int lastRepeatedValue = 0;
            bool threeDie = false;
            
            while (true)
            {
                lastScore = 0;
                
                if (!threeDie)
                {
                    if (player)
                    {
                        Console.WriteLine("\nPlayer One");
                        Console.WriteLine("Press Enter to roll your dice...");
                        Console.ReadKey();
                    }
                    else
                    {
                        if (!twoPlayer)
                        {
                            Console.WriteLine("\nPlayer 2 (Computer) Turn");
                            Console.WriteLine("Rolling....");
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            Console.WriteLine("\nPlayer 2 Turn");
                            Console.WriteLine("Press enter to roll...");
                            Console.ReadKey();
                        }
                    }
                }
                else
                {
                    InputManager.WriteColourTextLine("\nRolling 3 remaining die...", ConsoleColor.Green);
                }

                List<int> rolledDie;

                if (threeDie)
                {
                    rolledDie = RollThreeDie(lastDieRoll, lastRepeatedValue);
                }
                else
                {
                    rolledDie = RollFiveDie();
                }
                
                for (int i = 0; i < rolledDie.Count; i++)
                {
                    Console.WriteLine($"Die {i + 1}: {rolledDie[i]}");
                }

                lastDieRoll = rolledDie;
                
                (int courseOfAction, int rollValueOfAction) = CourseOfAction(rolledDie);
                
                //InputManager.WriteColourTextLine($"{courseOfAction}, {rollValueOfAction}", ConsoleColor.Red);
                lastRepeatedValue = rollValueOfAction;
                
                switch (courseOfAction)
                {
                   case 1:
                       if (threeDie)
                       {
                           InputManager.WriteColourTextLine("\nNo luck! Try again!", ConsoleColor.DarkRed);
                           break;
                       };

                       if (twoPlayer || player)
                       {
                           Console.WriteLine("\nYou rolled 2 of the same! Would you like to reroll the remaining die or try again?");
                       
                           string[] menuOptions = new string[] { "Roll Other Three Die", "Try Again" };
                           (bool, int) option = InputManager.HandleMenu(menuOptions);

                           while (!option.Item1)
                           {
                               InputManager.WriteColourTextLine("\nThis is not a valid option. Try again.\n", ConsoleColor.Red);
                               option = InputManager.HandleMenu(menuOptions);
                           }

                           if (option.Item2 == 0)
                           {
                               threeDie = true;
                               continue;
                           }
                       }
                       else
                       {
                           Console.WriteLine("\nYou rolled 2 of the same! Would you like to reroll the remaining die or try again?");
                           int computerRandomChoice = RandomSeed.Next(1, 2);
                           
                           InputManager.WriteColourTextLine($"Computer: {((computerRandomChoice == 1) ? "Yes" : "No")}", ConsoleColor.Magenta);

                           if (computerRandomChoice == 1)
                           {
                               threeDie = true;
                               continue;
                           }
                       }

                       break;
                   case 2:
                       Console.WriteLine("\nYou rolled 3 of the same! You earned 3 points.");
                       lastScore = 3;
                       break;
                   case 3:
                       Console.WriteLine("\nYou rolled 4 of the same! You earned 6 points.");
                       lastScore = 6;
                       break;
                   case 4:
                       Console.WriteLine("\nYou rolled 5 of the same! You earned 12 points.");
                       lastScore = 12;
                       break;
                   default:
                       InputManager.WriteColourTextLine("\nNo rolls are the same! Try again!", ConsoleColor.DarkRed);
                       break;
                }

                threeDie = false;
                playerOneScore += ((player) ? lastScore : 0);
                playerOneRounds += ((player) ? 1 : 0);
                playerTwoScore += ((!player) ? lastScore : 0);
                playerTwoRounds += ((!player) ? 1 : 0);

                int currentPlayerScore = ((player) ? playerOneScore : playerTwoScore);
                int currentPlayerRounds = ((player) ? playerOneRounds : playerTwoRounds);
                
                InputManager.WriteColourTextLine($"\nYour Current Score: {currentPlayerScore}", ConsoleColor.Blue);

                if (currentPlayerScore < 20) continue;
                InputManager.WriteColourTextLine($"\nYou have reached 20 points! Your Turns: {currentPlayerRounds}", ConsoleColor.Blue);

                if (player)
                {
                    player = false;
                }
                else
                {
                    break;
                }
            }
            
            InputManager.WriteColourTextLine($"\nWinner: {((playerOneRounds < playerTwoRounds) ? "Player One" : "Player Two")}!! Beat the other player by {Math.Abs(playerOneRounds - playerTwoRounds)} rounds.", ConsoleColor.Yellow);
            
            return (playerOneRounds, playerTwoRounds, lastScore);
        }
    }
}