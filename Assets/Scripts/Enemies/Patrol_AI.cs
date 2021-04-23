using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol_AI : MonoBehaviour
{
    public float speed;
    public float distance;
    private bool movingRight = true;
    public Transform groundDetection;
    private bool isPatrolling = true;

    // Update is called once per frame
    void Update()
    {

        

            Patrol();


    }

    private void Patrol()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        


        if (CanMove() && isPatrolling)
        {
                Vector2 newPos = Vector2.MoveTowards(gameObject.transform.position, groundDetection.position, speed * Time.fixedDeltaTime);
        }
        else
        {
            gameObject.GetComponent<LookAtPlayer>().Flip();
        }
    }

    public void SetPatrolling(bool patrolling) {
        isPatrolling = patrolling;
    }

    public bool CanMove()
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
        RaycastHit2D rightWallInfo = Physics2D.Raycast(groundDetection.position, Vector2.right, distance);
        RaycastHit2D leftWallInfo = Physics2D.Raycast(groundDetection.position, Vector2.left, distance);
        return groundInfo.collider == false && rightWallInfo.collider == false && leftWallInfo.collider == false;
    }
}
