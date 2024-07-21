
using UnityEngine;


[System.Serializable]
public class Item
{
    public string itemName;

    [TextArea(3,10)]
    public string itemDescription;
    public Sprite itemIcon;
}