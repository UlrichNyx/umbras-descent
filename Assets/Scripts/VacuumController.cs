using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class VacuumController : MonoBehaviour
{

    public Vector3 direction;
    public bool isSucking;
    public int MaxTargets = 10;
    public float SuckSpeed;
    public LayerMask layermask;
    private LineRenderer lineRenderer;
    private Stats stats;
    Inventory inventory;
    GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.instance;
        inventory = GetComponentInParent<Inventory>();
        lineRenderer = GetComponent<LineRenderer>();
                // Set the number of points in the line
        lineRenderer.positionCount = 2;

        // Set the width of the line
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        stats = GetComponentInParent<Stats>();
        StartCoroutine(Suck());
     }

    void FixedUpdate()
    {
        FollowCursor();
        if(!isSucking)
        {
            lineRenderer.enabled = false;
        }
        else
        {
            Vector3 endPoint = transform.position + direction.normalized * stats.AttackRange;
            
            // Set the positions of the line
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, endPoint);
        }
    }



    public void FollowCursor()
    {
        // Get the mouse position in the world
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        
        // Calculate the direction from the sprite to the mouse
        direction = mousePosition - transform.position;
        
        // Calculate the angle between the sprite's forward direction and the direction to the mouse
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        
        // Rotate the sprite to face the mouse
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void SetSuck(bool value)
    {
        isSucking = value;
    }

    IEnumerator Suck()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);
            List<Collider2D> hits = new List<Collider2D>();
            if(!isSucking)
            {
                lineRenderer.enabled = false;
                continue;
            }
            
            for(int i = 0; i < MaxTargets; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, stats.AttackRange, layermask);

                if(hit.collider == null) break;
                
                PickUpItem tempItem = hit.collider.GetComponentInParent<PickUpItem>();
                if(tempItem != null)
                {
                    //Debug.Log("Item hit");
                    //Debug.Log((transform.position - tempItem.transform.position).normalized * SuckSpeed);
                    tempItem.transform.position += (transform.position - tempItem.transform.position).normalized * SuckSpeed;
                    if(Vector2.Distance(tempItem.transform.position,transform.position) < 0.5f)
                    {
                        inventory.ModifyItem(tempItem.item,tempItem.amount);
                        inventory.ReceiveRecipe(tempItem.recipe);
                        gameManager.DeleteItem(tempItem.gameObject);
                    }
                }
                else
                {
                    //Debug.Log("Raycast hit: " + hit.collider.name);
                    hit.collider.GetComponent<Stats>().ModifyHealth(-stats.damage);
                    stats.ModifyHealth(stats.damage);
                    UIManager.instance.SetShadowEssenceSlider(stats.ShadowEssence);
                }
                hit.collider.enabled = false;
                hits.Add(hit.collider);
            }

            foreach(Collider2D i in hits)
            {
                i.enabled = true;
            }
        }
    }
}
