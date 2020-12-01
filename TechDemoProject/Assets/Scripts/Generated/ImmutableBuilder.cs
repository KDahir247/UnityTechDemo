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

        public void ReplaceAll(System.Collections.Generic.IList<Character> data)
        {
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new CharacterTable(newData);
            memory = new MemoryDatabase(
                table,
                memory.EquipmentTable,
                memory.ItemTable,
                memory.MaterialTable,
                memory.SkillTable,
                memory.WeaponTable
            
            );
        }

        public void RemoveCharacter(string[] keys)
        {
            var data = RemoveCore(memory.CharacterTable.GetRawDataUnsafe(), keys, x => x.Name, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new CharacterTable(newData);
            memory = new MemoryDatabase(
                table,
                memory.EquipmentTable,
                memory.ItemTable,
                memory.MaterialTable,
                memory.SkillTable,
                memory.WeaponTable
            
            );
        }

        public void Diff(Character[] addOrReplaceData)
        {
            var data = DiffCore(memory.CharacterTable.GetRawDataUnsafe(), addOrReplaceData, x => x.Name, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new CharacterTable(newData);
            memory = new MemoryDatabase(
                table,
                memory.EquipmentTable,
                memory.ItemTable,
                memory.MaterialTable,
                memory.SkillTable,
                memory.WeaponTable
            
            );
        }

        public void ReplaceAll(System.Collections.Generic.IList<Equipment> data)
        {
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new EquipmentTable(newData);
            memory = new MemoryDatabase(
                memory.CharacterTable,
                table,
                memory.ItemTable,
                memory.MaterialTable,
                memory.SkillTable,
                memory.WeaponTable
            
            );
        }

        public void RemoveEquipment(string[] keys)
        {
            var data = RemoveCore(memory.EquipmentTable.GetRawDataUnsafe(), keys, x => x.Name, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new EquipmentTable(newData);
            memory = new MemoryDatabase(
                memory.CharacterTable,
                table,
                memory.ItemTable,
                memory.MaterialTable,
                memory.SkillTable,
                memory.WeaponTable
            
            );
        }

        public void Diff(Equipment[] addOrReplaceData)
        {
            var data = DiffCore(memory.EquipmentTable.GetRawDataUnsafe(), addOrReplaceData, x => x.Name, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new EquipmentTable(newData);
            memory = new MemoryDatabase(
                memory.CharacterTable,
                table,
                memory.ItemTable,
                memory.MaterialTable,
                memory.SkillTable,
                memory.WeaponTable
            
            );
        }

        public void ReplaceAll(System.Collections.Generic.IList<Item> data)
        {
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new ItemTable(newData);
            memory = new MemoryDatabase(
                memory.CharacterTable,
                memory.EquipmentTable,
                table,
                memory.MaterialTable,
                memory.SkillTable,
                memory.WeaponTable
            
            );
        }

        public void RemoveItem(string[] keys)
        {
            var data = RemoveCore(memory.ItemTable.GetRawDataUnsafe(), keys, x => x.Name, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new ItemTable(newData);
            memory = new MemoryDatabase(
                memory.CharacterTable,
                memory.EquipmentTable,
                table,
                memory.MaterialTable,
                memory.SkillTable,
                memory.WeaponTable
            
            );
        }

        public void Diff(Item[] addOrReplaceData)
        {
            var data = DiffCore(memory.ItemTable.GetRawDataUnsafe(), addOrReplaceData, x => x.Name, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new ItemTable(newData);
            memory = new MemoryDatabase(
                memory.CharacterTable,
                memory.EquipmentTable,
                table,
                memory.MaterialTable,
                memory.SkillTable,
                memory.WeaponTable
            
            );
        }

        public void ReplaceAll(System.Collections.Generic.IList<Material> data)
        {
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new MaterialTable(newData);
            memory = new MemoryDatabase(
                memory.CharacterTable,
                memory.EquipmentTable,
                memory.ItemTable,
                table,
                memory.SkillTable,
                memory.WeaponTable
            
            );
        }

        public void RemoveMaterial(string[] keys)
        {
            var data = RemoveCore(memory.MaterialTable.GetRawDataUnsafe(), keys, x => x.Name, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new MaterialTable(newData);
            memory = new MemoryDatabase(
                memory.CharacterTable,
                memory.EquipmentTable,
                memory.ItemTable,
                table,
                memory.SkillTable,
                memory.WeaponTable
            
            );
        }

        public void Diff(Material[] addOrReplaceData)
        {
            var data = DiffCore(memory.MaterialTable.GetRawDataUnsafe(), addOrReplaceData, x => x.Name, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new MaterialTable(newData);
            memory = new MemoryDatabase(
                memory.CharacterTable,
                memory.EquipmentTable,
                memory.ItemTable,
                table,
                memory.SkillTable,
                memory.WeaponTable
            
            );
        }

        public void ReplaceAll(System.Collections.Generic.IList<Skill> data)
        {
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new SkillTable(newData);
            memory = new MemoryDatabase(
                memory.CharacterTable,
                memory.EquipmentTable,
                memory.ItemTable,
                memory.MaterialTable,
                table,
                memory.WeaponTable
            
            );
        }

        public void RemoveSkill(string[] keys)
        {
            var data = RemoveCore(memory.SkillTable.GetRawDataUnsafe(), keys, x => x.Name, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new SkillTable(newData);
            memory = new MemoryDatabase(
                memory.CharacterTable,
                memory.EquipmentTable,
                memory.ItemTable,
                memory.MaterialTable,
                table,
                memory.WeaponTable
            
            );
        }

        public void Diff(Skill[] addOrReplaceData)
        {
            var data = DiffCore(memory.SkillTable.GetRawDataUnsafe(), addOrReplaceData, x => x.Name, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new SkillTable(newData);
            memory = new MemoryDatabase(
                memory.CharacterTable,
                memory.EquipmentTable,
                memory.ItemTable,
                memory.MaterialTable,
                table,
                memory.WeaponTable
            
            );
        }

        public void ReplaceAll(System.Collections.Generic.IList<Weapon> data)
        {
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new WeaponTable(newData);
            memory = new MemoryDatabase(
                memory.CharacterTable,
                memory.EquipmentTable,
                memory.ItemTable,
                memory.MaterialTable,
                memory.SkillTable,
                table
            
            );
        }

        public void RemoveWeapon(string[] keys)
        {
            var data = RemoveCore(memory.WeaponTable.GetRawDataUnsafe(), keys, x => x.Name, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new WeaponTable(newData);
            memory = new MemoryDatabase(
                memory.CharacterTable,
                memory.EquipmentTable,
                memory.ItemTable,
                memory.MaterialTable,
                memory.SkillTable,
                table
            
            );
        }

        public void Diff(Weapon[] addOrReplaceData)
        {
            var data = DiffCore(memory.WeaponTable.GetRawDataUnsafe(), addOrReplaceData, x => x.Name, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.Name, System.StringComparer.Ordinal);
            var table = new WeaponTable(newData);
            memory = new MemoryDatabase(
                memory.CharacterTable,
                memory.EquipmentTable,
                memory.ItemTable,
                memory.MaterialTable,
                memory.SkillTable,
                table
            
            );
        }

    }
}