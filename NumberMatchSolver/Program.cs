// See https://aka.ms/new-console-template for more information

using NumberMatchSolver;
using NumberMatchSolver.SearchStrategies;

const int Cleared = -1;
const int Blocked = -2;

int[,] board =
{
<<<<<<< HEAD
  { 8, 5, 2, 7, 1, Cleared, 6, 3, 5 },
  { 7, 4, 9, 5, 6, Cleared, 2, 1, 4 },
  { Cleared, 5, 7, 8, 9, Cleared, Cleared, 7, 5 },
  { Cleared, 4, 1, 6, 5, 8, 5, 2, 7 },
  { 1, 6, 3, 5, 7, 4, 9, 5, 6 },
  { 2, 1, 4, 5, 7, 8, 9, 7, 5 },
  { 4, 1, 6, 5, Blocked, Blocked, Blocked, Blocked, Blocked },
=======
  { 5,9,8,4,5,3,2,7,8 },
  {2,5,3,9,8,7,1,4,1},
  {6,9,8,6,5,4,5,7,5},
  {7,5,3,9,7,Blocked,Blocked,Blocked,Blocked}
>>>>>>> fc4ca58 (Updates)
};

Node? root = new(new MoveInfo() { Board = board, Cell1 = Cell.Empty, Cell2 = Cell.Empty });
Queue<Node?> queue = new();
queue.Enqueue(root);
int skipCount = 0;

while (queue.Any())
{
  Node? node = queue.Dequeue();
  if (queue.Count % 100000 == 0)
    Console.WriteLine($"Elements in queue {queue.Count}");

  var find = new DiagonalThenLinearSearchStrategy();
  var found = new List<Pair>();
  for (int row = 0; row <= node.Value.Board.GetLength(0) - 1; row++)
  {
    for (int column = 0; column <= node.Value.Board.GetLength(1) - 1; column++)
    {
      var start = new Cell() { Row = row, Column = column };
      List<Pair> pairs = find.FindAllPairs(node.Value.Board, start);
      if (pairs.Any(p => found.Any(f => f.Equals(p))))
      {
        skipCount++;
        if (skipCount % 100000 == 0)
          Console.WriteLine($"Not adding this pair. Counter {skipCount}");
      }
      else
      {
        found.AddRange(pairs);
      }
    }
  }

  foreach (Pair pair in found)
  {
    if (pair.End.Equals(Cell.Empty))
    {
      continue;
    }

    var move = new MoveInfo()
    {
      Board = ClearPair(node.Value.Board.Clone() as int[,], pair.Start, pair.End),
      Cell1 = pair.Start,
      Cell2 = pair.End,
      Points = Game.CalculatePoints(node.Value.Board, pair.Start, pair.End)
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

root.PrintPretty("", true);

// Find highest scoring moves
Node value = CreateMaxPathTree(root);

Console.WriteLine($"Highest scoring moves {FindMaxSum(value)}");
value.PrintPretty("", true);



//value.PrintPretty("", true);

int FindMaxSum(Node root)
{
  int maxSum = int.MinValue;
  FindMaxSumHelper(root, ref maxSum);
  return maxSum;
}

int FindMaxSumHelper(Node node, ref int maxSum)
{
  int sum = node.Value.Points;

  foreach (Node child in node.Children)
  {
    int childSum = FindMaxSumHelper(child, ref maxSum);
    sum += Math.Max(childSum, 0); // Consider only positive child sums
  }

  maxSum = Math.Max(maxSum, sum);
  return sum;
}

Node CreateMaxPathTree(Node root)
{
  Node maxPathTree = CreateMaxPathTreeHelper(root);
  return maxPathTree;
}

Node CreateMaxPathTreeHelper(Node node)
{
  if (node == null)
    return null;

  Node newNode = new Node(node.Value);
  Node maxChild = null;
  int maxSum = int.MinValue;

  foreach (Node child in node.Children)
  {
    Node childNode = CreateMaxPathTreeHelper(child);
    int childSum = CalculateSum(childNode);

    if (childSum > maxSum)
    {
      maxSum = childSum;
      maxChild = childNode;
    }
  }

  if (maxChild != null)
    newNode.Children.Add(maxChild);

  return newNode;
}

int CalculateSum(Node node)
{
  if (node == null)
    return 0;

  int sum = node.Value.Points;

  foreach (Node child in node.Children)
    sum += CalculateSum(child);

  return sum;
}

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
  public int Points { get; set; }
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
      $"Cleared pair (R{Value.Cell1.Row}:C{Value.Cell1.Column}) (R{Value.Cell2.Row}:C{Value.Cell2.Column}). Points {Value.Points}");

    for (int i = 0; i < Children.Count; i++)
    {
      Children[i].PrintPretty(indent, i == Children.Count - 1);
    }
  }
}