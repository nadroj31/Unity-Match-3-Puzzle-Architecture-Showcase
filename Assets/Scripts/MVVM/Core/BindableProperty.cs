using System;
using System.Collections.Generic;

/// <summary>
/// A generic property wrapper that notifies subscribers when its value changes.
/// The setter fires <see cref="ValueChanged"/> only when the incoming value differs from
/// the current one (compared via <see cref="EqualityComparer{T}"/>).
/// A lock guards the stored value so reads and writes are safe if ever called from
/// a background thread, but in normal Unity usage all access is on the main thread.
/// </summary>
/// <typeparam name="T">The type of the wrapped value.</typeparam>
public class BindableProperty<T>
{
    /// <summary>Invoked when <see cref="Value"/> changes, providing the old and new values.</summary>
    public delegate void ValueChangedHandler(T oldValue, T newValue);

    /// <summary>Raised after <see cref="Value"/> has been updated to a different value.</summary>
    public event ValueChangedHandler ValueChanged;

    private T _value;
    private readonly object _lock = new();

    /// <param name="initialValue">The initial value; defaults to <c>default(T)</c>.</param>
    public BindableProperty(T initialValue = default)
    {
        _value = initialValue;
    }

    /// <summary>
    /// Gets or sets the value. The setter fires <see cref="ValueChanged"/> only when the
    /// new value differs from the current one, and always outside the lock to avoid deadlocks.
    /// </summary>
    public T Value
    {
        get
        {
            lock (_lock) { return _value; }
        }
        set
        {
            T oldValue;
            bool changed = false;
            lock (_lock)
            {
                if (!EqualityComparer<T>.Default.Equals(_value, value))
                {
                    oldValue = _value;
                    _value = value;
                    changed = true;
                }
                else
                {
                    return;
                }
            }

            if (changed)
            {
                ValueChanged?.Invoke(oldValue, value);
            }
        }
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        lock (_lock) { return _value != null ? _value.ToString() : "null"; }
    }
}
