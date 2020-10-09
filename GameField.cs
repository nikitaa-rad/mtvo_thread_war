using System;
using System.Threading;

namespace mtvo_thread_war
{
    public class GameField
    {
        //Символи акторів консолі
        public const string BlunkSpaceChar = " ";
        public const string GunChar = "A";
        public const string BulletChar = "|";
        public const string EnemyChar = "Ж";
        //Число промахів, після якого гра завершується
        public const int MaxMiss = 30;
        // Локер поля для синхронізації, робить те ж саме, що і м'ютекс
        private readonly object fieldLock = new object();

        private int miss;
        private int hit;
        private int fieldWith;
        private int fieldHigh;
        private Actor[,] field;

        private Actor gun;
        private Bullet bullet;

        public GameField(int with, int high)
        {
            fieldWith = with;
            fieldHigh = high;
            field = new Actor[fieldWith, fieldHigh];
            miss = 0;
            hit = 0;
            gun = new Actor(fieldWith/2, fieldHigh - 1, GunChar);
            //Ініціалізуємо клас кулі з семафором
            bullet = new Bullet(this);
            //Ініціалізація поля
            FillBlank();
            //Виводимо рахунок
            PrintScore();
            //Віводимо кулю
            MoveGun(-1);
        }
        
        public int FieldWith() { return fieldWith; }
        public int FieldHigh() { return fieldHigh; }

        public void Start()
        {
            //Виводимо ворогів у новому потоці
            Thread thread = new Thread(PullEnemies);
            thread.Start();
            //Слідкуємо за клавішами
            while (true)
            {
                if (Console.ReadKey().Key.Equals(ConsoleKey.LeftArrow)) { MoveGun(-1); }
                
                if (Console.ReadKey().Key.Equals(ConsoleKey.RightArrow)) { MoveGun(1); }
                
                if (Console.ReadKey().Key.Equals(ConsoleKey.Spacebar)) { Shot(); }
            }
        }

        private void PullEnemies()
        {
            //Створюємо потоки ворогів, тільки якщо рандом вирішує їм з'явитися
            while (true)
            {
                EnemyPull enemy = new EnemyPull(this);
                
                Thread thread = new Thread(enemy.Start);
                
                if(ShouldPoolEnemy()) { thread.Start(); }

                Thread.Sleep(1000);
            }
        }

        public void IncreaseMissScore()
        {
            //Збільшення числа промахів
            lock (fieldLock)
            {
                miss += 1;
                PrintScore();
                if (miss >= MaxMiss)
                {
                    CloseGame();
                }
            }
        }
        
        public void IncreaseHitScore()
        {
            //Збільшення числа влучень
            lock (fieldLock)
            {
                hit += 1;
                PrintScore();
            }
        }
        
        public void MoveBullet(Actor bullet)
        {
            lock (fieldLock)
            {
                //Перевірка на наявність ворога
                if (field[bullet.X(), bullet.Y() - 1].symbol.Equals(EnemyChar))
                {
                    //Маркер для того, щоб вбити потоки кулі та ворога
                    field[bullet.X(), bullet.Y() - 1].killed = true;
                    bullet.killed = true;
                    //Збільшуємо очки
                    IncreaseHitScore();
                    return;
                }
                //Очищуємо кулю
                Actor blunk = new Actor(bullet.X(), bullet.Y(),BlunkSpaceChar);
                field[bullet.X(), bullet.Y()] = blunk;
                Console.SetCursorPosition(bullet.X(), bullet.Y());
                Console.Write(BlunkSpaceChar);
                //Ставимо кулю
                bullet.SetCoordinates(bullet.X(), bullet.Y() - 1);
                
                Set(bullet);
            }
        }
        
        public void MoveEnemy(Actor enemy, int direction)
        {
            lock (fieldLock)
            {
                //Зтераємо попередню координату ворога
                Clean(enemy);
                //Оновлюємо координати ворога
                enemy.SetCoordinates(enemy.X() + direction, enemy.Y() + 1);
                //Ставимо нову ворога
                Set(enemy);
            }
        }

        public void Clean(Actor actor)
        {
            lock (fieldLock)
            {
                //Ставимо пустого актора на місце діючої особи
                Actor blunk = new Actor(actor.X(), actor.Y(),BlunkSpaceChar);
                field[actor.X(), actor.Y()] = blunk;
                Console.SetCursorPosition(actor.X(), actor.Y());
                Console.Write(BlunkSpaceChar);
            }
        }

        private void Set(Actor actor)
        {
            // Метод для оновлення координат актора та виводу на єкран
            lock (fieldLock)
            {
                field[actor.X(), actor.Y()] = actor;
                Console.SetCursorPosition(actor.X(), actor.Y());
                Console.Write(actor.symbol);
            }
        }

        private void MoveGun(int direction)
        {
            int coordinate = gun.X() + direction;
            // Необхідно для того, щоб гармата не заходила за вікно
            if (coordinate < fieldWith && coordinate >= 0)
            {
                lock (fieldLock)
                {
                    //Зтераємо попередню координату гармати
                    Clean(gun);
                    //Оновлюємо координати гармати
                    gun.SetCoordinates(coordinate, gun.Y());

                    //Ставимо нову координату
                    Set(gun);
                }
            }
        }

        private void Shot()
        {
            // Постріл. Виліт кулі з координати гармати
            Thread thread = new Thread(delegate() { bullet.Fire(gun.X(), fieldHigh - 2); });
            thread.Start();
        }

        private void FillBlank()
        {
            //Заповнюємо поле пустими акторами
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    Actor blunk = new Actor(i, j,BlunkSpaceChar);
                    field[i, j] = blunk;
                }
            }
        }

        private bool ShouldPoolEnemy()
        {
            // Рандомне число від 0 до 10, якщо воно більше 6 - ворог з'явиться.
            Random rnd = new Random();
            // З часом верхня граніця збільшується, тим самим зростає верогідність появи ворога
            int number = rnd.Next(0, miss + 10);
            
            return number > 6;
        }

        private void PrintScore()
        {
            //Виводить рахунок
            lock (fieldLock)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write("Miss: {0}", miss);
            
                Console.SetCursorPosition(0, 1);
                Console.Write("Hit: {0}", hit);
            }
        }

        private void CloseGame()
        {
            // Заверщення гри
            Console.Clear();
            Console.Write("YOU LOST!!!");
            Environment.Exit(0);
        }
    }
}