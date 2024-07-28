using System;

namespace CMP1903_A2_2324
{
    public abstract class Game
    {
        protected InputOutputManager InputManager = new InputOutputManager();
        protected Random RandomSeed = new Random();
        
        public abstract (int playerOneScore, int playerTwoScore, int lastScore) playGame(bool twoPlayer);
    }
}