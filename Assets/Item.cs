
using System;
using UnityEngine;

public class Item : ScriptableObject
{
    public string itemName;

    [TextArea(3,10)]
    public string itemDescription;
    public Sprite itemIcon;
}