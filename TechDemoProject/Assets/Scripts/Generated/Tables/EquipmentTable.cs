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
   public sealed partial class EquipmentTable : TableBase<Equipment>, ITableUniqueValidate
   {
        public Func<Equipment, string> PrimaryKeySelector => primaryIndexSelector;
        readonly Func<Equipment, string> primaryIndexSelector;

        readonly Equipment[] secondaryIndex2;
        readonly Func<Equipment, int> secondaryIndex2Selector;

        public EquipmentTable(Equipment[] sortedData)
            : base(sortedData)
        {
            this.primaryIndexSelector = x => x.Name;
            this.secondaryIndex2Selector = x => x.Index;
            this.secondaryIndex2 = CloneAndSortBy(this.secondaryIndex2Selector, System.Collections.Generic.Comparer<int>.Default);
            OnAfterConstruct();
        }

        partial void OnAfterConstruct();

        public RangeView<Equipment> SortByIndex => new RangeView<Equipment>(secondaryIndex2, 0, secondaryIndex2.Length - 1, true);

        public Equipment FindByName(string key)
        {
            return FindUniqueCore(data, primaryIndexSelector, System.StringComparer.Ordinal, key, false);
        }
        
        public bool TryFindByName(string key, out Equipment result)
        {
            return TryFindUniqueCore(data, primaryIndexSelector, System.StringComparer.Ordinal, key, out result);
        }

        public Equipment FindClosestByName(string key, bool selectLower = true)
        {
            return FindUniqueClosestCore(data, primaryIndexSelector, System.StringComparer.Ordinal, key, selectLower);
        }

        public RangeView<Equipment> FindRangeByName(string min, string max, bool ascendant = true)
        {
            return FindUniqueRangeCore(data, primaryIndexSelector, System.StringComparer.Ordinal, min, max, ascendant);
        }

        public Equipment FindByIndex(int key)
        {
            return FindUniqueCoreInt(secondaryIndex2, secondaryIndex2Selector, System.Collections.Generic.Comparer<int>.Default, key, false);
        }
        
        public bool TryFindByIndex(int key, out Equipment result)
        {
            return TryFindUniqueCoreInt(secondaryIndex2, secondaryIndex2Selector, System.Collections.Generic.Comparer<int>.Default, key, out result);
        }

        public Equipment FindClosestByIndex(int key, bool selectLower = true)
        {
            return FindUniqueClosestCore(secondaryIndex2, secondaryIndex2Selector, System.Collections.Generic.Comparer<int>.Default, key, selectLower);
        }

        public RangeView<Equipment> FindRangeByIndex(int min, int max, bool ascendant = true)
        {
            return FindUniqueRangeCore(secondaryIndex2, secondaryIndex2Selector, System.Collections.Generic.Comparer<int>.Default, min, max, ascendant);
        }


        void ITableUniqueValidate.ValidateUnique(ValidateResult resultSet)
        {
            ValidateUniqueCore(data, primaryIndexSelector, "Name", resultSet);       
            ValidateUniqueCore(secondaryIndex2, secondaryIndex2Selector, "Index", resultSet);       
        }

        public static MasterMemory.Meta.MetaTable CreateMetaTable()
        {
            return new MasterMemory.Meta.MetaTable(typeof(Equipment), typeof(EquipmentTable), "equipment",
                new MasterMemory.Meta.MetaProperty[]
                {
                    new MasterMemory.Meta.MetaProperty(typeof(Equipment).GetProperty("Id")),
                    new MasterMemory.Meta.MetaProperty(typeof(Equipment).GetProperty("Name")),
                    new MasterMemory.Meta.MetaProperty(typeof(Equipment).GetProperty("Address")),
                    new MasterMemory.Meta.MetaProperty(typeof(Equipment).GetProperty("Description")),
                    new MasterMemory.Meta.MetaProperty(typeof(Equipment).GetProperty("ImageBytes")),
                    new MasterMemory.Meta.MetaProperty(typeof(Equipment).GetProperty("EquipmentInfo")),
                    new MasterMemory.Meta.MetaProperty(typeof(Equipment).GetProperty("Index")),
                },
                new MasterMemory.Meta.MetaIndex[]{
                    new MasterMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(Equipment).GetProperty("Name"),
                    }, true, true, System.StringComparer.Ordinal),
                    new MasterMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(Equipment).GetProperty("Index"),
                    }, false, true, System.Collections.Generic.Comparer<int>.Default),
                });
        }

    }
}