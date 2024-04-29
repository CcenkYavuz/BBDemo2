using Platformer;
using System;
using UnityEngine;
public class BallBehavior : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] TargetManager targetManager;
    [SerializeField] MeshRenderer meshRenderer;
    [Header("Movement Settings")]
    [SerializeField] float acceleration = 0.01f;
    [SerializeField] private float rotationSmoothness = 0.2f;
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] float speedFactor = 1.0f;
    private const float Y_Offset = 1f;
    private const float defaultSpeed = 5f;
    private float currentSpeed = 0f;
    private float t = 0;
    private Color defaultColor;

    private Quaternion targetRotation;
   

    // Set this via inspector or programmatically

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        defaultColor = meshRenderer.material.color;
        targetManager.GameOver.AddListener(OnGameEnded);
        targetManager.ResetBall.AddListener(OnReset);
        targetManager.TargetChanged.AddListener(OnTargetChanged);
    }

    void OnTargetChanged()
    {
        if (targetManager.LastTarget.name == "Player")
        {
            meshRenderer.material.color = Color.red;
        }else
        {
            meshRenderer.material.color = defaultColor;
        }
        baseSpeed += speedFactor;
    }
    private void OnReset()
    {
        transform.position = new Vector3(0, 3, 0);
        baseSpeed = defaultSpeed;
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
        //currentSpeed = Mathf.Clamp(currentSpeed, baseSpeed, maxSpeed);
        rb.velocity = transform.forward * currentSpeed;
    }

    void SmoothRotateTowardsTarget()
    {
        targetRotation = Quaternion.LookRotation((targetManager.LastTarget.transform.position + new Vector3(0, Y_Offset, 0)) - transform.position);
        rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSmoothness);
    }
}
