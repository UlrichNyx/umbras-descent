using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CraftingUI : MonoBehaviour
{
    public GameObject RecipesParent;
    public GameObject RecipePrefab;
    public InventorySlot[] Ingredients;
    public TextMeshProUGUI ShadowEssenceRequired;
    public TextMeshProUGUI RecipeDescription;
    GameManager gameManager;
    public List<RecipeSlot> recipeSlots;
    void OnEnable()
    {
        gameManager = GameManager.instance;
        SetRecipes();
        foreach(InventorySlot i in Ingredients)
        {
            i.gameObject.SetActive(false);
        }
        ShadowEssenceRequired.gameObject.SetActive(false);
        RecipeDescription.gameObject.SetActive(false);
    }

    public void SetRecipes()
    {
        if(recipeSlots == null) recipeSlots = new List<RecipeSlot>();

        Recipe[] recipes = gameManager.player.inventory.myRecipes.ToArray();
        for(int i = 0; i < recipes.Length; i++)
        {
            if(i >= recipeSlots.Count)
            {
                RecipeSlot tempSlot = Instantiate(RecipePrefab,RecipesParent.transform).GetComponent<RecipeSlot>();
                tempSlot.SetSlot(recipes[i]);
                recipeSlots.Add(tempSlot);
            }
            else
            {
                recipeSlots[i].SetSlot(recipes[i]);
            }

        }
    }

    public void SetCraftingRecipe(Recipe recipe)
    {
        foreach(InventorySlot i in Ingredients)
        {
            i.gameObject.SetActive(false);
        }

        for(int i = 0; i < Mathf.Min(recipe.requirements.Length,Ingredients.Length,recipe.amounts.Length); i++)
        {
            Ingredients[i].gameObject.SetActive(true);
            Ingredients[i].SetSlot(recipe.requirements[i],recipe.amounts[i]);
        }
        ShadowEssenceRequired.gameObject.SetActive(true);
        RecipeDescription.gameObject.SetActive(true);
        RecipeDescription.text = recipe.description;
        ShadowEssenceRequired.text = recipe.ShadowEssence.ToString();
    }

    public void CraftRecipe()
    {
        Debug.Log("CRAFTING RECIPE");
    }
}
