using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace _1_13_Checkerbot
{
    internal class Program
    {
        // 2D array of the starting checkerboard
        public static int[,] boardArray = new int[8, 8]
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

        // Variables
        bool currentlyOccupied;
        bool legalMove;

        static void Main(string[] args)
        {            
            // Window title
            Console.Title = "Checkerbot";

            // Display the column indices above the board 
            Console.Write("\n\t 0 \t   1 \t     2\t       3\t 4\t   5\t     6\t       7");

            // Display the row indices left of the board
            Console.Write("\n\n\n\n  0\n\n\n\n\n  1\n\n\n\n\n  2\n\n\n\n\n  3\n\n\n\n\n  4\n\n\n\n\n  5\n\n\n\n\n  6\n\n\n\n\n  7");

            // Call the functions
            DrawBoard(4, 3);  // (xPos, yPos), offsets the checkerboard on the console

            // ReadKey requires keypress to close console
            Console.ReadKey();
    
        }// End Main()

        // Separate the board from the logic for now. Assign variables to the array indices.

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
            DrawSquare(xPos, yPos, width, height, color);
            Console.Write("\n\n\n");
        }// End DrawCheckerPiece()

        // Function to draw the board that uses the DrawSquare() function and DrawChecker() function.
        public static void DrawBoard(int xPos, int yPos)
        {            
            // Variables
            int squareCount = 8;            // Number of squares on each row and column of the board (8x8)
            int squareScale = 5;            // Scaling the size of each square
            int squarewidth = 2 * squareScale;  // Scaling the width of each square to account for the font of the console (font is twice as tall as wide)

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
        //public static void Square()
        //{
        //}// End

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