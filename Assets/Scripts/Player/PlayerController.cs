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
    [SerializeField] float GroundlayerRadius = 0.2f;

    [SerializeField] float movespeed = 8.5f;
    [SerializeField] float RotationSpeed = 500f;
    Quaternion TargetRotation;

    float FakeMoveamount = 0f;
    float horizontal;
    float vertical;
    float ySpeed;
    bool IsGrounded;

    [Header("Weapon")]
    bool DrawWeapon = false;
    bool SheathWeapon = false;

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

        float MoveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

        var moveinput = (new Vector3(horizontal, 0, vertical)).normalized;
        var moveDirection = cameracontroller.planarRotation * moveinput;
        Groundcheck();
        if (IsGrounded)
        {
            ySpeed = -0.5f;
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;
        }
        var velocity = moveDirection * movespeed;
        velocity.y = ySpeed;
        characterController.Move(velocity * Time.deltaTime);

        if (MoveAmount > 0)
        {
            TargetRotation = Quaternion.LookRotation(moveDirection);
        }
        if(Input.GetKey(KeyCode.LeftShift))
        {
            MoveAmount *= 1.1f;
            characterController.Move(velocity * 1.1f * Time.deltaTime);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation,
           RotationSpeed * Time.deltaTime);


        if (Input.GetKeyDown(KeyCode.Alpha1) && DrawWeapon == false)
        {
            //SheathWeapon = false;
            DrawWeapon = true;
            animator.SetBool("DrawWeapon", DrawWeapon);
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha2) && DrawWeapon == true)
        {
            DrawWeapon = false;
            SheathWeapon = true;
            animator.SetBool("SheathWeapon", SheathWeapon);
        }
        else
        {
            SheathWeapon = false;
        }

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
    }

    void Groundcheck()
    {
       IsGrounded = Physics.CheckSphere(transform.TransformPoint(GroundcheckOffset), GroundlayerRadius, Groundlayer);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0,1f,0,0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(GroundcheckOffset), GroundlayerRadius);
    }
}
 