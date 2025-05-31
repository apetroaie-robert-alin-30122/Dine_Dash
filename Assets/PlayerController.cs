using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public bool canLookAround = true;
	[Header("Referencer")]
	public Rigidbody rb;
	public Transform head;
	public Camera camera;
	
	[Header("Configurations")]
	public float walkSpeed;
	public float runSpeed;
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     Cursor.visible = false;
     Cursor.lockState = CursorLockMode.Locked;
     	 
    }

    // Update is called once per frame
    void Update()
    {
		if (!canLookAround) return;  // Disable rotation while dialogue
        //Horizontal rotation
		transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * 2f);
    }
	
	void FixedUpdate()
	{
	Vector3 newVelocity = Vector3.up * rb.linearVelocity.y;
	float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
	newVelocity.x = Input.GetAxis("Horizontal") * speed;
	newVelocity.z = Input.GetAxis("Vertical") * speed;
	rb.linearVelocity = transform.TransformDirection(newVelocity);
	}
	
	void LateUpdate () 
	{
	  if (!canLookAround) return;  // Disable rotation while dialogue
	  // Vertical rotation
	  Vector3 e = head.eulerAngles;
	  e.x -= Input.GetAxis("Mouse Y") * 2f;
	  e.x = RestricAngle(e.x, -85f, 85f);
	  head.eulerAngles = e;
	}
	// Clamp the vertical head rotaion
	public static float RestricAngle(float angle, float angleMin, float angleMax)
	{
		if (angle > 180)
			angle -= 360;
		else if (angle < -180)
			angle += 360;
		
		if (angle > angleMax)
			angle = angleMax;
		if (angle < angleMin)
			angle = angleMin;
		
		return angle;
	}
	
}
