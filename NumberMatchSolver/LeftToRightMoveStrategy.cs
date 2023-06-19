namespace NumberMatchSolver;

public class LeftToRightMoveStrategy : IMoveStrategy
{
  private int[,] _board;
  private int LastRowIndex => _board.GetLength(0) - 1;
  private int LastColumnIndex => _board.GetLength(1) - 1;

  public LeftToRightMoveStrategy(int[,] board) : this (board, 0,0)
  {
  }

  public LeftToRightMoveStrategy(int[,] board, int startColumnIndex = 0, int startRowIndex = 0)
  {
    _board = board;
    CurrentColumnIndex = startColumnIndex;
    CurrentRowIndex = startRowIndex;
  }
  
  public void RegisterPairFound()
  {
    CurrentRowIndex = 0;
    CurrentColumnIndex = 0;
  }

  public int CurrentColumnIndex { get; set; }

  public int CurrentRowIndex { get; set; }

  public void RegisterPairNotFound()
  {
    CurrentColumnIndex++;
  }

  public void RegisterOutOfBoundsColumn()
  {
    CurrentRowIndex++;
    CurrentColumnIndex = 0;
  }

  public void RegisterOutOfBoundsRow()
  {
    CurrentRowIndex = 0;
    CurrentColumnIndex = 0;
  }

  public bool ReachedLastColumn()
  {
    return CurrentColumnIndex > LastColumnIndex;
  }

  public Cell? NextCell()
  {
    return new Cell() { Row = CurrentRowIndex, Column = CurrentColumnIndex };
  }

  public bool ReachedLastRow()
  {
    return CurrentRowIndex > LastRowIndex;
  }

  public void UpdateBoard(int[,] board)
  {
    _board = board;
  }
}