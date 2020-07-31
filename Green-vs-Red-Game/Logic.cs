using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace Green_vs_Red_Game
{
    class Logic
    {
        /* This class represents the logic that this game uses.
           Contains all condtitions given by the player and executes game's logic behind the scenes. */

        // User conditions
        private int turns;
        private int cellX; // Watched cell X coordinate.
        private int cellY; // Watched cell Y coordinate.

        public void SetConditions(int x, int y, int turns)
        {
            this.turns = turns;
            cellX = x;
            cellY = y;
        }

        public void FillGridRow(int[,] grid, int row, string values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                grid[row, i] = int.Parse(values[i].ToString());
            }
        }

        public int Play(int[,] grid)
        {
            bool isGreen;
            int greenCellCounter = 0;
            int[,] nextGenerationGrid = (int[,])grid.Clone();

            for (int i = 0; i <= turns; i++) // Next generations loop (0 -> N(turns)).
            {

                if (grid[cellY, cellX] == 1) // Checks if the "watched" cell is still green or has become green.
                {
                    isGreen = true;
                }
                else isGreen = false;

                if (isGreen) // Adds to the counter in case it's green.
                {
                    greenCellCounter++;
                }

                for (int row = 0; row < grid.GetLength(1); row++) // Loop through every cell in the grid.
                {
                    for (int col = 0; col < grid.GetLength(0); col++)
                    {
                        nextGenerationGrid[row, col] = GetNewValue(grid, col, row); // Checks neighbours and returns the new value for next generation.
                    }
                }

                grid = (int[,])nextGenerationGrid.Clone(); // grid becomes next generation.
            }
            return greenCellCounter;
        }

        private int GetNewValue(int[,] grid, int x, int y)
        {
            int cellColor = -1; // Default value != any color value.
            int greenNeighbours = 0;

            for (int col = -1; col <= 1; col++) // Check all neighbours.
            {
                for (int row = -1; row <= 1; row++)
                {
                    if (col == 0 && row == 0)
                    {
                        cellColor = grid[y, x]; // Takes the color of the cell we check neighbours for.
                        continue;
                    }

                    try // When cheking corner or side cell some values throw out of boundries exception.
                    {
                        if (grid[y + row, x + col] == 1)
                        {
                            greenNeighbours++;
                        }
                    }
                    catch { continue; } // Catch the exception and continue with next iteration.
                }
            }

            int newValue = ApplyRules(cellColor, greenNeighbours); // Apply rules according to total green neighbours.
            return newValue;
        }

        private int ApplyRules(int color, int greenNeighbours)
        {
            switch (color)
            {
                case 0: // Red cell rules.
                    if (greenNeighbours == 3 || greenNeighbours == 6)
                    {
                        return 1;
                    }
                    else return 0;
                case 1: // Green cell rules.
                    if (greenNeighbours == 2 || greenNeighbours == 3 || greenNeighbours == 6)
                    {
                        return 1;
                    }
                    else return 0;
                default:
                    return color;
            }
        }
    }
}
