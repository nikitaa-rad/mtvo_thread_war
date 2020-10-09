using System;
using System.Threading;

namespace mtvo_thread_war
{
    public class EnemyPull
    {
        //Посилання на ігрове поле
        private GameField gameField;
        
        public EnemyPull(GameField gameField)
        {
            this.gameField = gameField;
        }

        public void Start()
        {
            Actor enemy = new Actor(RandomX(),1,GameField.EnemyChar);
            //Змінна для того, щоб воорог рухався не по прямій лінії
            int direction = 1;
            
            while (enemy.Y() < gameField.FieldHigh() - 1 && !enemy.killed)
            {
                //Рух ворога по полю
                gameField.MoveEnemy(enemy, direction);
                //Зміна напрямку руху
                direction *= -1;
                
                Thread.Sleep(1000);
            }
            //Очищення ворогу
            gameField.Clean(enemy);
            //Зарахувати промах якщо ворога не вбито
            if(!enemy.killed) { gameField.IncreaseMissScore(); }
        }

        private int RandomX()
        {
            Random rnd = new Random();
            int startX = rnd.Next(8, gameField.FieldWith() - 1);
            
            return startX;
        }
    }
}