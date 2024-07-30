using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class DarkPixie : Stats
{
    LineRenderer line;
    public LayerMask playerLayer;
    NavMeshAgent agent;
    public float AggroRange = 1;
    public float StopAggroRange = 4;
    public bool Aggroed;
    public List<Vector3> PatrolPoints;
    public float DropRate;
    public Item[] DropItems;
    public float[] DropItemChances;
    public int[] DropItemAmount;
    public float chargeSpeed;
    public GameObject PixieCircle;
    public PixieCircle circle;
    Vector3[] directionArray = new Vector3[] {new Vector3(0,1,0),new Vector3(1,0,0),new Vector3(-1,0,0),new Vector3(0,-1,0)};
    public override void Start()
    {
        base.Start();
        circle = Instantiate(PixieCircle,new Vector3(1000,1000,0),Quaternion.identity).GetComponent<PixieCircle>();
        agent = GetComponent<NavMeshAgent>();
        line = GetComponent<LineRenderer>();
        agent.speed = moveSpeed;
        StartCoroutine(ChasePlayer());

        
        for(int i = 0 ; i < 4 ; i++)
        {
            NavMeshHit hit;
            if(UnityEngine.AI.NavMesh.SamplePosition(transform.position + directionArray[i],out hit, 2f, UnityEngine.AI.NavMesh.AllAreas))
            {
                PatrolPoints.Add(new Vector3(hit.position.x,hit.position.y,0));
            }
        }
    }

    public override void Die()
    {
        base.Die();
        gameObject.SetActive(false);
        Destroy(circle);
        if(UnityEngine.Random.Range(0,100) < DropRate)
        {
            //Debug.Log("drop rate");
            float counter = 100;
            for(int i = 0; i < DropItems.Length; i++)
            {
                
                int number = UnityEngine.Random.Range(0,100);
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
        cooldown -= Time.deltaTime;
    }

    float cooldown = 0;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(cooldown <= 0)
        {
            PlayerStats stats = collider.GetComponentInParent<PlayerStats>();
            if(stats != null)
            {
                stats.ModifyHealth(-damage);
            }
            ModifyHealth(damage);
        }
    }
    IEnumerator ChasePlayer()
    {
        while(!IsDead)
        {
            yield return new WaitForSeconds(0.1f);

            Vector2 target = gameManager.player.transform.position;
            float distance = Vector2.Distance(transform.position,gameManager.player.transform.position);
            if((!Aggroed && distance < AggroRange) || Aggroed)
            {
                Aggroed = true;
                if(distance >= StopAggroRange)
                {
                    Aggroed = false;
                    continue;
                }
                //Debug.Log("Preparing");
                agent.isStopped = true;
                NavMeshHit hit;
                circle.SetInnerCircleSize(0);
                bool targetfound = false;
                for(int i = 0 ; i < 20; i++)
                {
                    Vector2 temp = directionArray[UnityEngine.Random.Range(0,directionArray.Length)];
                    if(UnityEngine.AI.NavMesh.SamplePosition(target + temp* 0.2f,out hit, 0.2f,UnityEngine.AI.NavMesh.AllAreas))
                    {
                        circle.transform.position = hit.position;
                        targetfound = true;
                        break;
                    }
                }
                if(targetfound)
                {
                    float timer = 0;
                    while(timer <= 1)
                    {
                        timer += Time.deltaTime;
                        circle.SetInnerCircleSize(timer);
                        yield return null;
                    }

                    transform.position = circle.transform.position;
                    if(circle.IsPlayerInside)
                    {
                        gameManager.player.GetComponentInParent<PlayerStats>().ModifyHealth(-damage);
                        ModifyHealth(damage);
                    }
                }
                circle.transform.position = new Vector3(1000,1000,0);
                yield return new WaitForSeconds(1f);
                
            }
            else if((agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending) || !agent.hasPath)
            {
                agent.speed = moveSpeed;
                agent.SetDestination(PatrolPoints[UnityEngine.Random.Range(0,PatrolPoints.Count)]);
            }
        }
    }

    void SetTrail()
    {
        if(agent.pathStatus != NavMeshPathStatus.PathInvalid)
        {
            line.positionCount = agent.path.corners.Length;
            for(int i = 0; i < agent.path.corners.Length; i++)
            {   
                line.SetPosition(i,agent.path.corners[i]);
            }
            line.enabled = true;
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
