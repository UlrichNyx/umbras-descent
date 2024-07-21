using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerController player;

    public bool inCombat;

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

    public void SpawnEssence(Vector2 position, int amount)
    {
        Debug.Log("Spawning Essense");
    }
}
