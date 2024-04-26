using Platformer;
using System;
using UnityEngine;
public class BallBehavior : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] TargetManager targetManager;

    [Header("Movement Settings")]
    [SerializeField] float acceleration = 0.01f;
    [SerializeField] float maxSpeed = 40f;
    [SerializeField] private float rotationSmoothness = 0.2f;
    [SerializeField] private float baseSpeed = 5f;
    private const float Y_Offset = 1f;

    private float currentSpeed = 0f;
    private float t = 0;

    private Quaternion targetRotation;

    // Set this via inspector or programmatically

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();

        targetManager.GameOver.AddListener(OnGameEnded);
        targetManager.ResetBall.AddListener(OnReset);

    }

    private void OnReset()
    {
        transform.position = new Vector3(0, 3, 0);
    }

    private void OnGameEnded()
    {
        targetManager.GameOver.RemoveAllListeners();
        targetManager.ResetBall.RemoveAllListeners();
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (targetManager.LastTarget != null)
        {
            SetLinearVelocity();
            SmoothRotateTowardsTarget();
        }


        //if (!currentTarget)  {
        //    Destroy(gameObject);
        //}
    }

    void SetLinearVelocity()
    {
     
        t += Time.deltaTime;
        currentSpeed = baseSpeed + t * acceleration;
        currentSpeed = Mathf.Clamp(currentSpeed, baseSpeed, maxSpeed);
        rb.velocity = transform.forward * currentSpeed;
    }

    void SmoothRotateTowardsTarget()
    {
        targetRotation = Quaternion.LookRotation((targetManager.LastTarget.transform.position + new Vector3(0, Y_Offset, 0)) - transform.position);
        rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSmoothness);
    }
}
