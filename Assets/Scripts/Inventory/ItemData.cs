using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item" , menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;          
    public Sprite itemIcon;
    public int maxStack = 99;           //최대 겹침 개수

    public bool isUsable = false;
    public int healAmount = 0;
}
