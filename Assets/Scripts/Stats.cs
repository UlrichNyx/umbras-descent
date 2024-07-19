using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public GameManager gameManager
    {
        get
        {
            if(_gameManager == null) _gameManager = GameManager.instance;
            return _gameManager;
        }
    }
    GameManager _gameManager;
    public float MaxHealth;
    public float myHealth;
    public int ShadowEssence;

    void Start()
    {
        myHealth = MaxHealth;
    }
    
    public virtual void ModifyHealth(float amount)
    {
        myHealth += amount;
        if(myHealth <= 0)
        {
            Die();
        }
        if(myHealth > MaxHealth)
        {
            myHealth = MaxHealth;
        }
    }
    public virtual void Die()
    {
        gameManager.SpawnEssence(transform.position,ShadowEssence);
    }
}
