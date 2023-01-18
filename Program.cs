using System;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace _1_13_Checkerbot
{
    internal class Program
    {   
        // Variables
        bool currentlyOccupied;
        bool legalSquare;
        bool legalMove;
        bool kingPiece;
        int player;
        const int squareCount = 8;                  // Number of squares on each row and column of the board (8x8)
        const int squareScale = 5;                  // Scaling the size of each square
        const int squarewidth = 2 * squareScale;    // Scaling the width of each square to account for the font of the console (font is twice as tall as wide)

        // 2D array of the starting checkerboard
        static int[,] boardArray = new int[squareCount, squareCount]
        {
            { -1, 2, -1, 2, -1, 2, -1, 2 },    // -1 = Illegal space (Red Square)
            { 2, -1, 2, -1, 2, -1, 2, -1 },    //  0 = Legal space   (Black Square)
            { -1, 2, -1, 2, -1, 2, -1, 2 },    //  1 = Player 1
            { 0, -1, 0, -1, 0, -1, 0, -1 },    //  2 = Player 2
            { -1, 0, -1, 0, -1, 0, -1, 0 },    //  3 = Player 1 King
            { 1, -1, 1, -1, 1, -1, 1, -1 },    //  4 = Player 2 King
            { -1, 1, -1, 1, -1, 1, -1, 1 },
            { 1, -1, 1, -1, 1, -1, 1, -1 }
        };

        static void Main(string[] args)
        {            
            // Window title
            Console.Title = "Checkerbot";

            // Display the column indices above the board 
            Console.Write("\n\t 0 \t   1 \t     2\t       3\t 4\t   5\t     6\t       7");

            // Display the row indices left of the board
            Console.Write("\n\n\n\n  0\n\n\n\n\n  1\n\n\n\n\n  2\n\n\n\n\n  3\n\n\n\n\n" +
                          "  4\n\n\n\n\n  5\n\n\n\n\n  6\n\n\n\n\n  7");

            // Call the functions
            DrawBoard(4, 3);  // (xPos, yPos), offsets the checkerboard on the console
            
            // ReadKey requires keypress to close console
            Console.ReadKey();
    
        }// End Main()

        // Separate the board from the logic for now. Assign variables to the array indices

        // WIP        

        // Function to draw a single square at any position and any color
        public static void DrawSquare(int xPos, int yPos, int width, int height, ConsoleColor color)
        {
            for (int row = 0; row < width; row++)           // Row
            {
                for (int col = 0; col < height; col++)      // Column
                {
                    FillSquare(xPos + row, yPos + col, width, height, color);
                }// End for
            }// End for
        }// End DrawSquare()

        // Function to draw a checker at any postion and any color
        public static void DrawCheckerPiece(int xPos, int yPos, int width, int height, ConsoleColor color)
        {
            // Each square right is (X + 20), starting value 6 (left-most square)
            // Each square down is (Y + 5), starting value 4 (top-most square)
            // Left-most X value = 6
            // Top-most Y value = 4
            // Right-most X value = 76
            // Bottom-most Y value = 39

            for (int row = 0; row < boardArray.Length; row++)
            {
                for (int col = 0; col < boardArray.Length; col++)
                {
                    // If a square is even (Dark Red squares)
                    if ((row + col) % 2 == 0)
                    {
                        // CODE GOES HERE
                    }// End if
                    // If a square is odd (Dark Green squares)
                    else
                    {
                        // CODE GOES HERE
                    }// End else
                }// End for
            }// End for

            DrawSquare(xPos, yPos, width, height, color);
            Console.Write("\n\n\n");
        }// End DrawCheckerPiece()

        // Function to draw the board that uses the DrawSquare() function and DrawChecker() function.
        public static void DrawBoard(int xPos, int yPos)
        {
            for (int row = 0; row < squareCount; row++)
            {
                for (int col = 0; col < squareCount; col++)
                {
                    // Drawing the checkerboard with DrawSquare()
                    if ((row + col) % 2 == 0)   // If the square is even
                    {
                        DrawSquare(xPos + row * squarewidth, yPos + col * squareScale, squarewidth, 1 * squareScale, ConsoleColor.DarkRed);
                    }// End if
                    else                        // Else the square is odd
                    {
                        DrawSquare(xPos + row * squarewidth, yPos + col * squareScale, squarewidth, 1 * squareScale, ConsoleColor.DarkGreen);
                    }// End else
                }// End for
            }// End for

            // Drawing the checker pieces with DrawChecker()
            // White pieces
            DrawCheckerPiece(16, 4, 6, 3, ConsoleColor.White);
            DrawCheckerPiece(36, 4, 6, 3, ConsoleColor.White);
            DrawCheckerPiece(56, 4, 6, 3, ConsoleColor.White);
            DrawCheckerPiece(76, 4, 6, 3, ConsoleColor.White);
            
            DrawCheckerPiece(6 , 9, 6, 3, ConsoleColor.White);
            DrawCheckerPiece(26, 9, 6, 3, ConsoleColor.White);
            DrawCheckerPiece(46, 9, 6, 3, ConsoleColor.White);
            DrawCheckerPiece(66, 9, 6, 3, ConsoleColor.White);
            
            DrawCheckerPiece(16, 14, 6, 3, ConsoleColor.White);
            DrawCheckerPiece(36, 14, 6, 3, ConsoleColor.White);
            DrawCheckerPiece(56, 14, 6, 3, ConsoleColor.White);
            DrawCheckerPiece(76, 14, 6, 3, ConsoleColor.White);
            
            // Black pieces
            DrawCheckerPiece(6, 29, 6, 3, ConsoleColor.Black);
            DrawCheckerPiece(26, 29, 6, 3, ConsoleColor.Black);
            DrawCheckerPiece(46, 29, 6, 3, ConsoleColor.Black);
            DrawCheckerPiece(66, 29, 6, 3, ConsoleColor.Black);
            
            DrawCheckerPiece(16, 34, 6, 3, ConsoleColor.Black);
            DrawCheckerPiece(36, 34, 6, 3, ConsoleColor.Black);
            DrawCheckerPiece(56, 34, 6, 3, ConsoleColor.Black);
            DrawCheckerPiece(76, 34, 6, 3, ConsoleColor.Black);
            
            DrawCheckerPiece(6, 39, 6, 3, ConsoleColor.Black);
            DrawCheckerPiece(26, 39, 6, 3, ConsoleColor.Black);
            DrawCheckerPiece(46, 39, 6, 3, ConsoleColor.Black);
            DrawCheckerPiece(66, 39, 6, 3, ConsoleColor.Black);

            // Each square right is (X + 20), starting value 6 (left-most square)
            // Each square down is (Y + 5), starting value 4 (top-most square)
            // Left-most X value = 6
            // Top-most Y value = 4
            // Right-most X value = 76
            // Bottom-most Y value = 39
        }// End DrawBoard()        
        
        // A single 2D array to represent the checkerboard’s data. Color of the square, the type of piece on it, etc.
        
        
        public static void MovePiece()
        {
            // Asks the user to enter the piece they want to move and where they want to move it
            Console.WriteLine("Enter the row and column of your piece followed by '-' and the new row and column (ex: 12-34): ");
            
            // Variables

            // Attempts to split the user input into (x, y) coords for boardArray[,]
            try
            {
                // Reads the input from the user
                string input                = Console.ReadLine();

                // Split the input into an array
                string[] inputSplit         = input.Split("-");
                
                // Split the array into two variables
                string inputSplitZero       = inputSplit[0];
                string inputSplitOne        = inputSplit[1];

                // Split the variables into arrays
                string[] currentCombined    = inputSplitZero.Split();
                string[] newCombined        = inputSplitOne.Split();

                // Split the arrays into individual numbers in a string
                string currentCombinedZero  = currentCombined[0];
                string currentCombinedOne   = currentCombined[1];
                string newCombinedZero      = newCombined[0];
                string newCombinedOne       = newCombined[1];

                // Convert the individual numbers in the strings to integers
                int currentX                = int.Parse(currentCombinedZero);
                int currentY                = int.Parse(currentCombinedOne);
                int newX                    = int.Parse(newCombinedZero);
                int newY                    = int.Parse(newCombinedOne);

                // If player 1 is moving a player 1 piece
                if (/*player == 1 &&*/boardArray[currentX, currentY] == 1)
                {
                    // If the new square is empty and legal
                    if (boardArray[newX, newY] == 0)
                    {
                        // Remove the player piece from the old square
                        boardArray[currentX, currentY] = 0;
                        // Place the player piece in the new square
                        boardArray[newX, newY] = 1;
                    }// End if

                    // If the new square already has a player 1 piece
                    else if (boardArray[newX, newY] == 1)
                    {
                        // The move cannot happen
                        Console.WriteLine("Enter another destination");
                    }// End else if

                    // If the new square has a player 2 piece
                    else if (boardArray[newX, newY] == 2)
                    {
                        // If the diagonal space behind the destination is empty (jumping right)
                        if (boardArray[newX + 1, newY + 1] == 0)
                        {
                            // Pick up the player 1 piece
                            boardArray[currentX, currentY] = 0;
                            // Place the player 1 piece in the space behind the player 2 piece
                            boardArray[newX + 1, newY + 1] = 1;
                            // Remove the player 2 piece
                            boardArray[newX, newY] = 0;
                        }// End if

                        // If the diagonal piece behind the destination is empty (jumping left)
                        else if (boardArray[newX + 1, newY - 1] == 0)
                        {
                            // Pick up the player 1 piece
                            boardArray[currentX, currentY] = 0;
                            // Place the player 1 piece in the space behind the player 2 piece
                            boardArray[newX + 1, newY - 1] = 1;
                            // Remove the player 2 piece
                            boardArray[newX, newY] = 0;
                        }// End else if

                        // If there is no empty space diagonally to the player 2 piece
                        else
                        {
                            // The move cannot happen
                            Console.WriteLine("Enter another destination");
                        }// End else
                    }// End else if
                }// End if

                // If player 2 is moving a player 2 piece
                else if (/*player == 2 &&*/boardArray[currentX, currentY] == 2)
                {
                    // If the new square is empty and legal
                    if (boardArray[newX, newY] == 0)
                    {
                        // Remove the player piece from the old square
                        boardArray[currentX, currentY] = 0;
                        // Place the player piece in the new square
                        boardArray[newX, newY] = 2;
                    }// End if

                    // If the new square already has a player 2 piece
                    else if (boardArray[newX, newY] == 2)
                    {
                        // The move cannot happen
                        Console.WriteLine("Enter another destination");
                    }// End else if

                    // If the new square has a player 1 piece
                    else if (boardArray[newX, newY] == 1)
                    {
                        // If the diagonal space behind the destination is empty (jumping left)
                        if (boardArray[newX - 1, newY - 1] == 0)
                        {
                            // Pick up the player 2 piece
                            boardArray[currentX, currentY] = 0;
                            // Place the player 2 piece in the space behind the player 1 piece
                            boardArray[newX - 1, newY + 1] = 2;
                            // Remove the player 1 piece
                            boardArray[newX, newY] = 0;
                        }// End if

                        // If the diagonal piece behind the destination is empty (jumping right)
                        else if (boardArray[newX + 1, newY - 1] == 0)
                        {
                            // Pick up the player 2 piece
                            boardArray[currentX, currentY] = 0;
                            // Place the player 2 piece in the space behind the player 1 piece
                            boardArray[newX + 1, newY - 1] = 2;
                            // Remove the player 2 piece
                            boardArray[newX, newY] = 0;
                        }// End else if

                        // If there is no empty space diagonally to the player 1 piece
                        else
                        {
                            // The move cannot happen
                            Console.WriteLine("Enter another destination");
                        }// End else
                    }// End else if

                    // If the square selected by either user is not currently occupied by their checker piece
                    else if (boardArray[currentX, currentY] == 0 || boardArray[currentX, currentY] == -1)
                    {
                        // A valid piece has not been selected
                        Console.WriteLine("You did not select one of your checker pieces");
                    }// End else if
                }// End else if
            }// End try
            catch
            {
                // Wrong format entered
                Console.WriteLine("Please enter the correct input");
            }// End catch

        }// End MovePiece()

        // Funtion to completely fill in a space on the console screen using char #9608
        public static void FillSquare(int xPos, int yPos, int width, int height, ConsoleColor color)
        {
            // Sets the cursor position
            Console.SetCursorPosition(xPos, yPos);

            // Sets the foreground color to fill with (char)9608
            Console.ForegroundColor = color;
            Console.Write((char)9608);

            // Resets the console color
            Console.ResetColor();
        }// End FillSpace

    }// End class Program
}// End namespace