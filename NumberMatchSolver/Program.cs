// See https://aka.ms/new-console-template for more information

using NumberMatchSolver;

const int Cleared = -1;
const int Blocked = -2;

int[,] board =
{
  { Cleared, Cleared, Cleared, Cleared, Cleared, Cleared, 7, Cleared, 5 },
  { 1, 5, 7, Cleared, Cleared, Cleared, 1, 7, 5 },
  { 1, 5, 7, 1, Blocked, Blocked, Blocked, Blocked, Blocked },
};

var game = new Game(board);
game.Solve();
game.PrintCopyableBoard();