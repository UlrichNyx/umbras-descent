using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerController player;
    public GameObject PickUpPrefab;
    public bool inCombat;
    List<GameObject> PickUps;

    void Awake()
    {
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

    public void DeleteItem(GameObject item)
    {
        if(PickUps.Contains(item)) PickUps.Remove(item);
        Destroy(item);
    }
}
