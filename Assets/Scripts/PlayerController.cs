using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utilities;

namespace Platformer
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        // [SerializeField] CharacterController characterController;
        [SerializeField] Animator animator;
        [SerializeField] CinemachineFreeLook freeLookVCam;
        [SerializeField] GroundChecker groundChecker;
        [SerializeField] InputReader input;
        public Rigidbody rb;
        [SerializeField] TargetManager targetManager;
        [SerializeField] GameObject shieldPrefab;

        [field: Header("Movement Settings")]
        [field: SerializeField] public float MoveSpeed { get; private set; } = 300f;
        [field: SerializeField] public float RotationSpeed { get; private set; } = 15f;
        [field: SerializeField] public float SmoothTime { get; private set; } = 0.2f;

        [Header("Jump Settings")]
        [SerializeField] float jumpForce = 10f;
        [SerializeField] float jumpDuration = .5f;
        [SerializeField] float jumpCooldown = 0f;
        [SerializeField] float gravityMultiplier = 3f;

        [Header("Dash Settings")]
        [SerializeField] float dashForce = 10f;
        [SerializeField] float dashDuration = 1f;
        [SerializeField] float dashCooldown = 2f;

        [Header("Shield Settings")]
        [SerializeField] float shieldDuration = 2f;
        [SerializeField] float shieldCooldown = 3f;
         bool hasShield = false;
        const float ZeroF = 0f;

        Transform mainCam;
        public float CurrentSpeed { get; set; }
        public float DashVelocity { get; set; } = 1f;
        float velocity;
        float jumpVelocity;

        GameObject shieldObject;
        List<Utilities.Timer> timers;
        CountdownTimer jumpTimer;
        CountdownTimer jumpCooldownTimer;
        CountdownTimer dashTimer;
        CountdownTimer dashCooldownTimer;
        CountdownTimer shieldTimer;
        CountdownTimer shieldCooldownTimer;

        StateMachine stateMachine;

        protected static readonly int ShieldHash = Animator.StringToHash("Shield");
        protected static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        protected const float crossFadeDuration = 0.1f;
        PlayerMovement playerMovement;
        private Vector3 movement;

        private void Awake()
        {
            CameraSetup();
            rb.freezeRotation = true;
            TimerSetup();
            StateSetup();
            playerMovement = new PlayerMovement(this);
        }

     
        void CameraSetup()
        {
            mainCam = Camera.main.transform;
            freeLookVCam.Follow = transform;
            freeLookVCam.LookAt = transform;
            freeLookVCam.OnTargetObjectWarped(transform, transform.position - freeLookVCam.transform.position - Vector3.forward);
        }
        void StateSetup()
        {
            stateMachine = new StateMachine();


            var locomotionState = new LocomotionState(this, animator);
            var jumpState = new JumpState(this, animator);
            var dashState = new DashState(this, animator);

            stateMachine.AddTransition(locomotionState, jumpState, new FuncPredicate(() => jumpTimer.IsRunning));
            stateMachine.AddTransition(locomotionState, dashState, new FuncPredicate(() => dashTimer.IsRunning));

            stateMachine.AddAnyTransition(locomotionState, new FuncPredicate(() => groundChecker.IsGrounded && !jumpTimer.IsRunning && !dashTimer.IsRunning));
            stateMachine.SetState(locomotionState);
        }
        void TimerSetup()
        {
            jumpTimer = new CountdownTimer(jumpDuration);
            jumpCooldownTimer = new CountdownTimer(jumpCooldown);

            jumpTimer.OnTimerStart += () => jumpVelocity = jumpForce;
            jumpTimer.OnTimerStop += () => jumpCooldownTimer.Start();

            dashTimer = new CountdownTimer(dashDuration);
            dashCooldownTimer = new CountdownTimer(dashCooldown);

            dashTimer.OnTimerStart += () => DashVelocity = dashForce;
            dashTimer.OnTimerStop += () =>
            {
                DashVelocity = 1.0f;
                dashCooldownTimer.Start();
            };

            shieldTimer = new CountdownTimer(shieldDuration);
            shieldCooldownTimer = new CountdownTimer(shieldCooldown);

            shieldTimer.OnTimerStart += () => hasShield = true;
            shieldTimer.OnTimerStop += () =>
            {
                hasShield = false;
                shieldCooldownTimer.Start();

            };

            timers = new List<Utilities.Timer>(capacity: 6) { jumpTimer, jumpCooldownTimer, dashTimer, dashCooldownTimer, shieldTimer, shieldCooldownTimer };
        }
        private void Start()
        {

            input.EnablePlayerActions();
            EventBus<AddTarget>.Raise(new AddTarget()
            {
                target = this.gameObject
            });

        }
        void OnTriggerEnter(Collider other)
        {
            if (targetManager.LastTarget != gameObject) return;
            if (other.gameObject.tag != "Ball") return;
            if (!hasShield)
            {
                EventBus<RemoveTarget>.Raise(new RemoveTarget()
                {
                    target = this.gameObject
                });
                Destroy(gameObject);
            }
            else
            {
                animator.CrossFade(ShieldHash, crossFadeDuration);

                StartCoroutine("OnCompleteAttackAnimation");
            }
            EventBus<ChooseTarget>.Raise(new ChooseTarget());
        }
        IEnumerator OnCompleteAttackAnimation()
        {
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            animator.CrossFade(LocomotionHash, crossFadeDuration);
            yield return null;
        }
        private void OnEnable()
        {
            input.Jump += OnJump;
            input.Dash += OnDash;
            input.Shield += OnShield;
        }
        private void OnDisable()
        {
            input.Jump -= OnJump;
            input.Dash -= OnDash;
            input.Shield -= OnShield;
        }
        private void OnShield()
        {
            if (!shieldTimer.IsRunning && !shieldCooldownTimer.IsRunning)
            {

                shieldTimer.Start();
                shieldObject = Instantiate(shieldPrefab, transform.position + new Vector3(0, 1, 0), transform.rotation);
                shieldObject.transform.parent = transform;
                Destroy(shieldObject, shieldDuration);
            }
        }
        private void OnJump(bool performed)
        {
            if (performed && !jumpTimer.IsRunning && !jumpCooldownTimer.IsRunning && groundChecker.IsGrounded)
            {
                jumpTimer.Start();
            }
            else if (!performed && jumpTimer.IsRunning)
            {
                jumpTimer.Stop();
            }
        }
        private void OnDash(bool performed)
        {
            if (performed && !dashTimer.IsRunning && !dashCooldownTimer.IsRunning)
            {
                dashTimer.Start();
            }
            else if (!performed && jumpTimer.IsRunning)
            {
                dashTimer.Stop();
            }
        }
        void Update()
        {
            movement = new Vector3(input.Direction.x, 0f, input.Direction.y);
            stateMachine.Update();
            HandleTimer();
            UpdateAnimator();
        }
        void FixedUpdate()
        {
            stateMachine.FixUpdate();
        }
        private void UpdateAnimator()
        {
            animator.SetFloat("Speed", CurrentSpeed);
        }
        private void HandleTimer()
        {
            foreach (var timer in timers)
            {
                timer.Tick(Time.deltaTime);
            }
        }
        public void HandleJump()
        {
            if (!jumpTimer.IsRunning && groundChecker.IsGrounded)
            {
                jumpVelocity = ZeroF;
                jumpTimer.Stop();
                return;
            }
            if (!jumpTimer.IsRunning)
            {
                jumpVelocity += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
            }

            rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);
        }
        public void HandleMovement()
        {
            Vector3 adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movement;
            playerMovement.HandleMovement(adjustedDirection);
        }

    }
}

