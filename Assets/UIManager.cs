using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Slider shadowEssenceSlider;
    public GameObject craftingUI;

    public void SetShadowEssenceSlider(int currentShadowEssence)
    { 
        shadowEssenceSlider.value = ((float) currentShadowEssence / (float) Stats.maxShadowEssence);
    }

    public void ToggleCraftingUI()
    {
        craftingUI.SetActive(!craftingUI.activeSelf);
        GameManager.instance.ToggleMovement(!craftingUI.activeSelf);
    }

    public void SetSelectedPotion(Potion potion)
    {
        Debug.Log("SELECTED THE POTION");
        Debug.Log(potion.item.itemName);
    }
}
