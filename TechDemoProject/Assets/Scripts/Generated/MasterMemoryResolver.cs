// <auto-generated />
#pragma warning disable CS0105
using MasterMemory.Validation;
using MasterMemory;
using MessagePack;
using System.Collections.Generic;
using System;
using Tech.Data.DB;
using Tech.DB;
using MasterData.Tables;

namespace MasterData
{
    public class MasterMemoryResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new MasterMemoryResolver();

        MasterMemoryResolver()
        {

        }

        public global::MessagePack.Formatters.IMessagePackFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.formatter;
        }

        static class FormatterCache<T>
        {
            public static readonly global::MessagePack.Formatters.IMessagePackFormatter<T> formatter;

            static FormatterCache()
            {
                var f = MasterMemoryResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class MasterMemoryResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static MasterMemoryResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(9)
            {
                {typeof(Ability[]), 0 },
                {typeof(Enemy[]), 1 },
                {typeof(Equipment[]), 2 },
                {typeof(Item[]), 3 },
                {typeof(Skill[]), 4 },
                {typeof(TechMaterial[]), 5 },
                {typeof(Unit[]), 6 },
                {typeof(User[]), 7 },
                {typeof(Weapon[]), 8 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new MessagePack.Formatters.ArrayFormatter<Ability>();
                case 1: return new MessagePack.Formatters.ArrayFormatter<Enemy>();
                case 2: return new MessagePack.Formatters.ArrayFormatter<Equipment>();
                case 3: return new MessagePack.Formatters.ArrayFormatter<Item>();
                case 4: return new MessagePack.Formatters.ArrayFormatter<Skill>();
                case 5: return new MessagePack.Formatters.ArrayFormatter<TechMaterial>();
                case 6: return new MessagePack.Formatters.ArrayFormatter<Unit>();
                case 7: return new MessagePack.Formatters.ArrayFormatter<User>();
                case 8: return new MessagePack.Formatters.ArrayFormatter<Weapon>();
                default: return null;
            }
        }
    }
}