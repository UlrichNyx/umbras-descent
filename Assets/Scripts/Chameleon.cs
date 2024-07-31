using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class Chameleon : Stats
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
    Vector3[] directionArray = new Vector3[] {new Vector3(0,1,0),new Vector3(1,0,0),new Vector3(-1,0,0),new Vector3(0,-1,0)};
    public SpriteRenderer tongue;
    public BoxCollider2D tongueCollider;
    public Transform tongueParent;

    public Transform tongueTip;
    ChameleonTongue myTongueScript;
    public SpriteRenderer myBody;
    Color color;
    public override void Start()
    {
        base.Start();
        color = myBody.color;
        color.a = 0.03f;
        myBody.color = color;
        agent = GetComponent<NavMeshAgent>();
        line = GetComponent<LineRenderer>();
        agent.speed = moveSpeed;
        StartCoroutine(ChasePlayer());
        tongue.size = new Vector2(tongue.size.x, 0f);
        tongue.gameObject.SetActive(false);
        myTongueScript = tongue.GetComponentInChildren<ChameleonTongue>();
        
        PatrolPoints.Add(transform.position);
    }

    public override void Die()
    {
        base.Die();
        gameObject.SetActive(false);
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
        if(agent.velocity.magnitude > 0.1f)
        {
            SetAnimatorParameter(AnimationParameters.Movement, true);
        }
        else
        {
            SetAnimatorParameter(AnimationParameters.Movement, false);
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
                color.a = 1;
                myBody.color = color;
                Aggroed = true;
                if(distance >= StopAggroRange)
                {
                    Aggroed = false;
                    
                    continue;
                }
                //Debug.Log("Preparing");

                if(distance < AttackRange)
                {
                    agent.isStopped = true;
                    tongue.gameObject.SetActive(true);
                    myTongueScript.hit = false;
                    float timer = 0;
                    Vector2 dir = target - new Vector2(tongueParent.position.x,tongueParent.position.y);
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    tongueParent.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    while(timer <= 1 && !myTongueScript.hit)
                    {
                        timer += Time.deltaTime;
                        float tempScale = Mathf.Lerp(0,1.5f,timer);
                        tongue.size = new Vector2(0.12f,tempScale);
                        tongueCollider.size = new Vector2(0.2f,tempScale);
                        tongueCollider.offset = new Vector2(0,tempScale/2);
                        tongueTip.position = Vector2.Lerp(tongueTip.position, target, timer);
                        yield return null;
                    }
                    timer = 0;
                    float Startvalue = tongue.size.y;
                    while(timer <= 1)
                    {
                        timer += Time.deltaTime;
                        float tempScale = Mathf.Lerp(Startvalue,0,timer);
                        tongue.size = new Vector2(0.12f,tempScale);
                        tongueCollider.size = new Vector2(0.2f,tempScale);
                        tongueCollider.offset = new Vector2(0,tempScale/2);
                        tongueTip.position = Vector2.Lerp(tongueTip.position, tongueParent.position, timer);
                        yield return null;
                    }
                    tongue.gameObject.SetActive(false);
                    agent.isStopped = false;
                }
                else
                {
                    agent.SetDestination(target);
                }
                
            }
            else if(Vector2.Distance(transform.position,PatrolPoints[0]) > 1f)
            {
                agent.speed = moveSpeed;
                agent.SetDestination(PatrolPoints[0]);
            }
            else if(color.a != 0.03f)
            {
                color.a = 0.03f;
                myBody.color = color;
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
