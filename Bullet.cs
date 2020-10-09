using System;
using System.Threading;

namespace mtvo_thread_war
{
    public class Bullet
    {
        private static Semaphore sem;
        //Посилання на ігрове поле
        private GameField gameField;

        public Bullet(GameField gameField)
        {
            this.gameField = gameField;
            //Максимально можлива кількість куль - 3 
            sem = new Semaphore(3, 3);
        }
        //Метод викликається під час натискання на пробіл
        public void Fire(int x, int y)
        {
            Actor bullet = new Actor(x, y, GameField.BulletChar);
            //Семафор не дозволяє рухатись більше ніж 3 кулям
            sem.WaitOne();
            while (bullet.Y() != 0 && !bullet.killed)
            {
                gameField.MoveBullet(bullet);
                
                Thread.Sleep(200);
            }
            //Очищення кулі з поля після того, як вона була вбита чи вийшла за поле
            gameField.Clean(bullet);
            
            sem.Release();
        }
    }
}