using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class VacuumController : MonoBehaviour
{

    public Vector3 direction;
    public int attackRange;
    public bool isSucking;

    public LayerMask enemyLayer;
    private LineRenderer lineRenderer;

    private Stats stats;

    void Start()
     {
        lineRenderer = GetComponent<LineRenderer>();
                // Set the number of points in the line
        lineRenderer.positionCount = 2;

        // Set the width of the line
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        stats = GetComponentInParent<Stats>();
     }

    void Update()
    {
        FollowCursor();
        Suck();
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

    public void Suck()
    {
        if(!isSucking)
        {
            lineRenderer.enabled = false;
            return;
        }
        Vector3 endPoint = transform.position + direction.normalized * attackRange;
        Debug.DrawLine(transform.position, endPoint, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, attackRange, enemyLayer);
        // Set the positions of the line
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPoint);
        
        if(hit.collider != null)
        {
            Debug.Log("Raycast hit: " + hit.collider.name);
            hit.collider.GetComponent<Stats>().ModifyHealth(-stats.damage);
            stats.ModifyHealth(stats.damage);
            UIManager.instance.SetShadowEssenceSlider(stats.ShadowEssence);
        }   
    }
}
