using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Weapon_Equipment Weapon_equipment;
    CameraController cameracontroller;
    CharacterController characterController;
    Animator animator;
    [SerializeField] LayerMask Groundlayer;
    [SerializeField] Vector3 GroundcheckOffset;
    [SerializeField] float GroundlayerRadius = 1f;

    [SerializeField] float movespeed = 4.5f;
    [SerializeField] float RotationSpeed = 500f;
    Quaternion TargetRotation;

    [Header("dodge")]
    [SerializeField]
    private float DodgeForce = 10f;
    [SerializeField]
    private float DodgeCooldown = 1f;
    [SerializeField]
    private float DodgeDuration = 0.2f;
    
    private bool CanDodge = true;
    private bool isDodging = false;

    float MoveAmount;
    float FakeMoveamount = 0f;
    float horizontal;
    float vertical;
    float ySpeed;
    bool IsGrounded;
    bool Jumped;
    public float JumpPower = 15;

    [Header("Weapon")]
    bool DrawWeapon = false;
    bool SheathWeapon = false;
    bool SwordInHand = false;

    void Awake()
    {
        cameracontroller = Camera.main.GetComponent<CameraController>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {

    }


    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        MoveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

        var moveinput = (new Vector3(horizontal, 0, vertical)).normalized;
        var moveDirection = cameracontroller.planarRotation * moveinput;
        var Velocity = moveDirection * movespeed;
        Groundcheck();
        if (IsGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            Velocity.y = ySpeed;
            characterController.Move(Velocity * Time.deltaTime);
        }
        else
        {
            Velocity.y = ySpeed;
            ySpeed += Physics.gravity.y * Time.deltaTime;
            characterController.Move(Velocity * Time.deltaTime);
        }

        if (MoveAmount > 0)
        {
            TargetRotation = Quaternion.LookRotation(moveDirection);
        }


        #region PLAYER RUNNING
        if (Input.GetKey(KeyCode.LeftShift))
        {
            MoveAmount *= 1.1f;
            characterController.Move(Velocity * 1.1f * Time.deltaTime);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation,
           RotationSpeed * Time.deltaTime);
        #endregion


        #region PLAYER WEAPON DRAWING OR SHEATHING
        if (Input.GetKeyDown(KeyCode.Alpha1) && DrawWeapon == false)
        {
            //SheathWeapon = false;
            DrawWeapon = true;
            SwordInHand = true;
            animator.SetBool("DrawWeapon", DrawWeapon);
            animator.SetBool("SwordinHand", SwordInHand);
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha2) && DrawWeapon == true)
        {
            DrawWeapon = false;
            SheathWeapon = true;
            SwordInHand = false;
            animator.SetBool("SheathWeapon", SheathWeapon);
            animator.SetBool("SwordinHand", SwordInHand);
        }
        else
        {
            SheathWeapon = false;
        }
        #endregion


        #region IMPLENTING DODGE 
        if (Input.GetKeyDown(KeyCode.Mouse4) && CanDodge && Weapon_equipment.WeaponEquiped==true)
        {
            Vector3 dodgeDirection = GetDodgeDirection();
            if (dodgeDirection != Vector3.zero)
            {
                StartCoroutine(Dodge(dodgeDirection));
                animator.SetTrigger("Dodge");
            }
        }
        #endregion

        //Animations
        animator.SetFloat("MoveAmount", MoveAmount,0.1f,Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Weapon_equipment.WeaponEquiped == true)
            {
                animator.SetTrigger("Attack");
                animator.SetFloat("FakeMoveamount", FakeMoveamount);
            }
        }

        Jumped = false;
    }
    #region DODGE
    Vector3 GetDodgeDirection()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 direction =new Vector3(h,0f,v).normalized;
        Debug.Log("Dodge Direction " +  direction);
        if (direction != Vector3.zero)
        {
            return transform.forward;
            //TransformDirection(direction);
        }
        else
        {
            return transform.forward;
        }
    }

    System.Collections.IEnumerator Dodge(Vector3 direction)
    {
        CanDodge = false;
        isDodging = true;

        // Enable root motion to let animation drive movement
        animator.applyRootMotion = true;

        float DodgeEndTime = Time.time + DodgeDuration;
        while (Time.time < DodgeEndTime)
        {
            // Sync CharacterController to animation's root motion position
            Vector3 rootMotion = animator.deltaPosition;
            characterController.Move(rootMotion+(direction * DodgeForce * Time.deltaTime));
            yield return null;
        }
        animator.applyRootMotion = false;

        isDodging = false;


        yield return new WaitForSeconds(DodgeCooldown);
        CanDodge = true;

    }
    #endregion
    void Groundcheck()
    {
       IsGrounded = Physics.CheckSphere(transform.TransformPoint(GroundcheckOffset), GroundlayerRadius, Groundlayer);
    }
    public void Jump()
    {

        var moveinput = (new Vector3(horizontal, 0, vertical)).normalized;
        var moveDirection = cameracontroller.planarRotation * moveinput;
        var Velocity = moveDirection * movespeed;

                ySpeed = JumpPower;
                Velocity.y = ySpeed;
                characterController.Move(Velocity * Time.deltaTime);
                print("Grounded");

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0,1f,0,0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(GroundcheckOffset), GroundlayerRadius);
    }
}
 