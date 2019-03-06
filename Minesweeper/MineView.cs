using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Minesweeper
{
    class MineView
    {
        //returns a TableLayoutPanel to select the size of the game
        public TableLayoutPanel ChooseSize()
        {

            TableLayoutPanel parent = new TableLayoutPanel();
            parent.Dock = DockStyle.Fill;
            parent.ColumnCount = 3;
            parent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            parent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            parent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            parent.RowCount = 1;
            parent.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Button small = new Button();
            small.Dock = DockStyle.Fill;
            small.Text = "Small";

            Button medium = new Button();
            medium.Dock = DockStyle.Fill;
            medium.Text = "Medium";

            Button large = new Button();
            large.Dock = DockStyle.Fill;
            large.Text = "Large";

            parent.Controls.Add(small);
            parent.Controls.Add(medium);
            parent.Controls.Add(large);

            return parent;

        }

        //returns a TableLayoutPanel to select the difficulty of the game
        public TableLayoutPanel ChooseDif()
        {

            TableLayoutPanel parent = new TableLayoutPanel();
            parent.Dock = DockStyle.Fill;
            parent.ColumnCount = 3;
            parent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            parent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            parent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            parent.RowCount = 1;
            parent.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Button easy = new Button();
            easy.Dock = DockStyle.Fill;
            easy.Text = "Easy";

            Button medium = new Button();
            medium.Dock = DockStyle.Fill;
            medium.Text = "Medium";

            Button hard = new Button();
            hard.Dock = DockStyle.Fill;
            hard.Text = "Hard";

            parent.Controls.Add(easy);
            parent.Controls.Add(medium);
            parent.Controls.Add(hard);

            return parent;

        }

        //returns a TableLayoutPanel generated from a 2d array of MineButtons
        public TableLayoutPanel GameRegion(MineButton [,] buttons)
        {
            int size = buttons.GetUpperBound(0);
            TableLayoutPanel parent = new TableLayoutPanel();
            parent.Dock = DockStyle.Fill;
            parent.ColumnCount = size + 2;
            parent.RowCount = size + 2;

            for (int i = 0; i < size + 1; i++) parent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / (float)size));
            for (int i = 0; i < size + 1; i++) parent.RowStyles.Add(new RowStyle(SizeType.Percent, 100 / (float)size));

            for(int row = 0; row < size + 1; row++)
            {
                for(int col = 0; col < size + 1; col++)
                {
                    buttons[row, col].Dock = DockStyle.Fill;
                    buttons[row, col].BackColor = Color.White;
                    parent.Controls.Add(buttons[row, col], row, col);
                }
            }

            return parent;

        }

        //colors a button based on its Status
        public void ColorButton(ref MineButton button)
        {
         
            button.BackColor = Color.LightGray;
            switch (button.Numadjacent) {
                case 0: break;
                case 1: {
                        button.Text = button.Numadjacent.ToString();
                        button.ForeColor = Color.Blue;
                        break;
                    }
                case 2:
                    {
                        button.Text = button.Numadjacent.ToString();
                        button.ForeColor = Color.Green;
                        break;
                    }
                case 3:
                    {
                        button.Text = button.Numadjacent.ToString();
                        button.ForeColor = Color.Red;
                        break;
                    }
                case 4:
                    {
                        button.Text = button.Numadjacent.ToString();
                        button.ForeColor = Color.Purple;
                        break;
                    }
                case 5:
                    {
                        button.Text = button.Numadjacent.ToString();
                        button.ForeColor = Color.Black;
                        break;
                    }
                case 6:
                    {
                        button.Text = button.Numadjacent.ToString();
                        button.ForeColor = Color.Maroon;
                        break;
                    }
                case 7:
                    {
                        button.Text = button.Numadjacent.ToString();
                        button.ForeColor = Color.Gray;
                        break;
                    }
                case 8:
                    {
                        button.Text = button.Numadjacent.ToString();
                        button.ForeColor = Color.Turquoise;
                        break;
                    }
            }

        }

        //marks all mines with an X
        public void SetFail(ref MineModel board)
        {

            for (int row = 0; row < board.Size; row++)
            {
                for (int col = 0; col < board.Size; col++)
                {
                    if (board.Buttons[row, col].isMine)
                    {
                        board.Buttons[row, col].Text = "X";
                    }
                }
            }
        }
    }
}
