using System;
using System.Collections.Generic;

namespace Ex02_01
{
    public class GameHandler
    {
        private int m_Player1Score; // Human Player 1 score
        private int m_Player2Score; //Human Player 2 or computer score
        private const byte k_FourInARow = 4;
        private char[,] m_GameMatrix;
        private char m_Coin;
        private readonly char r_GameMode;

        public int Player2Score //Player 2 Score or computer Score
        {
            get
            {
                return m_Player2Score;
            }
        }

        public int Player1Score // Human Player 1 score
        {
            get
            {
                return m_Player1Score;
            }
        }

        public char GameMode
        {
            get
            {
                return r_GameMode;
            }
        }

        public char Coin
        {
            get
            {
                return m_Coin;
            }
            set
            {
                m_Coin = value;
            }
        }

        public char[,] GameMatrix
        {
            get
            {
                return m_GameMatrix;
            }
        }

        public GameHandler(char i_GameMode, byte i_MatWidth, byte i_MatHeight)
        {
            this.m_Coin = 'X';
            this.r_GameMode = i_GameMode;
            this.m_GameMatrix = new char[i_MatWidth, i_MatHeight];
        }

        public void StartingBoard()//initializes the game board by setting all elements of the matrix to ' '
        {
            for (int i = 0; i < m_GameMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < m_GameMatrix.GetLength(1); j++)
                {
                    m_GameMatrix[i, j] = ' ';
                }
            }
        }

        private void changeCoinForTheNextPlayer() //between two different coins ('O' and 'X') it alternates turns between players in the game
        {
            if (m_Coin == 'O')
            {
                m_Coin = 'X';
            }
            else
            {
                m_Coin = 'O';
            }
        }

        public bool CheckIfTheCurrentColumnIsFull(int i_Column) //this method checks whether it's possible to place a coin in the specified column
        {
            return m_GameMatrix[i_Column - 1, m_GameMatrix.GetLength(1) - 1] != ' ';
        }

        public void DropCoinInsideBoard(int i_Column)
        {
            for (int i = 0; i < m_GameMatrix.GetLength(1); i++)
            {
                if (m_GameMatrix[i_Column - 1, i] == ' ')//If the current position is empty, it means that there's space available to drop the coin in that column at the current row
                {
                    m_GameMatrix[i_Column - 1, i] = m_Coin;
                    changeCoinForTheNextPlayer();//to switch the coin for the next player.

                    return;
                }
            }
        }

        public int GetComputerMove()//generates a random move for the computer player
        {
            List<int> freeColumns = new List<int>();

            for (int col = 1; col <= m_GameMatrix.GetLength(0); col++)//first we check every column that is free
            {
                if (!CheckIfTheCurrentColumnIsFull(col))
                {
                    freeColumns.Add(col); // // Add the free column index to the list
                }
            }

            Random random = new Random();
            int randomIndex = random.Next(0, freeColumns.Count);// we randomly take a column from the list of free columns

            return freeColumns[randomIndex];
        }

        public bool CheckIfBoardIsFullInCaseOfTie()
        {
            bool allColumnsFull = true;

            for (int i = 0; i < m_GameMatrix.GetLength(0); i++)
            {
                if (!CheckIfTheCurrentColumnIsFull(i + 1))
                {
                    allColumnsFull = false; // If any column is not full, set the flag to false
                    break;
                }
            }

            return allColumnsFull; // Return the flag indicating whether all columns are full
        }

        public void CalculatePlayerOrComputerScore(char i_Coin)//responsible for updating the score of either Player 1 or Player 2/Computer after a win
        {
            if (i_Coin == (char)ePlayer.Player1)
            {
                m_Player1Score++;

                return;
            }
            m_Player2Score++;
        }

        public bool CheckWin(char i_Coin)
        {
            bool win = false;
            int matHeight = m_GameMatrix.GetLength(1);
            int matWidth = m_GameMatrix.GetLength(0);

            for (int j = 0; j < matHeight; j++)//Checks if there are 4 coins Horizontally
            {
                for (int i = 0; i <= matWidth - k_FourInARow; i++)
                {
                    if (m_GameMatrix[i, j] == i_Coin && m_GameMatrix[i + 1, j] == i_Coin && m_GameMatrix[i + 2, j] == i_Coin && m_GameMatrix[i + 3, j] == i_Coin)
                    {
                        CalculatePlayerOrComputerScore(i_Coin);
                        win = true;
                        break;
                    }
                }
            }

            for (int i = 0; i < matWidth; i++)//Checks if there are 4 coins Vertically
            {
                for (int j = 0; j <= matHeight - k_FourInARow; j++)
                {
                    if (m_GameMatrix[i, j] == i_Coin && m_GameMatrix[i, j + 1] == i_Coin && m_GameMatrix[i, j + 2] == i_Coin && m_GameMatrix[i, j + 3] == i_Coin)
                    {
                        CalculatePlayerOrComputerScore(i_Coin);
                        win = true;
                        break;
                    }
                }
            }

            for (int i = 0; i <= matWidth - k_FourInARow; i++)//Checks if there are 4 coins diagonally upper side
            {
                for (int j = 0; j <= matHeight - k_FourInARow; j++)
                {
                    if (m_GameMatrix[i, j] == i_Coin && m_GameMatrix[i + 1, j + 1] == i_Coin && m_GameMatrix[i + 2, j + 2] == i_Coin && m_GameMatrix[i + 3, j + 3] == i_Coin)
                    {
                        CalculatePlayerOrComputerScore(i_Coin);
                        win = true;
                        break;
                    }
                }
            }

            for (int i = 0; i <= matWidth - k_FourInARow; i++)// Checks if there are 4 coins in the current matrix diagonally down side
            {
                for (int j = matHeight - 1; j >= 3; j--)
                {
                    if (m_GameMatrix[i, j] == i_Coin && m_GameMatrix[i + 1, j - 1] == i_Coin && m_GameMatrix[i + 2, j - 2] == i_Coin && m_GameMatrix[i + 3, j - 3] == i_Coin)
                    {
                        CalculatePlayerOrComputerScore(i_Coin);
                        win = true;
                        break;
                    }
                }
            }

            return win;
        }
    }
}
