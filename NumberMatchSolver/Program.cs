// See https://aka.ms/new-console-template for more information

using NumberMatchSolver;
using NumberMatchSolver.SearchStrategies;

const int Cleared = -1;
const int Blocked = -2;

int[,] board =
{
  {1,5,Cleared,Cleared,9,Cleared,Cleared,Cleared,Cleared},
  {4,2,Cleared,Cleared,Cleared,Cleared,Cleared,Cleared,Cleared},
  {Cleared,5,Cleared,Cleared,Cleared,Cleared,Cleared,Cleared,Cleared},
  {Cleared,Cleared,7,Cleared,Cleared,Cleared,Cleared,Cleared,Cleared},
  {5,4,2,Cleared,3,Cleared,Cleared,8,5},
  {7,1,5,Cleared,Cleared,Cleared,Cleared,1,Cleared},
  {Cleared,6,8,Cleared,5,4,Cleared,Cleared,Cleared},
  {Cleared,Cleared,7,1,5,9,4,2,5},
  {7,5,4,2,3,8,5,7,1},
  {5,1,6,8,5,4,7,Blocked,Blocked},

};

var linear = new LinearThenDiagonalSearchStrategy();
var diagonal = new DiagonalThenLinearSearchStrategy();

var game = new Game(board, diagonal);
game.Solve();
game.PrintCopyableBoard();