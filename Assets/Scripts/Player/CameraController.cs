using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform FollowTarget;
    [SerializeField] float Xvalue;
    [SerializeField] float Yvalue;
    [SerializeField] float Zvalue;

    [SerializeField] Vector2 FramingOffset;// This is for adjusting the camera angle to the player
    [SerializeField] float RotationSpeed;
    [SerializeField] float RotationX;
    [SerializeField] float RotationY;
    [SerializeField] float MinVerticalAngle = -45f;
    [SerializeField] float MaxVerticalAngle = 30f;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        RotationX += Input.GetAxis("Mouse Y") * RotationSpeed;//get the input mouse y
        RotationX = Mathf.Clamp(RotationX, MinVerticalAngle, MaxVerticalAngle);//clamp the rotation at some angle

        RotationY += Input.GetAxis("Mouse X") * RotationSpeed;//Get the input mouse x  
        var TargetRotation=Quaternion.Euler(-RotationX,RotationY,0);//Apply the mouse x and y here

        var FocusPosition = FollowTarget.position + new Vector3(FramingOffset.x,FramingOffset.y);
        transform.position = FocusPosition - TargetRotation * new Vector3(Xvalue, Yvalue, Zvalue);
        transform.rotation = TargetRotation;   
    }

    public Quaternion planarRotation => Quaternion.Euler(0,RotationY,0);// not to move the player in up or down direction when facing up or down
}
