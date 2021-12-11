using System.Runtime.InteropServices;

namespace Natick.SharedKernel;

[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct Maybe<T>
{
    public static Maybe<T> None = new Maybe<T>();

    public bool HasValue { get; private set; }

    private T _value;
    public T Value
    {
        get
        {
            if (!HasValue)
            {
                throw new InvalidOperationException("Maybe<T> object must have a value.");
            }

            return _value;
        }
        set
        {
            HasValue = value != null;
            _value = value;
        }
    }

    public Maybe(T value = default(T))
    {
        HasValue =  true;
        _value = value;
    }

    public T GetValueOrDefault()
    {
        return _value;
    }

    public T GetValueOrDefault(T defaultValue)
    {
        return HasValue ? _value : defaultValue;
    }

    public static explicit operator T(Maybe<T> optional)
    {
        return optional._value;
    }

    public static explicit operator Maybe<T>(T value)
    {
        return new Maybe<T>(value);
    }

    public static implicit operator bool(Maybe<T> optional)
    {
        return optional.HasValue;
    }

    public static bool operator true(Maybe<T> optional)
    {
        return optional.HasValue;
    }

    public static bool operator false(Maybe<T> optional)
    {
        return !optional.HasValue;
    }
}
