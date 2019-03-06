using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Minesweeper
{
    class MineController
    {
        private const int SMALLSIZE = 8;
        private const int MEDSIZE = 12;
        private const int LARGESIZE = 18;

        private const int EASYFREQ = 5;
        private const int MEDFREQ = 9;
        private const int HARDFREQ = 12;

        private int size = SMALLSIZE;
        private int freq = EASYFREQ;
        private MineModel board;
        private MineView view;
        private bool fail;
        private Button smiley;
        private Button mineCount;

        public MineController()
        {
            RunGame();
        }

        //primary game loop
        private void RunGame()
        {
            //build and display dialogue to choose board size
            fail = false;
            view = new MineView();
            Form sizeForm = new Form();
            TableLayoutPanel sizePanel = view.ChooseSize();
            foreach (Control c in sizePanel.Controls) c.Click += new EventHandler(SizeClick);
            sizeForm.Size = new Size(300, 100);
            sizeForm.Controls.Add(sizePanel);
            sizeForm.ShowDialog();
            sizeForm.Dispose();

            //build and display dialogue to choose difficulty
            Form difForm = new Form();
            TableLayoutPanel difPanel = view.ChooseDif();
            foreach (Control c in difPanel.Controls) c.Click += new EventHandler(DifClick);
            difForm.Size = new Size(300, 100);
            difForm.Controls.Add(difPanel);
            difForm.ShowDialog();
            difForm.Dispose();

            //build and display main game board
            board = new MineModel(size, freq);
            Form gameBoard = new Form();

            //create the parent region to hold the rest of the game controls
            TableLayoutPanel parent = new TableLayoutPanel();
            parent.Dock = DockStyle.Fill;
            parent.RowCount = 2;
            parent.ColumnCount = 3;
            parent.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
            parent.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            parent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            parent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            parent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));

            //create and register the new game button
            Button newGame = new Button();
            newGame.Dock = DockStyle.Fill;
            newGame.Text = "New Game";
            newGame.Click += new EventHandler(NewClick);
            parent.Controls.Add(newGame, 0, 0);

            //create and register smiley face
            smiley = new Button();
            smiley.Dock = DockStyle.Fill;
            smiley.Font = new Font(smiley.Font.FontFamily, 25);
            smiley.Text = ":D";
            parent.Controls.Add(smiley, 1, 0);

            //create and register the number of mines display
            mineCount = new Button();
            mineCount.Dock = DockStyle.Fill;
            mineCount.Text = board.Nummines.ToString();
            parent.Controls.Add(mineCount, 2, 0);

            //create and add the panel with the mine buttons, and add their event listeners
            TableLayoutPanel gamePanel = view.GameRegion(board.Buttons);
            foreach (Control c in gamePanel.Controls)
            {
                c.Click += new EventHandler(GameClick);
                c.MouseUp += new MouseEventHandler(GameRightClick);
            }

            //finalize appearance and display form
            gameBoard.Size = new Size(600, 600);
            parent.SetColumnSpan(gamePanel, 3);
            parent.Controls.Add(gamePanel, 0, 1);
            gameBoard.Controls.Add(parent);
            gameBoard.ShowDialog();

        }

        //event handler to choose size, refers to constants SMALLSIZE, MEDSIZE and LARGESIZE
        private void SizeClick(object e, EventArgs args)
        {
            Button chooseButton = (Button)e;
            if (chooseButton.Text == "Small")
            {
                size = SMALLSIZE;
            }
            else if (chooseButton.Text == "Medium")
            {
                size = MEDSIZE;
            }
            else if (chooseButton.Text == "Large")
            {
                size = LARGESIZE;
            }

            Form parent = (Form)chooseButton.Parent.Parent;
            parent.Close();
        }

        //event handler to choose frequency of mines (difficulty), refers to constants EASYFREG, MEDFREQ and HARDFREQ
        private void DifClick(object e, EventArgs args)
        {
            Button chooseButton = (Button)e;
            if (chooseButton.Text == "Easy")
            {
                freq = EASYFREQ;
            }
            else if (chooseButton.Text == "Medium")
            {
                freq = MEDFREQ;
            }
            else if (chooseButton.Text == "Hard")
            {
                freq = HARDFREQ;
            }

            Form parent = (Form)chooseButton.Parent.Parent;
            parent.Close();
        }

        //event handler that manages when a MineButton is left clicked
        private void GameClick(object e, EventArgs args)
        {
            if (!fail)
            {
                MineButton gameButton = (MineButton)e;

                //change the button to clicked
                gameButton.State = Status.Clicked;
                gameButton.Click -= GameClick;
                gameButton.MouseUp -= GameRightClick;
                view.ColorButton(ref gameButton);

                //if a mine is clicked, set failure state
                if (gameButton.isMine)
                {
                    smiley.Text = "X.X";
                    view.SetFail(ref board);
                    fail = true;
                }

                //if clicked button is not adjacent to a mine, recurse the click in all valid directions
                else
                {

                    if (gameButton.Numadjacent == 0)
                    {
                        if (gameButton.up != null)
                            if (gameButton.up.State != Status.Clicked) GameClickedRecurse(gameButton.up);
                        if (gameButton.down != null)
                            if (gameButton.down.State != Status.Clicked) GameClickedRecurse(gameButton.down);
                        if (gameButton.left != null)
                            if (gameButton.left.State != Status.Clicked) GameClickedRecurse(gameButton.left);
                        if (gameButton.right != null)
                            if (gameButton.right.State != Status.Clicked) GameClickedRecurse(gameButton.right);
                        if (gameButton.upleft != null)
                            if (gameButton.upleft.State != Status.Clicked) GameClickedRecurse(gameButton.upleft);
                        if (gameButton.upright != null)
                            if (gameButton.upright.State != Status.Clicked) GameClickedRecurse(gameButton.upright);
                        if (gameButton.downleft != null)
                            if (gameButton.downleft.State != Status.Clicked) GameClickedRecurse(gameButton.downleft);
                        if (gameButton.downright != null)
                            if (gameButton.downright.State != Status.Clicked) GameClickedRecurse(gameButton.downright);
                    }
                }
                


            }
        }

        //event handler that manages when a MineButton is right clicked
        private void GameRightClick(object e, MouseEventArgs args)
        {
            MineButton gameButton = (MineButton)e;
            if(args.Button == MouseButtons.Right)
            {
                //toggle whether or not the right clicked MineButton has the Marked status,
                //and increment or decrement the mine count display
                int val = int.Parse(mineCount.Text);
                if (gameButton.State == Status.Clear)
                {
                    val--;
                    gameButton.Text = "?";
                    gameButton.ForeColor = Color.Red;
                    gameButton.State = Status.Marked;
                    mineCount.Text = val.ToString();
                }
                else
                {
                    val++;
                    gameButton.Text = "";
                    gameButton.State = Status.Clear;
                    mineCount.Text = val.ToString();
                }
            }
        }

        //event handler that manages when New Game is clicked
        private void NewClick(object e, EventArgs args)
        {
            //dispose of the current game and start a new game
            Button newButton = (Button)e;
            Form parent = (Form)newButton.Parent.Parent;
            parent.Dispose();
            RunGame();
        }

        //handles the recursion of a MineButton with no adjacent mines being clicked
        private void GameClickedRecurse(MineButton gameButton)
        {
            //functionally identical to the GameClick method, minus event handling and checking for fail state
            gameButton.State = Status.Clicked;
            gameButton.Click -= GameClick;
            gameButton.MouseUp -= GameRightClick;
            view.ColorButton(ref gameButton);
            if (gameButton.Numadjacent == 0 && !gameButton.isMine)
            {
                if (gameButton.up != null)
                    if (gameButton.up.State != Status.Clicked) GameClickedRecurse(gameButton.up);
                if (gameButton.down != null)
                    if (gameButton.down.State != Status.Clicked) GameClickedRecurse(gameButton.down);
                if (gameButton.left != null)
                    if (gameButton.left.State != Status.Clicked) GameClickedRecurse(gameButton.left);
                if (gameButton.right != null)
                    if (gameButton.right.State != Status.Clicked) GameClickedRecurse(gameButton.right);
                if (gameButton.upleft != null)
                    if (gameButton.upleft.State != Status.Clicked) GameClickedRecurse(gameButton.upleft);
                if (gameButton.upright != null)
                    if (gameButton.upright.State != Status.Clicked) GameClickedRecurse(gameButton.upright);
                if (gameButton.downleft != null)
                    if (gameButton.downleft.State != Status.Clicked) GameClickedRecurse(gameButton.downleft);
                if (gameButton.downright != null)
                    if (gameButton.downright.State != Status.Clicked) GameClickedRecurse(gameButton.downright);
            }
            
        }
    }
}
