using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// This part handles the logic of the board.
/// Including finding pairs and refilling the board.
/// </summary>
public class GamePlayBoardHandler
{
    private static GamePlayBoardHandler instance;
    public static GamePlayBoardHandler Instance => instance ??= new GamePlayBoardHandler();

    private List<Brick> matchBricks = new List<Brick>();
    private Brick[,] bricks;

    public List<Brick> FindMatchBricks(Brick clickBrick, Brick[,] boardBricks)
    {
        matchBricks.Clear();
        GetMatchBricks(clickBrick, boardBricks);
        return matchBricks;
    }

    private void GetMatchBricks(Brick clickBrick, Brick[,] boardBricks)
    {
        bricks = boardBricks;

        if (!matchBricks.Contains(clickBrick))
        {
            matchBricks.Add(clickBrick);
        }

        int[][] directions =
        {
            new[] {1, 0}, // Right
            new[] {-1, 0}, // Left
            new[] {0, 1}, // Up
            new[] {0, -1} // Down
        };

        foreach (var dir in directions)
        {
            int x = clickBrick.X, y = clickBrick.Y;
            while (true)
            {
                x += dir[0];
                y += dir[1];

                if (x < 0 || x >= bricks.GetLength(0) || y < 0 || y >= bricks.GetLength(1))
                {
                    break;
                }

                Brick currentBrick = bricks[x, y];
                if (currentBrick.BrickType != clickBrick.BrickType || matchBricks.Contains(currentBrick))
                {
                    break;
                }

                matchBricks.Add(currentBrick);
                GetMatchBricks(currentBrick, boardBricks);
            }
        }
    }

    public void FillingBoard(List<Brick> matchBricks, Action<Brick, Brick> OnFillingBoard)
    {
        var processColumns = matchBricks.GroupBy(x => x.X).Select(x => x.First()).ToList();

        foreach (var column in processColumns)
        {
            List<Brick> elementsInColumn = matchBricks.FindAll(x => x.X == column.X).OrderByDescending(x => x.Y).ToList();
            int columnIndex = column.X;
            int topRowIndex = elementsInColumn[0].Y;
            int bottomRowIndex = elementsInColumn[elementsInColumn.Count - 1].Y;

            if (bottomRowIndex < bricks.GetLength(1) - 1)
            {
                for (int j = 0; j < bricks.GetLength(1) - bottomRowIndex; ++j)
                {
                    Brick bottomBrick = bricks[columnIndex, bottomRowIndex + j];
                    if (topRowIndex + 1 + j >= bricks.GetLength(1))
                    {
                        bottomBrick.SetBrickType(GetRandomBrickType());
                        OnFillingBoard?.Invoke(null, bottomBrick);
                    }
                    else
                    {
                        Brick aboveBrick = bricks[columnIndex, topRowIndex + 1 + j];
                        bottomBrick.SetBrickType(aboveBrick.BrickType);
                        OnFillingBoard?.Invoke(aboveBrick, bottomBrick);
                    }
                }
            }
            else
            {
                Brick bottomBrick = bricks[columnIndex, bricks.GetLength(1) - 1];
                bottomBrick.SetBrickType(GetRandomBrickType());
                OnFillingBoard?.Invoke(null, bottomBrick);
            }
        }
    }

    private BrickType GetRandomBrickType()
    {
        BrickType[] brickTypes = (BrickType[])Enum.GetValues(typeof(BrickType));
        return brickTypes[UnityEngine.Random.Range(2, brickTypes.Length)];
    }
}
