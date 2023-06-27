namespace NumberMatchSolver;

public class Cell : IEquatable<Cell>
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

  public bool Equals(Cell? other)
  {
    if (ReferenceEquals(null, other)) return false;
    if (ReferenceEquals(this, other)) return true;
    return Row == other.Row && Column == other.Column;
  }

  public override bool Equals(object? obj)
  {
    if (ReferenceEquals(null, obj)) return false;
    if (ReferenceEquals(this, obj)) return true;
    if (obj.GetType() != this.GetType()) return false;
    return Equals((Cell)obj);
  }

  public override int GetHashCode()
  {
    return HashCode.Combine(Row, Column);
  }
}