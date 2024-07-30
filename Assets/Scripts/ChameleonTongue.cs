using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChameleonTongue : MonoBehaviour
{
    public bool hit = false;
    Chameleon chameleon;
    void OnEnable()
    {
        if(chameleon == null)
        {
            chameleon = GetComponentInParent<Chameleon>();
        }
        hit = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(hit) return;
        PlayerStats stats = collider.GetComponentInParent<PlayerStats>();
        if(stats != null)
        {
            //Debug.Log("HIT");
            stats.ModifyHealth(-chameleon.damage);
            hit = true;
        }
    }
}
