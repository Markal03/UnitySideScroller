using UnityEngine;

public class Hydra_AI : StateMachineBehaviour
{
    public float attackRange = 1.3f;
    Transform player;
    Rigidbody2D rb;
    LookAtPlayer lookAtPlayer;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        lookAtPlayer = animator.GetComponent<LookAtPlayer>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        lookAtPlayer.LookAtPlayerCharacter();

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
