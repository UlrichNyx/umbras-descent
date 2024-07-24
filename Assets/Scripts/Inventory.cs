using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<Item,int> myItems;
    public List<Recipe> myRecipes;
    UIManager uIManager;
    GameManager gameManager;
    void Start()
    {
        myRecipes = new List<Recipe>();
        gameManager = GameManager.instance;
        uIManager = UIManager.instance;
        myItems = new Dictionary<Item, int>();
        foreach(Item i in gameManager.AllItems)
        {
            myItems.Add(i,0);
        }

        uIManager.UpdateInventory(myItems);
    }

    public void ModifyItem(Item item, int amount)
    {
        if(item == null) return;
        
        if(!myItems.ContainsKey(item)) 
        {
            Debug.Log("Item doesn't exist in dictionary, put in inspector please");
            return;
        }
        
        myItems[item] += amount;
        if(myItems[item] < 0)
        {
            myItems[item] = 0;
        }

        uIManager.UpdateInventory(myItems);
    }

    public void ReceiveRecipe(Recipe recipe)
    {
        if(recipe != null) myRecipes.Add(recipe);
    }
}
