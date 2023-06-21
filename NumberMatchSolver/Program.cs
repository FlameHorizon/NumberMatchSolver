// See https://aka.ms/new-console-template for more information

using NumberMatchSolver;
using NumberMatchSolver.SearchStrategies;

const int Cleared = -1;
const int Blocked = -2;

int[,] board =
{
  { 8, 5, 2, 7, 1, Cleared, 6, 3, 5 },
  { 7, 4, 9, 5, 6, Cleared, 2, 1, 4 },
  { Cleared, 5, 7, 8, 9, Cleared, Cleared, 7, 5 },
  { Cleared, 4, 1, 6, 5, 8, 5, 2, 7 },
  { 1, 6, 3, 5, 7, 4, 9, 5, 6 },
  { 2, 1, 4, 5, 7, 8, 9, 7, 5 },
  { 4, 1, 6, 5, Blocked, Blocked, Blocked, Blocked, Blocked },
};

var linear = new LinearThenDiagonalSearchStrategy();
var diagonal = new DiagonalThenLinearSearchStrategy();
List<(int startRow, int startColumn, List<(int points, (Cell cell1, Cell cell2))> mvs)> moves = new();
for (var row = 0; row < board.GetLength(0) - 1; row++)
{
  for (var column = 0; column < board.GetLength(1) - 1; column++)
  {
    var leftToRight = new LeftToRightMoveStrategy(board, column, row);
    var complete = new CompleteMoveStrategy(board);

    var game = new Game(board, linear, leftToRight);
    moves.Add((row, column, game.Solve()));
  }
}

Console.WriteLine($"Most points {moves.MaxBy(x => x.mvs.Sum(y => y.points)).mvs.Sum((y => y.points))}. " +
                  $"Start row {moves.MaxBy(x => x.mvs.Max(y => y.points)).startRow}, " +
                  $"Start column {moves.MaxBy(x => x.mvs.Max(y => y.points)).startColumn}");