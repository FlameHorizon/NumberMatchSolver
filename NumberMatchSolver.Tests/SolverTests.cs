namespace NumberMatchSolver.Tests;

using FluentAssertions;

public class SolverTests
{
  private const int Cleared = -1;
  private const int Blocked = -2;

  [Fact]
  public void ItDoesWork()
  {
    int[,] board =
    {
      { Cleared, Cleared, Cleared, Cleared, Cleared, Cleared, 7, Cleared, 5 },
      { 1, 5, 7, Cleared, Cleared, Cleared, 1, 7, 5 },
      { 1, 5, 7, 1, Blocked, Blocked, Blocked, Blocked, Blocked },
    };

    var sut = new Game(board);
    sut.Solve();

    sut.Board.Length.Should().Be(0);
  }
  
  [Fact]
  public void ItDoesWork2()
  {
    int[,] board =
    {
      {8,5,2,7,1,7,6,3,5},
      {7,4,9,5,6,5,2,1,4},
      {9,5,7,8,9,7,5,7,5},
      {1,4,1,6,5,Blocked,Blocked,Blocked,Blocked},
    };

    var sut = new Game(board);
    sut.Solve();

    int[,] expected =
    {
      { 8, 5, 2, 7, 1, Cleared, 6, 3, 5 },
      { 7, 4, 9, 5, 6, Cleared, 2, 1, 4 },
      { Cleared, 5, 7, 8, 9, Cleared, Cleared, 7, 5 },
      { Cleared, 4, 1, 6, 5, Blocked, Blocked, Blocked, Blocked },
    };
    sut.Board.Should().Equal(expected, (i, i1) => i == i1);
  }
  
  [Fact]
  public void ItDoesWork3()
  {
    int[,] board =
    {
      {8,5,2,7,1,Cleared,6,3,5},
      {7,4,9,5,6,Cleared,2,1,4},
      {Cleared,5,7,8,9,Cleared,Cleared,7,5},
      {Cleared,4,1,6,5,8,5,2,7},
      {1,6,3,5,7,4,9,5,6},
      {2,1,4,5,7,8,9,7,5},
      {4,1,6,5,Blocked,Blocked,Blocked,Blocked,Blocked},
    };

    var sut = new Game(board);
    sut.Solve();

    int[,] expected =
    {
      {Cleared,Cleared,Cleared,Cleared,Cleared,Cleared,Cleared,1,Cleared},
      {Cleared,Cleared,Cleared,Cleared,Cleared,Cleared,Cleared,Cleared,5},
      {4,1,6,5,Blocked,Blocked,Blocked,Blocked,Blocked},

    };
    sut.Board.Should().Equal(expected, (i, i1) => i == i1);
  }
  
  [Fact]
  public void ItDoesWork4()
  {
    int[,] board =
    {
      {Cleared,Cleared,Cleared,Cleared,Cleared,Cleared,Cleared,1,Cleared},
      {Cleared,Cleared,Cleared,Cleared,Cleared,Cleared,Cleared,Cleared,5},
      {4,1,6,5,1,1,5,4,6},
      {5,Blocked,Blocked,Blocked,Blocked,Blocked,Blocked,Blocked,Blocked},
    };

    var sut = new Game(board);
    sut.Solve();

    int[,] expected =
    {
      {Cleared,Cleared,Cleared,Cleared,Cleared,Cleared,Cleared,Cleared,5},
      {4,1,6,5,1,Cleared,Cleared,Cleared,Cleared},
    };
    sut.Board.Should().Equal(expected, (i, i1) => i == i1);
  }
}