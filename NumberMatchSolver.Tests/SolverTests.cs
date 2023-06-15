namespace NumberMatchSolver.Tests;
using FluentAssertions;

public class SolverTests
{
  private const int Cleared = -1;
  private const int Blocked = -2;
  
  [Fact]
  public void ItDoesWork()
  {
    int[,] board = new[,]
    {
      { Cleared, Cleared, Cleared, Cleared, Cleared, Cleared, 7, Cleared, 5 },
      { 1, 5, 7, Cleared, Cleared, Cleared, 1, 7, 5 },
      { 1, 5, 7, 1, Blocked, Blocked, Blocked, Blocked, Blocked },
    };
    
    var sut = new Game(board);
    sut.Solve();

    sut.Board.Length.Should().Be(0);
  }
}