// See https://aka.ms/new-console-template for more information

using NumberMatchSolver;

const int Cleared = -1;
const int Blocked = -2;

int[,] board =
{
  {9,1,5,7,2,3,1,5,6},
  {3,2,4,9,6,5,8,7,2},
  {6,1,7,2,3,1,6,1,4},
  {7,2,5,4,5,Blocked,Blocked,Blocked,Blocked},

};

var game = new Game(board);
game.Solve();
game.PrintCopyableBoard();