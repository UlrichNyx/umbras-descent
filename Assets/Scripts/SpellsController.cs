using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsController : MonoBehaviour
{
    public GameObject ShadowVortexPrefab;
    public float[] Cooldowns;
    float[] LastCastTime;
    void Start()
    {
        LastCastTime = new float[Cooldowns.Length];
    }
    public bool CastSpell(int spellID)
    {
        if(Time.time - LastCastTime[spellID] < Cooldowns[spellID]) return false;

        if(spellID == 0) 
        {
            ShadowVortex();
        }
        
        return true;
    }

    void ShadowVortex()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Instantiate(ShadowVortexPrefab,mousePos,Quaternion.identity);
    }

}
