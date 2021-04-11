using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementRootMotion : MonoBehaviour
{
    private Animator animator;
    public Transform cameraTransform;

    private float currentTurningVelocity;
    public float turningTime = 0.1f;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 inputMovement = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        float speed = 1;

        if(Input.GetButton("Sprint"))
            speed = 10;

        animator.SetFloat("Movement", inputMovement.magnitude * speed);

        if (inputMovement.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputMovement.x, inputMovement.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentTurningVelocity, turningTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }
}
