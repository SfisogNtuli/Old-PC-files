//Sfiso Ntuli 2023
//Practice, fun, lets go!
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PracAss1
{
    enum Direction { Up = 0, Right = 1, Down = 2, Left = 3} // enum Direction {Up, Right, Down, Left}
    public partial class CfrmMain : Form
    {
        List<Point> snake;
        int iTime = 0;
        Random rnd = new Random();
        Direction direction;
        Graphics g;
        bool isAlive;
        

        public CfrmMain()
        {
            InitializeComponent();
            g = pnlSnake.CreateGraphics();
            btnPauseStart.Text = "Start";
            lblTime.Text = "Time elapsed: ";
            lblLength.Text = "Length of snake: ";
            isAlive = false;
        }

        public void StartGame()
        {
            snake = new List<Point>(); // Create snake and add its head at a random location
            snake.Add(new Point(rnd.Next(0, 20), rnd.Next(0, 20)));
            direction = (Direction)rnd.Next(0, 4);
            tmrSnake.Start();
            btnPauseStart.Text = "Pause";
            isAlive = true;
            iTime = 0;
        }

        public void MoveSnake()
        {
            Point ptHead;
            // Determine the new position of the snake's head, using periodic boundary conditions
            switch (direction)
            {
                case Direction.Left: ptHead = new Point((snake[0].X + 19) % 20, snake[0].Y); break;
                case Direction.Right: ptHead = new Point((snake[0].X + 1) % 20, snake[0].Y); break;
                case Direction.Down: ptHead = new Point(snake[0].X, (snake[0].Y + 1) % 20); break;
                default: ptHead = new Point(snake[0].X, (snake[0].Y + 19) % 20); break;
            }
            // Determine if the snake is alive
            if (!snake.Any(i => i.X == ptHead.X && i.Y == ptHead.Y))
            {
                snake.Insert(0, ptHead); // Add the snake's head
                if (iTime % 10 != 0) // If the snake is not growing
                    snake.RemoveAt(snake.Count - 1); // Remove the tail 
            }
            else
                EndGame();
        }

        public void EndGame()
        {
            tmrSnake.Stop();
            btnPauseStart.Text = "Start";
            MessageBox.Show("Game Over!");
            isAlive = false;
        }
        public void DrawSnake()
        {
            g.Clear(Color.White);
            // Draw the head
            g.FillRectangle(Brushes.Blue, snake[0].X * 20, snake[0].Y * 20, 20, 20);
            g.DrawRectangle(Pens.Black, snake[0].X * 20, snake[0].Y * 20, 20, 20);
            // Draw the rest of the body
            for (int i = 1; i < snake.Count; i++)
            {
                g.FillRectangle(Brushes.Red, snake[i].X * 20, snake[i].Y * 20, 20, 20);
                g.DrawRectangle(Pens.Black, snake[i].X * 20, snake[i].Y * 20, 20, 20);
            }
        }

        private void tmrSnake_Tick(object sender, EventArgs e)
        {
            iTime++;
            lblLength.Text = "Length of snake: " + snake.Count.ToString();
            lblTime.Text = "Time elapsed: " + (iTime / 10.0).ToString("F1") + "s";
            MoveSnake();
            DrawSnake();
        }

        private void CfrmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.W || e.KeyCode == Keys.Up) && direction != Direction.Down)
                direction = Direction.Up;
            else if ((e.KeyCode == Keys.S || e.KeyCode == Keys.Down) && direction != Direction.Up)
                direction = Direction.Down;
            else if ((e.KeyCode == Keys.A || e.KeyCode == Keys.Left) && direction != Direction.Right)
                direction = Direction.Left;
            else if ((e.KeyCode == Keys.D || e.KeyCode == Keys.Right) && direction != Direction.Left)
                direction = Direction.Right;
        }

        private void btnPauseStart_Click(object sender, EventArgs e)
        {
            if (!isAlive)
                StartGame();
            else
            {
                tmrSnake.Enabled = !tmrSnake.Enabled;
                btnPauseStart.Text = tmrSnake.Enabled ? "Pause" : "Resume";
            }
        }
    }
}