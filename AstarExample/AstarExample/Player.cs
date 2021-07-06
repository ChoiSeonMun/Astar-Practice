using System;
using System.Collections.Generic;
using System.Text;

namespace AstarExample
{
    class Player
    {
        public int PosR { get; private set; }
        public int PosC { get; private set; }

        struct Pos
        {
            public int R;
            public int C;

            public Pos(int r, int c)
            {
                R = r;
                C = c;
            }
        }

        Board _board;
      
        public void Initialize(int posR, int posC, Board board)
        {
            PosR = posR;
            PosC = posC;

            _board = board;

            findPathByAStar();
        }

        class PQNode : IComparable<PQNode>
        {
            public int F { get; set; }
            public int G { get; set; }
            public int R { get; set; }
            public int C { get; set; }

            public int CompareTo(PQNode other)
            {
                if (F == other.F)
                {
                    return 0;
                }
                else if (F < other.F)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
        }

        List<Pos> _postions = new List<Pos>();
        private bool[,] _closed;
        private int[,] _opened;
        Pos[,] _path;
        PriorityQueue<PQNode> _q = new PriorityQueue<PQNode>();

        private void findPathByAStar()
        {
            _closed = new bool[_board.Size, _board.Size];
            _opened = new int[_board.Size, _board.Size];
            for (int r = 0; r < _board.Size; ++r)
            {
                for (int c = 0; c < _board.Size; ++c)
                {
                    _opened[r, c] = Int32.MaxValue;
                }
            }
            _path = new Pos[_board.Size, _board.Size];
            _q.Clear();

            Pos initialPos = new Pos(PosR, PosC);
            AddPath(initialPos, initialPos, 0, calcHueristic(PosR, PosC));

            // U R D L UR DR DL UL
            int[] dr = { -1, 0, 1, 0, -1, 1, 1, -1 };
            int[] dc = { 0, 1, 0, -1, 1, 1, -1, -1 };
            int[] cost = { 10, 10, 10, 10, 14, 14, 14, 14 };
            while (_q.Count > 0)
            {
                var node = _q.Pop();

                if (_closed[node.R, node.C])
                {
                    continue;
                }

                _closed[node.R, node.C] = true;

                if (node.R == _board.DestR && node.C == _board.DestC)
                {
                    break;
                }

                for (int i = 0; i < dr.Length; ++i)
                {
                    int newR = node.R + dr[i];
                    int newC = node.C + dc[i];

                    if (false == canMove(newR, newC))
                    {
                        continue;
                    }

                    if (_closed[newR, newC])
                    {
                        continue;
                    }

                    int g = node.G + cost[i];
                    int h = calcHueristic(newR, newC);

                    if (_opened[newR, newC] < g + h)
                    {
                        continue;
                    }

                    AddPath(new Pos(newR, newC), new Pos(node.R, node.C), g, h);
                }
            }

            Pos pos = new Pos(_board.DestR, _board.DestC);
            while (false == (pos.R == PosR && pos.C == PosC))
            {
                _postions.Add(pos);
                pos = _path[pos.R, pos.C];
            }
            _postions.Add(pos);

            _postions.Reverse();
        }

        private int calcHueristic(int r, int c) => 10 * Math.Abs(_board.DestR - r) + Math.Abs(_board.DestC - c);

        private bool canMove(int posR, int posC)
        {
            if (posR < 0 || posR >= _board.Size || posC < 0 || posC >= _board.Size)
            {
                return false;
            }

            if (_board.Tiles[posR, posC] != Board.TileType.Empty)
            {
                return false;
            }

            return true;
        }

        private void AddPath(Pos current, Pos prev, int g, int h)
        {
            _opened[current.R, current.C] = g + h;
            _q.Push(new PQNode() { F = g + h, G = g, R = current.R, C = current.C });
            _path[current.R, current.C] = prev;
        }

        static readonly int MOVE_TICK = 70;
        private int _tickSum = 0;
        private int _moveIndex = 0;

        public void Update(int deltaTime)
        {
            if (_moveIndex == _postions.Count)
            {
                _moveIndex = 0;
                _postions.Clear();
                _board.Initialize(_board.Size, this);
                Initialize(1, 1, _board);

                return;
            }

            _tickSum += deltaTime;
            if (_tickSum >= MOVE_TICK)
            {
                _tickSum = 0;

                Pos newPos = _postions[_moveIndex];
                PosR = newPos.R;
                PosC = newPos.C;

                ++_moveIndex;
            }
        }

        
    }
}
