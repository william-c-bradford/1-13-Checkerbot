using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

namespace _1_13_Checkerbot
{
    internal class Program
    {
        // Global variables
        static bool runGame           = true;               // Runs the game as long as this is true
        static bool afterFirstMove    = false;              // Determines if the player has not had their first move
        static int  playerTurn        = 1;                  // Sets the current player turn
        static int  currentX;                               // User input of the selected x coordinate for their piece
        static int  currentY;                               // User input of the selected y coordinate for their piece
        static int  newX;                                   // User input of their piece's x destination
        static int  newY;                                   // User input of their piece's y destination
        static int  lastMovedX;                             // Stores the value of the last newX and uses it as a currentX
        static int  lastMovedY;                             // Stores the value of the last newY and uses it as a currentY
        const  int  EMPTY_SQUARE      = 0;                  // Stores the value of an empty square on the board (0)
        const  int  PLAYER_1_CHECKER  = 1;                  // The integer representing player 1's checker piece
        const  int  PLAYER_2_CHECKER  = 2;                  // The integer representing player 2's checker piece
        const  int  PLAYER_1_KING     = 3;                  // The integer representing player 1's king piece
        const  int  PLAYER_2_KING     = 4;                  // The integer representing player 2's king piece
        const  int  JUMPED_PIECE      = 5;                  // The integer representing a jumped piece
        const  int  SQUARE_COUNT      = 8;                  // Number of squares on each row and column of the board (8x8)
        const  int  SQUARE_SCALE      = 5;                  // Scaling the size of each square
        const  int  SQUARE_WIDTH      = 2 * SQUARE_SCALE;   // Scaling the width of each square for font size (font is twice as tall as wide)
        const  int  CHECKER_WIDTH     = 6;                  // The width of a checker piece
        const  int  CHECKER_HEIGHT    = 3;                  // The height of a checker piece
        const  int  CHECKER_START_X   = 6;                  // The left-most x position for a checker piece
        const  int  CHECKER_START_Y   = 4;                  // The top-most y position for a checker piece
        const  int  CHECKER_X_SCALE   = 10;                 // The scale at which the x position of a checker changes between pieces
        const  int  CHECKER_Y_SCALE   = 5;                  // The scale at which the y position of a checker changes between pieces
        const  int  BOARD_OFFSET_X    = 4;                  // The x position offset of the checkerboard from the console window
        const  int  BOARD_OFFSET_Y    = 3;                  // The y position offset of the checkerboard from the console window
        const  int  TEXT_OFFSET_X     = 6;                  // The x position offset of the text below the board
        const  int  TEXT_OFFSET_Y     = 43;                 // The y position offset of the text below the board

        // 2D array of the starting checkerboard
        static int[,] boardArray = new int[SQUARE_COUNT, SQUARE_COUNT]
        {
            { 0, 2, 0, 0, 0, 1, 0, 1 },                     // 0 = Empty space
            { 2, 0, 2, 0, 0, 0, 1, 0 },                     // 1 = Player 1 checker piece
            { 0, 2, 0, 0, 0, 1, 0, 1 },                     // 2 = Player 2 checker piece
            { 2, 0, 2, 0, 0, 0, 1, 0 },                     // 3 = Player 1 king piece
            { 0, 2, 0, 0, 0, 1, 0, 1 },                     // 4 = Player 2 king piece
            { 2, 0, 2, 0, 0, 0, 1, 0 },                     // 5 = Jumped piece
            { 0, 2, 0, 0, 0, 1, 0, 1 },
            { 2, 0, 2, 0, 0, 0, 1, 0 }
        };

        // Testing boardArray[,]
        //{
        //    { 0, 0, 0, 0, 0, 0, 0, 4 },
        //    { 0, 0, 0, 0, 0, 0, 0, 0 },
        //    { 0, 0, 0, 0, 0, 0, 0, 1 },
        //    { 0, 0, 0, 0, 1, 0, 0, 0 },
        //    { 0, 2, 0, 0, 0, 0, 0, 0 },
        //    { 0, 0, 0, 0, 0, 0, 0, 0 },
        //    { 0, 2, 0, 0, 0, 2, 0, 1 },
        //    { 0, 0, 1, 0, 0, 0, 0, 0 }
        //};

        // Function Main()
        static void Main(string[] args)
        {
            // Window title
            Console.Title = "CheckerBot";

            // Play the game
            GameLoop();

            // Require a key press to close the console window
            Console.ReadKey();
        }// End Main()

        // Function to completely fill in a space on the console screen using char #9608
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
            // For loop over the rows
            for (int row = 0; row < width; row++)
            {
                // For loop over the columns
                for (int col = 0; col < height; col++)
                {
                    // Fills the square with a color at a specific (x, y), width, height, and color
                    FillSquare(xPos + row, yPos + col, width, height, color);
                }// End for columns
            }// End for rows
        }// End DrawSquare()

        // Function to draw a checker at any postion and any color
        public static void DrawCheckerPiece(int xPos, int yPos, int width, int height, ConsoleColor color)
        {
            // Draws the checker piece using (x, y), width, height, and color
            DrawSquare(xPos, yPos, width, height, color);
        }// End DrawCheckerPiece()

        // Function to draw a king piece at any position and any color
        public static void DrawKingPiece(int xPos, int yPos, int width, int height, ConsoleColor color)
        {
            // Draws the checker piece using DrawSquare()
            DrawCheckerPiece(xPos, yPos, width, height, color);

            // Draws a smaller "crown" rectangle on top of the checker piece
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
                                    // Draw the checkerboard pattern
            // For loop over rows
            for (int row = 0; row < SQUARE_COUNT; row++)
            {
                // For loop over columns
                for (int col = 0; col < SQUARE_COUNT; col++)
                {
                    // If the square is even
                    if ((row + col) % 2 == 0)
                    {
                        // Draw the square dark red
                        DrawSquare(xPos + row * SQUARE_WIDTH, yPos + col * SQUARE_SCALE, SQUARE_WIDTH,
                                   1 * SQUARE_SCALE, ConsoleColor.DarkRed);
                    }// End if square is even

                    // Else the square is odd
                    else
                    {
                        // Draw the square dark gray
                        DrawSquare(xPos + row * SQUARE_WIDTH, yPos + col * SQUARE_SCALE, SQUARE_WIDTH,
                                   1 * SQUARE_SCALE, ConsoleColor.DarkGray);
                    }// End else square is odd
                }// End for columns

                // Add a space after each loop to start a new row
                Console.WriteLine();
            }// End for rows

                                    // Draw the checkers on the board
            // For loop over rows
            for (int row = 0; row < SQUARE_COUNT; row++)
            {
                // For loop over columns
                for (int col = 0; col < SQUARE_COUNT; col++)
                {
                    // If player 1 checker piece is on board
                    if (boardArray[row, col] == PLAYER_1_CHECKER)
                    {
                        // Draw a player 1 checker piece (black)
                        DrawCheckerPiece(CHECKER_START_X + (row * CHECKER_X_SCALE), CHECKER_START_Y + (col * CHECKER_Y_SCALE),
                                         CHECKER_WIDTH, CHECKER_HEIGHT, ConsoleColor.Black);
                    }// End if player 1 checker piece is on board

                    // Else if player 2 checker piece is on board
                    else if (boardArray[row, col] == PLAYER_2_CHECKER)
                    {
                        // Draw a player 2 checker piece (white)
                        DrawCheckerPiece(CHECKER_START_X + (row * CHECKER_X_SCALE), CHECKER_START_Y + (col * CHECKER_Y_SCALE),
                                         CHECKER_WIDTH, CHECKER_HEIGHT, ConsoleColor.White);
                    }// End else if player 2 checker piece is on board

                    // Else if player 1 king piece is on board
                    else if (boardArray[row, col] == PLAYER_1_KING)
                    {
                        // Draw a player 1 king piece (black with dark yellow crown)
                        DrawKingPiece(CHECKER_START_X + (row * CHECKER_X_SCALE), CHECKER_START_Y + (col * CHECKER_Y_SCALE),
                                      CHECKER_WIDTH, CHECKER_HEIGHT, ConsoleColor.Black);
                    }// End else if player 1 king piece is on board

                    // Else if player 2 king piece is on board
                    else if (boardArray[row, col] == PLAYER_2_KING)
                    {
                        // Draw a player 2 king piece (white with dark yellow crown)
                        DrawKingPiece(CHECKER_START_X + (row * CHECKER_X_SCALE), CHECKER_START_Y + (col * CHECKER_Y_SCALE),
                                      CHECKER_WIDTH, CHECKER_HEIGHT, ConsoleColor.White);
                    }// End else if player 2 king piece is on board

                    // Else if jumped piece is on the board
                    else if (boardArray[row, col] == JUMPED_PIECE)
                    {
                        // Draw a jumped piece (dark blue)
                        DrawCheckerPiece(CHECKER_START_X + (row * CHECKER_X_SCALE), CHECKER_START_Y + (col * CHECKER_Y_SCALE),
                                         CHECKER_WIDTH, CHECKER_HEIGHT, ConsoleColor.DarkBlue);
                    }// End else if jumped piece is on board
                }// End for columns

                // Set the cursor position
                Console.SetCursorPosition(TEXT_OFFSET_X, top: TEXT_OFFSET_Y);
            }// End for rows
        }// End DrawBoard()

        // Function to clear jumped pieces from the board
        public static void ClearJumpedPieces()
        {
            // For rows on the board
            for (int row = 0; row < SQUARE_COUNT; row++)
            {
                // For columns on the board
                for (int col = 0; col < SQUARE_COUNT; col++)
                {
                    // If there are jumped pieces in a square
                    if (boardArray[row, col] == JUMPED_PIECE)
                    {
                        // The square becomes an empty square 
                        boardArray[row, col] = EMPTY_SQUARE;
                    }// End if there are jumped pieces on the board
                }// End for columns on the board
            }// End for rows on the board
        }// End ClearJumpedPieces()

        // Function to make a checker piece a king piece if it reaches the opposide side of the board
        public static void CheckKingPiece(int currentX, int currentY, int newX, int newY)
        {
            // Variable newSquare
            int newSquare = boardArray[newX, newY];

            // If player 1 reaches the opposite end of the board
            if (newY == 0 && newSquare == PLAYER_1_CHECKER)
            {
                // The player 1 checker piece is replaced with a player 1 king piece
                boardArray[newX, newY] = PLAYER_1_KING;

                // The turn ends
                playerTurn++;

                // Jumped pieces are cleared
                ClearJumpedPieces();

                // The game loop restarts
                GameLoop();
            }// End if player 1 reaches the opposite end of the board

            // If player 2 reaches the opposite end of the board
            if (newY == 7 && newSquare == PLAYER_2_CHECKER)
            {
                // The player 2 checker piece is replaced with a player 2 king piece
                boardArray[newX, newY] = PLAYER_2_KING;

                // The turn ends
                playerTurn++;

                // Jumped pieces are cleared
                ClearJumpedPieces();

                // The game loop restarts
                GameLoop();
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
            DrawBoard(BOARD_OFFSET_X, BOARD_OFFSET_Y);

            // Check if a player has no pieces with CheckNoPiecesWin()
            CheckWinConditions();

            // If the game has not finished
            if (runGame == true)
            {
                // Ask the player for their move with GetPlayerMoves()
                GetPlayerMoves();
            }// End if game has not finished
        }// End GameLoop()

        // Function to restart the game
        public static void RestartGame()
        {
            // Ask the user to restart the game or quit
            Console.WriteLine("\t      Restart the game? Press 'y' to restart. Press Enter to exit.");
            
            // Variable to store the input
            ConsoleKeyInfo restartInput = Console.ReadKey();

            // If the input is "y"
            if (restartInput.Key == ConsoleKey.Y)
            {
                // Reset the variables for the game
                afterFirstMove = false;
                playerTurn = 1;
                runGame = true;
                
                // Reset the boardArray to the starting state
                boardArray = new int[SQUARE_COUNT, SQUARE_COUNT]
                {
                    { 0, 2, 0, 0, 0, 1, 0, 1 },
                    { 2, 0, 2, 0, 0, 0, 1, 0 },
                    { 0, 2, 0, 0, 0, 1, 0, 1 },
                    { 2, 0, 2, 0, 0, 0, 1, 0 },
                    { 0, 2, 0, 0, 0, 1, 0, 1 },
                    { 2, 0, 2, 0, 0, 0, 1, 0 },
                    { 0, 2, 0, 0, 0, 1, 0, 1 },
                    { 2, 0, 2, 0, 0, 0, 1, 0 }
                };
                
                // Run the game loop
                GameLoop();
            }// End if the input is "y"

            // If they do not restart the game
            else
            {
                Environment.Exit(0);
            }// End else they do not restart the game
        }// End RestartGame()

        // Function to display winning text
        public static void WinningText(int winner)
        {
            // If player 1 wins
            if (winner == 1)
            {
                // Display winning message
                Console.Write("\t\t\t\t   ******************\n" +
                              "\t\t\t\t   * PLAYER 1 WINS! *\n" +
                              "\t\t\t\t   ******************\n");
                // Ask to restart the game
                RestartGame();
            }// End if player wins

            // If player 2 wins
            else if (winner == 2)
            {
                // Display winning message
                Console.Write("\t\t\t\t   ******************\n" +
                              "\t\t\t\t   * PLAYER 2 WINS! *\n" +
                              "\t\t\t\t   ******************\n");
                // Ask to restart the game
                RestartGame();
            }// End else if player 2 wins

            // If neither player wins
            else if (winner == 3)
            {
                // Display winning message
                Console.Write("\t\t\t\t   ******************\n" +
                              "\t\t\t\t   *     DRAW!!     *\n" +
                              "\t\t\t\t   ******************\n");
                // Ask to restart the game
                RestartGame();
            }// End else if neither player wins
        }// End WinningText()

        // Function to check if a player has no pieces or cannot move
        public static void CheckWinConditions()
        {
            // Variables to set the player count to 0
            int player1Count = 0;
            int player2Count = 0;

            // Variables to set the player moves to false
            bool player1HasMoves = false;
            bool player2HasMoves = false;

            // Count the number of player 1 and player 2 pieces
            for (int i = 0; i < SQUARE_COUNT; i++)
            {
                for (int j = 0; j < SQUARE_COUNT; j++)
                {
                    // If player 1 has pieces on the board
                    if (boardArray[i, j] == PLAYER_1_CHECKER || boardArray[i, j] == PLAYER_1_KING)
                    {
                        player1Count++;
                    }// End if player 1 has pieces on the board

                    // If player 2 has pieces on the board
                    else if (boardArray[i, j] == PLAYER_2_CHECKER || boardArray[i, j] == PLAYER_2_KING)
                    {
                        player2Count++;
                    }// End else if player 2 has pieces on the board
                }// End nested for loop
            }// End for loop to count player 1 and player 2 pieces

            // Find if a player can move their pieces
            for (int i = 0; i < SQUARE_COUNT; i++)
            {
                for (int j = 0; j < SQUARE_COUNT; j++)
                {
                    // If any of player 1's pieces
                    if (boardArray[i, j] == 1 || boardArray[i, j] == 3)
                    {
                        // If player 1 can move
                        if (CanMove(i, j))
                        {
                            player1HasMoves = true;
                        }// End if player 1 can move
                    }// End if player 1

                    // If any of player 2's pieces
                    if (boardArray[i, j] == 2 || boardArray[i, j] == 4)
                    {
                        // If player 2 can move
                        if (CanMove(i, j))
                        {
                            player2HasMoves = true;
                        }// End if player 2 can move
                    }// End if player 2
                }// End nested for
            }// End for to find if player can move

            // If player 2 has no pieces or has no moves player 1 wins
            if (player2Count == 0      || !player2HasMoves)
            {
                WinningText(1);
            } // End if player 1 wins
            
            // If player 1 has no pieces or has no moves player 2 wins
            else if (player1Count == 0 || !player1HasMoves)
            {
                WinningText(2);
            }// End if player 2 wins

            // Else if neither player has moves
            else if (!player1HasMoves  && !player2HasMoves)
            {
                WinningText(3);
            }// End else if neither player has moves
        }// End CheckWinConditions()

        // Function to check for a valid move
        public static bool IsValidMove(int currentX, int currentY, int newX, int newY)
        {
            // Variables
            int currentSquare = boardArray[currentX, currentY];
            int newSquare     = boardArray[newX, newY];

            // If the correct square and piece are selected and the new square is empty and diagonal
            if ((playerTurn == 1 && currentSquare == PLAYER_1_CHECKER || currentSquare == PLAYER_1_KING) ||
                (playerTurn == 2 && currentSquare == PLAYER_2_CHECKER || currentSquare == PLAYER_2_KING) &&
                 newSquare == EMPTY_SQUARE && currentX != newX && currentY != newY)
            {
                return true;
            }// End if move is valid

            // If move is invalid
            return false;
        }// End IsValidMove()

        // Function to check for legal jump
        public static bool CanJump(int currentX, int currentY, int newX, int newY)
        {
            // Variables
            int currentSquare = boardArray[currentX, currentY];
            int newSquare = boardArray[newX, newY];
            int diffX = newX - currentX;
            int diffY = newY - currentY;
            int midX = (newX + currentX) / 2;
            int midY = (newY + currentY) / 2;
            int midSquare = boardArray[midX, midY];

            // If player 1 piece
            if (currentSquare == PLAYER_1_CHECKER || currentSquare == PLAYER_1_KING)
            {
                // If the piece is on the board
                if (currentX > 1 && currentY < SQUARE_COUNT - 2 || currentX > 1 && currentY > 1 ||
                    currentX < SQUARE_COUNT - 2 && currentY < SQUARE_COUNT - 2 || currentX < SQUARE_COUNT - 2 && currentY > 1)
                {
                    // If the diagonal square next to the player has an opponent piece and the square behind that is empty
                    if (Math.Abs(diffX) == 2 && Math.Abs(diffY) == 2 && newSquare == EMPTY_SQUARE &&
                    midSquare != EMPTY_SQUARE && midSquare != PLAYER_1_CHECKER && midSquare != PLAYER_1_KING && midSquare != JUMPED_PIECE)
                    {
                        // Legal jump
                        return true;
                    }// End if the jump is a valid move

                    // If the diagonal is more than 2 squares away
                    if (Math.Abs(diffX) > 2 && Math.Abs(diffY) > 2 && newSquare == EMPTY_SQUARE)
                    {
                        // If the piece is a king
                        if (currentSquare == PLAYER_1_KING)
                        {
                            // Resets notEmptyCount to 0
                            int notEmptyCount = 0;

                            // For loop between 2 and the number of squares jumped
                            for (int i = 2; i < Math.Abs(diffX); i++)
                            {
                                // If down right
                                if (boardArray[newX, newY] == EMPTY_SQUARE && currentX < SQUARE_COUNT - 2 &&
                                    currentY < SQUARE_COUNT - 2 && newX > 0 && newY > 0)
                                {
                                    // If the square diagonal to the new square has an opponent piece
                                    if (boardArray[newX - 1, newY - 1] != EMPTY_SQUARE     &&
                                        boardArray[newX - 1, newY - 1] != PLAYER_1_CHECKER &&
                                        boardArray[newX - 1, newY - 1] != PLAYER_1_KING    &&
                                        boardArray[newX - 1, newY - 1] != JUMPED_PIECE)
                                    {
                                        // If the squares between are not empty
                                        if (boardArray[newX - i, newY - i] != EMPTY_SQUARE)
                                        {
                                            // Increase the count for not empty squares
                                            notEmptyCount++;
                                        }// End if the squares between are not empty
                                    }// End if the square diagonal to the new square has an opponent piece
                                }// End if down right

                                // If down left
                                if (boardArray[newX, newY] == EMPTY_SQUARE && currentX < SQUARE_COUNT - 2 &&
                                    currentY > 1 && i <= currentY && newX > 0 && newY < SQUARE_COUNT - 1)
                                {
                                    // If the square diagonal to the new square has an opponent piece
                                    if (boardArray[newX - 1, newY + 1] != EMPTY_SQUARE     &&
                                        boardArray[newX - 1, newY + 1] != PLAYER_1_CHECKER &&
                                        boardArray[newX - 1, newY + 1] != PLAYER_1_KING    &&
                                        boardArray[newX - 1, newY + 1] != JUMPED_PIECE)
                                    {
                                        // If the squares between are not empty
                                        if (boardArray[newX - i, newY + i] != EMPTY_SQUARE)
                                        {
                                            // Increase the count for not empty squares
                                            notEmptyCount++;
                                        }// End if the squares between are not empty
                                    }// End if the square diagonal to the new square has an opponent piece
                                }// End if down left

                                // If up right
                                if (boardArray[newX, newY] == EMPTY_SQUARE && currentX > 1 && currentY < SQUARE_COUNT - 2 &&
                                    i <= currentX && newX < SQUARE_COUNT - 1 && newY > 0)
                                {
                                    // If the square diagonal to the new square has an opponent piece
                                    if (boardArray[newX + 1, newY - 1] != EMPTY_SQUARE     &&
                                        boardArray[newX + 1, newY - 1] != PLAYER_1_CHECKER &&
                                        boardArray[newX + 1, newY - 1] != PLAYER_1_KING    &&
                                        boardArray[newX + 1, newY - 1] != JUMPED_PIECE)
                                    {
                                        // If the squares are not empty
                                        if (boardArray[newX + i, newY - i] != EMPTY_SQUARE)
                                        {
                                            // Increase the count for not empty squares
                                            notEmptyCount++;
                                        }// End if the squares between are not empty
                                    }// End if the square diagonal to the new square has an opponent piece
                                }// End if up right

                                // If up left
                                if (boardArray[newX, newY] == EMPTY_SQUARE && currentX > 1 && currentY > 1 &&
                                    i <= currentX && i <= currentY && newX < SQUARE_COUNT - 1 && newY < SQUARE_COUNT - 1)
                                {
                                    // If the square diagonal to the new square has an opponent piece
                                    if (boardArray[newX + 1, newY + 1] != EMPTY_SQUARE     &&
                                        boardArray[newX + 1, newY + 1] != PLAYER_1_CHECKER &&
                                        boardArray[newX + 1, newY + 1] != PLAYER_1_KING    &&
                                        boardArray[newX + 1, newY + 1] != JUMPED_PIECE)
                                    {
                                        // If the squares are not empty
                                        if (boardArray[newX + i, newY + i] != EMPTY_SQUARE)
                                        {
                                            // Increase the count for not empty squares
                                            notEmptyCount++;
                                        }// End if the squares are not empty
                                    }// End if the square diagonal to the new square has an opponent piece
                                }// End if up left
                            }// End for loop between 2 and the number of squares jumped

                            // If there are no empty squares when flying
                            if (notEmptyCount == 0)
                            {
                                // There were only empty squares between the jump
                                return true;
                            }// End if there are no empty squares when flying
                        }// End if the piece is a king
                    }// End if the diagonal is more than 2 squares away
                }// End if the piece is on the board
            }// End if player 1 piece

            // If player 2 piece
            if (currentSquare == PLAYER_2_CHECKER || currentSquare == PLAYER_2_KING)
            {
                // If the piece is on the board
                if (currentX > 1 && currentY < SQUARE_COUNT - 2 || currentX > 1 && currentY > 1 ||
                    currentX < SQUARE_COUNT - 2 && currentY < SQUARE_COUNT - 2 || currentX < SQUARE_COUNT - 2 && currentY > 1)
                {
                    // If the diagonal square next to the player has an opponent piece and the square behind that is empty
                    if (Math.Abs(diffX) == 2 && Math.Abs(diffY) == 2 && newSquare == EMPTY_SQUARE &&
                    midSquare != EMPTY_SQUARE && midSquare != PLAYER_2_CHECKER && midSquare != PLAYER_2_KING && midSquare != JUMPED_PIECE)
                    {
                        return true;
                    }// End if the jump is a valid move

                    // If the diagonal is more than 2 squares away
                    if (Math.Abs(diffX) > 2 && Math.Abs(diffY) > 2 && newSquare == EMPTY_SQUARE)
                    {
                        // If the piece is a king
                        if (currentSquare == PLAYER_2_KING)
                        {
                            // Resets notEmptyCount to 0
                            int notEmptyCount = 0;

                            // For loop between 2 and the number of squares jumped
                            for (int i = 2; i < Math.Abs(diffX); i++)
                            {
                                // If down right
                                if (boardArray[newX, newY] == EMPTY_SQUARE && currentX < SQUARE_COUNT - 2 &&
                                    currentY < SQUARE_COUNT - 2 && newX > 0 && newY > 0)
                                {
                                    // If the square diagonal to the new square has an opponent piece
                                    if (boardArray[newX - 1, newY - 1] != EMPTY_SQUARE     &&
                                        boardArray[newX - 1, newY - 1] != PLAYER_2_CHECKER &&
                                        boardArray[newX - 1, newY - 1] != PLAYER_2_KING    &&
                                        boardArray[newX - 1, newY - 1] != JUMPED_PIECE)
                                    {
                                        // If the squares are not empty
                                        if (boardArray[newX - i, newY - i] != EMPTY_SQUARE)
                                        {
                                            // Increase the count for not empty squares
                                            notEmptyCount++;
                                        }// End if the squares are not empty
                                    }// End if the square diagonal to the new square has an opponent piece
                                }// End if down right

                                // If down left
                                if (boardArray[newX, newY] == EMPTY_SQUARE && currentX < SQUARE_COUNT - 2 &&
                                    currentY > 1 && i <= currentY && newX > 0 && newY < SQUARE_COUNT - 1)
                                {
                                    // If the square diagonal to the new square has an opponent piece
                                    if (boardArray[newX - 1, newY + 1] != EMPTY_SQUARE     &&
                                        boardArray[newX - 1, newY + 1] != PLAYER_2_CHECKER &&
                                        boardArray[newX - 1, newY + 1] != PLAYER_2_KING    &&
                                        boardArray[newX - 1, newY + 1] != JUMPED_PIECE)
                                    {
                                        // If the squares are not empty
                                        if (boardArray[newX - i, newY + i] != EMPTY_SQUARE)
                                        {
                                            // Increase the count for not empty squares
                                            notEmptyCount++;
                                        }// End if the squares are not empty
                                    }// End if the square diagonal to the new square has an opponent piece
                                }// End if down left

                                // If up right
                                if (boardArray[newX, newY] == EMPTY_SQUARE && currentX > 1 && currentY < SQUARE_COUNT - 2 &&
                                    i <= currentX && newX < SQUARE_COUNT - 1 && newY > 0)
                                {
                                    // if the square diagonal to the new square has an opponent piece
                                    if (boardArray[newX + 1, newY - 1] != EMPTY_SQUARE     &&
                                        boardArray[newX + 1, newY - 1] != PLAYER_2_CHECKER &&
                                        boardArray[newX + 1, newY - 1] != PLAYER_2_KING    &&
                                        boardArray[newX + 1, newY - 1] != JUMPED_PIECE)
                                    {
                                        // If the squares are not empty
                                        if (boardArray[newX + i, newY - i] != EMPTY_SQUARE)
                                        {
                                            // Increase the count for not empty squares
                                            notEmptyCount++;
                                        }// End if the squares are not empty
                                    }// End if the square diagonal to the new square has an opponent piece
                                }// End if up right

                                // If up left
                                if (boardArray[newX, newY] == EMPTY_SQUARE && currentX > 1 && currentY > 1 &&
                                    i <= currentX && i <= currentY && newX < SQUARE_COUNT - 1 && newY < SQUARE_COUNT - 1)
                                {
                                    // If the square diagonal to the new square has an opponent piece
                                    if (boardArray[newX + 1, newY + 1] != EMPTY_SQUARE     &&
                                        boardArray[newX + 1, newY + 1] != PLAYER_2_CHECKER &&
                                        boardArray[newX + 1, newY + 1] != PLAYER_2_KING    &&
                                        boardArray[newX + 1, newY + 1] != JUMPED_PIECE)
                                    {
                                        // If the squares are not empty
                                        if (boardArray[newX + i, newY + i] != EMPTY_SQUARE)
                                        {
                                            // Increase the count for not empty squares
                                            notEmptyCount++;
                                        }// End if the squares are not empty
                                    }// End if the square diagonal to the new square has an opponent piece
                                }// End if up left
                            }// End for loop between 2 and the number of squares jumped

                            // If there are no empty squares when flying
                            if (notEmptyCount == 0)
                            {
                                // There were only empty squares between the jump
                                return true;
                            }// End if there are no empty squares when flying
                        }// End if the piece is a king
                    }// End if the diagonal is more than 2 squares away
                }// End if the piece is on the board
            }// End if player 2 piece
            return false;
        }// End CanJump()

        // Function to jump a piece
        public static void JumpMove(int currentX, int currentY, int newX, int newY)
        {
            // If jumping piece is checker
            if (PieceIsChecker(currentX, currentY))
            {
                // Variables
                int midX = (newX + currentX) / 2;
                int midY = (newY + currentY) / 2;

                // Place the player piece in the diagonal space behind the opponent player piece
                boardArray[newX, newY] = boardArray[currentX, currentY];
                // Remove the player piece from the old square
                boardArray[currentX, currentY] = EMPTY_SQUARE;
                // Remove the opponent player piece
                boardArray[midX, midY] = JUMPED_PIECE;
                // Check if a checker piece became a king piece in the new square
                CheckKingPiece(currentX, currentY, newX, newY);
            }// End if jumping piece is checker

            // If jumping piece is king
            if (PieceIsKing(currentX, currentY))
            {
                // Place the player piece in the diagonal space behind the opponent player piece
                boardArray[newX, newY] = boardArray[currentX, currentY];
                // Remove the player piece from the old square
                boardArray[currentX, currentY] = EMPTY_SQUARE;

                // If down right
                if (newY > currentY && newX > currentX)
                {
                    // Change mid variables
                    int midX = newX - 1;
                    int midY = newY - 1;
                    
                    // Remove the opponent player piece
                    boardArray[midX, midY] = JUMPED_PIECE;
                }// End if down right

                // Else if down left
                else if (newY > currentY && newX < currentX)
                {
                    // Change mid variables
                    int midX = newX + 1;
                    int midY = newY - 1;

                    boardArray[midX, midY] = JUMPED_PIECE;
                }// End else if down left

                // Else if up right
                else if (newY < currentY && newX > currentX)
                {
                    // Change mid variables
                    int midX = newX - 1;
                    int midY = newY + 1;
                    
                    boardArray[midX, midY] = JUMPED_PIECE;
                }// End else if up right

                // Else if up left
                else if (newY < currentY && newX < currentX)
                {
                    // Change mid variables
                    int midX = newX + 1;
                    int midY = newY + 1;

                    boardArray[midX, midY] = JUMPED_PIECE;
                }// End else if up left
            }// End if jumping piece is king
        }// End JumpMove()

        // Function to check for legal diagonal move
        public static bool CanDiagonalMove(int currentX, int currentY, int newX, int newY)
        {
            // The current square selected
            int currentSquare = boardArray[currentX, currentY];

            // If the square is on the boardArray[,]
            if ((currentX > 0 || currentX < SQUARE_COUNT - 1) && (currentY > 0 || currentY < SQUARE_COUNT - 1))
            {
                // The destination sqaure
                int newSquare = boardArray[newX, newY];

                // The difference between the current and new squares
                int diffX = newX - currentX;
                int diffY = newY - currentY;

                // If the new square is empty and diagonal to current square
                if (Math.Abs(diffX) == 1 && Math.Abs(diffY) == 1 && newSquare == EMPTY_SQUARE)
                {
                    // If player 1 checker piece is moving up or player 2 checker piece is moving down
                    if ((currentSquare == PLAYER_1_CHECKER && currentY > newY) ||
                        (currentSquare == PLAYER_2_CHECKER && newY > currentY))
                    {
                        // The piece can diagonal move
                        return true;
                    }// End if checker piece

                    // If the piece is a king
                    if (PieceIsKing(currentX, currentY))
                    {
                        // The piece can diagonal move
                        return true;
                    }// End if the piece is a king
                }// End if new square is empty and diagonal
            }// End if the square is on the boardArray[,]

            // If the square is not on the boardArray[,]
            return false;
        }// End CanDiagonalMove()

        // Function to move diagonally
        public static void DiagonalMove(int currentX, int currentY, int newX, int newY)
        {
            // Place the player piece in the new square
            boardArray[newX, newY]         = boardArray[currentX, currentY];
            // Remove the player piece from the old square
            boardArray[currentX, currentY] = EMPTY_SQUARE;
            // Check if a checker piece became a king piece in the new square
            CheckKingPiece(currentX, currentY, newX, newY);
        }// End DiagonalMove()

        // Function to check if a piece is a checker piece
        public static bool PieceIsChecker(int currentX, int currentY)
        {
            // Variable
            int currentSquare = boardArray[currentX, currentY];

            // If the value of the array is a checker piece
            if (currentSquare == PLAYER_1_CHECKER || currentSquare == PLAYER_2_CHECKER)
            {
                // It is a checker piece
                return true;
            }// End if value of the array is a checker piece

            // It is not a checker piece
            return false;
        }// End PieceIsChecker()

        // Function to check if a piece is a king piece
        public static bool PieceIsKing(int currentX, int currentY)
        {
            // Variable
            int currentSquare = boardArray[currentX, currentY];
            // If the value of the array is a king piece
            if (currentSquare == PLAYER_1_KING || currentSquare == PLAYER_2_KING)
            {
                // It is a king piece
                return true;
            }// End if the value of the array is a king piece

            // It is not a king piece
            return false;
        }// End PieceIsKing()

        // Function to check if a piece has a legal move between turns
        public static bool CanMove(int currentX, int currentY)
        {
            // If piece is a checker or king
            if (PieceIsChecker(currentX, currentY) || PieceIsKing(currentX, currentY))
            {
                                        // Diagonal moves
                // If up left
                if (currentX > 0 && currentY > 0 &&
                    CanDiagonalMove(currentX, currentY, (currentX - 1), (currentY - 1)))
                {
                    // The piece can move
                    return true;
                }// End if up left

                // If down left
                if (currentX > 0 && currentY < SQUARE_COUNT - 1 &&
                    CanDiagonalMove(currentX, currentY, (currentX - 1), (currentY + 1)))
                {
                    // The piece can move
                    return true;
                }// End if down left

                // If up right
                if (currentX < SQUARE_COUNT - 1 && currentY > 0 &&
                    CanDiagonalMove(currentX, currentY, (currentX + 1), (currentY -1)))
                {
                    // The piece can move
                    return true;
                }// End if up right

                // If down right
                if (currentX < SQUARE_COUNT - 1 && currentY < SQUARE_COUNT - 1 &&
                    CanDiagonalMove(currentX, currentY, (currentX + 1), (currentY + 1)))
                {
                    // The piece can move
                    return true;
                }// End if down right

                                        // Jump moves
                // If up left
                if (currentX > 1 && currentY > 1 &&
                    CanJump(currentX, currentY, (currentX - 2), (currentY - 2)))
                {
                    // The piece can move
                    return true;
                }// End if up left

                // If down left
                if (currentX > 1 && currentY < SQUARE_COUNT - 2 &&
                    CanJump(currentX, currentY, (currentX - 2), (currentY + 2)))
                {
                    // The piece can move
                    return true;
                }// End if down left

                // If up right
                if (currentX < SQUARE_COUNT - 2 && currentY > 1 &&
                    CanJump(currentX, currentY, (currentX + 2), (currentY - 2)))
                {
                    // The piece can move
                    return true;
                }// End if up right

                // If down right
                if (currentX < SQUARE_COUNT - 2 && currentY < SQUARE_COUNT - 2 &&
                    CanJump(currentX, currentY, (currentX + 2), (currentY + 2)))
                {
                    // The piece can move
                    return true;
                }// End if down right
            }// End if piece is checker or king

            // The piece cannot move
            return false;
        }// End CanMove()

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

            // If the player has had their first move
            if (afterFirstMove)
            {
                // Asks the user to end their turn or to enter the piece they want to move and where they want to move it
                Console.Write("\tType 'end' to end your turn, or enter the column and row of another empty" +
                              "\n\tsquare to continue jumping your opponent." +
                              "\n\t(ex: 34): ");
            }// End if the player has had their first move

            // Else the player has not moved yet
            else
            {
                // Asks the user to enter the piece they want to move and where they want to move it
                Console.Write("\tEnter the column and row of your piece followed by a '-' character" +
                              "\n\tand the column and row of a new, empty square." +
                              "\n\t(ex: 25-34): ");
            }// End else the player has not moved yet

            // Reads the input from the user
            string userCoords = Console.ReadLine();

            // Try to split the user input into (x, y) coords for boardArray[,] and move pieces
            try
            {
                // If it is the first move of the turn
                if (!afterFirstMove)
                {
                    // Convert the individual numbers from the input string to integers
                    currentX   = int.Parse(userCoords[0].ToString());
                    currentY   = int.Parse(userCoords[1].ToString());
                    newX       = int.Parse(userCoords[3].ToString());
                    newY       = int.Parse(userCoords[4].ToString());

                    // Store the new square (x, y) coords as the last square moved
                    lastMovedX = newX;
                    lastMovedY = newY;

                    // Move the piece on the board using MovePiece()
                    MovePiece(currentX, currentY, newX, newY);
                }// End if it is the first move of the turn

                // If it is AFTER the first move of the turn
                else if (afterFirstMove)
                {
                    // If the player ends their turn
                    if (userCoords.ToLower() == "end")
                    {
                        afterFirstMove = false;
                        ClearJumpedPieces();
                        playerTurn++;
                        GameLoop();
                    }// End if the player ends their turn

                    // Else the player jumps a piece
                    else
                    {
                        // Reset the values of the last moved square
                        lastMovedX = newX;
                        lastMovedY = newY;

                        // Take a user input, parse it to (x, y), and jump to that square
                        newX = int.Parse(userCoords[0].ToString());
                        newY = int.Parse(userCoords[1].ToString());

                        // Move the piece to the user coords
                        MovePiece(lastMovedX, lastMovedY, newX, newY);
                    }// End else the player jumps a piece
                }// End else if it is AFTER the first move of the turn
            }// End try

            // Catch input error
            catch
            {
                // Display a message that the wrong format was entered
                Console.WriteLine("\tYou did not use the correct format. Press any key to try again.");

                // Require a key press to continue
                Console.ReadKey();

                // Restart the game loop
                GameLoop();
            }// End catch input error
        }// End GetPlayerMoves()

        // Moves the checker pieces on the board
        public static void MovePiece(int currentX, int currentY, int newX, int newY)
        {
            // Assign the current value of boardArray[,] to the variable currentSquare
            int currentSquare = boardArray[currentX, currentY];

            // Assign the new value of boardArray[,] to the variable newSquare
            int newSquare     = boardArray[newX, newY];

            // Variables to store the middle square between current(x,y) and new(x,y)
            int midX          = (newX + currentX) / 2;
            int midY          = (newY + currentY) / 2;
            int midSquare     = (boardArray[midX, midY]);

            // Variables to store the difference between current position and new position
            int diffX         = Math.Abs(newX - currentX);
            int diffY         = Math.Abs(newY - currentY);
            
            // If the player's first move for the turn
            if (!afterFirstMove)
            {
                // If the player can jump
                if (CanJump(currentX, currentY, newX, newY))
                {
                    // If the move is valid
                    if (IsValidMove(currentX, currentY, newX, newY))
                    {
                        // Jump a piece
                        JumpMove(currentX, currentY, newX, newY);

                        // It is no longer the player's first move
                        afterFirstMove = true;

                        // Restart the game loop
                        GameLoop();
                    }// End if the move is valid

                    // Else the move is invalid
                    else
                    {
                        // Display message that the move is not valid
                        Console.WriteLine("\tThis is not a valid move. Press any key to try again.");

                        // Require a key press to continue
                        Console.ReadKey();

                        // Restart the game loop
                        GameLoop();
                    }// End else the move is invalid
                }// End if the player can jump

                                        // A player's turn ends after they move diagonally
                // Else if the player can move diagonally
                else if (CanDiagonalMove(currentX, currentY, newX, newY))
                {
                    // If the move is valid
                    if (IsValidMove(currentX, currentY, newX, newY) &&
                        PieceIsChecker(currentX, currentY) || PieceIsKing(currentX, currentY))
                    {
                        // Use DiagonalMove() to move to an empty square
                        DiagonalMove(currentX, currentY, newX, newY);

                        // End the turn and go to the next player's turn
                        playerTurn++;

                        // Restart the game loop
                        GameLoop();
                    }// End if the move is valid

                    // Else the move is invalid
                    else
                    {
                        // Display message that the move is not valid
                        Console.WriteLine("\tThis is not a valid move. Press any key to try again.");
                        // Require a key press to continue
                        Console.ReadKey();
                        // Restart the game loop
                        GameLoop();
                    }// End else the move is invalid
                }// End else if the player can move diagonally

                // Else the player's input is not a valid jump or diagonal move
                else
                {
                    // Display message that the move is not valid
                    Console.WriteLine("\tThis is not a valid move. Press any key to try again.");

                    // Require a key press to continue
                    Console.ReadKey();

                    // Restart the game loop
                    GameLoop();
                }// End else the player's input is not a valid jump or diagonal move
            }// End if player's first move for the turn

                                    // If a player jumps, their turn doesn't end in case they can double jump
            // Else it is not the player's first move of a turn
            else
            {
                // If the player can jump
                if (CanJump(lastMovedX, lastMovedY, newX, newY))
                {
                    // If the move is valid
                    if (IsValidMove(lastMovedX, lastMovedY, newX, newY))
                    {
                        // Use JumpMove() to jump a piece
                        JumpMove(lastMovedX, lastMovedY, newX, newY);
                        // Restart the game loop
                        GameLoop();
                    }// End if the move is valid
                }// End if the player can jump

                // Else the player's input is not a valid jump
                else
                {
                    // Display message that the move is not valid
                    Console.WriteLine("\tThis is not a valid move. Press any key to try again.");

                    // Require a key press to continue
                    Console.ReadKey();

                    // Restart the game loop
                    GameLoop();
                }// End else the player's input is not a valid jump
            }// End else it is not the first move of a turn
        }// End MovePiece()

    }// End class Program
}// End namespace _1_13_Checkerbot