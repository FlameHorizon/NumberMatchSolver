namespace NumberMatchSolver.SearchStrategies;

public class DiagonalThenLinearSearchStrategy : IFindCellStrategy
{
  private const int Cleared = -1;
  private const int Blocked = -2;

  private int LastRowIndex => Board.GetLength(0) - 1;
  private int LastColumnIndex => Board.GetLength(1) - 1;

  private int[,] Board { get; set; }

  public List<Cell> FindAllPairs(int[,] board, Cell start)
  {
    Board = board;
    List<Cell> results = new();
    if (SearchDiagonalTopLeft(start, out Cell found))
    {
      results.Add(found);
    }
    if (SearchDiagonalTopRight(start, out found))
    {
      results.Add(found);
    }
    if (SearchDiagonalBottomLeft(start, out found))
    {
      results.Add(found);
    }
    if (SearchDiagonalBottomRight(start, out found))
    {
      results.Add(found);
    }
    if (SearchLinearRight(start, out found))
    {
      results.Add(found);
    }
    if (SearchLinearLeft(start, out found))
    {
      results.Add(found);
    }
    if (SearchLinearTop(start, out found))
    {
      results.Add(found);
    }
    if (SearchLinearBottom(start, out found))
    {
      results.Add(found);
    }

    return results.DistinctBy(x => x.Column).DistinctBy(x => x.Row).ToList();
  }

  public Cell FindPair(int[,] board, Cell start)
  {
    Board = board;

    if (SearchDiagonalTopLeft(start, out Cell found)
        || SearchDiagonalTopRight(start, out found)
        || SearchDiagonalBottomLeft(start, out found)
        || SearchDiagonalBottomRight(start, out found)
        || SearchLinearRight(start, out found)
        || SearchLinearLeft(start, out found)
        || SearchLinearTop(start, out found)
        || SearchLinearBottom(start, out found))
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
      Direction.TopRight => new Cell { Row = -1, Column = 1 },
      Direction.BottomLeft => new Cell { Row = 1, Column = -1 },
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

  private int GetValue(Cell cell)
  {
    return Board[cell.Row, cell.Column];
  }
}