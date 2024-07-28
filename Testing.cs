using System;
using System.Diagnostics;

namespace CMP1903_A2_2324
{
    public class Testing
    {
        public void Test()
        {
            InputOutputManager io = new InputOutputManager();
            
            // Test Die
            Die testDie = new Die();

            for (int i = 0; i < 100; i++)
            {
                int testRoll = testDie.Roll();
                Debug.Assert(testRoll > 0 && testRoll <= 6, "Die is rolling out of bounds!");
            }
            
            // Variable Initialisation
            int playerOneScore;
            int playerTwoScore;
            int lastScore;
            
            // Test Sevens Out
            SevensOut sevensOut = new SevensOut();
            io.WriteColourTextLine("\n\n-- TEST GAME --\n\n", ConsoleColor.Magenta);
            (playerOneScore, playerTwoScore, lastScore) = sevensOut.playGame(false);
            
            Debug.Assert(lastScore == 7, "The last score given was not a 7!");
            
            // Test Three Or More
            ThreeOrMore threeOrMore = new ThreeOrMore();
            io.WriteColourTextLine("\n\n-- TEST GAME --\n\n", ConsoleColor.Magenta);
            (playerOneScore, playerTwoScore, lastScore) = threeOrMore.playGame(false);
            
            Debug.Assert(playerOneScore >= 20 && playerTwoScore >= 20, "Conditions are not working for ThreeOrMore.");
        }
    }
}