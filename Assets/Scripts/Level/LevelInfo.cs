/// <summary>JSON-serialisable data transfer object for a single level file.</summary>
[System.Serializable]
public class LevelInfo
{
    public int        levelNumber;
    public int        gridWidth;
    public int        gridHeight;

    /// <summary>Maximum number of moves the player is allowed. 0 means unlimited (no fail condition).</summary>
    public int        moveLimit;

    /// <summary>
    /// Minimum moves remaining on victory to earn 2 stars. 0 = threshold disabled (always 1 star for move-limited levels).
    /// Ignored when <see cref="moveLimit"/> is 0 (unlimited levels always award 3 stars).
    /// </summary>
    public int        star2Threshold;

    /// <summary>
    /// Minimum moves remaining on victory to earn 3 stars. 0 = threshold disabled.
    /// Ignored when <see cref="moveLimit"/> is 0 (unlimited levels always award 3 stars).
    /// </summary>
    public int        star3Threshold;

    /// <summary>
    /// Up to three brick-clearing goals. An empty array means no win condition (free play).
    /// </summary>
    public GoalInfo[] goals;

    /// <summary>Flat grid array in top-to-bottom, left-to-right order. Length must equal gridWidth × gridHeight.</summary>
    public string[]   grid;
}
