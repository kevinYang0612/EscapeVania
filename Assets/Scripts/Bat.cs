using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bat : EnemyMovement
{
    [SerializeField] private float verticalAmplitude = 0.5f;
    [SerializeField] private float verticalFrequency = 2f;

    protected override void Move()
    {
        float verticalMovement = Mathf.Sin(Time.time * verticalFrequency) * verticalAmplitude;
        enemyRB2D.velocity = new Vector2(moveSpeed, verticalMovement);
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            base.moveSpeed = -base.moveSpeed;
            base.FlipEnemyFacing();
        }
        
    }
    protected override void OnTriggerExit2D(Collider2D other)
    {
        return;
    }
}
