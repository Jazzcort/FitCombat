using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    public TouchingDirections touchingDirections;
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float airWalkSpeed = 3;
    public float jumpImplus = 10f;
    Vector2 moveInput;
    public Joystick stick;
    public Button jumpButton;
    public Button attackButton;


    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving && !touchingDirections.IsOnWall)
                {
                    if (touchingDirections.IsGrounded)
                    {
                        if (IsRunning)
                        {
                            return runSpeed;
                        }
                        else
                        {
                            return walkSpeed;
                        }
                    }
                    else
                    {
                        // Air Walk
                        return airWalkSpeed;
                    }


                }
                else
                {
                    // Hit the wall or not Moving
                    return 0;
                }
            }
            else
            {
                // Movement locked
                return 0;
            }

        }
    }

    [SerializeField]
    private bool _isMoving = false;

    private bool _isFacingRight = true;

    public bool IsFacingRight
    {
        get
        {
            return _isFacingRight;
        }
        private set
        {
            if (_isFacingRight != value)
            {
                // Flip the player to face the opposite direction
                transform.localScale *= new Vector2(-1, 1);
            }

            _isFacingRight = value;

        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
        private set { }
    }

    Rigidbody2D rb;

    [SerializeField]
    Animator animator;

    private void Awake()
    {
        // rb = GetComponent<Rigidbody2D>();
        // animator = GetComponent<Animator>();
        // jumpButton.onClick.AddListener(() =>
        // {
        //     jump();
        //     Debug.Log(animator);

        // });
        // Debug.Log(stick);
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            jumpButton = GameObject.FindWithTag("JumpButton").GetComponent<Button>();
            attackButton = GameObject.FindWithTag("AttackButton").GetComponent<Button>();
            stick = FindObjectOfType<FixedJoystick>();

            jumpButton.onClick.AddListener(() =>
            {
                jump();

            });
            attackButton.onClick.AddListener(() =>
            {
                attack();

            });

        }

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }



    // Start is called before the first frame update
    void Start()
    {

    }

    [SerializeField]
    private bool _isRunning = false;

    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        private set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }



    void Update()
    {



    }

    private void FixedUpdate()
    {
        if (IsOwner)
        {
            float x_v;
            if (stick.Horizontal > 0.2f)
            {
                x_v = 1.0f;
                if (stick.Horizontal > 0.8f)
                {
                    IsRunning = true;

                }
                else
                {
                    IsRunning = false;

                }
            }
            else if (stick.Horizontal < -0.2f)
            {
                x_v = -1.0f;
                if (stick.Horizontal < -0.8f)
                {
                    IsRunning = true;

                }
                else
                {
                    IsRunning = false;

                }
            }
            else
            {
                x_v = 0;
            }

            rb.velocity = new Vector2(x_v * CurrentMoveSpeed, rb.velocity.y);
            IsMoving = x_v != 0;
            SetFacingDirection(stick.Horizontal);
            SetFacingDirectionServerRpc(stick.Horizontal);
            animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);

        }


    }



    private void SetFacingDirection(float x_movement)
    {
        if (x_movement > 0 && !IsFacingRight)
        {

            // Face the right
            IsFacingRight = true;

        }
        else if (x_movement < 0 && IsFacingRight)
        {

            // Face the left
            IsFacingRight = false;
        }
    }

    [ServerRpc]
    private void SetFacingDirectionServerRpc(float x_movement)
    {
        if (x_movement > 0 && !IsFacingRight)
        {

            // Face the right
            IsFacingRight = true;

        }
        else if (x_movement < 0 && IsFacingRight)
        {

            // Face the left
            IsFacingRight = false;
        }

        SetFacingDirectionClientRpc(x_movement);

    }

    [ClientRpc]
    private void SetFacingDirectionClientRpc(float x_movement)
    {
        if (x_movement > 0 && !IsFacingRight)
        {

            // Face the right
            IsFacingRight = true;

        }
        else if (x_movement < 0 && IsFacingRight)
        {

            // Face the left
            IsFacingRight = false;
        }

    }

    public void jump()
    {

        //TODO: check if the player is alive as well
        if (touchingDirections.IsGrounded && CanMove)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpImplus);
            animator.SetTrigger(AnimationStrings.jump);
        }

    }

    public void attack()
    {

        animator.SetTrigger(AnimationStrings.attack);

    }

}
