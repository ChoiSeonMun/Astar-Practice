using System;

namespace AstarExample
{
    class Program
    {
        static readonly int WAIT_TICK = 1000 / 30;

        static void Main(string[] args)
        {
            Board board = new Board();
            Player player = new Player();
            board.Initialize(25, player);
            player.Initialize(1, 1, board);

            Console.CursorVisible = false;

            int lastTick = 0;
            while (true)
            {
                #region Frame Management
                int currentTick = Environment.TickCount;
                int deltaTick = currentTick - lastTick;
                if (deltaTick < WAIT_TICK)
                {
                    continue;
                }

                lastTick = currentTick;
                #endregion

                #region Update
                #endregion

                #region Render
                Console.SetCursorPosition(0, 0);

                board.Render();
                #endregion
            }
        }
    }
}
