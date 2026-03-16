namespace Spil;

class Program
{
    private const int STARCOUNT = 10;

    public static void Main()
    {
        Player player = new Player(0,0,'$');        // Der kan nu indtastet aktuelle parametre for player
        
        Console.CursorVisible = false;
        Console.Clear();
        
        var random = new Random();
        var stars = new List<(int x, int y)>();
        for (int i = 0; i < STARCOUNT; i++)                                 // Laver 5 stjerner på tilfældige positioner i konsollen
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
        private int xPos { get; set; }
        private int yPos { get; set; }
        private char sign { get; }
        private ConsoleKey leftKey { get; }
        public ConsoleKey rightKey { get; }
        public ConsoleKey upKey { get; }
        public ConsoleKey downKey { get; }
        
        public Player(int xPos = 0, int yPos = 1, char sign = '¥', ConsoleKey leftKey = ConsoleKey.LeftArrow,
            ConsoleKey rightKey = ConsoleKey.RightArrow, ConsoleKey upKey = ConsoleKey.UpArrow, ConsoleKey downKey = ConsoleKey.DownArrow)
        {
            this.xPos = xPos;
            this.yPos = Math.Max(1, yPos);
            this.sign = sign;
            this.leftKey = leftKey;
            this.rightKey = rightKey;
            this.upKey = upKey;
            this.downKey = downKey;
        }
        
        public void Draw()
        {
            Console.SetCursorPosition(xPos, yPos);
            Console.Write(sign);
        }

        private void Erase()
        {
            Console.SetCursorPosition(xPos, yPos);
            Console.Write(' ');
        }
        
        private void MoveHorizontal(int distance)
        {
            if (xPos + distance < 0 || xPos + distance >= Console.WindowWidth) return; // Holder den indenfor bounds
            Erase();
            xPos += distance;
            Draw();
        }
        
        private void MoveVertical(int distance)
        {
            if (yPos + distance < 1 || yPos + distance >= Console.WindowHeight) return; // Holder den indenfor bounds
            Erase();
            yPos += distance;
            Draw();
        }

        public List<(int, int)> ReadInput(ConsoleKeyInfo key, List<(int, int)> stars)
        {
            if (key.Key == leftKey) MoveHorizontal(-1);
            else if (key.Key == rightKey) MoveHorizontal(1);
            else if (key.Key == upKey) MoveVertical(-1);
            else if (key.Key == downKey) MoveVertical(1);
            
            for (var i = 0; i < stars.Count; i++)           // Går igennem alle stjernern og tjekker for collision med player
            {
                if (xPos != stars[i].Item1 || yPos != stars[i].Item2) continue;

                stars.RemoveAt(i);                          // Fjern stjernen fra listen
                Console.SetCursorPosition(7, 0);    // Position for point display
                Console.Write($"{STARCOUNT - stars.Count}");// Opdater point display
                return stars;                               // Returner den opdaterede stjerne liste
            }

            return stars;
        }
    }
}