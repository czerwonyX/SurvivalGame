using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Tool", fileName = "Tool")]
public class InventoryTool : InventoryItem
{
    public float damageToTarget;
    public float damageToLiving;
    public Damageable target;
}
