using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Tetris
{
    class Program
    {
        static void Main(string[] args)
        {
            new Game().Run();
        }
    }



    class Game
    {
        private static int x = 20, y = 15;
        private static int score = 0;
        private BlockElement[,] Map = new BlockElement[y, x];
        private int Tick_counter = 0;
        private bool Work = true;


        public void Initlialize()
        {
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    Map[i, j] = new BlockElement();
                    Map[i, j].Active = false;
                    Map[i, j].Value = ' ';

                    if (i == 0 || i == y - 1)
                    {
                        Map[i, j].Value = '-';
                    }

                    if (j == 0 || j == x - 1)
                    {
                        Map[i, j].Value = '|';
                    }
                }
            }

            for (int i = 1; i < x - 1; i++)
            {
                Map[y - 2, i].Value = '*';
            }
        }
        public void Gravity()
        {

            for (int i = y - 1; i >= 0; i--)
            {
                for (int j = 0; j < x; j++)
                {
                    if (Map[i, j].Value == '*' && Map[i + 1, j].Value == ' ')
                    {
                        Map[i + 1, j].Value = Map[i, j].Value;
                        Map[i + 1, j].Active = true;
                        Map[i, j].Value = ' ';
                        Map[i, j].Active = false;

                        Map[i, j].Moved = false;

                    }
                    else
                    {
                        Map[i, j].Active = false;
                    }
                }
            }
        }

        private void PrintMap()
        {

            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    Console.Write(Map[i, j]);
                }
                Console.WriteLine();
            }
        }

        public void Add()
        {
            Random random = new Random();
            int offsetx = random.Next(x - 4) + 1;

            char[,] b = Block.GenerateBlock();

            bool anyActive = false;

            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    if (Map[i, j].Active)
                    {
                        anyActive = true;
                        break;
                    }

                }
            }

            if (!anyActive)
            {
                for (int i = 0; i < b.GetLength(0); i++)
                {
                    for (int j = 0; j < b.GetLength(1); j++)
                    {
                        if (b[i, j] != '.')
                        {
                            if (Map[i + 1, offsetx + j].Value == '*')
                            {
                                Lose();
                            }
                            else
                            {
                                Map[i + 1, offsetx + j].Value = '*';
                                Map[i + 1, offsetx + j].Active = true;
                            }
                        }
                    }
                }
            }

        }

        public void Clear_Line()
        {
            bool filled = true;
            for (int i = 1; i < x - 1; i++)
            {
                if (Map[y - 2, i].Value != '*')
                {
                    filled = false;
                }
            }

            for (int i = 1; i < x - 1; i++)
            {
                if (filled)
                {
                    Map[y - 2, i].Value = ' ';
                }
            }

            if (filled)
            {
                score += 100;
            }
        }

        void Move(ConsoleKey ck)
        {
            for (int i = 1; i < Map.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < Map.GetLength(1) - 1; j++)
                {
                    if (ck == ConsoleKey.A && Map[i, j].Active && !Map[i, j].Moved && Map[i, j - 1].Value == ' ')
                    {
                        Map[i, j].Value = ' ';
                        Map[i, j].Active = false;

                        Map[i, j - 1].Value = '*';
                        Map[i, j - 1].Active = true;

                        Map[i, j - 1].Moved = true;
                    }
                    if (ck == ConsoleKey.D && Map[i, j].Active && !Map[i, j].Moved && Map[i, j + 1].Value == ' ')
                    {
                        Map[i, j].Value = ' ';
                        Map[i, j].Active = false;

                        Map[i, j + 1].Value = '*';
                        Map[i, j + 1].Active = true;

                        Map[i, j + 1].Moved = true;
                    }
                }
            }
        }

        public void Render_Tick()
        {
            Console.Clear();

            Gravity();

            ConsoleKey Ck = ConsoleKey.C;
            if (Console.KeyAvailable)
            {
                Ck = Console.ReadKey(true).Key;
            }

            PrintMap();
            Move(Ck);


            if (Tick_counter == 3)
            {
                Add();
                Clear_Line();
                Tick_counter = 0;
            }

            Tick_counter++;

            Console.WriteLine("====================");
            Console.WriteLine("     SCORE:" + score);
            Console.WriteLine("====================");

            Thread.Sleep(600);
        }

        public void Lose()
        {
            Work = false;
        }

        public void Run()
        {
            Initlialize();

            while (Work)
            {
                Render_Tick();
            }

            Console.WriteLine("  YOU HAVE LOST!!!");
            Console.WriteLine("ENDED WITH SCORE:" + score);
            Console.WriteLine("====================");
        }

        class BlockElement
        {
            private bool _active = false;
            private char _value = '*';
            private bool _moved = false;

            public bool Active
            {
                get => _active;
                set => _active = value;
            }

            public bool Moved
            {
                get => _moved;
                set => _moved = value;
            }

            public char Value
            {
                get => _value;
                set => this._value = value;
            }

            public override string ToString()
            {
                return _value.ToString();
            }
        }

        class Block
        {
            public static char[,] GenerateBlock()
            {
                char[,] data = new char[4, 4];

                for (int i = 0; i < data.GetLength(0); i++)
                {
                    for (int j = 0; j < data.GetLength(1); j++)
                    {
                        data[i, j] = '.';
                    }
                }

                switch (new Random().Next(5))
                {
                    case 0:
                        data[0, 0] = '*';
                        break;
                    case 1:
                        data[0, 0] = '*';
                        data[1, 0] = '*';
                        data[2, 0] = '*';
                        break;
                    case 2:
                        data[0, 1] = '*';
                        data[0, 0] = '*';
                        data[1, 0] = '*';
                        data[2, 0] = '*';
                        break;
                    case 3:
                        data[0, 2] = '*';
                        data[0, 1] = '*';
                        data[0, 0] = '*';
                        data[1, 0] = '*';
                        data[2, 0] = '*';
                        break;
                    case 4:
                        data[0, 0] = '*';
                        data[1, 0] = '*';
                        data[2, 0] = '*';
                        data[3, 0] = '*';
                        break;
                }

                return data;
            }
        }
    }
}
