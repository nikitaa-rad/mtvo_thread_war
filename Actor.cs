namespace mtvo_thread_war
{
    public class Actor
    {
        //Вбито актора чи ні
        public bool killed;
        //Координати на полі
        private int x;
        private int y;
        //Символ актора на полі
        public string symbol;

        public Actor(int x, int y, string symbol)
        {
            this.x = x;
            this.y = y;
            this.symbol = symbol;
            killed = false;
        }

        public int X() { return x; }

        public int Y() { return y; }
        //Метод оновлення координат актора
        public void SetCoordinates(int newX, int newY) { x = newX; y = newY; }
    }
}