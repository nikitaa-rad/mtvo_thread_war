namespace mtvo_thread_war
{
    public class Actor
    {
        private int x;
        private int y;
        public string symbol;

        public Actor(int x, int y, string symbol)
        {
            this.x = x;
            this.y = y;
            this.symbol = symbol;
        }

        public int X() { return x; }

        public int Y() { return y; }

       // public string Symbol() { return symbol; }

        public void SetCoordinates(int newX, int newY) { x = newX; y = newY; }
    }
}