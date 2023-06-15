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
  private Cell SearchForPair(Cell start)
  {
    if (SearchLinearRight(start, out Cell found)
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
      return Cell.Empty;
    }
  }

  private bool SearchDiagonalBottomRight(Cell start, out Cell found)
  {
    return SearchDiagonal(start, out found, Direction.BottomRight);
  }

  private bool SearchDiagonalBottomLeft(Cell start, out Cell found)
  {
    return SearchDiagonal(start, out found, Direction.BottomLeft);
  }

  private bool SearchDiagonalTopRight(Cell start, out Cell found)
  {
    return SearchDiagonal(start, out found, Direction.TopRight);
  }

  private bool SearchDiagonalTopLeft(Cell start, out Cell found)
  {
    return SearchDiagonal(start, out found, Direction.TopLeft);
  }

  private bool SearchDiagonal(Cell start, out Cell found, Direction direction)
  {
    if (GetValue(start) == Cleared || GetValue(start) == Blocked)
    {
      found = Cell.Empty;
      return false;
    }

    Cell offset = direction switch
    {
      Direction.TopLeft => new Cell { Row = -1, Column = -1 },
      Direction.TopRight => new Cell { Row = 1, Column = -1 },
      Direction.BottomLeft => new Cell { Row = -1, Column = 1 },
      _ => new Cell { Row = 1, Column = 1 }
    };
    Cell inspecting = start.AddOffset(offset);

    while (true)
    {
      // Boundary checks.
      if (inspecting.Row > LastRowIndex
          || inspecting.Row < 0
          || inspecting.Column > LastColumnIndex
          || inspecting.Column < 0)
      {
        found = Cell.Empty;
        return false;
      }

      int inspectValue = Board[inspecting.Row, inspecting.Column];
      if (inspectValue == Cleared)
      {
        inspecting = inspecting.AddOffset(offset);
        continue;
      }

      if (inspectValue == Blocked)
      {
        found = Cell.Empty;
        return false;
      }

      if (Enumerable.Range(1, 10).Contains(inspectValue))
      {
        return CompareValues(start, inspecting, out found);
      }
    }
  }

  private bool SearchLinearBottom(Cell start, out Cell found)
  {
    return SearchLinear(start, out found, Direction.Bottom);
  }

  private int GetValue(Cell cell)
  {
    return Board[cell.Row, cell.Column];
  }

  private bool SearchLinearTop(Cell start, out Cell found)
  {
    return SearchLinear(start, out found, Direction.Top);
  }

  private bool SearchLinearLeft(Cell start, out Cell found)
  {
    return SearchLinear(start, out found, Direction.Left);
  }

  private bool SearchLinearRight(Cell start, out Cell found)
  {
    return SearchLinear(start, out found, Direction.Right);
  }

  private bool SearchLinear(Cell start, out Cell found, Direction direction)
  {
    if (GetValue(start) == Cleared || GetValue(start) == Blocked)
    {
      found = Cell.Empty;
      return false;
    }

    Cell offset = direction switch
    {
      Direction.Right => new Cell { Row = 0, Column = 1 },
      Direction.Left => new Cell { Row = 0, Column = -1 },
      Direction.Top => new Cell { Row = -1, Column = 0 },
      _ => new Cell { Row = 1, Column = 0 },
    };
    Cell inspecting = start.AddOffset(offset);

    while (true)
    {
      if (inspecting.Row > LastRowIndex || inspecting.Row < 0)
      {
        found = Cell.Empty;
        return false;
      }

      if (inspecting.Column > LastColumnIndex || inspecting.Column < 0)
      {
        inspecting = direction switch
        {
          Direction.Right => new Cell() { Row = inspecting.Row + 1, Column = 0 },
          _ => new Cell() { Row = inspecting.Row - 1, Column = LastColumnIndex }
        };

        continue;
      }

      int inspectValue = Board[inspecting.Row, inspecting.Column];
      if (inspectValue == Cleared)
      {
        inspecting = inspecting.AddOffset(offset);
        continue;
      }

      if (inspectValue == Blocked)
      {
        found = Cell.Empty;
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

  private bool CompareValues(Cell start, Cell inspecting, out Cell found)
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
      found = Cell.Empty;
      return false;
    }
  }

  /// <summary>
  /// Marks a pair as Cleared and returns information if row cleared in the process.
  /// </summary>
  /// <param name="cell1">First cell to clear</param>
  /// <param name="cell2">Second cell to clear</param>
  /// <returns></returns>
  private void ClearPair(Cell cell1, Cell cell2)
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

  private void Clear(Cell cell)
  {
    Board[cell.Row, cell.Column] = Cleared;
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

  private int[] GetRow(int[,] array, int rowNumber)
  {
    return Enumerable.Range(0, array.GetLength(1))
      .Select(x => array[rowNumber, x])
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

      Cell start = new() { Row = currentRowIndex, Column = currentColumnIndex };
      Cell found = SearchForPair(start);

      // If nothing was found, go to next cell.
      if (found == Cell.Empty)
      {
        currentColumnIndex++;
        continue;
      }

      int points = CalculatePoints(start, found);

      Console.WriteLine(
        $"Cell (R{start.Row + 1}, C{start.Column + 1}) [{GetValue(start)}] can be matched with " +
        $"cell (R{found.Row + 1}, C{found.Column + 1}) [{GetValue(found)}]. Points {points}");

      ClearPair(start, found);

      currentColumnIndex = 0;
      currentRowIndex = 0;

      //PrintBoard();
    }

    // Console.WriteLine("Printing final state");
    // PrintBoard();
  }

  public int CalculatePoints(Cell start, Cell found)
  {
    if (WouldClearBoard(start, found))
    {
      return 150;
    }

    if (WouldClearTwoLines(start, found))
    {
      return 20;
    }

    if (WouldClearEntireLine(start, found))
    {
      return 10;
    }

    (int, int) dist = (start.Row - found.Row, start.Column - found.Column);
    if (dist.Item1 is -1 or 0 or 1 && dist.Item2 is -1 or 0 or 1)
    {
      return 1;
    }

    if (start.Column == 0 && found.Column == LastColumnIndex && start.Row != found.Row)
    {
      return 2;
    }
    else if (start.Column == LastColumnIndex && found.Column == 0 && start.Row != found.Row)
    {
      return 2;
    }

    if (IsClearedInBetween(start, found))
    {
      return 4;
    }

    return 150;
  }

  private bool WouldClearBoard(Cell start, Cell found)
  {
    var boardCopy = Board.Clone() as int[,];

    boardCopy![start.Row, start.Column] = Cleared;
    boardCopy[found.Row, found.Column] = Cleared;

    // check if all rows don't have any numeric values.
    for (var i = 0; i <= LastRowIndex; i++)
    {
      if (GetRow(boardCopy, i).Any(x => x is >= 1 and <= 9))
      {
        return false;
      }
    }

    return true;
  }

  private bool WouldClearTwoLines(Cell start, Cell found)
  {
    int[] row1 = GetRow(start.Row);
    row1[start.Column] = Cleared;

    int[] row2 = GetRow(found.Row);
    row2[found.Column] = Cleared;

    if (row1.All(x => x < 0) && row2.All(x => x < 0))
    {
      return true;
    }

    return false;
  }

  private bool WouldClearEntireLine(Cell start, Cell found)
  {
    if (start.Row == found.Row)
    {
      int[] row = GetRow(start.Row);
      row[start.Column] = Cleared;
      row[found.Column] = Cleared;

      if (row.All(x => x < 0))
      {
        return true;
      }
    }

    return false;
  }

  private bool IsClearedInBetween(Cell start, Cell found)
  {
    // First we have to find out if we are going towards left or right.
    if (found.Row < start.Row)
    {
      // search left
      var offset = new Cell { Row = 0, Column = -1 };
      Cell inspecting = start;

      while (found != inspecting)
      {
        inspecting = inspecting.AddOffset(offset);
        if (inspecting.Column > LastColumnIndex || inspecting.Column < 0)
        {
          inspecting = new Cell() { Row = inspecting.Row - 1, Column = LastColumnIndex };
          continue;
        }

        int inspectValue = Board[inspecting.Row, inspecting.Column];
        if (inspectValue == Cleared)
        {
          // we found empty cell, we can leave and award 4 points.
          return true;
        }
      }

      return false;
    }

    // If found cell is lower than start cell, then we have to go to the right.
    if (found.Row > start.Row)
    {
      // search right
      var offset = new Cell { Row = 0, Column = 1 };
      Cell inspecting = start;

      while (found != inspecting)
      {
        inspecting = inspecting.AddOffset(offset);
        if (inspecting.Column > LastColumnIndex || inspecting.Column < 0)
        {
          inspecting = new Cell() { Row = inspecting.Row + 1, Column = 0 };
          continue;
        }

        int inspectValue = Board[inspecting.Row, inspecting.Column];
        if (inspectValue == Cleared)
        {
          // we found empty cell, we can leave and award 4 points.
          return true;
        }
      }

      return false;
    }

    // If found cell is on the same row as start, we have to find out
    // if its to the left or to the right.
    if (found.Column > start.Column)
    {
      // search right
      var offset = new Cell { Row = 0, Column = 1 };
      Cell inspecting = start;

      while (found != inspecting)
      {
        inspecting = inspecting.AddOffset(offset);
        if (inspecting.Column > LastColumnIndex || inspecting.Column < 0)
        {
          inspecting = new Cell() { Row = inspecting.Row + 1, Column = 0 };
          continue;
        }

        int inspectValue = Board[inspecting.Row, inspecting.Column];
        if (inspectValue == Cleared)
        {
          // we found empty cell, we can leave and award 4 points.
          return true;
        }
      }

      return false;
    }
    else
    {
      // search left
      var offset = new Cell { Row = 0, Column = -1 };
      Cell inspecting = start;

      while (found != inspecting)
      {
        inspecting = inspecting.AddOffset(offset);
        if (inspecting.Column > LastColumnIndex || inspecting.Column < 0)
        {
          inspecting = new Cell() { Row = inspecting.Row - 1, Column = LastColumnIndex };
          continue;
        }

        int inspectValue = Board[inspecting.Row, inspecting.Column];
        if (inspectValue == Cleared)
        {
          // we found empty cell, we can leave and award 4 points.
          return true;
        }
      }

      return false;
    }
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

public class Cell
{
  public static readonly Cell Empty = new() { Row = int.MinValue, Column = int.MinValue };

  /// <summary>
  /// Row.
  /// </summary>
  public int Row { get; init; }

  /// <summary>
  /// Column
  /// </summary>
  public int Column { get; init; }

  private Cell AddOffset(int x, int y)
  {
    return new Cell() { Row = Row + x, Column = Column + y };
  }

  public Cell AddOffset(Cell cell)
  {
    return AddOffset(cell.Row, cell.Column);
  }
}