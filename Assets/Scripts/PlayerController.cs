using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float speedX, speedY;
    Rigidbody2D rb;

    public Stats stats;

    private VacuumController vacuumController;
    // Start is called before the first frame update

    public LayerMask layerMask;

    public float rollSpeed = 10f;
    public float rollDistance = 1f;
    public float rollCooldown = 1.5f;

    private bool isRolling = false;
    private float lastRollTime;
    public Inventory inventory;
    public bool canMove = true;
    SpellsController spellsController;

    public SpriteRenderer vaccuumSprite;

    public Transform leftSide;

    public Transform rightSide;

    void Start()
    {
        inventory = GetComponent<Inventory>();
        spellsController = GetComponent<SpellsController>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<Stats>(); 
        vacuumController = GetComponentInChildren<VacuumController>();
        UIManager.instance.SetShadowEssenceSlider(stats.ShadowEssence);
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            HandleActions();
        }
         HandleInteraction();
    }

     void FixedUpdate() {
        if(canMove)
        {
            Move();
        }
    }

    private void Move()
    {
        if(isRolling)
        {
            return;
        }
        speedX =  Input.GetAxisRaw("Horizontal") * stats.moveSpeed;
        stats.SetAnimatorParameter(Stats.AnimationParameters.Movement, speedX != 0 || speedY != 0 ? true :false );

        Vector3 mouseScreenPosition = Input.mousePosition;

        // Convert the screen position to world position
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        if(mouseWorldPosition.x < transform.position.x)
        {   
            stats.spriteRenderer.flipX = true;
            vaccuumSprite.transform.position = leftSide.position;
        }
        else if(mouseWorldPosition.x > transform.position.x)
        {
            stats.spriteRenderer.flipX = false;
            vaccuumSprite.transform.position = rightSide.position;

        }
         
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
        if(rollDirection == Vector2.zero) rollDirection = new Vector2(0,1);
        rb.velocity = rollDirection * rollSpeed;

        yield return new WaitForSeconds(rollDistance/rollSpeed);

        isRolling = false;
        rb.velocity = Vector2.zero;  // Stop rolling

        Move();  // Resume normal movement
    }

    void CastSpell()
    {
        
        if(spellsController.CastSpell(0)) Debug.Log("Spell cast");
        else Debug.Log("Its on cooldown");
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
        Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, 3f, layerMask);
        if (hitCollider)
        {
            //Debug.Log("HIT THE OBJECT");
            hitCollider.GetComponent<Interactable>().Interact();
        }
    }
}


