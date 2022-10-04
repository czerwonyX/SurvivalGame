using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item", fileName = "Item")]
public class InventoryItem : ScriptableObject
{
    public short id { get; private set; }
    [SerializeField] string _name;
    [SerializeField] string _description;
    [SerializeField] byte _maxStack;
    [SerializeField] short _maxDurability;
    [SerializeField] ItemType _itemType;
    [SerializeField] Sprite _icon;
    public string name => _name;
    public string description => _description;
    public byte maxStack => _maxStack;
    public short maxDurability => _maxDurability;
    public ItemType itemType => _itemType;
    public Sprite icon => _icon;
    public short durability;
    /*[HideInInspector]*/ public byte stackCount;
    public bool SetID(short id)
    {
        if (this.id == 0)
        {
            this.id = id;
            return true;
        }
        return false;
    }
    public bool Equals(InventoryItem other)
    {
        if (id == other.id) return true;
        return false;
    }
    public bool Copy(InventoryItem other)
    {
        id = other.id;
        _name = other.name;
        _description = other.description;
        _maxStack = other.maxStack;
        _maxDurability = other.maxDurability;
        _itemType = other.itemType;
        _icon = other.icon;
        return true;
    }
}
public enum Damageable
{
    Player, Tree, Rock
}
public enum ItemType
{
    Item, Tool
}
