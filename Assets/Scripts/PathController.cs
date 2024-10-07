using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    [SerializeField] private PathManager pathManager;

    private List<Waypoint> thePath;
    private Waypoint target;

    public float MoveSpeed;
    public float RotateSpeed;

    public Animator animator;
    protected bool isWalking;

    protected void Start()
    {
        isWalking = false;
        animator.SetBool("isWalking", isWalking);
        
        thePath = pathManager.GetPath();
        if (thePath != null && thePath.Count > 0)
        {
            target = thePath[0];
        }
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            isWalking = !isWalking;
            animator.SetBool("isWalking", isWalking);
        }

        if (!isWalking)
        {
            transform.Translate(Vector3.zero);
            return;
        }
        RotateTowardsTarget();
        MoveTowardsTarget();
    }

    protected void RotateTowardsTarget()
    {
        float stepSize = RotateSpeed * Time.deltaTime;

        Vector3 targetDir = target.GetPos() - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, stepSize, 0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    protected void MoveTowardsTarget()
    {
        float stepSize = Time.deltaTime * MoveSpeed;
        float distanceToObject = Vector3.Distance(transform.position, target.GetPos());

        if (distanceToObject < stepSize)
        {
            return;
        }

        Vector3 moveDir = Vector3.forward;
        transform.Translate(moveDir * stepSize);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(pathManager.prefabPoints[pathManager.currentPointIndex]))
            target = pathManager.GetNextTarget();
    }
}
