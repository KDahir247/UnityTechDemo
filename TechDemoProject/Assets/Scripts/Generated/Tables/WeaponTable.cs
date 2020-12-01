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
   public sealed partial class WeaponTable : TableBase<Weapon>, ITableUniqueValidate
   {
        public Func<Weapon, string> PrimaryKeySelector => primaryIndexSelector;
        readonly Func<Weapon, string> primaryIndexSelector;

        readonly Weapon[] secondaryIndex0;
        readonly Func<Weapon, uint> secondaryIndex0Selector;
        readonly Weapon[] secondaryIndex1;
        readonly Func<Weapon, WeaponType> secondaryIndex1Selector;
        readonly Weapon[] secondaryIndex2;
        readonly Func<Weapon, int> secondaryIndex2Selector;

        public WeaponTable(Weapon[] sortedData)
            : base(sortedData)
        {
            this.primaryIndexSelector = x => x.Name;
            this.secondaryIndex0Selector = x => x.Level;
            this.secondaryIndex0 = CloneAndSortBy(this.secondaryIndex0Selector, System.Collections.Generic.Comparer<uint>.Default);
            this.secondaryIndex1Selector = x => x.WeaponType;
            this.secondaryIndex1 = CloneAndSortBy(this.secondaryIndex1Selector, System.Collections.Generic.Comparer<WeaponType>.Default);
            this.secondaryIndex2Selector = x => x.Index;
            this.secondaryIndex2 = CloneAndSortBy(this.secondaryIndex2Selector, System.Collections.Generic.Comparer<int>.Default);
            OnAfterConstruct();
        }

        partial void OnAfterConstruct();

        public RangeView<Weapon> SortByLevel => new RangeView<Weapon>(secondaryIndex0, 0, secondaryIndex0.Length - 1, true);
        public RangeView<Weapon> SortByWeaponType => new RangeView<Weapon>(secondaryIndex1, 0, secondaryIndex1.Length - 1, true);
        public RangeView<Weapon> SortByIndex => new RangeView<Weapon>(secondaryIndex2, 0, secondaryIndex2.Length - 1, true);

        public Weapon FindByName(string key)
        {
            return FindUniqueCore(data, primaryIndexSelector, System.StringComparer.Ordinal, key, false);
        }
        
        public bool TryFindByName(string key, out Weapon result)
        {
            return TryFindUniqueCore(data, primaryIndexSelector, System.StringComparer.Ordinal, key, out result);
        }

        public Weapon FindClosestByName(string key, bool selectLower = true)
        {
            return FindUniqueClosestCore(data, primaryIndexSelector, System.StringComparer.Ordinal, key, selectLower);
        }

        public RangeView<Weapon> FindRangeByName(string min, string max, bool ascendant = true)
        {
            return FindUniqueRangeCore(data, primaryIndexSelector, System.StringComparer.Ordinal, min, max, ascendant);
        }

        public Weapon FindByLevel(uint key)
        {
            return FindUniqueCore(secondaryIndex0, secondaryIndex0Selector, System.Collections.Generic.Comparer<uint>.Default, key, false);
        }
        
        public bool TryFindByLevel(uint key, out Weapon result)
        {
            return TryFindUniqueCore(secondaryIndex0, secondaryIndex0Selector, System.Collections.Generic.Comparer<uint>.Default, key, out result);
        }

        public Weapon FindClosestByLevel(uint key, bool selectLower = true)
        {
            return FindUniqueClosestCore(secondaryIndex0, secondaryIndex0Selector, System.Collections.Generic.Comparer<uint>.Default, key, selectLower);
        }

        public RangeView<Weapon> FindRangeByLevel(uint min, uint max, bool ascendant = true)
        {
            return FindUniqueRangeCore(secondaryIndex0, secondaryIndex0Selector, System.Collections.Generic.Comparer<uint>.Default, min, max, ascendant);
        }

        public Weapon FindByWeaponType(WeaponType key)
        {
            return FindUniqueCore(secondaryIndex1, secondaryIndex1Selector, System.Collections.Generic.Comparer<WeaponType>.Default, key, false);
        }
        
        public bool TryFindByWeaponType(WeaponType key, out Weapon result)
        {
            return TryFindUniqueCore(secondaryIndex1, secondaryIndex1Selector, System.Collections.Generic.Comparer<WeaponType>.Default, key, out result);
        }

        public Weapon FindClosestByWeaponType(WeaponType key, bool selectLower = true)
        {
            return FindUniqueClosestCore(secondaryIndex1, secondaryIndex1Selector, System.Collections.Generic.Comparer<WeaponType>.Default, key, selectLower);
        }

        public RangeView<Weapon> FindRangeByWeaponType(WeaponType min, WeaponType max, bool ascendant = true)
        {
            return FindUniqueRangeCore(secondaryIndex1, secondaryIndex1Selector, System.Collections.Generic.Comparer<WeaponType>.Default, min, max, ascendant);
        }

        public Weapon FindByIndex(int key)
        {
            return FindUniqueCoreInt(secondaryIndex2, secondaryIndex2Selector, System.Collections.Generic.Comparer<int>.Default, key, false);
        }
        
        public bool TryFindByIndex(int key, out Weapon result)
        {
            return TryFindUniqueCoreInt(secondaryIndex2, secondaryIndex2Selector, System.Collections.Generic.Comparer<int>.Default, key, out result);
        }

        public Weapon FindClosestByIndex(int key, bool selectLower = true)
        {
            return FindUniqueClosestCore(secondaryIndex2, secondaryIndex2Selector, System.Collections.Generic.Comparer<int>.Default, key, selectLower);
        }

        public RangeView<Weapon> FindRangeByIndex(int min, int max, bool ascendant = true)
        {
            return FindUniqueRangeCore(secondaryIndex2, secondaryIndex2Selector, System.Collections.Generic.Comparer<int>.Default, min, max, ascendant);
        }


        void ITableUniqueValidate.ValidateUnique(ValidateResult resultSet)
        {
            ValidateUniqueCore(data, primaryIndexSelector, "Name", resultSet);       
            ValidateUniqueCore(secondaryIndex0, secondaryIndex0Selector, "Level", resultSet);       
            ValidateUniqueCore(secondaryIndex1, secondaryIndex1Selector, "WeaponType", resultSet);       
            ValidateUniqueCore(secondaryIndex2, secondaryIndex2Selector, "Index", resultSet);       
        }

        public static MasterMemory.Meta.MetaTable CreateMetaTable()
        {
            return new MasterMemory.Meta.MetaTable(typeof(Weapon), typeof(WeaponTable), "weapon",
                new MasterMemory.Meta.MetaProperty[]
                {
                    new MasterMemory.Meta.MetaProperty(typeof(Weapon).GetProperty("Name")),
                    new MasterMemory.Meta.MetaProperty(typeof(Weapon).GetProperty("Stat")),
                    new MasterMemory.Meta.MetaProperty(typeof(Weapon).GetProperty("Description")),
                    new MasterMemory.Meta.MetaProperty(typeof(Weapon).GetProperty("ImageBytes")),
                    new MasterMemory.Meta.MetaProperty(typeof(Weapon).GetProperty("Level")),
                    new MasterMemory.Meta.MetaProperty(typeof(Weapon).GetProperty("WeaponType")),
                    new MasterMemory.Meta.MetaProperty(typeof(Weapon).GetProperty("Index")),
                    new MasterMemory.Meta.MetaProperty(typeof(Weapon).GetProperty("Rarity")),
                },
                new MasterMemory.Meta.MetaIndex[]{
                    new MasterMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(Weapon).GetProperty("Name"),
                    }, true, true, System.StringComparer.Ordinal),
                    new MasterMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(Weapon).GetProperty("Level"),
                    }, false, true, System.Collections.Generic.Comparer<uint>.Default),
                    new MasterMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(Weapon).GetProperty("WeaponType"),
                    }, false, true, System.Collections.Generic.Comparer<WeaponType>.Default),
                    new MasterMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(Weapon).GetProperty("Index"),
                    }, false, true, System.Collections.Generic.Comparer<int>.Default),
                });
        }

    }
}