using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerController player;
    public GameObject PickUpPrefab;
    public bool inCombat;
    List<GameObject> PickUps;
    public Item[] AllItems;
    Dictionary<string,Item> itemFinder;
    void Awake()
    {
        itemFinder = new Dictionary<string, Item>();
        foreach(Item i in AllItems)
        {
            itemFinder.Add(i.name,i);
        }
        PickUps = new List<GameObject>();
    }
    public void ToggleMovement(bool value)
    {
        player.canMove = value;
    }

    public void SetInCombat(bool value)
    {
        inCombat = value;
    }

    public void PlayerDeath()
    {
        Debug.Log("GAME OVER");
    }

    public void SpawnItem(Vector3 position,Item item, int amount)
    {
        GameObject temp = Instantiate(PickUpPrefab,position,Quaternion.identity);
        PickUpItem tempItem = temp.GetComponent<PickUpItem>();
        tempItem.item = item;
        tempItem.amount = amount;
        PickUps.Add(temp);
    }

    public void SpawnItem(Vector3 position,string item, int amount)   
    {
        if(itemFinder.ContainsKey(item)) SpawnItem(position,itemFinder[item],amount);
    } 

    public void DeleteItem(GameObject item)
    {
        if(PickUps.Contains(item)) PickUps.Remove(item);
        Destroy(item);
    }
}
