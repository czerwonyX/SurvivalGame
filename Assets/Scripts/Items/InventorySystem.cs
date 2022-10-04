using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; private set; }
    [SerializeField] Transform inventoryUI;
    [SerializeField] Transform hotbarUI;
    [SerializeField] Button inventoryOpenButton;
    [SerializeField] Button inventoryCloseButton;
    [HideInInspector] public InventoryItem[] items;
    static Dictionary<short, InventoryItem> itemsDic = new Dictionary<short, InventoryItem>();

    private void Awake()
    {
        Instance = this;

        items = Resources.LoadAll<InventoryItem>("Items");
        short index = 1;
        foreach (InventoryItem item in items)
        {
            item.SetID(index);
            itemsDic.Add(index, item);
            index++;
        }
        inventoryOpenButton.onClick.AddListener(delegate { inventoryUI.parent.gameObject.SetActive(true); });
        inventoryCloseButton.onClick.AddListener(delegate { inventoryUI.parent.gameObject.SetActive(false); });
    }
    private void Start()
    {
        //Inventory inv = new Inventory(36);
        //inv.AddItem(2);
    }
    public static InventoryItem GetItemFromID(short id)
    {
        return itemsDic[id];
    }
    public static void RefreshHotbar()
    {
        Transform[] itemSlots = new Transform[Instance.hotbarUI.childCount];
        Inventory inv = ((GameObject)PhotonNetwork.LocalPlayer.TagObject).GetComponent<PlayerInventory>().inventory;
        Inventory hotbar = ((GameObject)PhotonNetwork.LocalPlayer.TagObject).GetComponent<PlayerInventory>().hotbar;
        int index = 0;
        foreach(Transform t in Instance.hotbarUI)
        {
            itemSlots[index] = t;
            index++;
        }
        print(itemSlots[0]);
        print(itemSlots[1]);
        for (int i = 0; i < hotbar.content.Length; i++)
        {
            hotbar.content[i] = inv.content[i];
            SetSlotContent(hotbar.content[i],itemSlots[i]);
        }

    }
    public static void RefreshInventory()
    {
        print("refresh");
        Transform[] itemSlots = new Transform[Instance.inventoryUI.childCount];
        Inventory inv =((GameObject) PhotonNetwork.LocalPlayer.TagObject).GetComponent<PlayerInventory>().inventory;
        int index = 0;
        foreach(Transform t in Instance.inventoryUI)
        {
            itemSlots[index] = t;
            index++;
        }
        index = 0;
        foreach (InventoryItem item in inv.content)
        {
            SetSlotContent(item,itemSlots[index]);
            index++;
        }
        RefreshHotbar();
       
    }
    public static void SetSlotContent(InventoryItem item, Transform itemSlot)
    {
        Transform iconTransform = itemSlot.GetChild(2);
        Transform countTransform = itemSlot.GetChild(3);

        if (item == null)
        {
            iconTransform.gameObject.SetActive(false);
            countTransform.gameObject.SetActive(false);
            return;
        }

        iconTransform.GetComponent<Image>().sprite = item.icon;
        iconTransform.gameObject.SetActive(true);

        if (item.stackCount > 1)
        {
            countTransform.GetComponent<TMP_Text>().text = item.stackCount.ToString();
            countTransform.gameObject.SetActive(true);
        }
    }
}
public class Inventory
{
    public int size;
    public InventoryItem[] content;
    public byte AddItem(InventoryItem item, byte amount = 1)
    {
        return AddItem(item.id, amount);
    }
    public byte AddItem(short id, byte amount = 1)
    {
        byte failedToAdd = 0;
        // Find item of same type
        foreach (InventoryItem item in content)
        {
            if (item == null) continue;
            if (item.Equals(InventorySystem.GetItemFromID(id)))
            {
                if (item.stackCount + amount <= item.maxStack)
                {
                    item.stackCount += amount;
                    InventorySystem.RefreshInventory();
                    return 0;
                }
                byte _failedToAdd = (byte)((item.stackCount + amount) - item.maxStack);
                item.stackCount += (byte)(amount - _failedToAdd);

                failedToAdd += _failedToAdd;
            }
        }
        // Find free slot
        byte itemToAdd = failedToAdd == 0 ? amount : failedToAdd;
        int index = 0;
        foreach (InventoryItem item in content)
        {
            if (itemToAdd == 0) { InventorySystem.RefreshInventory(); return 0; }
            if (item == null)
            {
                InventoryItem newItem = ScriptableObject.CreateInstance<InventoryItem>();
                newItem.Copy(InventorySystem.GetItemFromID(id));
                if (newItem.stackCount + itemToAdd <= newItem.maxStack)
                {
                    newItem.stackCount += itemToAdd;
                    content[index] = newItem;
                    InventorySystem.RefreshInventory();
                    return 0;
                }
                itemToAdd -= (byte)(newItem.maxStack - newItem.stackCount);
                newItem.stackCount = newItem.maxStack;
                content[index] = newItem;
            }
            index++;
        }

        InventorySystem.RefreshInventory();
        return itemToAdd;
    }

    public bool RemoveItem(byte slot, byte amount = 1)
    {
        if (content[slot] == null) return false;

        if (content[slot].stackCount <= amount)
        {
            content[slot] = null;
            InventorySystem.RefreshInventory();
            return true;
        }
        content[slot].stackCount -= amount;
        InventorySystem.RefreshInventory();
        return true;
    }
    
    public void SetItem(short id, byte slot, byte amount = 1)
    {
        content[slot] = ScriptableObject.CreateInstance<InventoryItem>();
        content[slot].Copy(InventorySystem.GetItemFromID(id));
        content[slot].stackCount = amount;
        InventorySystem.RefreshInventory();
    }
    public void SetItem(InventoryItem item, byte slot, byte amount = 1)
    {
        SetItem(item.id,slot,amount);
    }
    public Inventory(int size)
    {
        this.size = size;
        content = new InventoryItem[size];
    }
}
