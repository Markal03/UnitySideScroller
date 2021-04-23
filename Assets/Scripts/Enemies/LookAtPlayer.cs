using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform player;
    public Animator animator;
    public bool isFlipped = false;
    public Transform attackPoint;
    public float attackRange = 2.0f;
    public void LookAtPlayerCharacter()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.position.x && isFlipped)
        {
            //transform.localScale = flipped;
            //transform.Rotate(0f, 180f, 0f);
            //isFlipped = false;
            Flip();
        } else if (transform.position.x < player.position.x && !isFlipped)
        {
            //transform.localScale = flipped;
            //transform.Rotate(0f, 180f, 0f);
            //isFlipped = true;
            Flip();
        }
    }

    public void Flip()
    {
        isFlipped = !isFlipped;
        animator.SetFloat("HorizontalFacing", isFlipped ? 1 : 0);
        Vector2 colliderPosition = attackPoint.transform.localPosition;
        colliderPosition.x *= -1;
        attackPoint.transform.localPosition = colliderPosition;
    }
}
