namespace NumberMatchSolver;

public class CompleteMoveStrategy : IMoveStrategy
{
  private readonly int[,] _board;

  public CompleteMoveStrategy(int[,] board)
  {
    _board = board;
  }

  public void RegisterPairFound()
  {
    throw new NotImplementedException();
  }

  public int CurrentColumnIndex { get; set; } = 0;
  public int CurrentRowIndex { get; set; } = 0;
  
  public void RegisterPairNotFound()
  {
    throw new NotImplementedException();
  }

  public void RegisterOutOfBoundsColumn()
  {
    throw new NotImplementedException();
  }

  public void RegisterOutOfBoundsRow()
  {
    throw new NotImplementedException();
  }

  public bool ReachedLastColumn()
  {
    throw new NotImplementedException();
  }

  public Cell? NextCell()
  {
    throw new NotImplementedException();
  }

  public bool ReachedLastRow()
  {
    throw new NotImplementedException();
  }

  public void UpdateBoard(int[,] board)
  {
    throw new NotImplementedException();
  }
}