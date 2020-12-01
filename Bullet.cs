using System.Threading;

namespace mtvo_thread_war
{
    public class Bullet
    {
        private const int SleepDuration = 200;
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
            while (bullet.Y() != 0 && !bullet.killed)
            {
                gameField.MoveBullet(bullet);
                
                Thread.Sleep(SleepDuration);
            }

            gameField.Clean(bullet);
            
            sem.Release();
        }
    }
}