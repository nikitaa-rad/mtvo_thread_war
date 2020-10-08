using System;

namespace mtvo_thread_war
{
    class Program
    {
        static void Main(string[] args)
        { 
            Console.ReadLine();
            int with = Console.WindowWidth;
            int high = Console.WindowHeight;

            
            
            GameField gameField = new GameField(with, high);
            
            gameField.Start();
        }
    }
}