// <auto-generated />
#pragma warning disable CS0105
using MasterMemory.Validation;
using MasterMemory;
using MessagePack;
using System.Collections.Generic;
using System;
using MasterData.Tables;

namespace MasterData
{
   public sealed class MemoryDatabase : MemoryDatabaseBase
   {
        public TestTableTable TestTableTable { get; private set; }

        public MemoryDatabase(
            TestTableTable TestTableTable
        )
        {
            this.TestTableTable = TestTableTable;
        }

        public MemoryDatabase(byte[] databaseBinary, bool internString = true, MessagePack.IFormatterResolver formatterResolver = null)
            : base(databaseBinary, internString, formatterResolver)
        {
        }

        protected override void Init(Dictionary<string, (int offset, int count)> header, System.ReadOnlyMemory<byte> databaseBinary, MessagePack.MessagePackSerializerOptions options)
        {
            this.TestTableTable = ExtractTableData<TestTable, TestTableTable>(header, databaseBinary, options, xs => new TestTableTable(xs));
        }

        public ImmutableBuilder ToImmutableBuilder()
        {
            return new ImmutableBuilder(this);
        }

        public DatabaseBuilder ToDatabaseBuilder()
        {
            var builder = new DatabaseBuilder();
            builder.Append(this.TestTableTable.GetRawDataUnsafe());
            return builder;
        }

        public DatabaseBuilder ToDatabaseBuilder(MessagePack.IFormatterResolver resolver)
        {
            var builder = new DatabaseBuilder(resolver);
            builder.Append(this.TestTableTable.GetRawDataUnsafe());
            return builder;
        }

        public ValidateResult Validate()
        {
            var result = new ValidateResult();
            var database = new ValidationDatabase(new object[]
            {
                TestTableTable,
            });

            ((ITableUniqueValidate)TestTableTable).ValidateUnique(result);
            ValidateTable(TestTableTable.All, database, "Name", TestTableTable.PrimaryKeySelector, result);

            return result;
        }

        static MasterMemory.Meta.MetaDatabase metaTable;

        public static object GetTable(MemoryDatabase db, string tableName)
        {
            switch (tableName)
            {
                case "Test":
                    return db.TestTableTable;
                
                default:
                    return null;
            }
        }

        public static MasterMemory.Meta.MetaDatabase GetMetaDatabase()
        {
            if (metaTable != null) return metaTable;

            var dict = new Dictionary<string, MasterMemory.Meta.MetaTable>();
            dict.Add("Test", MasterData.Tables.TestTableTable.CreateMetaTable());

            metaTable = new MasterMemory.Meta.MetaDatabase(dict);
            return metaTable;
        }
    }
}