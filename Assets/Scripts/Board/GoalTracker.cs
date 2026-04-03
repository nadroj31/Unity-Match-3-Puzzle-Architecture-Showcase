using System;
using UnityEngine;

/// <summary>
/// Tracks progress toward a single brick-type goal and fires events when updated or completed.
/// Once completed, all further <see cref="RegisterMatch"/> calls are ignored.
/// </summary>
public class GoalTracker
{
    /// <summary>Fired each time the remaining count decreases. Argument is the new count.</summary>
    public event Action<int> OnGoalCountChanged;

    /// <summary>Fired exactly once when the remaining count first reaches zero.</summary>
    public event Action OnGoalCompleted;

    public BrickTypeSO GoalType   { get; }
    public int         Remaining  { get; private set; }
    public bool        IsComplete { get; private set; }

    public GoalTracker(BrickTypeSO goalType, int count)
    {
        GoalType  = goalType;
        Remaining = count;
    }

    /// <summary>
    /// Registers a completed match. Decrements the counter when the matched type qualifies.
    /// A goal whose <see cref="BrickTypeSO.IsRandom"/> flag is set accepts any colour.
    /// Does nothing if this goal is already complete.
    /// </summary>
    public void RegisterMatch(BrickTypeSO matchedType, int count)
    {
        if (IsComplete) return;
        if (!GoalType.IsRandom && GoalType != matchedType) return;

        Remaining = Mathf.Max(Remaining - count, 0);
        OnGoalCountChanged?.Invoke(Remaining);

        if (Remaining <= 0)
        {
            IsComplete = true;
            OnGoalCompleted?.Invoke();
        }
    }
}
