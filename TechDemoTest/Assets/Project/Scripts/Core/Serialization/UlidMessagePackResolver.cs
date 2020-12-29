using System;
using MessagePack;
using MessagePack.Formatters;

public class UlidMessagePackResolver : IFormatterResolver
{
    public static IFormatterResolver Instance = new UlidMessagePackResolver();

    public IMessagePackFormatter<T> GetFormatter<T>()
    {
        return Cache<T>.formatter;
    }

    private static class Cache<T>
    {
        public static readonly IMessagePackFormatter<T> formatter;

        static Cache()
        {
            if (typeof(T) == typeof(Ulid))
                formatter = (IMessagePackFormatter<T>) (object) new UlidMessagePackFormatter();
        }
    }
}