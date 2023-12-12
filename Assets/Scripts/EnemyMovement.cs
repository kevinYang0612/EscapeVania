using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    protected Rigidbody2D enemyRB2D;
    [SerializeField] protected float moveSpeed = 1f;
    protected bool initiallyFacingRight = true;
    protected virtual void Start()
    {
        enemyRB2D = GetComponent<Rigidbody2D>();
    }
    protected virtual void Update()
    {
        Move();
    }
    protected virtual void Move()
    {
        enemyRB2D.velocity = new Vector2(moveSpeed, 0f);
    }
    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            moveSpeed = -moveSpeed;
            FlipEnemyFacing();
        }
        
    }
    protected void FlipEnemyFacing()
    {
        float directionMultiplier = initiallyFacingRight ? 1f : -1f;
        // Flip the x scale based on moveSpeed
        transform.localScale = new Vector2(-Mathf.Sign(moveSpeed) * 
            directionMultiplier * Mathf.Abs(transform.localScale.x), transform.localScale.y);
    }
}
