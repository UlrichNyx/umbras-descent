using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Unity.VisualScripting;

public class RecipeSlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI Name;
    CraftingUI craftingUI;
    Recipe myRecipe;
    public void SetSlot(Recipe recipe)
    {
        myRecipe = recipe;
        icon.sprite = recipe.Result.itemIcon;
        Name.text = recipe.Name;
    }

    public void ButtonClick()
    {
        if(craftingUI == null) craftingUI = GetComponentInParent<CraftingUI>();

        craftingUI.SetCraftingRecipe(myRecipe);
    }
}
