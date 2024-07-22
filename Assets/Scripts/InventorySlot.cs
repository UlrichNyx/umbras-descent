using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI counter;

    public void SetSlot(Item item,int amount)
    {
        icon.sprite = item.itemIcon;
        counter.text = amount + "x";
    }
}
