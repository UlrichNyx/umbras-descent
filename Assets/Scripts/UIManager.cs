using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Slider shadowEssenceSlider;
    public GameObject craftingUI;
    public GameObject InventoryLayout;
    public GameObject InvetorySlotPrefab;
    public TextMeshProUGUI seTextValue;
    List<InventorySlot> InventorySlots;

    void Awake()
    {
        InventorySlots = new List<InventorySlot>();
    }
    public void SetShadowEssenceSlider(float currentShadowEssence)
    { 
        shadowEssenceSlider.value = currentShadowEssence / Stats.maxShadowEssence;
        seTextValue.text = currentShadowEssence.ToString();

    }

    public void ToggleCraftingUI()
    {
        craftingUI.SetActive(!craftingUI.activeSelf);
        GameManager.instance.ToggleMovement(!craftingUI.activeSelf);
    }

    public void SetSelectedPotion(Potion potion)
    {
        Debug.Log("SELECTED THE POTION");
        Debug.Log(potion.itemName);
    }

    public void UpdateInventory(Dictionary<Item,int> items)
    {
        int counter = 0;
        foreach(KeyValuePair<Item,int> i in items)
        {
            if(counter < InventorySlots.Count)
            {
                InventorySlots[counter].SetSlot(i.Key,i.Value);
            }
            else
            {
                InventorySlots.Add(Instantiate(InvetorySlotPrefab,InventoryLayout.transform).GetComponent<InventorySlot>());
                InventorySlots[counter].SetSlot(i.Key,i.Value);
            }
            counter++;
        }
    }
}
