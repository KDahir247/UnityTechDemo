using System;
using MessagePack;
using MessagePack.Resolvers;

namespace Tech.Utility
{
    public static class TechUtility
    {
        private static MessagePackSerializerOptions _options;

        static TechUtility()
        {
            _options = MessagePackSerializerOptions.Standard
                .WithResolver(StaticCompositeResolver.Instance)
                .WithCompression(MessagePackCompression.Lz4BlockArray);
        }

        internal static byte[] RegisterUlid(in Ulid ulid)
        {
            return MessagePackSerializer.Serialize(ulid, _options);
        }

        internal static Ulid UnRegisterUlid(in byte[] byteBuffer) 
        {
            return MessagePackSerializer.Deserialize<Ulid>(byteBuffer, _options); 
        }

    }
}