namespace mtvo_thread_war
{
    public class Actor
    {
        public bool killed;

        private int x;
        private int y;
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
        public void SetCoordinates(int newX, int newY) { x = newX; y = newY; }
    }
}