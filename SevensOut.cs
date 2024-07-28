using System;
using System.Threading;

namespace CMP1903_A2_2324
{
    public class SevensOut : Game
    {
        private readonly Die _dieOne = new Die();
        private readonly Die _dieTwo = new Die();

        public override (int playerOneScore, int playerTwoScore, int lastScore) playGame(bool twoPlayer)
        {
            int playerTwoTotal = 0;
            int playerOneTotal = 0;
            int lastTotal = 0;
            bool player = true;

            while (true)
            {
                if (player)
                {
                    Console.WriteLine("\nPlayer 1 Turn");
                    Console.WriteLine("Press enter to roll...");
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

                int rollOne = _dieOne.Roll();
                int rollTwo = _dieTwo.Roll();
                lastTotal = ((rollOne == rollTwo) ? (rollOne + rollTwo) * 2 : rollOne + rollTwo);

                Console.WriteLine($"Roll One: {rollOne}");
                Thread.Sleep(1000);
                Console.WriteLine($"Roll Two: {rollTwo}");
                Thread.Sleep(500);

                if (rollOne == rollTwo)
                {
                    Console.WriteLine("Double Detected! Doubling Total.");
                }

                Console.WriteLine($"Total: {lastTotal}");

                if (lastTotal == 7)
                {
                    Console.WriteLine("Seven Detected!");

                    if (!player)
                    {
                        break;
                    }

                    player = false;
                    continue;
                }

                playerOneTotal += ((player) ? lastTotal : 0);
                playerTwoTotal += ((player) ? 0: lastTotal);
                
                Console.WriteLine($"Combined Total: {((player) ? playerOneTotal : playerTwoTotal)}");
            }
            
            Console.WriteLine(((playerOneTotal > playerTwoTotal) ? "\nPlayer 1 Wins" : "\nPlayer 2 Wins"));
            Console.WriteLine($"Player 1 Total: {playerOneTotal}");
            Console.WriteLine($"Player 2 Total: {playerTwoTotal}");
            
            return (playerOneTotal, playerTwoTotal, lastTotal);
        }
    }
}
