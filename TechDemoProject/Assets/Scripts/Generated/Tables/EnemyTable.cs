// <auto-generated />
#pragma warning disable CS0105
using MasterMemory.Validation;
using MasterMemory;
using MessagePack;
using System.Collections.Generic;
using System;
using Tech.Data.DB;
using Tech.DB;

namespace MasterData.Tables
{
   public sealed partial class EnemyTable : TableBase<Enemy>, ITableUniqueValidate
   {
        public Func<Enemy, string> PrimaryKeySelector => primaryIndexSelector;
        readonly Func<Enemy, string> primaryIndexSelector;

        readonly Enemy[] secondaryIndex0;
        readonly Func<Enemy, int> secondaryIndex0Selector;

        public EnemyTable(Enemy[] sortedData)
            : base(sortedData)
        {
            this.primaryIndexSelector = x => x.Name;
            this.secondaryIndex0Selector = x => x.Index;
            this.secondaryIndex0 = CloneAndSortBy(this.secondaryIndex0Selector, System.Collections.Generic.Comparer<int>.Default);
            OnAfterConstruct();
        }

        partial void OnAfterConstruct();

        public RangeView<Enemy> SortByIndex => new RangeView<Enemy>(secondaryIndex0, 0, secondaryIndex0.Length - 1, true);

        public Enemy FindByName(string key)
        {
            return FindUniqueCore(data, primaryIndexSelector, System.StringComparer.Ordinal, key, false);
        }
        
        public bool TryFindByName(string key, out Enemy result)
        {
            return TryFindUniqueCore(data, primaryIndexSelector, System.StringComparer.Ordinal, key, out result);
        }

        public Enemy FindClosestByName(string key, bool selectLower = true)
        {
            return FindUniqueClosestCore(data, primaryIndexSelector, System.StringComparer.Ordinal, key, selectLower);
        }

        public RangeView<Enemy> FindRangeByName(string min, string max, bool ascendant = true)
        {
            return FindUniqueRangeCore(data, primaryIndexSelector, System.StringComparer.Ordinal, min, max, ascendant);
        }

        public Enemy FindByIndex(int key)
        {
            return FindUniqueCoreInt(secondaryIndex0, secondaryIndex0Selector, System.Collections.Generic.Comparer<int>.Default, key, false);
        }
        
        public bool TryFindByIndex(int key, out Enemy result)
        {
            return TryFindUniqueCoreInt(secondaryIndex0, secondaryIndex0Selector, System.Collections.Generic.Comparer<int>.Default, key, out result);
        }

        public Enemy FindClosestByIndex(int key, bool selectLower = true)
        {
            return FindUniqueClosestCore(secondaryIndex0, secondaryIndex0Selector, System.Collections.Generic.Comparer<int>.Default, key, selectLower);
        }

        public RangeView<Enemy> FindRangeByIndex(int min, int max, bool ascendant = true)
        {
            return FindUniqueRangeCore(secondaryIndex0, secondaryIndex0Selector, System.Collections.Generic.Comparer<int>.Default, min, max, ascendant);
        }


        void ITableUniqueValidate.ValidateUnique(ValidateResult resultSet)
        {
            ValidateUniqueCore(data, primaryIndexSelector, "Name", resultSet);       
            ValidateUniqueCore(secondaryIndex0, secondaryIndex0Selector, "Index", resultSet);       
        }

        public static MasterMemory.Meta.MetaTable CreateMetaTable()
        {
            return new MasterMemory.Meta.MetaTable(typeof(Enemy), typeof(EnemyTable), "enemy",
                new MasterMemory.Meta.MetaProperty[]
                {
                    new MasterMemory.Meta.MetaProperty(typeof(Enemy).GetProperty("Id")),
                    new MasterMemory.Meta.MetaProperty(typeof(Enemy).GetProperty("Name")),
                    new MasterMemory.Meta.MetaProperty(typeof(Enemy).GetProperty("Index")),
                    new MasterMemory.Meta.MetaProperty(typeof(Enemy).GetProperty("Description")),
                    new MasterMemory.Meta.MetaProperty(typeof(Enemy).GetProperty("ImageBytes")),
                    new MasterMemory.Meta.MetaProperty(typeof(Enemy).GetProperty("EnemyInfo")),
                    new MasterMemory.Meta.MetaProperty(typeof(Enemy).GetProperty("Weapon")),
                    new MasterMemory.Meta.MetaProperty(typeof(Enemy).GetProperty("Equipment")),
                    new MasterMemory.Meta.MetaProperty(typeof(Enemy).GetProperty("Ability")),
                    new MasterMemory.Meta.MetaProperty(typeof(Enemy).GetProperty("Skills")),
                },
                new MasterMemory.Meta.MetaIndex[]{
                    new MasterMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(Enemy).GetProperty("Name"),
                    }, true, true, System.StringComparer.Ordinal),
                    new MasterMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(Enemy).GetProperty("Index"),
                    }, false, true, System.Collections.Generic.Comparer<int>.Default),
                });
        }

    }
}