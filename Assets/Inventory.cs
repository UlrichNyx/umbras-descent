using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<Item,int> myItems;
    UIManager uIManager;
    GameManager gameManager;
    void Start()
    {
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
}
