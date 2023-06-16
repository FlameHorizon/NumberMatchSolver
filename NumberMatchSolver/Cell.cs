namespace NumberMatchSolver;

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