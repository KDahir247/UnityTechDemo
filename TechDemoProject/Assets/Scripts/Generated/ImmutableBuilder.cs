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
   public sealed class ImmutableBuilder : ImmutableBuilderBase
   {
        MemoryDatabase memory;

        public ImmutableBuilder(MemoryDatabase memory)
        {
            this.memory = memory;
        }

        public MemoryDatabase Build()
        {
            return memory;
        }

        public void ReplaceAll(System.Collections.Generic.IList<Ability> data)
        {
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new AbilityTable(newData);
            memory = new MemoryDatabase(
                table,
                memory.EnemyTable,
                memory.EquipmentTable,
                memory.ItemTable,
                memory.SkillTable,
                memory.TechMaterialTable,
                memory.UnitTable,
                memory.WeaponTable
            
            );
        }

        public void RemoveAbility(string[] keys)
        {
            var data = RemoveCore(memory.AbilityTable.GetRawDataUnsafe(), keys, x => x.Name, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new AbilityTable(newData);
            memory = new MemoryDatabase(
                table,
                memory.EnemyTable,
                memory.EquipmentTable,
                memory.ItemTable,
                memory.SkillTable,
                memory.TechMaterialTable,
                memory.UnitTable,
                memory.WeaponTable
            
            );
        }

        public void Diff(Ability[] addOrReplaceData)
        {
            var data = DiffCore(memory.AbilityTable.GetRawDataUnsafe(), addOrReplaceData, x => x.Name, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new AbilityTable(newData);
            memory = new MemoryDatabase(
                table,
                memory.EnemyTable,
                memory.EquipmentTable,
                memory.ItemTable,
                memory.SkillTable,
                memory.TechMaterialTable,
                memory.UnitTable,
                memory.WeaponTable
            
            );
        }

        public void ReplaceAll(System.Collections.Generic.IList<Enemy> data)
        {
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new EnemyTable(newData);
            memory = new MemoryDatabase(
                memory.AbilityTable,
                table,
                memory.EquipmentTable,
                memory.ItemTable,
                memory.SkillTable,
                memory.TechMaterialTable,
                memory.UnitTable,
                memory.WeaponTable
            
            );
        }

        public void RemoveEnemy(string[] keys)
        {
            var data = RemoveCore(memory.EnemyTable.GetRawDataUnsafe(), keys, x => x.Name, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new EnemyTable(newData);
            memory = new MemoryDatabase(
                memory.AbilityTable,
                table,
                memory.EquipmentTable,
                memory.ItemTable,
                memory.SkillTable,
                memory.TechMaterialTable,
                memory.UnitTable,
                memory.WeaponTable
            
            );
        }

        public void Diff(Enemy[] addOrReplaceData)
        {
            var data = DiffCore(memory.EnemyTable.GetRawDataUnsafe(), addOrReplaceData, x => x.Name, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new EnemyTable(newData);
            memory = new MemoryDatabase(
                memory.AbilityTable,
                table,
                memory.EquipmentTable,
                memory.ItemTable,
                memory.SkillTable,
                memory.TechMaterialTable,
                memory.UnitTable,
                memory.WeaponTable
            
            );
        }

        public void ReplaceAll(System.Collections.Generic.IList<Equipment> data)
        {
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new EquipmentTable(newData);
            memory = new MemoryDatabase(
                memory.AbilityTable,
                memory.EnemyTable,
                table,
                memory.ItemTable,
                memory.SkillTable,
                memory.TechMaterialTable,
                memory.UnitTable,
                memory.WeaponTable
            
            );
        }

        public void RemoveEquipment(string[] keys)
        {
            var data = RemoveCore(memory.EquipmentTable.GetRawDataUnsafe(), keys, x => x.Name, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new EquipmentTable(newData);
            memory = new MemoryDatabase(
                memory.AbilityTable,
                memory.EnemyTable,
                table,
                memory.ItemTable,
                memory.SkillTable,
                memory.TechMaterialTable,
                memory.UnitTable,
                memory.WeaponTable
            
            );
        }

        public void Diff(Equipment[] addOrReplaceData)
        {
            var data = DiffCore(memory.EquipmentTable.GetRawDataUnsafe(), addOrReplaceData, x => x.Name, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new EquipmentTable(newData);
            memory = new MemoryDatabase(
                memory.AbilityTable,
                memory.EnemyTable,
                table,
                memory.ItemTable,
                memory.SkillTable,
                memory.TechMaterialTable,
                memory.UnitTable,
                memory.WeaponTable
            
            );
        }

        public void ReplaceAll(System.Collections.Generic.IList<Item> data)
        {
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new ItemTable(newData);
            memory = new MemoryDatabase(
                memory.AbilityTable,
                memory.EnemyTable,
                memory.EquipmentTable,
                table,
                memory.SkillTable,
                memory.TechMaterialTable,
                memory.UnitTable,
                memory.WeaponTable
            
            );
        }

        public void RemoveItem(string[] keys)
        {
            var data = RemoveCore(memory.ItemTable.GetRawDataUnsafe(), keys, x => x.Name, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new ItemTable(newData);
            memory = new MemoryDatabase(
                memory.AbilityTable,
                memory.EnemyTable,
                memory.EquipmentTable,
                table,
                memory.SkillTable,
                memory.TechMaterialTable,
                memory.UnitTable,
                memory.WeaponTable
            
            );
        }

        public void Diff(Item[] addOrReplaceData)
        {
            var data = DiffCore(memory.ItemTable.GetRawDataUnsafe(), addOrReplaceData, x => x.Name, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new ItemTable(newData);
            memory = new MemoryDatabase(
                memory.AbilityTable,
                memory.EnemyTable,
                memory.EquipmentTable,
                table,
                memory.SkillTable,
                memory.TechMaterialTable,
                memory.UnitTable,
                memory.WeaponTable
            
            );
        }

        public void ReplaceAll(System.Collections.Generic.IList<Skill> data)
        {
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new SkillTable(newData);
            memory = new MemoryDatabase(
                memory.AbilityTable,
                memory.EnemyTable,
                memory.EquipmentTable,
                memory.ItemTable,
                table,
                memory.TechMaterialTable,
                memory.UnitTable,
                memory.WeaponTable
            
            );
        }

        public void RemoveSkill(string[] keys)
        {
            var data = RemoveCore(memory.SkillTable.GetRawDataUnsafe(), keys, x => x.Name, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new SkillTable(newData);
            memory = new MemoryDatabase(
                memory.AbilityTable,
                memory.EnemyTable,
                memory.EquipmentTable,
                memory.ItemTable,
                table,
                memory.TechMaterialTable,
                memory.UnitTable,
                memory.WeaponTable
            
            );
        }

        public void Diff(Skill[] addOrReplaceData)
        {
            var data = DiffCore(memory.SkillTable.GetRawDataUnsafe(), addOrReplaceData, x => x.Name, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new SkillTable(newData);
            memory = new MemoryDatabase(
                memory.AbilityTable,
                memory.EnemyTable,
                memory.EquipmentTable,
                memory.ItemTable,
                table,
                memory.TechMaterialTable,
                memory.UnitTable,
                memory.WeaponTable
            
            );
        }

        public void ReplaceAll(System.Collections.Generic.IList<TechMaterial> data)
        {
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new TechMaterialTable(newData);
            memory = new MemoryDatabase(
                memory.AbilityTable,
                memory.EnemyTable,
                memory.EquipmentTable,
                memory.ItemTable,
                memory.SkillTable,
                table,
                memory.UnitTable,
                memory.WeaponTable
            
            );
        }

        public void RemoveTechMaterial(string[] keys)
        {
            var data = RemoveCore(memory.TechMaterialTable.GetRawDataUnsafe(), keys, x => x.Name, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new TechMaterialTable(newData);
            memory = new MemoryDatabase(
                memory.AbilityTable,
                memory.EnemyTable,
                memory.EquipmentTable,
                memory.ItemTable,
                memory.SkillTable,
                table,
                memory.UnitTable,
                memory.WeaponTable
            
            );
        }

        public void Diff(TechMaterial[] addOrReplaceData)
        {
            var data = DiffCore(memory.TechMaterialTable.GetRawDataUnsafe(), addOrReplaceData, x => x.Name, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new TechMaterialTable(newData);
            memory = new MemoryDatabase(
                memory.AbilityTable,
                memory.EnemyTable,
                memory.EquipmentTable,
                memory.ItemTable,
                memory.SkillTable,
                table,
                memory.UnitTable,
                memory.WeaponTable
            
            );
        }

        public void ReplaceAll(System.Collections.Generic.IList<Unit> data)
        {
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new UnitTable(newData);
            memory = new MemoryDatabase(
                memory.AbilityTable,
                memory.EnemyTable,
                memory.EquipmentTable,
                memory.ItemTable,
                memory.SkillTable,
                memory.TechMaterialTable,
                table,
                memory.WeaponTable
            
            );
        }

        public void RemoveUnit(string[] keys)
        {
            var data = RemoveCore(memory.UnitTable.GetRawDataUnsafe(), keys, x => x.Name, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new UnitTable(newData);
            memory = new MemoryDatabase(
                memory.AbilityTable,
                memory.EnemyTable,
                memory.EquipmentTable,
                memory.ItemTable,
                memory.SkillTable,
                memory.TechMaterialTable,
                table,
                memory.WeaponTable
            
            );
        }

        public void Diff(Unit[] addOrReplaceData)
        {
            var data = DiffCore(memory.UnitTable.GetRawDataUnsafe(), addOrReplaceData, x => x.Name, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new UnitTable(newData);
            memory = new MemoryDatabase(
                memory.AbilityTable,
                memory.EnemyTable,
                memory.EquipmentTable,
                memory.ItemTable,
                memory.SkillTable,
                memory.TechMaterialTable,
                table,
                memory.WeaponTable
            
            );
        }

        public void ReplaceAll(System.Collections.Generic.IList<Weapon> data)
        {
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new WeaponTable(newData);
            memory = new MemoryDatabase(
                memory.AbilityTable,
                memory.EnemyTable,
                memory.EquipmentTable,
                memory.ItemTable,
                memory.SkillTable,
                memory.TechMaterialTable,
                memory.UnitTable,
                table
            
            );
        }

        public void RemoveWeapon(string[] keys)
        {
            var data = RemoveCore(memory.WeaponTable.GetRawDataUnsafe(), keys, x => x.Name, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new WeaponTable(newData);
            memory = new MemoryDatabase(
                memory.AbilityTable,
                memory.EnemyTable,
                memory.EquipmentTable,
                memory.ItemTable,
                memory.SkillTable,
                memory.TechMaterialTable,
                memory.UnitTable,
                table
            
            );
        }

        public void Diff(Weapon[] addOrReplaceData)
        {
            var data = DiffCore(memory.WeaponTable.GetRawDataUnsafe(), addOrReplaceData, x => x.Name, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new WeaponTable(newData);
            memory = new MemoryDatabase(
                memory.AbilityTable,
                memory.EnemyTable,
                memory.EquipmentTable,
                memory.ItemTable,
                memory.SkillTable,
                memory.TechMaterialTable,
                memory.UnitTable,
                table
            
            );
        }

    }
}