namespace _1_13_Checkerbot
{
    internal class Program
    {
        // Global variables
        static bool runGame         = true;               // Runs the game as long as this is true
        static int  playerTurn      = 1;                  // Sets the current player turn
        static int  currentSquare;                        // Stores the value of which piece is in use from boardArray[,]
        static int  newSquare;                            // Stores the value of the destination square from boardArray[,]
        static int  emptySquare     = 0;                  // Stores the value of an empty square on the board (0)
        static int  currentX;                             // User input of the selected x coordinate for their piece
        static int  currentY;                             // User input of the selected y coordinate for their piece
        static int  newX;                                 // User input of their piece's x destination
        static int  newY;                                 // User input of their piece's y destination
        const  int  player1Checker  = 1;                  // The integer representing player 1's checker piece
        const  int  player2Checker  = 2;                  // The integer representing player 2's checker piece
        const  int  player1King     = 3;                  // The integer representing player 1's king piece
        const  int  player2King     = 4;                  // The integer representing player 2's king piece
        const  int  squareCount     = 8;                  // Number of squares on each row and column of the board (8x8)
        const  int  squareScale     = 5;                  // Scaling the size of each square
        const  int  squareWidth     = 2 * squareScale;    // Scaling the width of each square for font size (font is twice as tall as wide)
        const  int  checkerWidth    = 6;                  // The width of a checker piece
        const  int  checkerHeight   = 3;                  // The height of a checker piece
        const  int  checkerStartX   = 6;                  // The left-most x position for a checker piece
        const  int  checkerStartY   = 4;                  // The top-most y position for a checker piece
        const  int  checkerXScale   = 10;                 // The scale at which the x position of a checker changes between pieces
        const  int  checkerYScale   = 5;                  // The scale at which the y position of a checker changes between pieces
        const  int  boardOffsetX    = 4;                  // The x position offset of the checkerboard from the console window
        const  int  boardOffsetY    = 3;                  // The y position offset of the checkerboard from the console window
        const  int  textOffsetX     = 6;                  // The x position offset of the text below the board
        const  int  textOffsetY     = 43;                 // The y position offset of the text below the board

        // 2D array of the starting checkerboard
        static int[,] boardArray = new int[squareCount, squareCount]
        //{
        //    { 0, 2, 0, 0, 0, 1, 0, 1 },                 // 0 = Empty space
        //    { 2, 0, 2, 0, 0, 0, 1, 0 },                 // 1 = Player 1 checker piece
        //    { 0, 2, 0, 0, 0, 1, 0, 1 },                 // 2 = Player 2 checker piece
        //    { 2, 0, 2, 0, 0, 0, 1, 0 },                 // 3 = Player 1 king piece
        //    { 0, 2, 0, 0, 0, 1, 0, 1 },                 // 4 = Player 2 king piece
        //    { 2, 0, 2, 0, 0, 0, 1, 0 },
        //    { 0, 2, 0, 0, 0, 1, 0, 1 },
        //    { 2, 0, 2, 0, 0, 0, 1, 0 }
        //};

        // Testing boardArray[,]
        {
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 1, 0 },
            { 0, 0, 0, 2, 0, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 1, 0 },
            { 0, 1, 0, 0, 0, 1, 0, 0 },
            { 0, 0, 0, 0, 1, 0, 1, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 2, 0 }
        };

        // Function Main()
        static void Main(string[] args)
        {
            // Window title
            Console.Title = "Checkerbot";

            // Play the game
            GameLoop();

            // Require a key press to close the console window
            Console.ReadKey();
        }// End Main()

        // Funtion to completely fill in a space on the console screen using char #9608
        public static void FillSquare(int xPos, int yPos, int width, int height, ConsoleColor color)
        {
            // Sets the cursor position
            Console.SetCursorPosition(xPos, yPos);

            // Sets the foreground and background color to fill with (char)9608
            Console.ForegroundColor = color;
            Console.BackgroundColor = color;
            Console.Write((char)9608);

            // Resets the console color
            Console.ResetColor();
        }// End FillSquare()

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
            // Draws the checker piece using DrawSquare()
            DrawSquare(xPos, yPos, width, height, color);
        }// End DrawCheckerPiece()

        // Function to draw a king piece at any position and any color
        public static void DrawKingPiece(int xPos, int yPos, int width, int height, ConsoleColor color)
        {
            // Draws the checker piece using DrawSquare() as normal
            DrawCheckerPiece(xPos, yPos, width, height, color);

            // Draws a smaller "crown" square on top of the checker piece
            DrawCheckerPiece(xPos, yPos, width, height / 2, ConsoleColor.DarkYellow);
        }// End DrawKingPiece()

        // Funtion to draw the indices above and to the left of the board
        public static void DrawBoardIndices()
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
            // Draw the checkerboard pattern with DrawSquare()
            for (int row = 0; row < squareCount; row++)
            {
                for (int col = 0; col < squareCount; col++)
                {
                    // If the square is even draw the square Dark Red
                    if ((row + col) % 2 == 0)
                    {
                        DrawSquare(xPos + row * squareWidth, yPos + col * squareScale, squareWidth, 1 * squareScale, ConsoleColor.DarkRed);
                    }// End if square is even
                    // Else the square is odd draw the square Dark Grey
                    else
                    {
                        DrawSquare(xPos + row * squareWidth, yPos + col * squareScale, squareWidth, 1 * squareScale, ConsoleColor.DarkGray);
                    }// End else square is odd
                }// End for columns

                // Add a space after each loop to start a new row
                Console.WriteLine();
            }// End for rows

            // Draw the player checkers on the board using DrawChecker()
            for (int row = 0; row < squareCount; row++)
            {
                for (int col = 0; col < squareCount; col++)
                {
                    // If player 1 checker piece is on board
                    if (boardArray[row, col] == player1Checker)
                    {
                        // Draw a player 1 checker piece (black)
                        DrawCheckerPiece(checkerStartX + (row * checkerXScale), checkerStartY + (col * checkerYScale),
                            checkerWidth, checkerHeight, ConsoleColor.Black);
                    }// End if player 1 checker piece is on board

                    // Else if player 2 checker piece is on board
                    else if (boardArray[row, col] == player2Checker)
                    {
                        // Draw a player 2 checker piece (white)
                        DrawCheckerPiece(checkerStartX + (row * checkerXScale), checkerStartY + (col * checkerYScale),
                            checkerWidth, checkerHeight, ConsoleColor.White);
                    }// End else if player 2 checker piece is on board

                    // Else if player 1 king piece is on board
                    else if (boardArray[row, col] == player1King)
                    {
                        // Draw a player 1 king piece (black with dark yellow crown)
                        DrawKingPiece(checkerStartX + (row * checkerXScale), checkerStartY + (col * checkerYScale),
                            checkerWidth, checkerHeight, ConsoleColor.Black);
                    }// End else if player 1 king piece is on board

                    // Else if player 2 king piece is on board
                    else if (boardArray[row, col] == player2King)
                    {
                        // Draw a player 2 king piece (white with dark yellow crown)
                        DrawKingPiece(checkerStartX + (row * checkerXScale), checkerStartY + (col * checkerYScale),
                            checkerWidth, checkerHeight, ConsoleColor.White);
                    }// End else if player 2 king piece is on board
                }// End for columns

                // Set the cursor position
                Console.SetCursorPosition(textOffsetX, top: textOffsetY);
            }// End for rows
        }// End DrawBoard()

        // Function to make a checker piece a king piece if it reaches the opposide side of the board
        public static void CheckKingPiece()
        {
            // If player 1 reaches the opposite end of the board
            if (newY == 0 && currentSquare == player1Checker)
            {
                // The player 1 checker piece is replaced with a player 1 king piece
                boardArray[newX, newY] = player1King;
            }// End if player 1 reaches the opposite end of the board

            // If player 2 reaches the opposite end of the board
            if (newY == 7 && currentSquare == player2Checker)
            {
                // The player 2 checker piece is replaced with a player 2 king piece
                boardArray[newX, newY] = player2King;
            }// End if player 2 reaches the opposite end of the board
        }// End CheckKingPiece()

        // Function to loop the functions needed to play the game
        public static void GameLoop()
        {
            // Clear the console
            Console.Clear();

            // Draw the indices with DrawBoardIndices()
            DrawBoardIndices();

            // Draw the checkerboard and all pieces with DrawBoard()
            DrawBoard(boardOffsetX, boardOffsetY);

            // Check if a player has no more pieces with CheckNoPiecesWin()
            CheckNoPieces();

            // If the game has not finished
            if (runGame == true)
            {
                // Ask the player for their move with GetPlayerMoves()
                GetPlayerMoves();
            }// End if game has not finished
        }// End GameLoop()

        // Function to display winning text
        public static void WinningText(int winner)
        {
            // If player 2 has no pieces
            if (winner == 1)
            {
                // Display winning message
                Console.Write("\t\t\t\t   ******************\n" +
                              "\t\t\t\t   * PLAYER 1 WINS! *\n" +
                              "\t\t\t\t   ******************\n");
                // Ends the game loop
                runGame = false;
            }// End if player 2 has no pieces

            // If player 1 has no pieces
            else if (winner == 2)
            {
                // Display winning message
                Console.Write("\t\t\t\t   ******************\n" +
                              "\t\t\t\t   * PLAYER 2 WINS! *\n" +
                              "\t\t\t\t   ******************\n");
                // Ends the game loop
                runGame = false;
            }// End else if player 1 has no pieces
        }// End WinningText()

        // Function to check if a player has pieces remaining on the board
        public static void CheckNoPieces()
        {
            // Variables to set the player count to 0
            int player1Count = 0;
            int player2Count = 0;

            // Count the number of player 1 and player 2 pieces on the board (boardArray[,])
            for (int i = 0; i < squareCount; i++)
            {
                for (int j = 0; j < squareCount; j++)
                {
                    // If player 1 has pieces on the board
                    if (boardArray[i, j] == player1Checker || boardArray[i, j] == player1King)
                    {
                        player1Count++;
                    }// End if player 1 has pieces on the board

                    // If player 2 has pieces on the board
                    if (boardArray[i, j] == player2Checker || boardArray[i, j] == player2King)
                    {
                        player2Count++;
                    }// End if player 2 has pieces on the board
                }// End nested for loop
            }// End for loop to count player 1 and player 2 pieces on the board

            // If player 2 has no pieces left
            if (player2Count == 0)
            {
                WinningText(1);
            } // End if player 1 has no pieces left

            // If player 1 has no pieces left
            else if (player1Count == 0)
            {
                WinningText(2);
            }// End if player 2 has no pieces left
        }// End CheckNoPieces()

        // Function to get move input from the player
        public static void GetPlayerMoves()
        {
            // If playerTurn goes over 2
            if (playerTurn > 2)
            {
                // Resets playerTurn back to player 1
                playerTurn = 1;
            }// End if playerTurn goes over 2

            // Display current player
            Console.WriteLine("\tPlayer " + playerTurn);

            // Asks the user to enter the piece they want to move and where they want to move it
            Console.Write("\tEnter the column and row of your piece followed by a '-' character\n\tand the column and row of a new square (ex: 12-34): ");

            // Reads the input from the user
            string userCoords = Console.ReadLine();

            // Try to split the user input into (x, y) coords for boardArray[,] and move pieces
            try
            {
                // Convert the individual numbers from the input string to integers
                currentX = int.Parse(userCoords[0].ToString());
                currentY = int.Parse(userCoords[1].ToString());
                newX     = int.Parse(userCoords[3].ToString());
                newY     = int.Parse(userCoords[4].ToString());

                // Move the piece on the board using MovePiece()
                MovePiece(currentX, currentY, newX, newY);
            }// End try to get user input
            
            // Catch if there is an error when trying to get the user input
            catch
            {
                // Wrong format entered
                Console.WriteLine("\tYou did not use the correct format. Press any key to try again.");
                Console.ReadKey();
                GameLoop();
            }// End catch input error
        }// End GetPlayerMoves()

        // Moves the checker pieces on the board
        public static void MovePiece(int currentX, int currentY, int newX, int newY)
        {
            // Assign the current value of boardArray[,] to the variable currentSquare
            currentSquare = boardArray[currentX, currentY];

            // Assign the new value of boardArray[,] to the variable newSquare
            newSquare     = boardArray[newX, newY];

            // Variables to store the middle square between current(x,y) and new(x,y)
            int midX      = (newX + currentX) / 2;
            int midY      = (newY + currentY) / 2;
            int midSquare = (boardArray[midX, midY]);

            // Variables to store the difference between current position and new position
            int diffX     = Math.Abs(newX - currentX);
            int diffY     = Math.Abs(newY - currentY);

            // If player 1 is moving a player 1 piece or player 2 is moving a player 2 piece (regular pieces can only move one direction)
            if (((playerTurn == 1 && ((currentSquare == player1Checker && currentY > newY) || currentSquare == player1King))   ||
               (( playerTurn == 2 && ((currentSquare == player2Checker && newY > currentY) || currentSquare == player2King)))) &&
                  newSquare  == emptySquare && currentX != newX && currentY != newY)
            {
                // If jumping a piece and the diagonal space behind the other player's piece is empty
                if (Math.Abs(diffX) == 2 && Math.Abs(diffY) == 2 && midSquare != emptySquare && midSquare != currentSquare && newSquare == emptySquare)
                {
                    // Remove the player piece from the old square
                    boardArray[currentX, currentY] = emptySquare;
                    // Place the player piece in the diagonal space behind the opponent player piece
                    boardArray[newX, newY] = currentSquare;
                    // Remove the opponent player piece
                    boardArray[midX, midY] = emptySquare;
                    // Check if a checker piece became a king piece in the new square
                    CheckKingPiece();
                    // End the turn and go to the next player's turn
                    playerTurn++;
                    // Restart the game loop
                    GameLoop();
                }// End if jumping piece

                // Else if the player is moving to an empty diagonal square next to a player
                else if (Math.Abs(diffX) == 1 && Math.Abs(diffY) == 1 && newSquare == emptySquare)
                {
                    // Remove the player piece from the old square
                    boardArray[currentX, currentY] = emptySquare;
                    // Place the player piece in the new square
                    boardArray[newX, newY] = currentSquare;
                    // Check if a checker piece became a king piece in the new square
                    CheckKingPiece();
                    // End the turn and go to the next player's turn
                    playerTurn++;
                    // Restart the game loop
                    GameLoop();
                }// End else if moving to an empty diagonal square

                // Else the new square is not valid
                else
                {
                    // Display message that the move is not valid
                    Console.WriteLine("\tThis is not a valid move. Press any key to try again.");
                    // Require a key press to continue
                    Console.ReadKey();
                    // Restart the game loop
                    GameLoop();
                }// End else if the square is not valid
            }// End if player is moving

            // Else a player piece was not selected
            else
            {
                // Display message that the move is not valid
                Console.WriteLine("\tThis is not a valid move. Press any key to try again.");
                // Require a key press to continue
                Console.ReadKey();
                // Restart the game loop
                GameLoop();
            }// End else a player piece was not selected
        }// End MovePiece()
    }// End class Program
}// End namespace