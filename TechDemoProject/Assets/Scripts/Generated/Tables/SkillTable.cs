// <auto-generated />
#pragma warning disable CS0105
using MasterMemory.Validation;
using MasterMemory;
using MessagePack;
using System.Collections.Generic;
using System;
using Tech.DB;
using UnityEngine;

namespace MasterData.Tables
{
   public sealed partial class SkillTable : TableBase<Skill>, ITableUniqueValidate
   {
        public Func<Skill, int> PrimaryKeySelector => primaryIndexSelector;
        readonly Func<Skill, int> primaryIndexSelector;

        readonly Skill[] secondaryIndex0;
        readonly Func<Skill, string> secondaryIndex0Selector;

        public SkillTable(Skill[] sortedData)
            : base(sortedData)
        {
            this.primaryIndexSelector = x => x.index;
            this.secondaryIndex0Selector = x => x.name;
            this.secondaryIndex0 = CloneAndSortBy(this.secondaryIndex0Selector, System.StringComparer.Ordinal);
            OnAfterConstruct();
        }

        partial void OnAfterConstruct();

        public RangeView<Skill> SortByname => new RangeView<Skill>(secondaryIndex0, 0, secondaryIndex0.Length - 1, true);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Skill FindByindex(int key)
        {
            var lo = 0;
            var hi = data.Length - 1;
            while (lo <= hi)
            {
                var mid = (int)(((uint)hi + (uint)lo) >> 1);
                var selected = data[mid].index;
                var found = (selected < key) ? -1 : (selected > key) ? 1 : 0;
                if (found == 0) { return data[mid]; }
                if (found < 0) { lo = mid + 1; }
                else { hi = mid - 1; }
            }
            return default;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool TryFindByindex(int key, out Skill result)
        {
            var lo = 0;
            var hi = data.Length - 1;
            while (lo <= hi)
            {
                var mid = (int)(((uint)hi + (uint)lo) >> 1);
                var selected = data[mid].index;
                var found = (selected < key) ? -1 : (selected > key) ? 1 : 0;
                if (found == 0) { result = data[mid]; return true; }
                if (found < 0) { lo = mid + 1; }
                else { hi = mid - 1; }
            }
            result = default;
            return false;
        }

        public Skill FindClosestByindex(int key, bool selectLower = true)
        {
            return FindUniqueClosestCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<int>.Default, key, selectLower);
        }

        public RangeView<Skill> FindRangeByindex(int min, int max, bool ascendant = true)
        {
            return FindUniqueRangeCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<int>.Default, min, max, ascendant);
        }

        public Skill FindByname(string key)
        {
            return FindUniqueCore(secondaryIndex0, secondaryIndex0Selector, System.StringComparer.Ordinal, key, false);
        }
        
        public bool TryFindByname(string key, out Skill result)
        {
            return TryFindUniqueCore(secondaryIndex0, secondaryIndex0Selector, System.StringComparer.Ordinal, key, out result);
        }

        public Skill FindClosestByname(string key, bool selectLower = true)
        {
            return FindUniqueClosestCore(secondaryIndex0, secondaryIndex0Selector, System.StringComparer.Ordinal, key, selectLower);
        }

        public RangeView<Skill> FindRangeByname(string min, string max, bool ascendant = true)
        {
            return FindUniqueRangeCore(secondaryIndex0, secondaryIndex0Selector, System.StringComparer.Ordinal, min, max, ascendant);
        }


        void ITableUniqueValidate.ValidateUnique(ValidateResult resultSet)
        {
            ValidateUniqueCore(data, primaryIndexSelector, "index", resultSet);       
            ValidateUniqueCore(secondaryIndex0, secondaryIndex0Selector, "name", resultSet);       
        }

        public static MasterMemory.Meta.MetaTable CreateMetaTable()
        {
            return new MasterMemory.Meta.MetaTable(typeof(Skill), typeof(SkillTable), "Image",
                new MasterMemory.Meta.MetaProperty[]
                {
                    new MasterMemory.Meta.MetaProperty(typeof(Skill).GetProperty("index")),
                    new MasterMemory.Meta.MetaProperty(typeof(Skill).GetProperty("name")),
                },
                new MasterMemory.Meta.MetaIndex[]{
                    new MasterMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(Skill).GetProperty("index"),
                    }, true, true, System.Collections.Generic.Comparer<int>.Default),
                    new MasterMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(Skill).GetProperty("name"),
                    }, false, true, System.StringComparer.Ordinal),
                });
        }

    }
}