using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player_Move : MonoBehaviour
{
    private Collision collision;
    [HideInInspector]
    private AnimationScript anim;
    public Rigidbody2D rb;


    [Space]
    [Header("Stats")]
    public PauseMenu pauseMenu;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 20;
    public LayerMask enemyLayers;
    public float attackSpeed = 2f;
    public float nextAttackTime = 0f;

    [Header("Booleans")]
    public bool canMove;
    public bool wallGrab;
    public bool wallJumped;
    public bool wallSlide;
    public bool isDashing;

    //Movement
    public int speed = 10;
    public float playerJumpPower = 10;
    public float slideSpeed = 5;
    public float wallJumpLerp = 10;
    private bool facingRight = false;
    private float moveX;
    private Vector2 lastMoveDirection = Vector2.right;
    private Vector2 moveDirection;

    //Jump
    private bool groundTouch;
    private bool hasDashed;

    public int side = -1;




    public bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    private int extraJumps;
    public int extraJumpsValue;


    //Dash
    public float dashSpeed;
    public float startDashTime;
    private float gravity;

    private void Awake()
    {
        collision = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<AnimationScript>();
        gravity = rb.gravityScale;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameObject.GetComponent<Player_Health>().hasDied)
            return;

        //PlayerMove();
        Animate();
    }
    
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y);
        Walk(dir);

        anim.SetHorizontalMovement(x, y, rb.velocity.y);

        if (gameObject.GetComponent<Player_Health>().hasDied)
            return;

        if (Input.GetButtonDown("Cancel"))
        {
            pauseMenu.ShowPauseMenu();
        }
        if (collision.onWall && Input.GetButton("Fire3") && canMove)
        {
           if (side == collision.wallSide)
                Flip();
            wallGrab = true;
            wallSlide = false;
        }

        if (Input.GetButtonUp("Fire3") || !collision.onWall || !canMove)
        {
            wallGrab = false;
            wallSlide = false;
        }

        if (collision.onGround && !isDashing)
        {
            wallJumped = false;
            GetComponent<ImprovedJump>().enabled = true;
        }

        if (wallGrab && !isDashing)
        {
            rb.gravityScale = 0;
            if (x > .2f || x < -.2f)
               rb.velocity = new Vector2(rb.velocity.x, 0);

            float speedModifier = y > 0 ? .5f : 1;

            rb.velocity = new Vector2(rb.velocity.x, y * (speed * speedModifier));
        }
        else
        {
            rb.gravityScale = 3;
        }

        if (collision.onWall && !collision.onGround)
        {
            if (x != 0 && !wallGrab)
            {
                wallSlide = true;
                WallSlide();
            }
        }

        if (!collision.onWall || collision.onGround)
            wallSlide = false;

        if (Input.GetButtonDown("Jump"))
        {
            if (collision.onGround || extraJumps > 0)
                Jump(Vector2.up, false);
            if (collision.onWall && !collision.onGround)
                WallJump();
        }

        if (Input.GetButtonDown("Fire2") && !hasDashed)
        {
            if (xRaw != 0 || yRaw != 0)
                Dash(xRaw, yRaw);
        }

        if (collision.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        if (!collision.onGround && groundTouch)
        {
            groundTouch = false;
        }

        //WallParticle(y);

        if (wallGrab || wallSlide || !canMove)
            return;

        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackSpeed;
            }
        }
    }

    void PlayerMove()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);


        if (!isDashing)
        {
            moveX = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(moveX * speed, rb.velocity.y);
            anim.SetFloat("Speed", Mathf.Abs(moveX * speed));
            if (moveX == 0 && moveDirection.x != 0)
            {
                lastMoveDirection = moveDirection;
            }

            moveDirection = new Vector2(moveX, 0).normalized;

            if ((moveDirection.x < 0 && facingRight == false) || (moveDirection.x > 0 && facingRight == true))
            {
                Flip();
            }
        }
            


    }
    void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;
        extraJumps = extraJumpsValue + 1;

        //jumpParticle.Play();
    }

    private void Jump(Vector2 dir, bool wall)
    {
        //slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
        //ParticleSystem particle = wall ? wallJumpParticle : jumpParticle;

        extraJumps--;
        anim.SetTrigger("Jump");
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * playerJumpPower;

        //particle.Play();
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    void RigidbodyDrag(float x)
    {
        rb.drag = x;
    }

    void Attack()
    {
        anim.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit" + enemy.name);
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    private void Walk(Vector2 dir)
    {
        if (!canMove)
            return;

        if (wallGrab)
            return;

        if (!wallJumped)
        {
            rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
            anim.SetFloat("Speed", Mathf.Abs(dir.x * speed));


            //Direction handling for animations
            if (dir.x == 0 && moveDirection.x != 0)
                lastMoveDirection = moveDirection;

            moveDirection = new Vector2(dir.x, 0).normalized;

            if ((moveDirection.x < 0 && facingRight == false) || (moveDirection.x > 0 && facingRight == true))
                Flip();

        }
        else
        {
            //Different angle for wall jump given by linear interpolation
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
        }
    }

    //Hitbox helper, it does not affect game
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void Animate()
    {
        anim.SetFloat("LastHorizontal", lastMoveDirection.x);
        //animator.SetFloat("Horizontal", moveDirection.x);
    }

    /// <summary>Function <c>Flip</c> updates the player's facing direction.
    /// <para>Necessary for the Animator to know which variant of the animation to run.</para></summary>
    void Flip()
    {
        facingRight = !facingRight;
        
        anim.SetFloat("HorizontalFacing", facingRight ? 0 : 1);
        Vector2 colliderPosition = attackPoint.transform.localPosition;
        colliderPosition.x *= -1;
        side *= -1;
        attackPoint.transform.localPosition = colliderPosition;
    }

    /// <summary>Function <c>Dash</c> makes the player dash towards a certain direction.
    /// <para><c>X</c> is the raw horizontal direction</para>
    /// <para><c>Y</c> is the raw vertical direction</para></summary>
    private void Dash(float x, float y)
    {
        Camera.main.transform.DOComplete();
        Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
        //FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));
        gameObject.GetComponent<Player_Stamina>().ConsumeStamina(20);
        hasDashed = true;

        anim.SetTrigger("dash");

        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(x, y);

        rb.velocity += dir.normalized * dashSpeed;
        StartCoroutine(DashWait());
    }

    IEnumerator DashWait()
    {
        //FindObjectOfType<GhostTrail>().ShowGhost();
        StartCoroutine(GroundDash());
        //DOVirtual.Float(14, 0, .8f, RigidbodyDrag);

        //dashParticle.Play();
        rb.gravityScale = 0;
        GetComponent<ImprovedJump>().enabled = false;
        wallJumped = true;
        isDashing = true;

        yield return new WaitForSeconds(.3f);

        //dashParticle.Stop();
        rb.gravityScale = 3;
        GetComponent<ImprovedJump>().enabled = true;
        wallJumped = false;
        isDashing = false;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        if (collision.onGround)
            hasDashed = false;
    }

    private void WallJump()
    {
        if ((side == -1 && collision.onRightWall) || side == 1 && !collision.onRightWall)
        {
            //side *= -1;
            Flip();
        }
        //Flip();

        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        Vector2 wallDir = collision.onRightWall ? Vector2.left : Vector2.right;

        Jump((Vector2.up / 1.5f + wallDir / 1.5f), true);

        wallJumped = true;
    }

    private void WallSlide()
    {
        if (collision.wallSide == side)
            Flip();

        if (!canMove)
            return;

        bool pushingWall = false;
        if ((rb.velocity.x > 0 && collision.onRightWall) || (rb.velocity.x < 0 && collision.onLeftWall))
        {
            pushingWall = true;
        }
        float push = pushingWall ? 0 : rb.velocity.x;

        rb.velocity = new Vector2(push, -slideSpeed);
    }

    //void WallParticle(float vertical)
    //{
    //    var main = slideParticle.main;

    //    if (wallSlide || (wallGrab && vertical < 0))
    //    {
    //        slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
    //        main.startColor = Color.white;
    //    }
    //    else
    //    {
    //        main.startColor = Color.clear;
    //    }
    //}

    //int ParticleSide()
    //{
    //    int particleSide = coll.onRightWall ? 1 : -1;
    //    return particleSide;
    //}
}
