using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Green_vs_Red_Game
{
    public class Game
    {
        /* This class represents the communication between the game and the player.
           Guides player, validates player input and sets initial grid size. */

        private int[,] grid;
        readonly Logic logic = new Logic();

        public void Start(bool firstGame)
        {
            if (firstGame)
            {
                Console.WriteLine("Welcome to Green vs. Red!");
                Console.WriteLine("Before we start we need to ask some questions :).");
            }

            InitialData();
            int result = logic.Play(grid); // By now the logic layer have all information needed to generate all generations of the grid.
            Console.WriteLine($"The chosen cell was green {result} times!");
            GameOver();
        }

        private void InitialData()
        {
            // First Step
            Console.WriteLine("\n1. What size would you like the grid to be ? Keep in mind that it cannot be higher that 1000 or negative.");
            int gridX;
            int gridY;

            while (true)
            {
                Console.Write("Y = ");
                gridY = int.Parse(Console.ReadLine());
                if (gridY > 1000 || gridY <= 0) // Input validation
                {
                    Console.WriteLine("It must be between 0 and 1000! Try again.\n");
                    continue;
                }

                Console.Write("X = ");
                gridX = int.Parse(Console.ReadLine());
                if (gridX > gridY || gridX <= 0) // Input validation
                {
                    Console.WriteLine("It must be between 0 and 1000! Try again.\n");
                    continue;
                }

                grid = new int[gridY, gridX];
                break;
            }

            // Second Step
            Console.WriteLine("\n2. Time to fill the grid with values! Enter every row separately. Green is represented as 1 and Red is represented by 0.");
            for (int currentRow = 0; currentRow < gridY; currentRow++) //Fills every row of the grid with values
            {
                Console.Write($"Row#{currentRow}: ");
                string values = Console.ReadLine();
                if (values.Length != gridX) // Input validation
                { 
                    Console.WriteLine($"Oops! The row must have exactly {gridX} values!");
                    Console.WriteLine("Please enter the row again.\n");
                    currentRow--;
                    continue;
                }

                logic.FillGridRow(grid, currentRow, values);
            }

            //Third Step
            Console.WriteLine("\n3. Great! Now just enter which cell of the grid u want to watch over and how many times the grid should change.");
            int cellX;
            int cellY;

            while (true)
            {
                Console.Write("Cell's X = ");
                cellX = int.Parse(Console.ReadLine());
                if (cellX >= gridX || cellX < 0) // Input validation
                {
                    Console.WriteLine("Oops! Theres no such X!");
                    continue;
                }
                break;
            }

            while (true)
            {
                Console.Write("Cell's Y = ");
                cellY = int.Parse(Console.ReadLine());
                if (cellY >= gridY || cellY < 0) // Input validation
                {
                    Console.WriteLine("Oops! Theres no such Y!");
                    continue;
                }
                break;
            }

            Console.Write("Turns = ");
            int turns = int.Parse(Console.ReadLine());

            logic.SetConditions(cellX, cellY, turns); // Sends player conditions to the logic layer.
        }

        private void GameOver()
        {
            Console.WriteLine("GAME OVER!");
            Console.Write("Would you like to play again? (Y/N): ");
            string answer = Console.ReadLine();
            if (answer.ToLower() == "y")
            {
                Start(false);
            }
        }
    }
}
