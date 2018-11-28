using System;
using System.Text;

class Program
{
    static int w, h; static int[] hs;
    static Random r = new Random();
    
    static void Main()
    {

        Console.CursorVisible = false; Console.OutputEncoding = Encoding.UTF8;
        h = Console.WindowHeight = 50; w = Console.WindowWidth = 150;

        hs = new int[w];
        for (int i = 0; i < w; i++) hs[i] = r.Next(h);

        int col = 0;
        while (true) Update(col++ % w);
    }

    static char Rand() => (char)r.Next(0x01F8, 0x0248);

    static void Update(int col)
    {
        int t = hs[col];

        Console.SetCursorPosition(col, t-10 < 0 ? h-t : t-10); // tää
        Console.Write(' ');

        Console.ForegroundColor = ConsoleColor.DarkGreen;
        for (int i = 0; i < 8; i++)
        {
            Console.SetCursorPosition(col, t++ % h);
            Console.Write(Rand());
        }

        Console.ForegroundColor = ConsoleColor.Green;
        for (int i = 0; i < 2; i++)
        {
            Console.SetCursorPosition(col, t++ % h);
            Console.Write(Rand());
        }

        Console.SetCursorPosition(col, t++ % h);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(Rand());

        if (hs[col] < h) hs[col]++;
        else hs[col] = 0;
    }
}
