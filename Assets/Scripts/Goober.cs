using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Goober : EnemyMovement
{
    protected override void Start()
    {
        base.Start();
        initiallyFacingRight = false; // Set this based on the Goober's initial sprite orientation
    }
}
