using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    private readonly Dictionary<int, LevelDetails> levels = new Dictionary<int, LevelDetails>();

    private static LevelData instance;
    public static LevelData Instance => instance ??= new LevelData();

    public void LoadLevelData()
    {
        levels.Clear();
        foreach (var jsonFile in Resources.LoadAll<TextAsset>("LevelInfos"))
        {
            LevelInfo levelInfo = JsonUtility.FromJson<LevelInfo>(jsonFile.text);
            LevelDetails levelDetails = new LevelDetails
            {
                levelNumber = levelInfo.levelNumber,
                gridWidth = levelInfo.gridWidth,
                gridHeight = levelInfo.gridHeight,
                goal = levelInfo.goal,
                goalNumber = levelInfo.goalNumber,
                gridData = new BrickType[levelInfo.gridWidth, levelInfo.gridHeight]
            };

            int gridIndex = 0;
            BrickType[] brickTypes = (BrickType[])Enum.GetValues(typeof(BrickType));
            for (int i = levelInfo.gridHeight - 1; i >= 0; --i)
            {
                for (int j = 0; j < levelInfo.gridWidth; ++j)
                {
                    levelDetails.gridData[j, i] = levelInfo.grid[gridIndex++] switch
                    {
                        "b" => BrickType.BLUE_BRICK,
                        "g" => BrickType.GREEN_BRICK,
                        "r" => BrickType.RED_BRICK,
                        "y" => BrickType.YELLOW_BRICK,
                        _ => brickTypes[UnityEngine.Random.Range(2, brickTypes.Length)]
                    };
                }
            }

            levels.Add(levelDetails.levelNumber, levelDetails);
        }
    }

    public List<int> GetAllLevelKeys()
    {
        List<int> keyList = new List<int>(levels.Keys);
        return keyList;
    } 

    public LevelDetails GetLevelDetails(int level)
    {
        if (levels.TryGetValue(level, out var value))
        {
            return value;
        }

        Debug.LogError($"There is no data for level {level}");
        return null;
    }
}
