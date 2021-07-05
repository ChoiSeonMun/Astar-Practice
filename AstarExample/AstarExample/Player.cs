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
        }
    }
}
