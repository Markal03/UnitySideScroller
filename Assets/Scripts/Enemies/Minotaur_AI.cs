using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minotaur_AI : StateMachineBehaviour
{
    public float speed = 2.5f;
    public float attackRange = 1.0f;
    public float aggroRange = 20.0f;
    Transform player;
    Rigidbody2D rb;
    LookAtPlayer lookAtPlayer;
    private Transform attackPoint;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        lookAtPlayer = animator.GetComponent<LookAtPlayer>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        lookAtPlayer.LookAtPlayerCharacter();

        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        float distance = Vector2.Distance(player.position, rb.position);

        if (distance < aggroRange)
            rb.MovePosition(newPos);

        if (Vector2.Distance(player.position, rb.position) < attackRange)
        {
            animator.SetTrigger("Attack");
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
}
