using System;
using MessagePack;
using MessagePack.Formatters;

internal sealed class UlidMessagePackResolver : IFormatterResolver
{
    internal static readonly IFormatterResolver Instance = new UlidMessagePackResolver();

    public IMessagePackFormatter<T> GetFormatter<T>()
    {
        return Cache<T>.Formatter;
    }

    private static class Cache<T>
    {
        public static readonly IMessagePackFormatter<T> Formatter;

        static Cache()
        {
            if (typeof(T) == typeof(Ulid))
                Formatter = (IMessagePackFormatter<T>) (object) new UlidMessagePackFormatter();
        }
    }
}