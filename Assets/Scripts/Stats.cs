using UnityEngine;

public class Stats : MonoBehaviour
{
    public GameManager gameManager
    {
        get
        {
            if(_gameManager == null) _gameManager = GameManager.instance;
            return _gameManager;
        }
    }
    GameManager _gameManager;
    public float baseShadowEssence = 20;
    public float ShadowEssence= 20;
    public static int maxShadowEssence = 9999;
    public bool invincible;
    public bool invisible;
    public float moveSpeed;
    public SpriteRenderer spriteRenderer;
    private float invisibleAlpha = 0.5f;
    private float visibleAlpha = 1f;
    private Color initialColor;
    private float colorChangeSpeed = 1.0f;
    private float rainbowTime;
    public float damage;
    public float AttackRange;
    public bool IsDead = false;

    public bool isFlipped = false;

    public Vector3 previousPosition;

    public Vector3 currentPosition;

    private bool controlledSprite;

    public Animator animator;

    public bool isPlayer;
    public enum AnimationParameters {
        Movement
    }
    public virtual void Start()
    {
        ShadowEssence = baseShadowEssence;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        initialColor = spriteRenderer.color;
        animator = GetComponentInChildren<Animator>();
        previousPosition = transform.position;
        controlledSprite = true;
        isPlayer = GetComponent<PlayerController>() != null;
    }

    
    public void SetAnimatorParameter(AnimationParameters param, bool value)
    {
        if(!animator)
        {
            return;
        }

        animator.SetBool(param.ToString(), value);
    }

    public void SetSpriteFlipped(float horizontalMovement)
    {
        if(!controlledSprite)
        {
            Debug.Log(controlledSprite);
            //spriteRenderer.flipX = horizontalMovement >= 0 ? false : true;
        }
        
    }
    
    public virtual void ModifyHealth(float amount)
    {
        //Debug.Log("MODIFYING HEALTH");
        if(isPlayer)
        {
            UIManager.instance.SetShadowEssenceSlider(ShadowEssence);
        }
        ShadowEssence += amount;
        if(ShadowEssence <= 0)
        {
            Die();
        }
        if(ShadowEssence > maxShadowEssence)
        {
            ShadowEssence = maxShadowEssence;
        }

    }


    public virtual void Update()
    {
        RainbowEffect();


    }

    public virtual void FixedUpdate()
    {
        currentPosition = transform.position;
        float deltaX = currentPosition.x - previousPosition.x;
        SetSpriteFlipped(deltaX);
        previousPosition = currentPosition;
    }

    public void RainbowEffect()
    {
        if(invincible)
        {
            rainbowTime += Time.deltaTime * colorChangeSpeed;
            float hue = Mathf.Repeat(rainbowTime, 1.0f);
            Color newColor = Color.HSVToRGB(hue, 1.0f, 1.0f);
            spriteRenderer.color = newColor;
        }
    }

    public void SetInvincible(bool value)
    {
        invincible = value;
        if(!value)
        {
            spriteRenderer.color = initialColor;
        }

    }

    public void SetInvisible(bool value)
    {
        invisible = value;
        Color currentColor = spriteRenderer.color;

        if(value)
        {
            spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, invisibleAlpha);
        }
        else
        {
            spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, visibleAlpha);
        }

    }
    public virtual void Die()
    {
        IsDead = true;
    }

    public bool EssenceCheck(int amount)
    {
        return ShadowEssence > amount;
    }
}
