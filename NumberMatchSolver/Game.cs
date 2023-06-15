namespace NumberMatchSolver;

public class Game
{
  private const int Cleared = -1;
  private const int Blocked = -2;

  private int[,] _board;
  private readonly int _lowerRowLimit = 0;
  private int _upperRowLimit;
  private int LastRowIndex => _upperRowLimit - 1;
  private readonly int _lowerColumnLimit = 0;
  private int _upperColumnLimit;
  private int LastColumnIndex => _upperColumnLimit - 1;

  public int[,] Board => _board;

  public Game(int[,] board)
  {
    _board = board;
    _upperRowLimit = _board.GetLength(0);
    _upperColumnLimit = _board.GetLength(1);
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
      // Boundry checks.
      if (inspecting.X > LastRowIndex
          || inspecting.X < _lowerRowLimit
          || inspecting.Y >= _upperColumnLimit
          || inspecting.Y < _lowerColumnLimit)
      {
        found = Cords.Empty;
        return false;
      }

      int inspectValue = _board[inspecting.X, inspecting.Y];
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
    return _board[cords.X, cords.Y];
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
      if (inspecting.X >= _upperRowLimit || inspecting.X < _lowerRowLimit)
      {
        found = Cords.Empty;
        return false;
      }

      if (inspecting.Y >= _upperColumnLimit || inspecting.Y < _lowerColumnLimit)
      {
        inspecting = direction switch
        {
          Direction.Right => new Cords() { X = inspecting.X + 1, Y = 0 },
          _ => new Cords() { X = inspecting.X - 1, Y = _upperColumnLimit - 1 }
        };

        continue;
      }

      int inspectValue = _board[inspecting.X, inspecting.Y];
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
    int rowsToRemove = 0;
    for (int i = 0; i < _board.GetLength(0); i++)
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

    // Create new array with smaller size.
    int newRowsCount = _board.GetLength(0) - rowsToRemove;
    int currentRow = 0;
    int[,] array = new int[newRowsCount, _upperColumnLimit];

    for (int i = 0; i < _board.GetLength(0); i++)
    {
      int[] row = GetRow(i);
      if (row.Any(x => x is >= 1 and <= 9))
      {
        for (int j = 0; j < _upperColumnLimit; j++)
        {
          array[currentRow, j] = _board[i, j];
        }

        currentRow++;
      }
    }

    _board = array;
    _upperRowLimit = _board.GetLength(0);
    _upperColumnLimit = _board.GetLength(1);
  }

  private void Clear(Cords cords)
  {
    _board[cords.X, cords.Y] = Cleared;
  }

  internal void PrintBoard()
  {
    if (_board.GetLength(0) == 0)
    {
      Console.WriteLine("Board has been cleared.");
      return;
    }

    for (int i = 0; i < _board.GetLength(0); i++)
    {
      string row = string.Join(',', GetRow(i));
      row = row.Replace("-1", "#").Replace("-2", "*");

      Console.WriteLine(row);
    }

    Console.WriteLine("===");
  }

  private int[] GetRow(int rowNumber)
  {
    return Enumerable.Range(0, _board.GetLength(1))
      .Select(x => _board[rowNumber, x])
      .ToArray();
  }

  public void Solve()
  {
    // Console.WriteLine("Printing initial state");
    // PrintBoard();

    int currentRowIndex = 0;
    int currentColumnIndex = 0;

    while (true)
    {
      if (currentColumnIndex > LastColumnIndex)
      {
        currentRowIndex++;
        currentColumnIndex = 0;
      }

      // If we went over last row, that means we are done with this board and we can leave.
      if (currentRowIndex > LastRowIndex)
      {
        break;
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
    if (_board.GetLength(0) == 0)
    {
      Console.WriteLine("Board has been cleared.");
      return;
    }

    for (int i = 0; i < _board.GetLength(0); i++)
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
    return new Cords() { X = this.X + x, Y = this.Y + y };
  }

  public Cords AddOffset(Cords cords)
  {
    return AddOffset(cords.X, cords.Y);
  }
}