using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float speedX, speedY;
    Rigidbody2D rb;

    private Stats stats;

    private VacuumController vacuumController;
    // Start is called before the first frame update

    public LayerMask layerMask;

    public float rollSpeed = 10f;
    public float rollDuration = 0.5f;
    public float rollCooldown = 1.5f;

    private bool isRolling = false;
    private float lastRollTime;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<Stats>(); 
        vacuumController = GetComponentInChildren<VacuumController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        HandleActions();
        HandleInteraction();
    }

    private void Move()
    {
        if(isRolling)
        {
            return;
        }
        speedX =  Input.GetAxisRaw("Horizontal") * stats.moveSpeed;
        speedY = Input.GetAxisRaw("Vertical") * stats.moveSpeed;
        rb.velocity = new Vector2(speedX, speedY);
    }

    private void VeilOfNight()
    {
        // Check if the Space key is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stats.SetInvisible(!stats.invisible);
        }

    }

    private void ArmorOfDarkness()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            stats.SetInvincible(!stats.invincible);
        }
    }

    void HandleActions()
    {
        // Suck action
        if (Input.GetMouseButton(0)) // Left click
        {
            Attack();
        }
        else
        {
            StopAttack();
        }

        if(!isRolling && Input.GetKeyDown(KeyCode.Space) && Time.time > lastRollTime + rollCooldown)
        {
            StartCoroutine(Roll());
        }


        // Spells action
        if (Input.GetMouseButtonDown(1)) // Right click
        {
            CastSpell();
        }

        // Consume Alchemy action
        if (Input.GetMouseButtonDown(2)) // Middle mouse click
        {
            ConsumeAlchemy();
        }

        // Swap equipped spell
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            SwapSpell(scroll);
        }

        // Swap equipped alchemy
        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                SwapAlchemy(i);
            }
        }
    }

    void HandleInteraction()
    {
        // Interact action
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void Attack()
    {
        // Debug.Log("Attack action performed");
        vacuumController.SetSuck(true);
    }

    void StopAttack()
    {
        vacuumController.SetSuck(false);
    }

    private IEnumerator Roll()
    {
        isRolling = true;
        lastRollTime = Time.time;

        Vector2 rollDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        rb.velocity = rollDirection * rollSpeed;

        yield return new WaitForSeconds(rollDuration);

        isRolling = false;
        rb.velocity = Vector2.zero;  // Stop rolling

        Move();  // Resume normal movement
    }

    void CastSpell()
    {
        Debug.Log("Spell cast");
        // Add your spell casting logic here
    }

    void ConsumeAlchemy()
    {
        Debug.Log("Alchemy consumed");
        // Add your alchemy consumption logic here
    }

    void SwapSpell(float scrollValue)
    {
        Debug.Log("Spell swapped with scroll value: " + scrollValue);
        // Add your spell swapping logic here
    }

    void SwapAlchemy(int alchemyIndex)
    {
        Debug.Log("Alchemy swapped to slot: " + alchemyIndex);
        // Add your alchemy swapping logic here
    }

    void Interact()
    {
        Debug.Log("Interact action performed");
        // Add your interaction logic here
        // This could be for opening doors, dialogue, or picking up items
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3f, layerMask))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                // Example: hit.collider.GetComponent<Interactable>().Interact();
                Debug.Log("Interacted with " + hit.collider.name);
            }
        }
    }
}


