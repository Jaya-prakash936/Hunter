using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.InputSystem;
public class New_PlayerController : MonoBehaviour
{
    public Weapon_Equipment weapon_manager;

    [SerializeField]
    private float movespeed=3f;

    [SerializeField]
    private float SprintValue = 1.1f;
    
    [SerializeField]
    private float MaxSpeed;

    [SerializeField]
    private float MinSpeed;



    [SerializeField]
    private float RotationSpeed = 400f;

    [SerializeField]
    private float JumpPower = 90f;


    [Header ("GroundCheck Values")]
    [SerializeField] LayerMask Groundlayer;
    [SerializeField] Vector3 GroundcheckOffset;
    [SerializeField] float GroundlayerRadius = 0.2f;



    private Player_Input_Actions PlayerInput;
    private CameraController cameracontroller;

    private Vector3 Move;
    private Rigidbody rb;
    private Quaternion TargetRotation;
    private Animator animator;
    private bool drawweapon = false;

    private float Movement;
    private float tempmovespeed;

    private Vector3 velocity;
    private void Awake()
    {
        PlayerInput = new Player_Input_Actions();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameracontroller = Camera.main.GetComponent<CameraController>();
        animator = GetComponent <Animator>(); 
        tempmovespeed = movespeed;
    }

    private void Update()
    {
        
    }
    private void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * rb.mass * 12f);
        PlayerMovement();

        if (PlayerInput.Player.Sprint.IsPressed()) //Assigned the Left shift button for sprinting
        {
            Sprint();
        }
        else
        {
            movespeed -= 0.25f;
        }

        //This is to limit the Maximum and Minimum Speed of the Player
        if (movespeed > MaxSpeed)
        {
            movespeed = MaxSpeed;
        }
        else if (movespeed < MinSpeed)
        {
            movespeed = MinSpeed;
        }


        if (PlayerInput.Player.Jump.triggered)//Assigned the Spacebar button for jumping
        {
            PlayerJump();
            
        }


        if (PlayerInput.Player.DrawWeapon.triggered)//Assigned the Number 1 button for Drawing and sheathing the weapon
        {
            drawweapon = !drawweapon;

        }

        if(PlayerInput.Player.Attack.triggered)//Assigned the Left Mouse button for Attacking
        {
            Attack();
        }
        AnimationController();

    }
    

    #region FUNCTIONS


    void PlayerMovement()
    {
        Move = PlayerInput.Player.Move.ReadValue<Vector2>();

        Movement = Mathf.Clamp01(Mathf.Abs(Move.x) + Mathf.Abs(Move.y));
        var moveinput = (new Vector3(Move.x, 0, Move.y)).normalized;
        var moveDirection = cameracontroller.planarRotation * moveinput;
         velocity = moveDirection * movespeed;


        rb.velocity = velocity;
       
        if (Movement > 0)
        {
            TargetRotation = Quaternion.LookRotation(moveDirection);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation,
          RotationSpeed * Time.deltaTime);
    }
    void Sprint()
    {
        Movement = SprintValue;
        movespeed += 0.1f;
    }

    bool GroundCheck()
    {
        return Physics.CheckSphere(transform.TransformPoint(GroundcheckOffset), GroundlayerRadius, Groundlayer);
    }

    void PlayerJump()
    {
        if (GroundCheck())
        {
            animator.SetTrigger("Jump");
            rb.AddForce(Vector3.up * JumpPower,ForceMode.Impulse);
            
        }
    }

    void Attack()
    {
        if (weapon_manager.WeaponEquiped == true)
        {
            animator.SetTrigger("Attack");
        }
    }
    #endregion

    void AnimationController()
    {
        animator.SetFloat("MoveAmount", Movement, 0.1f, Time.deltaTime); //This is for movement animation

       if(drawweapon)
        {
            animator.SetTrigger("DrawWeapon");
        }
       else
        {
            animator.SetTrigger("SheathWeapon");
        }
        
    }

    #region ENABLE_DISABLE

    private void OnEnable()
    {
        PlayerInput.Player.Enable();
    }
    private void OnDisable()
    {
        PlayerInput.Player.Disable();
    }
    #endregion
}
