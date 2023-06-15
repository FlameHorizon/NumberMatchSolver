// See https://aka.ms/new-console-template for more information

using NumberMatchSolver;

const int Cleared = -1;
const int Blocked = -2;

int[,] board =
{
  {4,8,6,8,Cleared,6,5,9,4},
  {Cleared,5,3,Cleared,5,2,7,2,5},
  {Cleared,6,8,4,3,Cleared,Cleared,6,7},
  {5,3,5,9,5,4,8,6,8},
  {6,5,9,4,5,3,5,2,7},
  {2,5,6,8,4,3,6,7,5},
  {3,5,9,5,Cleared,Cleared,Cleared,Cleared,Cleared},
};

var game = new Game(board);
game.Solve();
game.PrintCopyableBoard();