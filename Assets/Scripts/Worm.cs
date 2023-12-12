using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : EnemyMovement
{
    [SerializeField] private float wormSpeed = 0.5f;

    protected override void Start()
    {
        base.Start();
        moveSpeed = -wormSpeed;
        initiallyFacingRight = true;
    }
}
