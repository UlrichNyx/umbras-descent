using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public void PlayerDeath()
    {
        Debug.Log("GAME OVER");
    }

    public void SpawnEssence(Vector2 position, int amount)
    {
        Debug.Log("Spawning Essense");
    }
}
