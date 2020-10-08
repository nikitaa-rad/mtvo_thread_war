using System;

namespace mtvo_thread_war
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadLine();
            Console.Clear();
            
            int with = Console.WindowWidth;
            int high = Console.WindowHeight;

            
            
            GameField gameField = new GameField(with, high);
            // EnemyPull e = new EnemyPull(gameField);
            // e.Start();
            gameField.Start();
        }
    }
}