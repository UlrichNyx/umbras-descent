using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Stats
{
    public override void Start()
    {
        base.Start();   
    }

    public override void Die()
    {
        base.Die();
        gameObject.SetActive(false);
    }
}
