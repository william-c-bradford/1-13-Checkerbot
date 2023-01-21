namespace _1_13_Checkerbot
{
    internal class Program
    {
        // Variables
        static bool gameState = true;
        static int playerTurn;
        static int playerPiece;
        static int emptyPiece = 0;
        static int currentX;
        static int currentY;
        static int newX;
        static int newY;
        const int squareCount = 8;                  // Number of squares on each row and column of the board (8x8)
        const int squareScale = 5;                  // Scaling the size of each square
        const int squareWidth = 2 * squareScale;    // Scaling the width of each square to account for the font of the console (font is twice as tall as wide)
        const int checkerWidth = 6;
        const int checkerHeight = 3;
        const int checkerStartX = 6;
        const int checkerStartY = 4;
        const int checkerXScale = 10;
        const int checkerYScale = 5;
        const int boardOffsetX = 4;
        const int boardOffsetY = 3;


        // 2D array of the starting checkerboard
        static int[,] boardArray = new int[squareCount, squareCount]
        {
            { 0, 2, 0, 0, 0, 1, 0, 1 },     //  0 = Empty space
            { 2, 0, 2, 0, 0, 0, 1, 0 },     //  1 = Player 1
            { 0, 2, 0, 0, 0, 1, 0, 1 },     //  2 = Player 2
            { 2, 0, 2, 0, 0, 0, 1, 0 },     //  3 = Player 1 King
            { 0, 2, 0, 0, 0, 1, 0, 1 },     //  4 = Player 2 King
            { 2, 0, 2, 0, 0, 0, 1, 0 },
            { 0, 2, 0, 0, 0, 1, 0, 1 },
            { 2, 0, 2, 0, 0, 0, 1, 0 }
        };

        static void Main(string[] args)
        {
            // Window title
            Console.Title = "Checkerbot";



            // Set the player turn
            playerTurn = 1;

            // Call the functions during the game loop
            while (gameState)
            {
                // If player 1 or player 2's turns
                if (playerTurn < 3)
                {
                    DrawBoard(boardOffsetX, boardOffsetY);

                    GetPlayerMoves();

                    MovePiece(currentX, currentY, newX, newY);
                }// End if player turn is greater than or equal to 3

                // Else player turn resets to 1
                else
                {
                    playerTurn = 1;
                }// End else player turn resets
                playerTurn++;
            }

            // ReadKey requires keypress to close console
            Console.ReadKey();

        }// End Main()

        // Function to draw a single square at any position and any color
        public static void DrawSquare(int xPos, int yPos, int width, int height, ConsoleColor color)
        {
            for (int row = 0; row < width; row++)           // Row
            {
                for (int col = 0; col < height; col++)      // Column
                {
                    FillSquare(xPos + row, yPos + col, width, height, color);
                }// End for columns
            }// End for rows
        }// End DrawSquare()

        // Function to draw a checker at any postion and any color
        public static void DrawCheckerPiece(int xPos, int yPos, int width, int height, ConsoleColor color)
        {
            DrawSquare(xPos, yPos, width, height, color);
            Console.Write("\n\n\n\n\n\n\n");
        }// End DrawCheckerPiece()

        // Function to draw a king piece at any position and any color
        public static void DrawKingPiece(int xPos, int yPos, int width, int height, ConsoleColor color)
        {
            DrawCheckerPiece(xPos, yPos, width, height, color);
            DrawCheckerPiece(xPos, yPos, width, height / 2, ConsoleColor.DarkYellow);
        }// End DrawKingPiece()

        // Funtion to draw the indices above and to the left of the board
        public static void DrawBoardIncices()
        {
            // Display the column indices above the board 
            Console.Write("\n\t 0 \t   1 \t     2\t       3\t 4\t   5\t     6\t       7");

            // Display the row indices left of the board
            Console.Write("\n\n\n\n  0\n\n\n\n\n  1\n\n\n\n\n  2\n\n\n\n\n  3\n\n\n\n\n" +
                          "  4\n\n\n\n\n  5\n\n\n\n\n  6\n\n\n\n\n  7");
        }// End DrawBoardIndices()

        // Function to draw the board that uses the DrawSquare() function and DrawChecker() function.
        public static void DrawBoard(int xPos, int yPos)
        {
            DrawBoardIncices();
            // Drawing the checkerboard pattern with DrawSquare()
            for (int row = 0; row < squareCount; row++)
            {
                for (int col = 0; col < squareCount; col++)
                {
                    // Drawing the checkerboard with DrawSquare()
                    // If the square is even
                    if ((row + col) % 2 == 0)
                    {
                        DrawSquare(xPos + row * squareWidth, yPos + col * squareScale, squareWidth, 1 * squareScale, ConsoleColor.DarkRed);
                    }// End if square is even
                    // Else the square is odd
                    else
                    {
                        DrawSquare(xPos + row * squareWidth, yPos + col * squareScale, squareWidth, 1 * squareScale, ConsoleColor.DarkGray);
                    }// End else square is odd
                }// End for columns
                Console.WriteLine();
            }// End for rows

            // Draw the player checkers on the board using DrawChecker()
            for (int row = 0; row < squareCount; row++)
            {
                for (int col = 0; col < squareCount; col++)
                {
                    // If player 1 checker piece is on board
                    if (boardArray[row, col] == 1)
                    {
                        DrawCheckerPiece(checkerStartX + (row * checkerXScale), checkerStartY + (col * checkerYScale),
                            checkerWidth, checkerHeight, ConsoleColor.Black);
                    }// End if player 1 checker piece is on board

                    // Else if player 2 checker piece is on board
                    else if (boardArray[row, col] == 2)
                    {
                        DrawCheckerPiece(checkerStartX + (row * checkerXScale), checkerStartY + (col * checkerYScale),
                            checkerWidth, checkerHeight, ConsoleColor.White);
                    }// End else if player 2 checker piece is on board

                    // Else if player 1 king piece is on board
                    else if (boardArray[row, col] == 3)
                    {
                        DrawKingPiece(checkerStartX + (row * checkerXScale), checkerStartY + (col * checkerYScale),
                            checkerWidth, checkerHeight, ConsoleColor.Black);
                    }// End else if player 1 king piece is on board

                    // Else if player 2 king piece is on board
                    else if (boardArray[row, col] == 4)
                    {
                        DrawKingPiece(checkerStartX + (row * checkerXScale), checkerStartY + (col * checkerYScale),
                            checkerWidth, checkerHeight, ConsoleColor.White);
                    }// End else if player 2 king piece is on board
                }// End col for loop
                Console.WriteLine();
            }// End row for loop
        }// End DrawBoard()

        // Get move input from the player
        public static void GetPlayerMoves()
        {
            // Asks the user to enter the piece they want to move and where they want to move it
            Console.WriteLine("Enter the row and column of your piece followed by a '-' and \nthe row and column of a new square (ex: 12-34): ");

            // Reads the input from the user
            string userCoords = Console.ReadLine();

            // Attempts to split the user input into (x, y) coords for boardArray[,] and move pieces
            try
            {
                // Convert the individual numbers from the input string to integers
                currentX = int.Parse(userCoords[0].ToString());
                currentY = int.Parse(userCoords[1].ToString());
                newX = int.Parse(userCoords[3].ToString());
                newY = int.Parse(userCoords[4].ToString());

                // Move the piece on the board
                MovePiece(currentX, currentY, newX, newY);

            }// End try

            catch
            {
                // Wrong format entered
                Console.WriteLine("You did not use the correct format. Try again.");
                Console.ReadKey();
                Console.Clear();
                DrawBoard(boardOffsetX, boardOffsetY);
                GetPlayerMoves();
            }// End catch

        }// End GetPlayerMoves()

        // Moves the checker pieces on the board
        public static void MovePiece(int currentX, int currentY, int newX, int newY)
        {
            // Assign the value of boardArray to the variable playerPiece
            playerPiece = boardArray[currentX, currentY];

            // If player 1's turn
            if (playerTurn == 1)
            {
                // If player 1 is moving a player 1 piece
                if (playerPiece == 1 || playerPiece == 3)
                {
                    // If player 1 piece is moving diagonally (up the board)
                    if (currentX - newX == 1 && Math.Abs(currentY - newY) == 1) // X moving 1 space up, Y moving 1 space left or right
                    {
                        // If the new square is currently occupied
                        if (boardArray[newX, newY] != emptyPiece)
                        {
                            // The move cannot happen
                            Console.WriteLine("1This is not a valid move. Enter another destination.");
                        }// End if the square is occupied

                        // Else if the new square is empty and legal
                        else
                        {
                            // Remove the player piece from the old square
                            boardArray[currentX, currentY] = emptyPiece;
                            // Place the player piece in the new square
                            boardArray[newX, newY] = playerPiece;
                        }// End else the new square is empty and legal
                    }// End if the player is not moving diagonally

                    // If player 1 piece is jumping a player 2 piece
                    else if (currentX - newX == 2 && Math.Abs(currentY - newY) == 2 &&
                            (boardArray[newX, newY] == 2 || boardArray[newX, newY] == 4))
                    {
                        // Remove the player piece from the old square
                        boardArray[currentX, currentY] = emptyPiece;
                        // Place the player 1 piece in the space behind the player 2 piece
                        boardArray[newX + 1, newY - 1] = playerPiece;
                        // Remove the player 2 piece
                        boardArray[newX, newY] = emptyPiece;
                    }// End else if player 1 checker piece is jumping a player 2 piece

                    // Else if player piece is king piece
                    else if (playerPiece != 1 && playerPiece == 3)
                    {
                        // If player 1 king piece is moving diagonally (down the board)
                        if (newX - currentX == 1 && Math.Abs(newY - currentY) == 1) // X moving 1 space down, Y moving 1 space left or right
                        {
                            // If the new square is currently occupied
                            if (boardArray[newX, newY] != emptyPiece)
                            {
                                // The move cannot happen
                                Console.WriteLine("2This is not a valid move. Enter another destination.");
                            }// End if the square is occupied

                            // Else if the new square is empty and legal
                            else
                            {
                                // Remove the player piece from the old square
                                boardArray[currentX, currentY] = emptyPiece;
                                // Place the player piece in the new square
                                boardArray[newX, newY] = playerPiece;
                            }// End else the new square is empty and legal
                        }// End if the player is not moving diagonally
                    }// End else if player piece is not king piece

                    // If player 1 is not moving diagonally
                    else
                    {
                        // The move cannot happen
                        Console.WriteLine("3This is not a valid move. Try again.");
                    }// End else if player 1 is not moving diagonally
                }// End if player 1 is moving a player 1 checker piece

                // Else player 1 did not select a player 1 piece
                else
                {
                    // The piece cannot be moved by player 1
                    Console.WriteLine("4This is not a valid piece. Enter another selection.");
                }// End else player 1 did not select a player 1 piece
            }// End if player 1's turn

            // Else if player 2's turn
            else if (playerTurn == 2)
            {
                // If player 2 is moving a player 2 piece
                if (playerPiece == 2 || playerPiece == 4)
                {
                    // If player 2 piece is moving diagonally (up the board)
                    if (newX - currentX == 1 && Math.Abs(newY - currentY) == 1) // X moving 1 space down, Y moving 1 space left or right
                    {
                        // If the new square is currently occupied
                        if (boardArray[newX, newY] != emptyPiece)
                        {
                            // The move cannot happen
                            Console.WriteLine("AThis is not a valid move. Enter another destination.");
                        }// End if the square is occupied

                        // Else if the new square is empty and legal
                        else
                        {
                            // Remove the player piece from the old square
                            boardArray[currentX, currentY] = emptyPiece;
                            // Place the player piece in the new square
                            boardArray[newX, newY] = playerPiece;
                        }// End else the new square is empty and legal
                    }// End if the player is not moving diagonally

                    // If player 2 piece is jumping a player 1 piece
                    else if (currentX - newX == 2 && Math.Abs(currentY - newY) == 2 &&
                            (boardArray[newX, newY] == 1 || boardArray[newX, newY] == 3))
                    {
                        // Remove the player piece from the old square
                        boardArray[currentX, currentY] = emptyPiece;
                        // Place the player 2 piece in the space behind the player 1 piece
                        boardArray[newX + 1, newY - 1] = playerPiece;
                        // Remove the player 1 piece
                        boardArray[newX, newY] = emptyPiece;
                    }// End else if player 2 checker piece is jumping a player 1 piece

                    // Else if player piece is king piece
                    else if (playerPiece != 2 && playerPiece == 4)
                    {
                        // If player 2 king piece is moving diagonally (down the board)
                        if (currentX - newX == 1 && Math.Abs(currentY - newY) == 1) // X moving 1 space up, Y moving 1 space left or right
                        {
                            // If the new square is currently occupied
                            if (boardArray[newX, newY] != emptyPiece)
                            {
                                // The move cannot happen
                                Console.WriteLine("BThis is not a valid move. Enter another destination.");
                            }// End if the square is occupied

                            // Else if the new square is empty and legal
                            else
                            {
                                // Remove the player piece from the old square
                                boardArray[currentX, currentY] = emptyPiece;
                                // Place the player piece in the new square
                                boardArray[newX, newY] = playerPiece;
                            }// End else the new square is empty and legal
                        }// End if the player is not moving diagonally
                    }// End else if player piece is not king piece

                    // If player 2 is not moving diagonally
                    else
                    {
                        // The move cannot happen
                        Console.WriteLine("CThis is not a valid move. Try again.");
                    }// End else if player 1 is not moving diagonally
                }// End if player 1 is moving a player 1 checker piece

                // Else player 2 did not select a player 2 piece
                else
                {
                    // The piece cannot be moved by player 2
                    Console.WriteLine("DThis is not a valid piece. Enter another selection.");
                }// End else player 2 did not select a player 2 piece
            }// End if player 2's turn

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