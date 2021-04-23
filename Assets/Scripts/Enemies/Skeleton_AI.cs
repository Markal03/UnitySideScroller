using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_AI : StateMachineBehaviour
{
    public float speed = 2.5f;
    public float attackRange = 1.0f;
    Transform player;
    Rigidbody2D rb;
    LookAtPlayer lookAtPlayer;
    //Patrol_AI patrol;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        lookAtPlayer = animator.GetComponent<LookAtPlayer>();
        //patrol = animator.GetComponent<Patrol_AI>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        



        if (Vector2.Distance(player.position, rb.position) < 25) //&& patrol.CanMove())
        {
            lookAtPlayer.LookAtPlayerCharacter();
            //patrol.SetPatrolling(false);

            Vector2 target = new Vector2(player.position.x, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);



            rb.MovePosition(newPos);

            if (Vector2.Distance(player.position, rb.position) < attackRange)
            {
                animator.SetTrigger("Attack");
            }
        } else
        {
            //patrol.SetPatrolling(true);
        }
 
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }


}
