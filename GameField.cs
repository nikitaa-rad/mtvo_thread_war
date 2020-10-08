using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading;

namespace mtvo_thread_war
{
    public class GameField
    {
        public const string BlunkSpaceChar = " ";
        public const string GunChar = "A";
        public const string BulletChar = "|";
        public const string EnemyChar = "Ж";
        public const string KilledChar = "R";

        public const int MaxMiss = 30;
        
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
            
            bullet = new Bullet(this);
            
            FillBlank();
            PrintScore();
            MoveGun(-1);
        }
        
        public int FieldWith() { return fieldWith; }
        public int FieldHigh() { return fieldHigh; }

        public void Start()
        {
            Thread thread = new Thread(PullEnemies);
                
            thread.Start();
            
            while (true)
            {
                if (Console.ReadKey().Key.Equals(ConsoleKey.LeftArrow)) { MoveGun(-1); }
                
                if (Console.ReadKey().Key.Equals(ConsoleKey.RightArrow)) { MoveGun(1); }
                
                if (Console.ReadKey().Key.Equals(ConsoleKey.Spacebar)) { Shot(); }
            }
        }

        private void PullEnemies()
        {
            int timeout = 2000;
            
            while (true)
            {
                EnemyPull enemy = new EnemyPull(this);
                
                Thread thread = new Thread(enemy.Start);
                
                thread.Start();

                Thread.Sleep(timeout);
            }
        }

        public void IncreaseMissScore()
        {
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
                if (field[bullet.X(), bullet.Y() - 1].symbol.Equals(EnemyChar))
                {
                    field[bullet.X(), bullet.Y() - 1].killed = true;
                    bullet.killed = true;
                    IncreaseHitScore();
                    return;
                }
                
                Actor blunk = new Actor(bullet.X(), bullet.Y(),BlunkSpaceChar);
                field[bullet.X(), bullet.Y()] = blunk;
                Console.SetCursorPosition(bullet.X(), bullet.Y());
                Console.Write(BlunkSpaceChar);

                bullet.SetCoordinates(bullet.X(), bullet.Y() - 1);

                field[bullet.X(), bullet.Y()] = bullet;
                Console.SetCursorPosition(bullet.X(), bullet.Y());
                Console.Write(bullet.symbol);
            }
        }
        
        public void MoveEnemy(Actor enemy, int direction)
        {
            lock (fieldLock)
            {
                Clean(enemy);
                
                enemy.SetCoordinates(enemy.X() + direction, enemy.Y() + 1);

                field[enemy.X(), enemy.Y()] = enemy;
                Console.SetCursorPosition(enemy.X(), enemy.Y());
                Console.Write(enemy.symbol);
            }
        }

        public void Clean(Actor actor)
        {
            lock (fieldLock)
            {
                Actor blunk = new Actor(actor.X(), actor.Y(),BlunkSpaceChar);
                field[actor.X(), actor.Y()] = blunk;
                Console.SetCursorPosition(actor.X(), actor.Y());
                Console.Write(BlunkSpaceChar);
            }
        }

        private void MoveGun(int direction)
        {
            int coordinate = gun.X() + direction;

            if (coordinate < fieldWith && coordinate >= 0)
            {
                lock (fieldLock)
                {
                    Clean(gun);
                    gun.SetCoordinates(coordinate, gun.Y());

                    field[gun.X(), gun.Y()] = gun;
                    Console.SetCursorPosition(gun.X(), gun.Y());
                    Console.Write(gun.symbol);
                }
            }
        }

        private void Shot()
        {
            Thread thread = new Thread(delegate() { bullet.Fire(gun.X(), fieldHigh - 2); });
            thread.Start();
        }

        private void FillBlank()
        {
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    Actor blunk = new Actor(i, j,BlunkSpaceChar);
                    field[i, j] = blunk;
                }
            }
        }

        private void PrintScore()
        {
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
            Console.Clear();
            Console.Write("YOU LOST!");
            Environment.Exit(0);
        }
    }
}