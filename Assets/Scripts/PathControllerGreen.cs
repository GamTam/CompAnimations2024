using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathControllerGreen : PathController
{
    protected void Start()
    {
        base.Start();
        isWalking = true;
        animator.SetBool("isWalking", isWalking);
    }
    
    private void Update()
    {
        if (!isWalking)
        {
            transform.Translate(Vector3.zero);
            return;
        }
        
        RotateTowardsTarget();
        MoveTowardsTarget();
    }

    private void OnCollisionEnter(Collision other)
    {
        isWalking = false;
        animator.SetBool("isWalking", isWalking);
    }
    
    private void OnCollisionExit(Collision other)
    {
        isWalking = true;
        animator.SetBool("isWalking", isWalking);
    }
}
