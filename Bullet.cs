using System;
using System.Threading;

namespace mtvo_thread_war
{
    public class Bullet
    {
        private static Semaphore sem;
        private GameField gameField;

        public Bullet(GameField gameField)
        {
            this.gameField = gameField;
            
            sem = new Semaphore(3, 3);
        }
        
        public void Fire(int x, int y)
        {
            Actor bullet = new Actor(x, y, GameField.BulletChar);
            
            sem.WaitOne();
            while (bullet.Y() != 0 && bullet.symbol != GameField.KilledChar)
            {
                gameField.MoveBullet(bullet);
                
                Thread.Sleep(200);
            }

            // if (bullet.symbol != GameField.KilledChar)
            // {
            gameField.Clean(bullet);
            //}
            
            sem.Release();
        }
    }
}