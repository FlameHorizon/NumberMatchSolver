namespace NumberMatchSolver;

public interface IMoveStrategy
{
  void RegisterPairFound();

  int CurrentColumnIndex { get; set; }

  int CurrentRowIndex { get; set; }

  void RegisterPairNotFound();

  void RegisterOutOfBoundsColumn();

  void RegisterOutOfBoundsRow();

  bool ReachedLastColumn();

  Cell? NextCell();

  bool ReachedLastRow();

  void UpdateBoard(int[,] board);
}