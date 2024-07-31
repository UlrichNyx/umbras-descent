using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

public class ShadowVortex : MonoBehaviour
{
    public List<NavMeshAgent> enemies;
    public float SuckSpeed = 0.2f;
    public int DamagePerTick = 1;
    public float Duration = 3f;

    void Start()
    {
        StartCoroutine(Suck());
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        NavMeshAgent temp = collider.GetComponent<NavMeshAgent>();
        if(temp != null)
        {
            temp.isStopped = true;
            enemies.Add(temp);
        }
    }

    

    void OnTriggerExit2D(Collider2D collider)
    {
        NavMeshAgent temp = collider.GetComponent<NavMeshAgent>();
        if(temp != null && enemies.Contains(temp))
        { 
            temp.isStopped = false;
            enemies.Remove(temp);
        }
    }


    IEnumerator Suck()
    {
        float timer = Time.time;
        while(true)
        {
            yield return new WaitForSeconds(0.1f);
            for(int i = 0; i < enemies.Count; i++)
            {
                enemies[i].isStopped = true;
                if(Vector2.Distance(enemies[i].transform.position,transform.position) > 0.2f)
                {
                    Vector3 direction = (transform.position - enemies[i].transform.position).normalized;
                    enemies[i].transform.position += direction * SuckSpeed;
                }
                enemies[i].GetComponent<Stats>().ModifyHealth(-DamagePerTick);
            }
            if(Time.time - timer >= Duration)
            {
                break;
            }
        }

        Destroy(gameObject);
    }

    void OnDisable()
    {
        foreach(NavMeshAgent i in enemies)
        {
            if(i != null) i.isStopped = false;
        }
    }
}
