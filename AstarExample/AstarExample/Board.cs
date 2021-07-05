using System;
using System.Collections.Generic;
using System.Text;

namespace AstarExample
{
    class Board
    {
        private readonly char CIRCLE = '\u25cf';

        public enum TileType
        {
            Empty,
            Wall
        }

        public TileType[,] Tiles { get; private set; }
        public int Size { get; private set; }
        public int DestR { get; private set; }
        public int DestC { get; private set; }

        private Random _rand = new Random();
        private Player _player;
        public void Initialize(int size, Player player)
        {
            Tiles = new TileType[size, size];
            Size = size;
            DestR = Size - 2;
            DestC = size - 2;

            _player = player;

            int prop = _rand.Next(0, 2);
            if (prop == 0)
            {
                generateByBinaryTree();
            }
            else
            {
                generateBySideWinder();
            }
        }

        public void Render()
        {
            ConsoleColor prevColor = Console.ForegroundColor;
            for (int r = 0; r < Size; ++r)
            {
                for (int c = 0; c < Size; ++c)
                {
                    if (r == _player.PosR && c == _player.PosC)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else if (r == DestR && c == DestC)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else
                    {
                        Console.ForegroundColor = getTileColor(Tiles[r, c]);
                    }
                    Console.Write(CIRCLE);
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = prevColor;
        }

        private ConsoleColor getTileColor(TileType tile)
        {
            switch (tile)
            {
                case TileType.Empty:
                    return ConsoleColor.Green;
                case TileType.Wall:
                    return ConsoleColor.Red;
                default:
                    return ConsoleColor.White;
            }
        }

        private void generateByBinaryTree()
        {
            for (int r = 0; r < Size; ++r)
            {
                for (int c = 0; c < Size; ++c)
                {
                    if (r % 2 == 0 || c % 2 == 0)
                    {
                        Tiles[r, c] = TileType.Wall;
                    }
                }
            }

            for (int r = 1; r < Size - 1; ++r)
            {
                for (int c = 1; c < Size - 1; ++c)
                {
                    if (r % 2 == 0 || c % 2 == 0)
                    {
                        continue;
                    }

                    if (r == Size - 2 && c == Size - 2)
                    {
                        continue;
                    }

                    if (r == Size - 2)
                    {
                        Tiles[r, c + 1] = TileType.Empty;
                    }
                    else if (c == Size - 2)
                    {
                        Tiles[r + 1, c] = TileType.Empty;
                    }
                    else if (0 == _rand.Next(0, 2))
                    {
                        Tiles[r, c + 1] = TileType.Empty;
                    }
                    else
                    {
                        Tiles[r + 1, c] = TileType.Empty;
                    }
                }
            }
        }

        private void generateBySideWinder()
        {
            for (int r = 0; r < Size; ++r)
            {
                for (int c = 0; c < Size; ++c)
                {
                    if (r % 2 == 0 || c % 2 == 0)
                    {
                        Tiles[r, c] = TileType.Wall;
                    }
                }
            }

            for (int r = 1; r < Size - 1; ++r)
            {
                int count = 1;
                for (int c = 1; c < Size - 1; ++c)
                {
                    if (r % 2 == 0 || c % 2 == 0)
                    {
                        continue;
                    }

                    if (r == Size - 2 && c == Size - 2)
                    {
                        continue;
                    }

                    if (r == Size - 2)
                    {
                        Tiles[r, c + 1] = TileType.Empty;
                        continue;
                    }

                    if (c == Size - 2)
                    {
                        Tiles[r + 1, c] = TileType.Empty;
                        continue;
                    }

                    if (0 == _rand.Next(0, 2))
                    {
                        Tiles[r, c + 1] = TileType.Empty;
                        ++count;
                    }
                    else
                    {
                        int randomIndex = _rand.Next(0, count);
                        Tiles[r + 1, c - randomIndex * 2] = TileType.Empty;

                        count = 1;
                    }
                }
            }
        }
    }
}
