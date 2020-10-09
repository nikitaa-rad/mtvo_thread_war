using System;
using System.Timers;
using Timer = System.Timers.Timer;

namespace mtvo_thread_war
{
    class Program
    {
        private static bool wait;
        private static Timer aTimer;
        static void Main(string[] args)
        {
            wait = true;
            //Створення таймеру
            SetTimer();
            //Блокуємо початок роботи програми
            while (wait)
            {
                if (Console.ReadKey().Key.Equals(ConsoleKey.LeftArrow) ||
                    Console.ReadKey().Key.Equals(ConsoleKey.RightArrow))
                {
                    wait = false;
                }
            }
            //Очищуємо консоль
            Console.Clear();
            //Розмір вікна консолі
            int with = Console.WindowWidth;
            int high = Console.WindowHeight;
            //Початок гри
            GameField gameField = new GameField(with, high);
            gameField.Start();
        }

        public static void SetTimer()
        {
            // Створення таймеру 
            aTimer = new Timer();
            aTimer.Interval = 15000;
            //Встановлюємо колбек таймера
            aTimer.Elapsed += DontWait;
            //Не повторюємо виклик методу
            aTimer.AutoReset = true;
            //Старт таймеру
            aTimer.Enabled = true;
        }
        //Колбек таймера
        private static void DontWait(object sender, ElapsedEventArgs e)
        {
            wait = false;
        }
    }
}