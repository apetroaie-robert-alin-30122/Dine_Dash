using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool canLookAround = true;

    [Header("Referencer")]
    public Rigidbody rb;
    public Transform head;
    public new Camera camera;

    [Header("Configurations")]
    public float walkSpeed;
    public float runSpeed;

    [Header("Food")]
    public int carriedFoodValue = -1;  // -1 = nimic în mână
    [HideInInspector] public GameObject heldDishObject;

    public float pickupRange = 3f;

    [HideInInspector]
    public bool isLocked = false;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (isLocked) return;

        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * 2f);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Held Food Value: " + carriedFoodValue);
        }
    }

    void FixedUpdate()
    {
        if (isLocked) return;

        Vector3 newVelocity = Vector3.up * rb.linearVelocity.y;
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        newVelocity.x = Input.GetAxis("Horizontal") * speed;
        newVelocity.z = Input.GetAxis("Vertical") * speed;

        rb.linearVelocity = transform.TransformDirection(newVelocity);
    }

    void LateUpdate()
    {
        if (isLocked) return;

        Vector3 e = head.eulerAngles;
        e.x -= Input.GetAxis("Mouse Y") * 2f;
        e.x = RestricAngle(e.x, -85f, 85f);
        head.eulerAngles = e;
    }

    public static float RestricAngle(float angle, float angleMin, float angleMax)
    {
        if (angle > 180)
            angle -= 360;
        else if (angle < -180)
            angle += 360;

        return Mathf.Clamp(angle, angleMin, angleMax);
    }

    public void ClearHeldFood()
    {
        carriedFoodValue = -1;

        if (heldDishObject != null)
        {
            Destroy(heldDishObject);
            heldDishObject = null;
        }
    }
}
