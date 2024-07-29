using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class Enemy : Stats
{
    Rigidbody2D rb;
    LineRenderer lineRenderer;
    public LayerMask playerLayer;
    NavMeshAgent agent;
    public float AggroRange;
    public float StopAggroRange;
    public bool Aggroed;
    public List<Vector3> PatrolPoints;
    public float DropRate;
    public Item[] DropItems;
    public float[] DropItemChances;
    public int[] DropItemAmount;
    public override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        lineRenderer = GetComponent<LineRenderer>();
        StartCoroutine(ChasePlayer());

        Vector3[] direction = new Vector3[] {new Vector3(0,1,0),new Vector3(1,0,0),new Vector3(-1,0,0),new Vector3(0,-1,0)};
        for(int i = 0 ; i < 4 ; i++)
        {
            NavMeshHit hit;
            if(UnityEngine.AI.NavMesh.SamplePosition(transform.position + direction[i],out hit, 2f, UnityEngine.AI.NavMesh.AllAreas))
            {
                PatrolPoints.Add(new Vector3(hit.position.x,hit.position.y,0));
            }
        }
    }

    public override void Die()
    {
        base.Die();
        gameObject.SetActive(false);
        if(Random.Range(0,100) < DropRate)
        {
            //Debug.Log("drop rate");
            float counter = 100;
            for(int i = 0; i < DropItems.Length; i++)
            {
                
                int number = Random.Range(0,100);
                counter -= DropItemChances[i];
                //Debug.Log(number + " - " +counter);
                if(number >= counter)
                {
                    gameManager.SpawnItem(transform.position,DropItems[i],DropItemAmount[i]);
                    break;
                }
            }
        }
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
            float distance = Vector2.Distance(transform.position,gameManager.player.transform.position);
            if(!Aggroed && distance < AggroRange)
            {
                Aggroed = true;
                agent.SetDestination(gameManager.player.transform.position);
            }
            if(Aggroed)
            {
                if(distance < StopAggroRange)
                {
                    agent.SetDestination(gameManager.player.transform.position);
                }
                else
                {
                    Aggroed = false;
                }
                if(distance <= AttackRange)
                {
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
                else
                {
                    lineRenderer.enabled = false;
                }
            }

        }
    }


    public static void DrawGizmos(NavMeshAgent agent, bool showPath, bool showAhead)
    {
        if (Application.isPlaying && agent != null)
        {
            if (showPath && agent.hasPath)
            {
                var corners = agent.path.corners;
                if (corners.Length < 2) { return; }
                int i = 0;
                for (; i < corners.Length - 1; i++)
                {
                    Debug.DrawLine(corners[i], corners[i + 1], Color.blue);
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(agent.path.corners[i + 1], 0.03f);
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(agent.path.corners[i], agent.path.corners[i + 1]);
                }
                Debug.DrawLine(corners[0], corners[1], Color.blue);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(agent.path.corners[1], 0.03f);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(agent.path.corners[0], agent.path.corners[1]);
            }

            if (showAhead)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(agent.transform.position, agent.transform.up * 0.5f);
            }
        }
    }
}
