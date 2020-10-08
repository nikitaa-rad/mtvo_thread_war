using System;
using System.Threading;

namespace mtvo_thread_war
{
    public class EnemyPull
    {
        private GameField gameField;
        
        public EnemyPull(GameField gameField)
        {
            this.gameField = gameField;
        }

        public void Start()
        {
            Actor enemy = new Actor(RandomX(),1,GameField.EnemyChar);
            int direction = 1;
            
            while (enemy.Y() < gameField.FieldHigh() - 1 && enemy.symbol != GameField.KilledChar)
            {
                gameField.MoveEnemy(enemy, direction);
                Thread.Sleep(1000);
                direction *= -1;
            }
            
            gameField.Clean(enemy);

            gameField.IncreaseMissScore();
        }

        private int RandomX()
        {
            Random rnd = new Random();
            int startX = rnd.Next(8, gameField.FieldWith() - 1);
            
            return startX;
        }
    }
}