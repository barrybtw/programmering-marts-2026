namespace FloatingText;

class Program
{
    private const int Textcount = 10;
    private static readonly List<string> Options = ["BOO!", "BANG!", "POW!", "ZAP!", "WHAM!"];

    static Task Main()
    {
        ReadyConsole();

        var texts = new List<FloatingText>();
        Enumerable.Range(0, Textcount).ToList().ForEach(i => texts.Add(new FloatingText()));

        while (true)
        {
            Enumerable.Range(0, texts.Count).ToList().ForEach(i =>
            {
                texts[i].Move();
                texts[i].Draw();
            });
            Thread.Sleep(100);
            Enumerable.Range(0, texts.Count).ToList().ForEach(i => texts[i].Erase());
        }
    }

    private static void ReadyConsole()
    {
        Console.CursorVisible = false;
        Console.Clear();
        Console.WriteLine("Tryk på enhver knap for at starte...");
        Console.ReadKey(true);
        Console.Clear();
    }

    private class FloatingText
    {
        private string _text;
        private readonly Random _random = new Random();
        private int _xSpeed;
        private int _ySpeed;
        private int _xPos;
        private int _yPos;

        public FloatingText()
        {
            _text = Options[_random.Next(Options.Count)];
            _xSpeed = _random.Next(1, 3) * (_random.Next(2) == 0 ? -1 : 1);
            _ySpeed = _random.Next(1, 3) * (_random.Next(2) == 0 ? -1 : 1);

            _xPos = _random.Next(0, Math.Max(1, Console.WindowWidth - _text.Length));
            _yPos = _random.Next(0, Console.WindowHeight);
        }

        public void Erase()
        {
            Console.SetCursorPosition(_xPos, _yPos);
            Console.Write(new string(' ', _text.Length));
        }

        public void Draw()
        {
            if (_xPos < 0 || _yPos < 0) return;

            Console.SetCursorPosition(_xPos, _yPos);
            Console.Write(_text);
        }

        public void Move()
        {
            _xPos += _xSpeed;
            _yPos += _ySpeed;

            if (_xPos >= Console.WindowWidth - _text.Length)
            {
                _xPos = Console.WindowWidth - _text.Length;
                _xSpeed = -Math.Abs(_xSpeed); // FIX: Gør altid negativ ved kollision
                _text = Options[_random.Next(Options.Count)];
            }
            else if (_xPos < 0)
            {
                _xPos = 0;
                _xSpeed = Math.Abs(_xSpeed);
                _text = Options[_random.Next(Options.Count)];
            }

            if (_yPos >= Console.WindowHeight)
            {
                _yPos = Console.WindowHeight - 1;
                _ySpeed = -Math.Abs(_ySpeed);
                _text = Options[_random.Next(Options.Count)];
            }
            else if (_yPos < 0)
            {
                _yPos = 0;
                _ySpeed = Math.Abs(_ySpeed);
                _text = Options[_random.Next(Options.Count)];
            }
        }
    }
}