using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    class MineModel
    {
        public MineButton[,] Buttons { get; set; }
        public int Nummines { get; set; }
        const int THRESHOLD = 1;
        public int Size { get; set; }


        public MineModel (int s, int f)
        {
            Random rando = new Random();
            Buttons = new MineButton[s, s];
            Size = s;

            //build each MineButton, link them together, and place into buttons array
            //repeat until number of mines is within THRESHOLD of expected
            do
            {
                Nummines = 0;
                for (int row = 0; row < s; row++)
                {
                    for (int col = 0; col < s; col++)
                    {
                        bool hasMine = false;
                        if (rando.Next(0, 99) < f)
                        {
                            hasMine = true;
                            Nummines++;
                        }
                        MineButton newButton = new MineButton(row, col, hasMine);

                        if (row > 0)
                        {
                            newButton.up = Buttons[row - 1, col];
                            Buttons[row - 1, col].down = newButton;
                        }
                        if (col > 0)
                        {
                            newButton.left = Buttons[row, col - 1];
                            Buttons[row, col - 1].right = newButton;
                        }
                        if (row > 0 && col > 0)
                        {
                            newButton.upleft = Buttons[row - 1, col - 1];
                            Buttons[row - 1, col - 1].downright = newButton;
                        }
                        if (row > 0 && col < s - 1)
                        {
                            newButton.upright = Buttons[row - 1, col + 1];
                            Buttons[row - 1, col + 1].downleft = newButton;
                        }

                        Buttons[row, col] = newButton;
                    }
                }
            } while (!(s * s * (float) f/100 - Nummines > -THRESHOLD && s * s * (float) f/100 - Nummines < THRESHOLD));
            //iterate through each MineButton and increment numadjacent for each adjacent entry that isMine == true
            for (int row = 0; row < s; row++)
            {
                for (int col = 0; col < s; col++)
                {
                    if (row > 0)
                    {
                        if (Buttons[row, col].up.isMine == true) Buttons[row, col].Numadjacent++;
                        if(col > 0)
                        {
                            if (Buttons[row, col].upleft.isMine == true) Buttons[row, col].Numadjacent++;
                        }
                        if(col < s - 1)
                        {
                            if (Buttons[row, col].upright.isMine == true) Buttons[row, col].Numadjacent++;
                        }
                    }
                    if (row < s - 1)
                    {
                        if (Buttons[row, col].down.isMine == true) Buttons[row, col].Numadjacent++;
                        if(col > 0)
                        {
                            if (Buttons[row, col].downleft.isMine == true) Buttons[row, col].Numadjacent++;
                        }
                        if(col < s - 1)
                        {
                            if (Buttons[row, col].downright.isMine == true) Buttons[row, col].Numadjacent++;
                        }
                    }
                    if (col > 0)
                    {
                        if (Buttons[row, col].left.isMine == true) Buttons[row, col].Numadjacent++;
                    }
                    if(col < s - 1)
                    {
                        if (Buttons[row, col].right.isMine == true) Buttons[row, col].Numadjacent++;
                    }
                }
            }
        }

        //returns a boolean array, where true means that a corresponding MineButton in the Buttons array has a mine
        public bool[,] GetMines()
        {
            bool[,] ret = new bool[Size, Size];

            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    if (Buttons[row, col].isMine) ret[row, col] = true;
                }
            }

            return ret;
        }

    }

}
