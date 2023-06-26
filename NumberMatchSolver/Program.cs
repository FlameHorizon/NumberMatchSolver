// See https://aka.ms/new-console-template for more information

using NumberMatchSolver;
using NumberMatchSolver.SearchStrategies;

const int Cleared = -1;
const int Blocked = -2;

int[,] board =
{
  { 1, 2, 3, 4 },
  { 2, 1, 4, 3 },
};

Node root = new(new MoveInfo() { Board = board, Cell1 = Cell.Empty, Cell2 = Cell.Empty });
Queue<Node> queue = new();
queue.Enqueue(root);

while (queue.Any())
{
  Node node = queue.Dequeue();
  Console.WriteLine($"Elements in queue {queue.Count}");

  var find = new DiagonalThenLinearSearchStrategy();
  var found = new List<(List<Cell> matches, Cell start)>();
  for (int row = 0; row <= node.Value.Board.GetLength(0) - 1; row++)
  {
    for (int column = 0; column <= node.Value.Board.GetLength(1) - 1; column++)
    {
      var start = new Cell() { Row = row, Column = column };
      found.Add((find.FindAllPairs(node.Value.Board, start), start));
    }
  }

  foreach ((List<Cell> matches, Cell start) cell in found)
  {
    foreach (Cell? match in cell.matches)
    {
      if (match == Cell.Empty)
      {
        continue;
      }

      var move = new MoveInfo()
      {
        Board = ClearPair(node.Value.Board.Clone() as int[,], cell.start, match),
        Cell1 = cell.start,
        Cell2 = match
      };

      var child = new Node(move);

      node.AddChild(child);
      if (move.Board.GetLength(0) == 0)
      {
        continue;
      }

      queue.Enqueue(child);
    }
  }
}

root.PrintPretty("", true);

//Console.WriteLine($"Found {moves.Count} moves.");


// Console.WriteLine($"Most points {moves.MaxBy(x => x.mvs.Sum(y => y.points)).mvs.Sum((y => y.points))}. " +
//                   $"Start row {moves.MaxBy(x => x.mvs.Max(y => y.points)).startRow}, " +
//                   $"Start column {moves.MaxBy(x => x.mvs.Max(y => y.points)).startColumn}");

int[,] ClearPair(int[,] board, Cell cell1, Cell cell2)
{
  Clear(board, cell1);
  Clear(board, cell2);

  return RemoveClearedRows(board);
}

int[,] RemoveClearedRows(int[,] board)
{
  int lastRowIndex = board.GetLength(0) - 1;
  int lastColumnIndex = board.GetLength(1) - 1;

  var rowsToRemove = 0;
  for (var i = 0; i <= lastRowIndex; i++)
  {
    int[] row = GetRow(board, i);
    if (row.All(x => x == Cleared || x == Blocked))
    {
      rowsToRemove++;
    }
  }

  // If there is nothing to remove, exit.
  if (rowsToRemove == 0)
  {
    return board;
  }

  // Create new array with smaller size
  // and fill it with content.
  int newRowsCount = board.GetLength(0) - rowsToRemove;
  var currentRowIndex = 0;
  var array = new int[newRowsCount, lastColumnIndex + 1];

  for (var i = 0; i <= lastRowIndex; i++)
  {
    int[] row = GetRow(board, i);
    if (row.Any(x => x is >= 1 and <= 9))
    {
      for (var j = 0; j <= lastColumnIndex; j++)
      {
        array[currentRowIndex, j] = board[i, j];
      }

      currentRowIndex++;
    }
  }

  return array;
}

int[] GetRow(int[,] array, int rowNumber)
{
  return Enumerable.Range(0, array.GetLength(1))
    .Select(x => array[rowNumber, x])
    .ToArray();
}

void Clear(int[,] board, Cell cell)
{
  board[cell.Row, cell.Column] = Cleared;
}

int GetValue(int[,] board, Cell cell)
{
  return board[cell.Row, cell.Column];
}

public class MoveInfo
{
  public Cell Cell1 { get; set; }
  public Cell Cell2 { get; set; }
  public int[,] Board { get; set; }
  public static MoveInfo Root { get; } = new() { Board = null, Cell1 = null, Cell2 = null };
}

public class Node
{
  public List<Node> Children { get; }

  public MoveInfo Value { get; }
  public List<Node> Parents { get; }

  public Node(MoveInfo value)
  {
    Value = value;
    Children = new List<Node>();
    Parents = new List<Node>();
  }

  public void AddParent(Node parent)
  {
    Parents.Add(parent);
  }

  public void AddChild(Node edge)
  {
    Children.Add(edge);
  }

  public void PrintPretty(string indent, bool last)
  {
    Console.Write(indent);
    if (last)
    {
      Console.Write("\\-");
      indent += "  ";
    }
    else
    {
      Console.Write("|-");
      indent += "| ";
    }

    Console.WriteLine(
      $"Cleared pair (R{Value.Cell1.Row}:C{Value.Cell1.Column}) (R{Value.Cell2.Row}:C{Value.Cell2.Column})");

    for (int i = 0; i < Children.Count; i++)
    {
      Children[i].PrintPretty(indent, i == Children.Count - 1);
    }
  }
}