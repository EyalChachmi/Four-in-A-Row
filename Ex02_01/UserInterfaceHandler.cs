using System;

namespace Ex02_01
{
    internal class UserInterfaceHandler
    {
        public static void StartGame()
        {
            bool playAgain = true;// Flag for the game if it'll be replayed
            byte matWidth;
            byte matHeight;
            char gameMode;
            bool gameOver;
            string userInput;
            byte columnPlayerSelect;
            ePlayer currPlayer;

            getUserInputOnGameStart(out gameMode, out matHeight, out matWidth);
            eGameMode currGameMode = (eGameMode)gameMode;
            GameHandler gameHandler = new GameHandler(gameMode, matWidth, matHeight);

            while (playAgain)
            {
                currPlayer = ePlayer.Player1;//Set starting player to player 1
                gameHandler.Coin = 'X'; 
                gameOver = false;
                gameHandler.StartingBoard(); //Initialize game board
                while (!gameOver)
                {
                    Ex02.ConsoleUtils.Screen.Clear();
                    printMatrix(gameHandler.GameMatrix);
                    if (currPlayer == ePlayer.Player1 || currGameMode == eGameMode.HumanVsHuman)//check if its a human player turn
                    {
                        Console.WriteLine(string.Format($"{currPlayer}, The turn is yours. "));
                        userInput = getUserInputOnColumnSelect(gameHandler.GameMatrix.GetLength(0));
                        if (userInput.ToUpper() == "Q") // quit if user enters "Q"
                        {
                            if (currPlayer == ePlayer.Player1)//update score based on the current player
                            {
                                gameHandler.CalculatePlayerOrComputerScore((char)ePlayer.Player2);
                            }
                            else
                            {
                                gameHandler.CalculatePlayerOrComputerScore((char)ePlayer.Player1);
                            }

                            break;
                        }

                        columnPlayerSelect = byte.Parse(userInput);//parse user input as byte for column selection
                    }
                    else
                    {
                        Console.WriteLine("The computer is making now a move......");
                        System.Threading.Thread.Sleep(500);
                        columnPlayerSelect = (byte)gameHandler.GetComputerMove();
                    }

                    if (!gameHandler.CheckIfTheCurrentColumnIsFull(columnPlayerSelect))// check if the selected column is not full
                    {
                        gameHandler.DropCoinInsideBoard(columnPlayerSelect);
                        if (gameHandler.CheckWin((char)currPlayer))// display winner message and score
                        {
                            Ex02.ConsoleUtils.Screen.Clear();
                            printMatrix(gameHandler.GameMatrix);
                            if (currGameMode == eGameMode.HumanVsComputer)
                            {
                                Console.WriteLine(string.Format("{0} wins!", currPlayer == ePlayer.Player1 ? currPlayer.ToString() : "Computer"));
                            }
                            else
                            {
                                Console.WriteLine(string.Format($"{currPlayer} wins!"));
                            }

                            gameOver = true;
                        }
                        else if (gameHandler.CheckIfBoardIsFullInCaseOfTie())
                        {
                            Ex02.ConsoleUtils.Screen.Clear();
                            printMatrix(gameHandler.GameMatrix);
                            Console.WriteLine("Its a tie!!!");
                            gameOver = true;
                        }
                        else
                        {
                            currPlayer = currPlayer == ePlayer.Player1 ? ePlayer.Player2 : ePlayer.Player1;//Switch between players
                        }
                    }
                    else
                    {
                        if (!(currGameMode == eGameMode.HumanVsComputer && currPlayer == ePlayer.Player2))//display message to the human player if column is full
                        {
                            Console.WriteLine("Do not submit to a column that is already full!");
                        }

                        System.Threading.Thread.Sleep(2000);
                    }
                }

                showCurrentScore(currGameMode, gameHandler);
                while (true) // loop to handle asking if the user wants to play again
                {
                    Console.WriteLine("Do you want to play again? (Y/N):");
                    string playAgainInput = Console.ReadLine().Trim().ToUpper();

                    if (playAgainInput == "Y")
                    {
                        break; // break out of the inner loop to restart the game
                    }
                    else if (playAgainInput == "N")//exit the game if user wants to quit
                    {
                        playAgain = false;
                        Console.WriteLine("Exiting the game....");
                        System.Threading.Thread.Sleep(1000);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter 'Y' to play again or 'N' to quit.");
                    }
                }
            }
        }

        private static void showCurrentScore(eGameMode i_currGameMode, GameHandler i_GameHandler)
        {
            if (i_currGameMode == eGameMode.HumanVsComputer) //displays score based on gamemode
            {
                Console.WriteLine(string.Format($"Player One has {i_GameHandler.Player1Score} points, Computer has {i_GameHandler.Player2Score} points"));
            }
            else
            {
                Console.WriteLine(string.Format($"Player One has {i_GameHandler.Player1Score} points, Player Two has {i_GameHandler.Player2Score} points"));
            }
        }

        private static void getUserInputOnGameStart(out char o_GameMode, out byte o_MatHeight, out byte o_MatWidth)
        {
            o_GameMode = '1';
            o_MatHeight = 0;
            o_MatWidth = 0;
            bool validInput = false; //flag for valid input
            string matWidth;
            string matHeight;
            string gameMode;

            Console.WriteLine("Welcome to a game of 4 in a row :)");
            while (!validInput)
            {
                Console.WriteLine("Please enter the size of width of the game board (minimum 4 maximum 8)");
                matWidth = Console.ReadLine();
                Console.WriteLine("Please enter the size of Height of the game board (minimum 4 maximum 8)");
                matHeight = Console.ReadLine();
                validInput = checkIfTheDimensionsInputIsValid(matWidth, matHeight);
                if (validInput == false) //If dimensions that the user has submitted are invalid
                {
                    Console.WriteLine("You must enter minimum 4X4 maximum 8X8 for the width and height of the Game Board");
                    continue; // repeat the loop
                }

                Console.WriteLine("Please Select the gamemode! Type 1 for Game against Another player, Type 2 for a game against a Computer");
                gameMode = Console.ReadLine();
                validInput = checkIfGameModeInputIsValid(gameMode);
                if (validInput == false) //if the gamemode that the user has submitted is invalid
                {
                    Console.WriteLine("Please type a valid gamemode: 1 for HumanVsHuman 2 for HumanVsComputer");
                    continue;
                }

                o_GameMode = gameMode[0]; //assign validated inputs to output variables
                o_MatHeight = byte.Parse(matHeight);
                o_MatWidth = byte.Parse(matWidth);
                validInput = true; // validInput flag set to true to exit loop
            }
        }

        private static void printMatrix(char[,] i_GameMat)
        {
            for (int i = 1; i <= i_GameMat.GetLength(0); i++)//prints column numbers
            {
                Console.Write(string.Format($"  {i} "));
            }

            Console.WriteLine("  ");
            for (int j = i_GameMat.GetLength(1); j >= 1; j--) //prints cell contents
            {
                for (int i = 1; i <= i_GameMat.GetLength(0); i++)
                {
                    Console.Write(string.Format($"| {i_GameMat[i - 1, j - 1]} "));
                }

                Console.WriteLine("|");
                for (int k = 1; k <= i_GameMat.GetLength(0); k++)//prints separators 
                {
                    Console.Write("====");
                }

                Console.WriteLine("=");
            }
        }

        private static bool checkIfGameModeInputIsValid(string i_GameMode)
        {
            int validGameMode;

            return int.TryParse(i_GameMode, out validGameMode) && (validGameMode == 1 || validGameMode == 2);
        }

        private static bool checkIfTheDimensionsInputIsValid(string i_MatWidth, string i_MatHeight)
        {
            byte validWidth;
            byte validHeight;

            return byte.TryParse(i_MatWidth, out validWidth) && byte.TryParse(i_MatHeight, out validHeight) &&
                   validWidth >= 4 && validWidth <= 8 && validHeight >= 4 && validHeight <= 8;
        }

        private static bool checkIfPlayersSelectedColumnIsValid(string i_PlayerSelectedColumn, int i_ColumnMax)
        {
            byte selectedColumn;

            return byte.TryParse(i_PlayerSelectedColumn, out selectedColumn) &&
                   selectedColumn >= 1 && selectedColumn <= i_ColumnMax;
        }

        private static string getUserInputOnColumnSelect(int i_ColumnMaxSize)
        {
            bool validInput = false;
            string column = null;

            while (!validInput)
            {
                Console.WriteLine("Please select the column you would like to insert your coin in, or enter 'Q' to quit:");
                column = Console.ReadLine().Trim().ToUpper(); // convert input to uppercase for case-insensitivity
                if (column == "Q")
                {
                    return "Q"; // return "Q" if user wants to quit
                }

                validInput = checkIfPlayersSelectedColumnIsValid(column, i_ColumnMaxSize);
                if (!validInput)
                {
                    Console.WriteLine("You must enter a valid column between the range of the dimensions or 'Q' enter to quit.");
                }
            }

            return column;
        }
    }
}
