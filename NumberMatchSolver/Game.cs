using NumberMatchSolver.SearchStrategies;

namespace NumberMatchSolver;

public class Game
{
  private readonly IFindCellStrategy _findCellFindCellStrategy;
  private readonly IMoveStrategy _moveStrategy;
  private const int Cleared = -1;
  private const int Blocked = -2;

  private int LastRowIndex => Board.GetLength(0) - 1;
  private int LastColumnIndex => Board.GetLength(1) - 1;

  public int[,] Board { get; private set; }

  public Game(int[,] board) : this(board, new LinearThenDiagonalSearchStrategy(), new LeftToRightMoveStrategy(board))
  {
  }

  public Game(int[,] board, IFindCellStrategy findCellStrategy) : this(board, findCellStrategy,
    new LeftToRightMoveStrategy(board))
  {
  }

  public Game(int[,] board, IFindCellStrategy findCellStrategy, IMoveStrategy moveStrategy)
  {
    Board = board;
    _findCellFindCellStrategy = findCellStrategy;
    _moveStrategy = moveStrategy;
  }

  /// <summary> 
  /// Main loop solver.
  /// Goes through each cell and searches for a pair.
  /// If pair is found, it is cleared and loop starts over.
  /// </summary>
  /// <returns>Returns a list of moves .</returns>
  public List<(int points, (Cell cell1, Cell cell2))> Solve()
  {
    // Console.WriteLine("Printing initial state");
    // PrintBoard();

    List<(int points, (Cell cell1, Cell cell2))> result = new();
    var appendMoreRowsCounter = 5;

    // If we went over last row, that means we are done with this board and we can leave.
    while (true)
    {
      if (_moveStrategy.ReachedLastRow())
      {
        if (Board.GetLength(0) >= 1 && appendMoreRowsCounter > 0)
        {
          AppendRows();
          appendMoreRowsCounter--;

          _moveStrategy.UpdateBoard(Board);
          _moveStrategy.RegisterOutOfBoundsRow();
        }
        else
        {
          Console.WriteLine("Board has been cleared.");
          break;
        }
      }

      // Go to next row when we are over the last column.
      if (_moveStrategy.ReachedLastColumn())
      {
        _moveStrategy.RegisterOutOfBoundsColumn();
        continue;
      }

      Cell? start = _moveStrategy.NextCell();
      Cell found = _findCellFindCellStrategy.FindPair(Board, start);

      // If nothing was found, go to next cell.
      if (found == Cell.Empty)
      {
        _moveStrategy.RegisterPairNotFound();
        continue;
      }
      else
      {
        _moveStrategy.RegisterPairFound();
      }

      int points = CalculatePoints(start, found);
      result.Add((points, (start, found)));

      Console.WriteLine(
        $"Cell (R{start.Row + 1}, C{start.Column + 1}) [{GetValue(start)}] can be matched with " +
        $"cell (R{found.Row + 1}, C{found.Column + 1}) [{GetValue(found)}]. Points {points}. " +
        $"Total points {result.Sum(x => x.points)}");

      ClearPair(start, found);
      _moveStrategy.UpdateBoard(Board);

      //PrintBoard();
    }

    return result;

    // Console.WriteLine("Printing final state");
    // PrintBoard();
  }

  public List<(List<(int points, (Cell cell1, Cell cell2))>, (int row, int column))> ExhaustiveSearch()
  {
    // Start at each cell on the board and gather information how many points
    // each run will collect. Prioritize highest score.
    List<(List<(int points, (Cell cell1, Cell cell2))>, (int row, int column))> result = new();
    for (var row = 0; row < LastRowIndex; row++)
    {
      for (var column = 0; column < LastColumnIndex; column++)
      {
        _moveStrategy.CurrentRowIndex = row;
        _moveStrategy.CurrentColumnIndex = column;
        result.Add((Solve(), (row, column)));
      }
    }

    return result;
  }

  private int GetValue(Cell cell)
  {
    return Board[cell.Row, cell.Column];
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

  private static int[] GetRow(int[,] array, int rowNumber)
  {
    return Enumerable.Range(0, array.GetLength(1))
      .Select(x => array[rowNumber, x])
      .ToArray();
  }

<<<<<<< HEAD
=======

  /// <summary> 
  /// Main loop solver.
  /// Goes through each cell and searches for a pair.
  /// If pair is found, it is cleared and loop starts over.
  /// </summary>

  public void Solve()
  {
    // Console.WriteLine("Printing initial state");
    // PrintBoard();

    var currentRowIndex = 0;
    var currentColumnIndex = 0;
    var appedMoreRowsCounter = 5;

    // If we went over last row, that means we are done with this board and we can leave.
    while (true)
    {
      if (currentRowIndex > LastRowIndex)
      {
        if (Board.GetLength(0) >= 1 && appedMoreRowsCounter > 0)
        {
          System.Console.WriteLine($"We have {Board.GetLength(0)} " +
            $"rows and {appedMoreRowsCounter} more row appends. Appending...");

          // PrintBoard();

          appedMoreRowsCounter--;
          AppendRows();

          currentColumnIndex = 0;
          currentRowIndex = 0;

        }
        else
        {
          System.Console.WriteLine("Board has been cleared.");
          break;
        }
      }

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
        $"cell (R{found.Row + 1}, C{found.Column + 1}) [{GetValue(found)}]. Points {points} " +
        $"Direction: {GetDirection(start, found)}");

      ClearPair(start, found);

      currentColumnIndex = 0;
      currentRowIndex = 0;

      //PrintBoard();
    }

    // Console.WriteLine("Printing final state");
    // PrintBoard();
  }

  private string GetDirection(Cell start, Cell found)
  {
    (int x, int y) vector = (start.Column - found.Column, start.Row - found.Row);
    string direction = string.Empty;
    if (vector.x <= -1 && vector.y == 0)
    {
      direction = "→";
    }
    else if (vector.x >= 1 && vector.y == 0)
    {
      direction = "←";
    }
    else if (vector.x == 0 && vector.y >= 1)
    {
      direction = "↑";
    }
    else if (vector.x == 0 && vector.y <= -1)
    {
      direction = "↓";
    }
    else if (vector.x <= -1 && vector.y <= -1)
    {
      direction = "↘";
    }
    else if (vector.x <= -1 && vector.y >= 1)
    {
      direction = "↗";
    }
    else if (vector.x >= 1 && vector.y >= 1)
    {
      direction = "↖";
    }
    else
    {
      direction = "↙";
    }

    return direction;
  }

>>>>>>> fc4ca58 (Updates)
  private void AppendRows()
  {
    // We have to find out how many new rows we do need to add.
    // The formula is following:
    // Number of values between 1 and 9 - number of blocked cells / number of columns

    int notClearedCellsCount = Board.Cast<int>().Where(x => x is >= 1 and <= 9).Count();
<<<<<<< HEAD
    Console.WriteLine($"Not cleared cells count: {notClearedCellsCount}");
=======
>>>>>>> fc4ca58 (Updates)

    int blockedCells = Board.Cast<int>().Count(x => x == Blocked);
    int columns = Board.GetLength(1);

    int newRowsCount = (9 + (notClearedCellsCount - blockedCells)) / columns;
<<<<<<< HEAD
    Console.WriteLine($"Adding {newRowsCount} rows.");
=======
>>>>>>> fc4ca58 (Updates)

    int[,] newBoard = new int[Board.GetLength(0) + newRowsCount, Board.GetLength(1)];

    // Fill new board with Blcocked values.
    for (int i = 0; i < newBoard.GetLength(0); i++)
    {
      for (int j = 0; j < newBoard.GetLength(1); j++)
      {
        newBoard[i, j] = Blocked;
      }
    }

    // Fill newBoard with values which are on Board already.
    for (int i = 0; i <= LastRowIndex; i++)
    {
      for (int j = 0; j <= LastColumnIndex; j++)
      {
        newBoard[i, j] = Board[i, j];
      }
    }

<<<<<<< HEAD
    Console.WriteLine($"Last row index: {LastRowIndex}");

    // Append them to the board.
    int[] lastRow = GetRow(Board, LastRowIndex);

    Console.WriteLine($"Last row: {string.Join(',', lastRow)}");
=======
    // Append them to the board.
    int[] lastRow = GetRow(Board, LastRowIndex);
>>>>>>> fc4ca58 (Updates)

    int row;
    int column;
    // Find location first blocked value in that row.
    int firstBlockedIndex = Array.IndexOf(lastRow, Blocked);
    if (firstBlockedIndex == -1)
    {
      row = LastRowIndex + 1;
      column = 0;
    }
    else
    {
<<<<<<< HEAD
      Console.WriteLine($"Found first blocked index at {firstBlockedIndex}");
=======
>>>>>>> fc4ca58 (Updates)
      row = LastRowIndex;
      column = firstBlockedIndex;
    }

    // Copy all values from the board which aren't blocked or cleared.
    IEnumerable<int> values = Board.Cast<int>().Where(x => x != Blocked && x != Cleared);

<<<<<<< HEAD
    Console.WriteLine("Appending new rows...");
    foreach (int value in values)
    {
      //Console.WriteLine($"Appending value {value} to row {row} and column {column}");
=======
    foreach (int value in values)
    {
>>>>>>> fc4ca58 (Updates)
      newBoard[row, column] = value;

      column++;
      if (column > LastColumnIndex)
      {
        row++;
        column = 0;
      }

      if (row > newBoard.GetLength(0) - 1)
      {
        break;
      }
    }

    Board = newBoard;
<<<<<<< HEAD
    PrintBoard();
=======
    // PrintBoard();

>>>>>>> fc4ca58 (Updates)
  }

  public int CalculatePoints(Cell start, Cell found)
  {
    return CalculatePoints((Board.Clone() as int[,])!, start, found);
  }

  public static int CalculatePoints(int[,] board, Cell start, Cell found)
  {
    if (WouldClearBoard(board, start, found))
    {
      return 150;
    }

    if (WouldClearTwoLines(board, start, found))
    {
      return 20;
    }

    if (WouldClearEntireLine(board, start, found))
    {
      return 10;
    }

    (int, int) dist = (start.Row - found.Row, start.Column - found.Column);
    if (dist.Item1 is -1 or 0 or 1 && dist.Item2 is -1 or 0 or 1)
    {
      return 1;
    }
    
    int lastColumnIndex = board.GetLength(1) - 1;
    if (start.Column == 0 && found.Column == lastColumnIndex && start.Row != found.Row)
    {
      return 2;
    }
    else if (start.Column == lastColumnIndex && found.Column == 0 && start.Row != found.Row)
    {
      return 2;
    }

    if (IsClearedInBetween(board, start, found))
    {
      return 4;
    }

    return 150;
  }

  private static bool WouldClearBoard(int[,] board, Cell start, Cell found)
  {
    int[,] boardCopy = board;
    int lastRowIndex = boardCopy.GetLength(0) - 1;

    boardCopy![start.Row, start.Column] = Cleared;
    boardCopy[found.Row, found.Column] = Cleared;

    // check if all rows don't have any numeric values.
    for (var i = 0; i <= lastRowIndex; i++)
    {
      if (GetRow(boardCopy, i).Any(x => x is >= 1 and <= 9))
      {
        return false;
      }
    }

    return true;
  }

  private static bool WouldClearTwoLines(int[,] board, Cell start, Cell found)
  {
    if (start.Row == found.Row)
    {
      return false;
    }
    
    int[] row1 = GetRow(board, start.Row);
    row1[start.Column] = Cleared;

    int[] row2 = GetRow(board, found.Row);
    row2[found.Column] = Cleared;

    if (row1.All(x => x < 0) && row2.All(x => x < 0))
    {
      return true;
    }

    return false;
  }

  private static bool WouldClearEntireLine(int[,] board, Cell start, Cell found)
  {
    if (start.Row == found.Row)
    {
      int[] row = GetRow(board, start.Row);
      row[start.Column] = Cleared;
      row[found.Column] = Cleared;

      if (row.All(x => x < 0))
      {
        return true;
      }
    }

    return false;
  }

  private static bool IsClearedInBetween(int[,] board, Cell start, Cell found)
  { 
    int lastColumnIndex = board.GetLength(1) - 1;
    
    // First we have to find out if we are going towards left or right.
    if (found.Row < start.Row)
    {
      // search left
      var offset = new Cell { Row = 0, Column = -1 };
      Cell inspecting = start;

      while (found != inspecting)
      {
        inspecting = inspecting.AddOffset(offset);
        if (inspecting.Column > lastColumnIndex || inspecting.Column < 0)
        {
          inspecting = new Cell() { Row = inspecting.Row - 1, Column = lastColumnIndex };
          continue;
        }

        int inspectValue = board[inspecting.Row, inspecting.Column];
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
        if (inspecting.Column > lastColumnIndex || inspecting.Column < 0)
        {
          inspecting = new Cell() { Row = inspecting.Row + 1, Column = 0 };
          continue;
        }

        int inspectValue = board[inspecting.Row, inspecting.Column];
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
        if (inspecting.Column > lastColumnIndex || inspecting.Column < 0)
        {
          inspecting = new Cell() { Row = inspecting.Row + 1, Column = 0 };
          continue;
        }

        int inspectValue = board[inspecting.Row, inspecting.Column];
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
        if (inspecting.Column > lastColumnIndex || inspecting.Column < 0)
        {
          inspecting = new Cell() { Row = inspecting.Row - 1, Column = lastColumnIndex };
          continue;
        }

        int inspectValue = board[inspecting.Row, inspecting.Column];
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
    // if (Board.GetLength(0) == 0)
    // {
    //   Console.WriteLine("Board has been cleared.");
    //   return;
    // }

    for (var i = 0; i < Board.GetLength(0); i++)
    {
      string row = "{" + string.Join(',', GetRow(i)) + "},";
      row = row.Replace("-1", "Cleared").Replace("-2", "Blocked");

      Console.WriteLine(row);
    }

    Console.WriteLine("===");
  }
}

public interface IFindCellStrategy
{
  /// <summary>
  /// Searches for either the same value as in start or
  /// for value where sum of two is ten.
  /// </summary>
  /// <param name="board">State of the board</param>
  /// <param name="start">Search start location.</param>
  /// <returns>Location where pair was found. If pair wasn't found
  /// return Cords.Empty.</returns>
  Cell FindPair(int[,] board, Cell start);
}