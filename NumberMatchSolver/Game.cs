namespace NumberMatchSolver;

public class Game
{
  private const int Cleared = -1;
  private const int Blocked = -2;
  
  private int LastRowIndex => Board.GetLength(0) - 1;
  private int LastColumnIndex => Board.GetLength(1) - 1;

  public int[,] Board { get; private set; }

  public Game(int[,] board)
  {
    Board = board;
  }

  /// <summary>
  /// Searches for either the same value as in start or
  /// for value where sum of two is ten.
  /// </summary>
  /// <param name="start">Search start location.</param>
  /// <returns>Location where pair was found. If pair wasn't found
  /// return Cords.Empty.</returns>
  private Cords SearchForPair(Cords start)
  {
    if (SearchLinearRight(start, out Cords found)
        || SearchLinearLeft(start, out found)
        || SearchLinearTop(start, out found)
        || SearchLinearBottom(start, out found)
        || SearchDiagonalTopLeft(start, out found)
        || SearchDiagonalTopRight(start, out found)
        || SearchDiagonalBottomLeft(start, out found)
        || SearchDiagonalBottomRight(start, out found))
    {
      return found;
    }
    else
    {
      return Cords.Empty;
    }
  }

  private bool SearchDiagonalBottomRight(Cords start, out Cords found)
  {
    return SearchDiagonal(start, out found, Direction.BottomRight);
  }

  private bool SearchDiagonalBottomLeft(Cords start, out Cords found)
  {
    return SearchDiagonal(start, out found, Direction.BottomLeft);
  }

  private bool SearchDiagonalTopRight(Cords start, out Cords found)
  {
    return SearchDiagonal(start, out found, Direction.TopRight);
  }

  private bool SearchDiagonalTopLeft(Cords start, out Cords found)
  {
    return SearchDiagonal(start, out found, Direction.TopLeft);
  }

  private bool SearchDiagonal(Cords start, out Cords found, Direction direction)
  {
    if (GetValue(start) == Cleared || GetValue(start) == Blocked)
    {
      found = Cords.Empty;
      return false;
    }

    Cords offset = direction switch
    {
      Direction.TopLeft => new Cords { X = -1, Y = -1 },
      Direction.TopRight => new Cords { X = 1, Y = -1 },
      Direction.BottomLeft => new Cords { X = -1, Y = 1 },
      _ => new Cords { X = 1, Y = 1 }
    };
    Cords inspecting = start.AddOffset(offset);

    while (true)
    {
      // Boundary checks.
      if (inspecting.X > LastRowIndex
          || inspecting.X < 0
          || inspecting.Y > LastColumnIndex
          || inspecting.Y < 0)
      {
        found = Cords.Empty;
        return false;
      }

      int inspectValue = Board[inspecting.X, inspecting.Y];
      if (inspectValue == Cleared)
      {
        inspecting = inspecting.AddOffset(offset);
        continue;
      }

      if (inspectValue == Blocked)
      {
        found = Cords.Empty;
        return false;
      }

      if (Enumerable.Range(1, 10).Contains(inspectValue))
      {
        return CompareValues(start, inspecting, out found);
      }
    }
  }

  private bool SearchLinearBottom(Cords start, out Cords found)
  {
    return SearchLinear(start, out found, Direction.Bottom);
  }

  private int GetValue(Cords cords)
  {
    return Board[cords.X, cords.Y];
  }

  private bool SearchLinearTop(Cords start, out Cords found)
  {
    return SearchLinear(start, out found, Direction.Top);
  }

  private bool SearchLinearLeft(Cords start, out Cords found)
  {
    return SearchLinear(start, out found, Direction.Left);
  }

  private bool SearchLinearRight(Cords start, out Cords found)
  {
    return SearchLinear(start, out found, Direction.Right);
  }

  private bool SearchLinear(Cords start, out Cords found, Direction direction)
  {
    if (GetValue(start) == Cleared || GetValue(start) == Blocked)
    {
      found = Cords.Empty;
      return false;
    }

    Cords offset = direction switch
    {
      Direction.Right => new Cords { X = 0, Y = 1 },
      Direction.Left => new Cords { X = 0, Y = -1 },
      Direction.Top => new Cords { X = -1, Y = 0 },
      _ => new Cords { X = 1, Y = 0 },
    };
    Cords inspecting = start.AddOffset(offset);

    while (true)
    {
      if (inspecting.X > LastRowIndex || inspecting.X < 0)
      {
        found = Cords.Empty;
        return false;
      }

      if (inspecting.Y > LastColumnIndex || inspecting.Y < 0)
      {
        inspecting = direction switch
        {
          Direction.Right => new Cords() { X = inspecting.X + 1, Y = 0 },
          _ => new Cords() { X = inspecting.X - 1, Y = LastColumnIndex }
        };

        continue;
      }

      int inspectValue = Board[inspecting.X, inspecting.Y];
      if (inspectValue == Cleared)
      {
        inspecting = inspecting.AddOffset(offset);
        continue;
      }

      if (inspectValue == Blocked)
      {
        found = Cords.Empty;
        return false;
      }

      if (Enumerable.Range(1, 10).Contains(inspectValue))
      {
        return CompareValues(start, inspecting, out found);
      }
    }
  }

  private enum Direction
  {
    Left,
    Right,
    Top,
    Bottom,
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight
  }

  private bool CompareValues(Cords start, Cords inspecting, out Cords found)
  {
    int startValue = GetValue(start);
    int inspectValue = GetValue(inspecting);
    if (startValue + inspectValue == 10 || startValue == inspectValue)
    {
      found = inspecting;
      return true;
    }
    else
    {
      found = Cords.Empty;
      return false;
    }
  }

  /// <summary>
  /// Marks a pair as Cleared and returns information if row cleared in the process.
  /// </summary>
  /// <param name="cell1">First cell to clear</param>
  /// <param name="cell2">Second cell to clear</param>
  /// <returns></returns>
  private void ClearPair(Cords cell1, Cords cell2)
  {
    Clear(cell1);
    Clear(cell2);

    RemoveClearedRows();
  }

  private void RemoveClearedRows()
  {
    var rowsToRemove = 0;
    for (var i = 0; i <= LastRowIndex; i++)
    {
      int[] row = GetRow(i);
      if (row.All(x => x == Cleared || x == Blocked))
      {
        rowsToRemove++;
      }
    }

    // If there is nothing to remove, exit.
    if (rowsToRemove == 0)
    {
      return;
    }

    // Create new array with smaller size
    // and fill it with content.
    int newRowsCount = Board.GetLength(0) - rowsToRemove;
    var currentRowIndex = 0;
    var array = new int[newRowsCount, LastColumnIndex + 1];

    for (var i = 0; i <= LastRowIndex; i++)
    {
      int[] row = GetRow(i);
      if (row.Any(x => x is >= 1 and <= 9))
      {
        for (var j = 0; j <= LastColumnIndex; j++)
        {
          array[currentRowIndex, j] = Board[i, j];
        }

        currentRowIndex++;
      }
    }

    Board = array;
  }

  private void Clear(Cords cords)
  {
    Board[cords.X, cords.Y] = Cleared;
  }

  internal void PrintBoard()
  {
    if (Board.GetLength(0) == 0)
    {
      Console.WriteLine("Board has been cleared.");
      return;
    }

    for (var rowIndex = 0; rowIndex < Board.GetLength(0); rowIndex++)
    {
      string row = string.Join(',', GetRow(rowIndex));
      row = row.Replace("-1", "#").Replace("-2", "*");

      Console.WriteLine(row);
    }

    Console.WriteLine("===");
  }

  private int[] GetRow(int rowNumber)
  {
    return Enumerable.Range(0, Board.GetLength(1))
      .Select(x => Board[rowNumber, x])
      .ToArray();
  }

  public void Solve()
  {
    // Console.WriteLine("Printing initial state");
    // PrintBoard();

    var currentRowIndex = 0;
    var currentColumnIndex = 0;

    // If we went over last row, that means we are done with this board and we can leave.
    while (currentRowIndex <= LastRowIndex)
    {
      // Go to next row when we are over the last column.
      if (currentColumnIndex > LastColumnIndex)
      {
        currentRowIndex++;
        currentColumnIndex = 0;
        continue;
      }

      Cords start = new() { X = currentRowIndex, Y = currentColumnIndex };
      Cords found = SearchForPair(start);

      // If nothing was found, go to next cell.
      if (found == Cords.Empty)
      {
        currentColumnIndex++;
        continue;
      }

      Console.WriteLine(
        $"Cell (R{start.X + 1}, C{start.Y + 1}) [{GetValue(start)}] can be matched with " +
        $"cell (R{found.X + 1}, C{found.Y + 1}) [{GetValue(found)}]");

      ClearPair(start, found);

      currentColumnIndex = 0;
      currentRowIndex = 0;

      //PrintBoard();
    }

    // Console.WriteLine("Printing final state");
    // PrintBoard();
  }

  public void PrintCopyableBoard()
  {
    if (Board.GetLength(0) == 0)
    {
      Console.WriteLine("Board has been cleared.");
      return;
    }

    for (var i = 0; i < Board.GetLength(0); i++)
    {
      string row = "{" + string.Join(',', GetRow(i)) + "},";
      row = row.Replace("-1", "Cleared").Replace("-2", "Blocked");

      Console.WriteLine(row);
    }

    Console.WriteLine("===");
  }
}

public class Cords
{
  public static readonly Cords Empty = new() { X = int.MinValue, Y = int.MinValue };

  /// <summary>
  /// Row.
  /// </summary>
  public int X { get; init; }

  /// <summary>
  /// Column
  /// </summary>
  public int Y { get; init; }

  private Cords AddOffset(int x, int y)
  {
    return new Cords() { X = X + x, Y = Y + y };
  }

  public Cords AddOffset(Cords cords)
  {
    return AddOffset(cords.X, cords.Y);
  }
}