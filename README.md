Four in a Row Game Implementation

This console-based game is essentially a version of "Connect Four."

Game Flow:

The user is prompted to set the size of the game board (minimum 4x4, maximum 8x8, non-square boards allowed).
The user decides whether it's a two-player game or against the computer. In a game against the computer, the human player starts.
An empty board is displayed (e.g., for a 5x6 board).
Each player, in their turn, selects the column to drop their coin into.
If the column is already full, an appropriate message is displayed until a valid column is chosen.
After the player selects the column, the screen is cleared, and the board is redrawn with the added coin at the bottom of the selected column.
If there's no sequence of four coins of the same type (vertically, horizontally, or diagonally), the turn passes to the next player.
If there's a sequence, the winning player is announced, a point is awarded, and the current score is displayed.
If the board is completely filled with no winner, a tie is announced, and the current score is displayed.
In a game against the computer, the gameplay does not pause for the computer's turn.
At any stage, a player can quit the game by selecting "Q" instead of choosing a column (the opposing player receives a point).
Upon game completion (tie, victory, or quitting), the user is prompted if they want to play another round. Another round maintains the same board size and player configuration.
This implementation provides an engaging and interactive experience, allowing players to enjoy the classic "Four in a Row" game in a console environment.
