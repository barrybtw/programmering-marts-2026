namespace Spil;

public static class Program
{
    private const int Starcount = 10;

    public static void Main()
    {
        var player = new Player(0,0,'$');        // Der kan nu indtastet aktuelle parametre for player
        
        Console.CursorVisible = false;
        Console.Clear();
        
        var random = new Random();
        var stars = new List<(int x, int y)>();
        for (var i = 0; i < Starcount; i++)         // Laver x antal stjerner på tilfældige positioner i konsollen
        {
            var x = random.Next(0, Console.WindowWidth);
            var y = random.Next(1, Console.WindowHeight);
            stars.Add((x,y));
            Console.SetCursorPosition(x, y);
            Console.Write('*');
        }
        Console.SetCursorPosition(0,0);
        Console.Write("Point: 0");
        
        player.Draw();
        while (true)
        {
            var keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.Escape) break;
            
            stars = player.ReadInput(keyInfo, stars);
        }
    }

    class Player
    {
        private int XPos { get; set; }
        private int YPos { get; set; }
        private char Sign { get; }
        private ConsoleKey LeftKey { get; }
        private ConsoleKey RightKey { get; }
        private ConsoleKey UpKey { get; }
        private ConsoleKey DownKey { get; }
        
        public Player(int xPos = 0, int yPos = 1, char sign = '¥', ConsoleKey leftKey = ConsoleKey.LeftArrow,
            ConsoleKey rightKey = ConsoleKey.RightArrow, ConsoleKey upKey = ConsoleKey.UpArrow, ConsoleKey downKey = ConsoleKey.DownArrow)
        {
            XPos = xPos;
            YPos = Math.Max(1, yPos);
            Sign = sign;
            LeftKey = leftKey;
            RightKey = rightKey;
            UpKey = upKey;
            DownKey = downKey;
        }
        
        public void Draw()
        {
            Console.SetCursorPosition(XPos, YPos);
            Console.Write(Sign);
        }

        private void Erase()
        {
            Console.SetCursorPosition(XPos, YPos);
            Console.Write(' ');
        }
        
        private void MoveHorizontal(int distance)
        {
            if (XPos + distance < 0 || XPos + distance >= Console.WindowWidth) return; // Holder den indenfor bounds
            Erase();
            XPos += distance;
            Draw();
        }
        
        private void MoveVertical(int distance)
        {
            if (YPos + distance < 1 || YPos + distance >= Console.WindowHeight) return; // Holder den indenfor bounds
            Erase();
            YPos += distance;
            Draw();
        }

        public List<(int, int)> ReadInput(ConsoleKeyInfo key, List<(int, int)> stars)
        {
            if (key.Key == LeftKey) MoveHorizontal(-1);
            else if (key.Key == RightKey) MoveHorizontal(1);
            else if (key.Key == UpKey) MoveVertical(-1);
            else if (key.Key == DownKey) MoveVertical(1);
            
            for (var i = 0; i < stars.Count; i++)           // Går igennem alle stjernerne og tjekker for collision med player
            {
                if (XPos != stars[i].Item1 || YPos != stars[i].Item2) continue;

                stars.RemoveAt(i);                          // Fjern stjernen fra listen
                Console.SetCursorPosition(7, 0);            // Position for point display
                Console.Write($"{Starcount - stars.Count}");// Opdater point display
                return stars;                               // Returner den opdaterede stjerne liste
            }

            return stars;
        }
    }
}