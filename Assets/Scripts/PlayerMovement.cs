using Platformer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable { 

    void HandleMovement(Vector3 targetPos);
}

public class PlayerMovement : IMoveable
{
    Rigidbody rb;
    PlayerController playerController;
    private float velocity;
    const float ZeroF = 0f;
    public PlayerMovement( PlayerController playerController)
    {
        this.playerController = playerController;
        rb = playerController.rb;
    }

    public void HandleMovement(Vector3 adjustedDirection)
    {
        if (adjustedDirection.magnitude > ZeroF)
        {
            HandleRotation(adjustedDirection);
            HandleHorizantalMovement(adjustedDirection);
            SmoothSpeed(adjustedDirection.magnitude);
        }
        else
        {
            SmoothSpeed(ZeroF);
            rb.velocity = new Vector3(ZeroF, rb.velocity.y, ZeroF);
        }
    }
    private void HandleHorizantalMovement(Vector3 adjustedDirection)
    {
        Vector3 velocity = adjustedDirection * (playerController.MoveSpeed * playerController.DashVelocity * Time.fixedDeltaTime);
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
       
    }

    private void HandleRotation(Vector3 adjustedDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(adjustedDirection);
        playerController.transform.rotation = Quaternion.Slerp(playerController.transform.rotation, targetRotation, playerController.RotationSpeed * Time.deltaTime);
    }
    private void SmoothSpeed(float value)
    {
        playerController.CurrentSpeed = Mathf.SmoothDamp(playerController.CurrentSpeed, value, ref velocity, playerController.SmoothTime);
    }
}
