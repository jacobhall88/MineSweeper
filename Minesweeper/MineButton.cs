using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    enum Status { Clicked, Marked, Clear}
    class MineButton : Button
    {
        public int row;
        public int col;

        //number of adjacent buttons that contain a mine
        public int Numadjacent { get; set; } = 0;

        public Status State { get; set; }
        public bool isMine;
        public MineButton left;
        public MineButton right;
        public MineButton up;
        public MineButton down;
        public MineButton upleft;
        public MineButton upright;
        public MineButton downleft;
        public MineButton downright;

        public MineButton(int ro, int co, bool mine)
        {
            row = ro; col = co; isMine = mine; State = Status.Clear;
        }

    }
}
