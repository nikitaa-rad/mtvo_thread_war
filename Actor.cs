namespace mtvo_thread_war
{
    public class Point
    {
        private int x;
        private int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
        public int X { get; }
        public int Y { get; }

        public void SetCoordinates(int newX, int newY)
        {
            x = newX;
            y = newY;
        }
    }
}