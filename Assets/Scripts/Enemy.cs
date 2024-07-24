using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Stats
{
    Rigidbody2D rb;
    LineRenderer lineRenderer;
    public LayerMask playerLayer;
    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();   
        lineRenderer = GetComponent<LineRenderer>();
        StartCoroutine(ChasePlayer());
    }

    public override void Die()
    {
        base.Die();
        gameObject.SetActive(false);
        gameManager.SpawnItem(transform.position,"Red Herb",1);
        gameManager.SpawnItem(transform.position,"Blue Herb",1);
    }

    public override void Update()
    {
        base.Update();
    }
    

    IEnumerator ChasePlayer()
    {
        while(!IsDead)
        {
            yield return new WaitForSeconds(0.1f);
            Vector3 target = gameManager.player.transform.position;
            Vector2 direction = (target - transform.position).normalized;
            if(Vector2.Distance(transform.position,gameManager.player.transform.position) > AttackRange)
            {
                rb.velocity = direction * moveSpeed;
                lineRenderer.enabled = false;
            }
            else
            {
                rb.velocity = new Vector2();
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, AttackRange, playerLayer);

        
                if(hit.collider != null)
                {
                    lineRenderer.enabled = true;
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, target);
                    //Debug.Log("Raycast hit: " + hit.collider.name);
                    hit.collider.GetComponentInParent<Stats>().ModifyHealth(-damage);
                    ModifyHealth(damage);
                }  
            }
        }
    }
}
