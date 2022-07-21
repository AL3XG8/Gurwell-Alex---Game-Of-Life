using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gurwell__Alex___Game_Of_Life
{
    public partial class Form1 : Form
    {
        // The universe array
        private static int universeHeight = 40;
        private static int universeWidth = 40;
        bool[,] universe = new bool[universeWidth, universeHeight];
        bool[,] scratchPad = new bool[universeWidth, universeHeight];
        bool countNeighbors = true;

        // Current Seed
        int seed = 0;

        //Copy entire collumns instead of individual cells
        //void ResizeArray<T>(ref T[,] original, int newCoNum, int newRoNum)
        //{
        //    T[,] newArray = new T[newCoNum, newRoNum];
        //    int columnCount = original.GetLength(1);
        //    int columnCount2 = newRoNum;
        //    int columns = original.GetUpperBound(0);
        //    if (columnCount < newCoNum)
        //    {
        //        for (int co = 0; co <= columns; co++)
        //        {
        //            Array.Copy(original, co * columnCount, newArray, co * columnCount2, columnCount);
        //        }
        //    }
        //    else
        //    {
        //        for (int co = 0; co >= columns; co++)
        //        {
        //            Array.Copy(original, co * columnCount, newArray, co * columnCount2, columnCount);
        //        }
        //    }
        //    original = newArray;
        //}

        // (Slower) Individual cell copy
        //
        void ResizeArray<T>(ref T[,] original, int m, int n)
        {

            T[,] newArray = new T[m, n];
            int mMin = Math.Min(original.GetLength(0), newArray.GetLength(0));
            int nMin = Math.Min(original.GetLength(1), newArray.GetLength(1));

            for (int i = 0; i < mMin; i++)
                Array.Copy(original, i * original.GetLength(1), newArray, i * newArray.GetLength(1), nMin);

            original = newArray;
        }

        // Drawing colors
        Color gridColor = Color.Gray;
        Color tenGridColor = Color.White;
        Color cellColor = Color.Gray;

        // The Timer class
        Timer timer = new Timer();
        int interval = 100;

        // Generation count
        int generations = 0;

        //Cell count
        int countCells = 0;

        bool drawNeighbor = true;

        public Form1()
        {
            InitializeComponent();

            // Turn on double buffering.
            this.DoubleBuffered = true;
            // Allow repainting when the windows is resized.
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            graphicsPanel1.BackColor = Properties.Settings.Default.BackgroundColor;
            cellColor = Properties.Settings.Default.CellColor;
            gridColor = Properties.Settings.Default.GridColor;
            tenGridColor = Properties.Settings.Default.TenGridColor;

            //Resizing arrays
            universeWidth = Properties.Settings.Default.UniverseWidth;
            universeHeight = Properties.Settings.Default.UniverseHeight;
            ResizeArray(ref universe, universeWidth, universeHeight);
            ResizeArray(ref scratchPad, universeWidth, universeHeight);

            // Setup the timer
            interval = Properties.Settings.Default.Timer;
            timer.Interval = interval;
            timer.Tick += Timer_Tick;
            timer.Enabled = false; // start not running timer
        }

        // Calculate the next generation of cells
        private void NextGeneration()
        {
            ScratchPadClear();
            //CountCells();
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    int count = CountNeighbors(x, y);

                    //Apply Rules
                    //Turn it on/off in scratchPad and swap with universe's bool array;

                    if (universe[x, y])
                    {
                        if ((count < 2) || (count > 3))
                        {
                            scratchPad[x, y] = false;
                        }
                        else
                        {
                            scratchPad[x, y] = true;
                            countCells++;
                        }
                    }
                    else
                    {
                        if (count == 3)
                        {
                            scratchPad[x, y] = true;
                            countCells++;
                        }
                    }
                }
            }

            bool[,] temp = universe;
            universe = scratchPad;
            scratchPad = temp;
            graphicsPanel1.Invalidate();

            // Increment generation count
            generations++;
        }

        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);

            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 1);
            Pen tenGridPen = new Pen(tenGridColor, 3);

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);

            Font font = new Font("Arial", cellHeight - 8);
            StringFormat stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            Rectangle cellRect = Rectangle.Empty;
            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    int count = CountNeighbors(x, y);

                    // A rectangle to represent each cell in pixels
                    cellRect.X = x * cellWidth;
                    cellRect.Y = y * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;

                    // Fill the cell with a brush if alive
                    if (universe[x, y] == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRect);
                        if (CountNeighbors(x, y) != 0)
                        {
                            if (drawNeighbor)
                            {
                                e.Graphics.DrawString(count.ToString(), font, Brushes.Black, cellRect, stringFormat);
                            }
                        }
                    }
                    else if (CountNeighbors(x, y) != 0)
                    {
                        if (drawNeighbor)
                        {
                            e.Graphics.DrawString(count.ToString(), font, Brushes.White, cellRect, stringFormat);
                        }
                    }

                    // Outline the cell with a pen
                    if (gridToolStripMenuItem.Checked)
                    {
                        e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                    }
                    // If the 10x10 grid is enabled, draw
                    if (x10GridToolStripMenuItem.Checked)
                    {

                        // Draw 10x10 Horizontal - Does not draw if the y is 0 (at the top of the screen)
                        if (y % 10 == 0 && y != 0)
                        {
                            e.Graphics.DrawRectangle(tenGridPen, cellRect.X, cellRect.Y, cellRect.Width, 1);
                        }

                        // Draw 10x10 Vertical - Does not draw if the x is 0 (at the left of the screen)
                        if (x % 10 == 0 && x != 0)
                        {
                            e.Graphics.DrawRectangle(tenGridPen, cellRect.X, cellRect.Y, 1, cellRect.Height);
                        }
                    }

                    // Bottom panel text(s)
                    toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
                    CellsAlive.Text = "Alive = " + countCells.ToString();
                    IntervalTimer.Text = "Interval = " + interval.ToString();
                }
            }

            if (hUDToolStripMenuItem.Checked)
            {
                // HUD String Setup
                StringFormat hudStringFormat = new StringFormat();

                // HUD Font Setip
                Font hudFont = new Font("Open Sans", 15f);

                // Draw HUD Rectangle
                Rectangle hudRect = new Rectangle(0, graphicsPanel1.ClientSize.Height - 92, 256, 256);


                // Draw HUD Data
                // Current Generation
                e.Graphics.DrawString("Generation: " + generations.ToString(), hudFont, Brushes.Red, hudRect, hudStringFormat);

                // Alive Cell Count
                e.Graphics.DrawString("\nAlive Cells: " + countCells.ToString(), hudFont, Brushes.Red, hudRect, hudStringFormat);

                // Boundry Type
                string BoundryType = toroidalToolStripMenuItem.Checked ? "Toroidal" : "Finite";
                e.Graphics.DrawString("\n\nBoundry Type: " + BoundryType, hudFont, Brushes.Red, hudRect, hudStringFormat);

                // Universe Size
                e.Graphics.DrawString("\n\n\nUniverse Size: " + universeWidth.ToString() + "x" + universeHeight.ToString(), hudFont, Brushes.Red, hudRect, hudStringFormat);
            }

            // Cleaning up pens and brushes
            gridPen.Dispose();
            tenGridPen.Dispose();
            cellBrush.Dispose();
        }

        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the width and height of each cell in pixels
                int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
                int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                int x = e.X / cellWidth;
                // CELL Y = MOUSE Y / CELL HEIGHT
                int y = e.Y / cellHeight;

                // Toggle the cell's state
                universe[x, y] = !universe[x, y];

                if (universe[x, y] == true)
                {
                    countCells++;
                }
                else
                {
                    countCells--;
                }

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }

        private void ToolStripButton2_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }
        private void ToolStripButton3_Click(object sender, EventArgs e)
        {
            NextGeneration();
        }
        private int CountNeighbors(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);

            if (!countNeighbors)
            {
                for (int yOffset = -1; yOffset <= 1; yOffset++)
                {
                    for (int xOffset = -1; xOffset <= 1; xOffset++)
                    {
                        int xCheck = x + xOffset;
                        int yCheck = y + yOffset;

                        if (xOffset == 0 && yOffset == 0)
                        {
                            continue;
                        }
                        if (xCheck < 0)
                        {
                            continue;
                        }
                        if (yCheck < 0)
                        {
                            continue;
                        }
                        if (xCheck >= xLen)
                        {
                            continue;
                        }
                        if (yCheck >= yLen)
                        {
                            continue;
                        }
                        if (universe[xCheck, yCheck] == true)
                        {
                            count++;
                        }
                    }
                }
            }
            else
            {
                for (int yOffset = -1; yOffset <= 1; yOffset++)
                {
                    for (int xOffset = -1; xOffset <= 1; xOffset++)
                    {
                        int xCheck = x + xOffset;
                        int yCheck = y + yOffset;

                        if (xOffset == 0 && yOffset == 0)
                        {
                            continue;
                        }
                        if (xCheck < 0)
                        {
                            xCheck = xLen - 1;
                        }
                        if (yCheck < 0)
                        {
                            yCheck = yLen - 1;
                        }
                        if (xCheck >= xLen)
                        {
                            xCheck = 0;
                        }
                        if (yCheck >= yLen)
                        {
                            yCheck = 0;
                        }
                        if (universe[xCheck, yCheck] == true)
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }

        private void ScratchPadClear()
        {
            countCells = 0;

            for (int y = 0; y < scratchPad.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < scratchPad.GetLength(0); x++)
                {
                    scratchPad[x, y] = false;
                }
            }

            graphicsPanel1.Invalidate();
        }
        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            generations = 0;
            countCells = 0;
            timer.Enabled = false;

            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x, y] = false;
                }
            }

            graphicsPanel1.Invalidate();
        }

        private void BackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = graphicsPanel1.BackColor;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                graphicsPanel1.BackColor = dlg.Color;

                if (dlg.Color == Color.Black && gridColor == Color.Black)
                {
                    gridColor = Color.White;
                }
                else if (dlg.Color == Color.White && gridColor == Color.White)
                {
                    gridColor = Color.Black;
                }
            }
        }

        private void CellColorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog
            {
                Color = cellColor
            };

            if (DialogResult.OK == dlg.ShowDialog())
            {
                cellColor = dlg.Color;
            }
            graphicsPanel1.Invalidate();
        }

        private void GridColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog
            {
                Color = gridColor
            };

            if (DialogResult.OK == dlg.ShowDialog())
            {
                gridColor = dlg.Color;
            }
            graphicsPanel1.Invalidate();
        }
        private void TenGridColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog
            {
                Color = tenGridColor
            };

            if (DialogResult.OK == dlg.ShowDialog())
            {
                tenGridColor = dlg.Color;
            }
            graphicsPanel1.Invalidate();
        }
        private void optionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Options dlg = new Options();
            dlg.Interval = timer.Interval;
            dlg.XCell = universeWidth;
            dlg.YCell = universeHeight;

            //Show Dialog Box to User & Determine Result
            if (DialogResult.OK == dlg.ShowDialog())
            {

                //    Set New Interval
                interval = dlg.Interval;
                timer.Interval = interval;


                //Only reset universe size if size has changed
                if (universeWidth != dlg.XCell || universeHeight != dlg.YCell)
                {
                    //Set new Universe Sizes
                    universeWidth = dlg.XCell;
                    universeHeight = dlg.YCell;

                    //Create new Univese
                    ResizeArray(ref universe, universeWidth, universeHeight);
                    ResizeArray(ref scratchPad, universeWidth, universeHeight);
                    for (int y = 0; y < universe.GetLength(1); y++)
                    {
                        // Iterate through the universe in the x, left to right
                        for (int x = 0; x < universe.GetLength(0); x++)
                        {
                            if (universe[x, y] == true && countCells == 0)
                            {
                                countCells++;
                            }
                        }
                    }
                }

                //Invalidate Graphics Panel
                graphicsPanel1.Invalidate();
            }
        }

        private void neighborCountToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (neighborCountToolStripMenuItem1.Checked == false)
            {
                drawNeighbor = false;
            }
            else
            {
                drawNeighbor = true;
            }

            graphicsPanel1.Invalidate();
        }

        private void toroidalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toroidalToolStripMenuItem.Checked = true;
            finiteToolStripMenuItem.Checked = false;
            countNeighbors = true;

            graphicsPanel1.Invalidate();
        }

        private void finiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toroidalToolStripMenuItem.Checked = false;
            finiteToolStripMenuItem.Checked = true;
            countNeighbors = false;

            graphicsPanel1.Invalidate();
        }
        private void Randomize()
        {
            Random rand = new Random(seed); // Init Rand Variable

            // Clear ScratchPad
            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x, y] = false;
                }
            }

            // Iterate through the universe in the x, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the y, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    int num = rand.Next(0, 2);

                    // if number is 0, turn cell on
                    if (num == 0) scratchPad[x, y] = true;
                }
            }

            // Copy scratchPad to existing universe by swapping
            bool[,] temp = universe;
            universe = scratchPad;
            scratchPad = temp;

            // Update Strip Info
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            CellsAlive.Text = "Alive = " + countCells.ToString();

            // Invalidate Graphics Panel
            graphicsPanel1.Invalidate();
        }

        private void ResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();

            interval = Properties.Settings.Default.Timer;
            timer.Interval = interval;
            graphicsPanel1.BackColor = Properties.Settings.Default.BackgroundColor;
            cellColor = Properties.Settings.Default.CellColor;
            gridColor = Properties.Settings.Default.GridColor;
            tenGridColor = Properties.Settings.Default.TenGridColor;

            //array size
            universeWidth = Properties.Settings.Default.UniverseWidth;
            universeHeight = Properties.Settings.Default.UniverseHeight;
            ResizeArray(ref universe, universeWidth, universeHeight);
            ResizeArray(ref scratchPad, universeWidth, universeHeight);


            graphicsPanel1.Invalidate();
        }

        private void ReloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();

            interval = Properties.Settings.Default.Timer;
            timer.Interval = interval;
            graphicsPanel1.BackColor = Properties.Settings.Default.BackgroundColor;
            cellColor = Properties.Settings.Default.CellColor;
            gridColor = Properties.Settings.Default.GridColor;
            tenGridColor = Properties.Settings.Default.TenGridColor;

            //array size
            universeWidth = Properties.Settings.Default.UniverseWidth;
            universeHeight = Properties.Settings.Default.UniverseHeight;
            ResizeArray(ref universe, universeWidth, universeHeight);
            ResizeArray(ref scratchPad, universeWidth, universeHeight);

            graphicsPanel1.Invalidate();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.BackgroundColor = graphicsPanel1.BackColor;
            Properties.Settings.Default.CellColor = cellColor;
            Properties.Settings.Default.GridColor = gridColor;
            Properties.Settings.Default.Timer = interval;
            Properties.Settings.Default.TenGridColor = tenGridColor;

            //Resizing arrays
            Properties.Settings.Default.UniverseWidth = universeWidth;
            Properties.Settings.Default.UniverseHeight = universeHeight;

            Properties.Settings.Default.Save();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Import();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        public void SaveFile()
        {
            SaveAs(); // Call SaveAs()
        }

        // Save As New File
        public void SaveAs()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2; dlg.DefaultExt = "cells";

            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamWriter writer = new StreamWriter(dlg.FileName);

                // Write any comments you want to include first.
                // Prefix all comment strings with an exclamation point.
                // Use WriteLine to write the strings to the file. 
                // It appends a CRLF for you.
                writer.WriteLine("!This File was Written at: " + DateTime.Now);

                // Iterate through the universe one row at a time.
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    // Create a string to represent the current row.
                    String currentRow = string.Empty;

                    // Iterate through the current row one cell at a time.
                    for (int x = 0; x < universe.GetLength(0); x++)
                    {
                        // If the universe[x,y] is alive then append 'O' (capital O)
                        // to the row string.
                        // Else if the universe[x,y] is dead then append '.' (period)
                        // to the row string.
                        if (universe[x, y]) currentRow += "O";
                        else currentRow += ".";
                    }

                    // Once the current row has been read through and the 
                    // string constructed then write it to the file using WriteLine.
                    writer.WriteLine(currentRow);
                }

                // After all rows and columns have been written then close the file.
                writer.Close();
            }
        }

        // Oprn Saved file
        public void OpenFile()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamReader reader = new StreamReader(dlg.FileName);

                // Create a couple variables to calculate the width and height
                // of the data in the file.
                int maxWidth = 0;
                int maxHeight = 0;

                // Iterate through the file once to get its size.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();


                    // If the row begins with '!' then it is a comment
                    // and should be ignored.
                    if (row.Substring(0, 1) == "!") continue;

                    // If the row is not a comment then it is a row of cells.
                    // Increment the maxHeight variable for each row read.
                    maxHeight++;

                    // Get the length of the current row string
                    // and adjust the maxWidth variable if necessary.
                    if (row.Length > maxWidth) maxWidth = row.Length;
                }

                // Resize the current universe and scratchPad
                // to the width and height of the file calculated above.
                universeWidth = maxWidth;
                universeHeight = maxHeight;

                // Create new Univese
                universe = new bool[universeWidth, universeHeight];
                scratchPad = new bool[universeWidth, universeHeight];

                // Reset the file pointer back to the beginning of the file.
                reader.BaseStream.Seek(0, SeekOrigin.Begin);

                // Iterate through the file again, this time reading in the cells.
                int yPos = 0;
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then
                    // it is a comment and should be ignored.
                    if (row.Substring(0, 1) == "!") continue;

                    // If the row is not a comment then 
                    // it is a row of cells and needs to be iterated through.
                    for (int xPos = 0; xPos < row.Length; xPos++)
                    {
                        // If row[xPos] is a 'O' (capital O) then
                        // set the corresponding cell in the universe to alive.
                        // If row[xPos] is a '.' (period) then
                        // set the corresponding cell in the universe to dead.
                        if (row[xPos] == 'O') universe[xPos, yPos] = true;
                    }
                    yPos++;
                }

                // Close the file.
                reader.Close();
            }

            // Update Strip Info
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            CellsAlive.Text = "Alive = " + countCells.ToString();

            // Invalidate Graphics Panel
            graphicsPanel1.Invalidate();
        }

        // Import File
        public void Import()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamReader reader = new StreamReader(dlg.FileName);

                // Create a couple variables to calculate the width and height
                // of the data in the file.
                int maxWidth = 0;
                int maxHeight = 0;

                // Iterate through the file once to get its size.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();


                    // If the row begins with '!' then it is a comment
                    // and should be ignored.
                    if (row.Substring(0, 1) == "!") continue;

                    // If the row is not a comment then it is a row of cells.
                    // Increment the maxHeight variable for each row read.
                    if (maxHeight < universeWidth) maxHeight++;

                    // Get the length of the current row string
                    // and adjust the maxWidth variable if necessary.
                    if (row.Length > maxWidth) maxWidth = row.Length;
                }

                // Reset the file pointer back to the beginning of the file.
                reader.BaseStream.Seek(0, SeekOrigin.Begin);

                // Iterate through the file again, this time reading in the cells.
                int yPos = 0;
                while (!reader.EndOfStream)
                {
                    if (yPos > universeHeight) break;
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then
                    // it is a comment and should be ignored.
                    if (row.Substring(0, 1) == "!") continue;

                    // If the row is not a comment then 
                    // it is a row of cells and needs to be iterated through.
                    for (int xPos = 0; xPos < row.Length; xPos++)
                    {
                        if (xPos > universeWidth) break;
                        // If row[xPos] is a 'O' (capital O) then
                        // set the corresponding cell in the universe to alive.
                        // If row[xPos] is a '.' (period) then
                        // set the corresponding cell in the universe to dead.
                        if (row[xPos] == 'O') universe[xPos, yPos] = true;
                    }
                    yPos++;
                }

                // Close the file.
                reader.Close();
            }

            // Update Strip Info
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            CellsAlive.Text = "Alive = " + countCells.ToString();

            // Invalidate Graphics Panel
            graphicsPanel1.Invalidate();
        }

        private void fromSeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Seed dlg = new Seed();

            // Set Current Seed
            dlg.seed = seed;


            // Show Dialog Box to User & Determine Result
            if (DialogResult.OK == dlg.ShowDialog())
            {
                seed = dlg.seed;
                Randomize();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Randomize();
        }

        private void fromTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            seed = (int)DateTime.Now.Ticks;
            Randomize();
        }

        private void hUDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hUDToolStripMenuItem.Checked = !hUDToolStripMenuItem.Checked;
            // Tell Windows you need to repaint
            graphicsPanel1.Invalidate();
        }

        private void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gridToolStripMenuItem.Checked = !gridToolStripMenuItem.Checked;
            // Tell Windows you need to repaint
            graphicsPanel1.Invalidate();
        }

        private void x10GridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            x10GridToolStripMenuItem.Checked = !x10GridToolStripMenuItem.Checked;
            // Tell Windows you need to repaint
            graphicsPanel1.Invalidate();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }

        private void runToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }
    }
}

