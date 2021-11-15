namespace System.Collections.Generic;

public static class ListExtensions
{
    public static int Replace<T>(this IList<T> source, T oldValue, T newValue)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        var index = source.IndexOf(oldValue);
        if (index != -1)
            source[index] = newValue;
        return index;
    }
}
