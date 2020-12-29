using System;
using System.Buffers;
using MessagePack;
using MessagePack.Formatters;

internal sealed class UlidMessagePackFormatter : IMessagePackFormatter<Ulid>
{
    public void Serialize(ref MessagePackWriter writer, Ulid value, MessagePackSerializerOptions options)
    {
        const int length = 16;

        writer.WriteBinHeader(length);
        var buffer = writer.GetSpan(length);
        value.TryWriteBytes(buffer);
        writer.Advance(length);
    }

    public Ulid Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var bin = reader.ReadBytes();
        if (bin == null)
            throw new MessagePackSerializationException(string.Format("Unexpected msgpack code {0} ({1}) encountered.",
                MessagePackCode.Nil, MessagePackCode.ToFormatName(MessagePackCode.Nil)));

        var seq = bin.Value;
        if (seq.IsSingleSegment) return new Ulid(seq.First.Span);

        Span<byte> buf = stackalloc byte[16];
        seq.CopyTo(buf);
        return new Ulid(buf);
    }
}