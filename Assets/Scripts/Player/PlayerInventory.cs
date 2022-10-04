using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Inventory inventory;
    public Inventory hotbar;
    private void Start()
    {
        inventory = new Inventory(36);
        hotbar = new Inventory(6);
        //inventory.SetItem(2, 1, 6);
        //inventory.AddItem(2, 12);
        //inventory.AddItem(1, 4);
        //inventory.AddItem(2, 5);

        //inventory.AddItem(3, 2);
    }
}
