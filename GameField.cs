namespace mtvo_thread_war
{
    public class Field
    {
        public const char BlunkSpaceChar = ' ';
        public const char GunChar = 'A';
        public const char BulletChar = '|';
        public const char EnemyChar = 'Ж';
        
        private int fieldWith;
        private int fieldHigh;
        private char[,] field;

        public Field(int with, int high)
        {
            fieldWith = with;
            fieldHigh = high;
            field = new char[fieldWith, fieldHigh];
            FillBlank();
        }
        
        public void MoveActor(Actor actor, int newX, int newY)
        {
            field[actor.X, actor.Y] = BlunkSpaceChar;
        
            actor.SetCoordinates(newX, newY);
        
            field[actor.X, actor.Y] = actor.Symbol;
        }
        
        private void FillBlank()
        {
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    field[i, j] = BlunkSpaceChar;
                }
            }
        }
    }
}