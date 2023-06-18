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
      { 8, 5, 2, 7, 1, 7, 6, 3, 5 },
      { 7, 4, 9, 5, 6, 5, 2, 1, 4 },
      { 9, 5, 7, 8, 9, 7, 5, 7, 5 },
      { 1, 4, 1, 6, 5, Blocked, Blocked, Blocked, Blocked },
    };

    var sut = new Game(board);
    sut.Solve();

    sut.Board.Length.Should().Be(0);
  }

  [Fact]
  public void ItDoesWork3()
  {
    int[,] board =
    {
      { 8, 5, 2, 7, 1, Cleared, 6, 3, 5 },
      { 7, 4, 9, 5, 6, Cleared, 2, 1, 4 },
      { Cleared, 5, 7, 8, 9, Cleared, Cleared, 7, 5 },
      { Cleared, 4, 1, 6, 5, 8, 5, 2, 7 },
      { 1, 6, 3, 5, 7, 4, 9, 5, 6 },
      { 2, 1, 4, 5, 7, 8, 9, 7, 5 },
      { 4, 1, 6, 5, Blocked, Blocked, Blocked, Blocked, Blocked },
    };

    var sut = new Game(board);
    sut.Solve();

    sut.Board.Length.Should().Be(0);
  }

  [Fact]
  public void ItDoesWork4()
  {
    int[,] board =
    {
      { Cleared, Cleared, Cleared, Cleared, Cleared, Cleared, Cleared, 1, Cleared },
      { Cleared, Cleared, Cleared, Cleared, Cleared, Cleared, Cleared, Cleared, 5 },
      { 4, 1, 6, 5, 1, 1, 5, 4, 6 },
      { 5, Blocked, Blocked, Blocked, Blocked, Blocked, Blocked, Blocked, Blocked },
    };

    var sut = new Game(board);
    sut.Solve();

    sut.Board.Length.Should().Be(0);
  }

  [Fact]
  // For finding a pair of numbers with equal value in adjacent cells.
  public void CalculatePointsForEqualValueAdjacentCells1()
  {
    int[,] board =
    {
      { 4, 4, 3 },
      { 4, 4, 3 }
    };

    var game = new Game(board);
    var start = new Cell() { Row = 0, Column = 0 };
    var found = new Cell() { Row = 0, Column = 1 };

    int actual = game.CalculatePoints(start, found);
    var expected = 1;
    actual.Should().Be(expected);
  }

  [Fact]
  public void CalculatePointsForEqualValueAdjacentCells2()
  {
    int[,] board =
    {
      { 4, 2 },
      { 4, 3 }
    };

    var game = new Game(board);
    var start = new Cell() { Row = 0, Column = 0 };
    var found = new Cell() { Row = 1, Column = 0 };

    int actual = game.CalculatePoints(start, found);
    var expected = 1;
    actual.Should().Be(expected);
  }
  
  [Fact]
  public void CalculatePointsForEqualValueAdjacentCells3()
  {
    int[,] board =
    {
      { 4, 2, 3 },
      { 1, 4, 3 }
    };

    var game = new Game(board);
    var start = new Cell() { Row = 0, Column = 0 };
    var found = new Cell() { Row = 1, Column = 1 };

    int actual = game.CalculatePoints(start, found);
    var expected = 1;
    actual.Should().Be(expected);
  }
  
  [Fact]
  public void CalculatePointsForDifferentValuesAddUp10AdjacentCells1()
  {
    int[,] board =
    {
      { 6, 4,6 },
      { 5, 3,7 }
    };

    var game = new Game(board);
    var start = new Cell() { Row = 0, Column = 0 };
    var found = new Cell() { Row = 0, Column = 1 };

    int actual = game.CalculatePoints(start, found);
    var expected = 1;
    actual.Should().Be(expected);
  }
  
  [Fact]
  public void CalculatePointsForDifferentValuesAddUp10AdjacentCells2()
  {
    int[,] board =
    {
      { 4, 2 },
      { 6, 3 }
    };

    var game = new Game(board);
    var start = new Cell() { Row = 0, Column = 0 };
    var found = new Cell() { Row = 1, Column = 0 };

    int actual = game.CalculatePoints(start, found);
    var expected = 1;
    actual.Should().Be(expected);
  }
  
  [Fact]
  public void CalculatePointsForDifferentValuesAddUp10AdjacentCells3()
  {
    int[,] board =
    {
      { 4, 2, 3 },
      { 1, 6, 3 }
    };

    var game = new Game(board);
    var start = new Cell() { Row = 0, Column = 0 };
    var found = new Cell() { Row = 1, Column = 1 };

    int actual = game.CalculatePoints(start, found);
    var expected = 1;
    actual.Should().Be(expected);
  }
  
  // For finding a pair of numbers with equal value, one of which is at the end of a
  // line and the other is at the beginning of the next line.
  [Fact]
  public void CalculatePointsForEqualValueEndOfLineAndAtBeginning1()
  {
    int[,] board =
    {
      { 1, 2, 3 },
      { 3, 6, 4 }
    };

    var game = new Game(board);
    var start = new Cell() { Row = 0, Column = 2 };
    var found = new Cell() { Row = 1, Column = 0 };

    int actual = game.CalculatePoints(start, found);
    var expected = 2;
    actual.Should().Be(expected);
  }
  
  // For finding a pair of numbers with equal values separated by empty cells.
  [Fact]
  public void CalculatePointsForEqualValueSeparatedByEmptyCells1()
  {
    int[,] board =
    {
      { 1, Cleared, 1, 2},
      { 3, 2, 4, 2}
    };

    var game = new Game(board);
    var start = new Cell() { Row = 0, Column = 0 };
    var found = new Cell() { Row = 0, Column = 2 };

    int actual = game.CalculatePoints(start, found);
    var expected = 4;
    actual.Should().Be(expected);
  }
  
  // For deleting a line of numbers
  [Fact]
  public void CalculatePointsForDeletingALineOfNumbers1()
  {
    int[,] board =
    {
      { 1, Cleared, 1},
      { 3, 2, 4}
    };

    var game = new Game(board);
    var start = new Cell() { Row = 0, Column = 0 };
    var found = new Cell() { Row = 0, Column = 2 };

    int actual = game.CalculatePoints(start, found);
    var expected = 10;
    actual.Should().Be(expected);
  }
  
  // For deleting a line of numbers
  [Fact]
  public void CalculatePointsForDeletingALineOfNumbers2()
  {
    int[,] board =
    {
      { 1, Cleared, Cleared},
      { 1, Cleared, Cleared},
      { 3, 2, 4}
    };

    var game = new Game(board);
    var start = new Cell() { Row = 0, Column = 0 };
    var found = new Cell() { Row = 1, Column = 0 };

    int actual = game.CalculatePoints(start, found);
    var expected = 20;
    actual.Should().Be(expected);
  }
  
  // For finding all pairs on the field
  [Fact]
  public void CalculatePointsForClearingTheBoard()
  {
    int[,] board =
    {
      { 1, Cleared, Cleared},
      { 1, Cleared, Cleared},
    };

    var game = new Game(board);
    var start = new Cell() { Row = 0, Column = 0 };
    var found = new Cell() { Row = 1, Column = 0 };

    int actual = game.CalculatePoints(start, found);
    var expected = 150;
    actual.Should().Be(expected);
  }

}